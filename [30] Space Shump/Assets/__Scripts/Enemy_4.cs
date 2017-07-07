using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;


// Part is another serializable data storage class just like WeaponDefinition
[System.Serializable]
public class Part
{
    // There three fields need to defined in the inspector pane
    public string name;

    public float health;

    public string[] protectedBy;



    // These two fields are set automatically in Start()
    // Caching like this makes it fater and easier to find these later
    public GameObject go;

    public Material mat;
}

public class Enemy_4 : Enemy {

    // Initialize points for moving
    public Vector3[] points;

    // Birth time for this Enemy_4
    public float timeStart;

    // Duration of each movement
    public float duration = 4;

    // The array of ship parts
    public Part[] parts;

	// Use this for initialization
	void Start () {

        // Initialize the data
        points = new Vector3[2];
	    points[0] = Pos;
	    points[1] = Pos;

	    // Initialize the movement
        InitMovement();

        // Cache GameObject and Material of each part in parts
	    Transform t;
	    foreach (Part prt in parts)
	    {
	        t = transform.Find(prt.name);
	        if (t != null)
	        {
	            prt.go = t.gameObject;
	            prt.mat = t.GetComponent<Renderer>().material;
	        }
	    }
	}


    void InitMovement()
    {
        // Initialize the movement coordinates
        Vector3 p1 = Vector3.zero;
        float esp = Main.S.EnemySpawnPadding;
        Bounds cBounds = Utils.CamBounds;

        p1.x = Random.Range(cBounds.min.x + esp, cBounds.max.x - esp);
        p1.y = Random.Range(cBounds.min.y + esp, cBounds.max.y - esp);

        // Shift the points form [1] to [0]
        points[0] = points[1];
        points[1] = p1;

        // Reset the startTime
        timeStart = Time.time;
    }
	
	// Override the move function
	public override void Move() {

        // Use linear interpolation to move the enemy_4
	    float u = (Time.time - timeStart) / duration;



	    // If u > 1, initialize the movement again
	    if (u>=1)
	    {
	        InitMovement();
	        u = 0;
	    }

	    // Apply eased out to the [u]
	    u = 1 - Mathf.Pow(1 - u, 2);

        Pos = (1 - u) * points[0] + u * points[1];




	}


    // This will override the OnCollisionEnter that is part of Enemy.cs
    // Because of the way that MonoBehaviour declares common Unity functions
    // like OnCollisionEnter(), the override keyword is not necessary.
    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;


        // Enemies dont take damage unless they are on screen
        // THis stops the player from shooting them before they are visible
        switch (other.tag)
        {
            case "ProjectileHero":
                Projectile p = other.GetComponent<Projectile>();
                // Enemies dont take damage unless they are on screen
                // This stops the player from shooting them before they are visible
                boundBox.center = transform.position + boundsCentreOffVector3;
                if (boundBox.extents == Vector3.zero || Utils.ScreenBoundsCheck(boundBox, BoundsTest.offScreen) != Vector3.zero)
                {
                    Destroy(other);
                    break;
                }

                // Hurt this Enemy
                // Find the gameObject that was hit
                // the Collision coll has contacts[], an array of ContactPoints
                // Because there was a collision, we are guaranteed that there is at
                // least a contacts[0], and ContactPoints have a reference to
                // thisCollider, which will be the collider for the part of
                // the Enemy_4 that was hit.
                GameObject goHit = coll.contacts[0].thisCollider.gameObject;
                Part prtHit = FindPart(goHit);

                if (prtHit == null)
                // If prtHit was not found, then it's usually because, very rarely, thisCollider on
                // contacts[0] will be the ProjectileHero instead of the ship part, if so.
                // Just look for ohterCollider instead
                {
                    goHit = coll.contacts[0].otherCollider.gameObject;
                    prtHit = FindPart(goHit);
                }


                // Check whether this part is still protected
                if (prtHit.protectedBy != null)
                {
                    // Check if all of the protections have gone
                    foreach (string s in prtHit.protectedBy)
                    {
                        if (!Destroyed(s))
                        {
                            // if one of the protecting parts has not been destroyed
                            Destroy(other); // Destroy the hero projectile
                            return; // return before casuing damage
                        }
                    }
                }


                // If it is not protected, so make it take damage
                // get the damage amount from the Projectile.type and main.W_DEFS
                prtHit.health -= Main.W_DEFS[p.Type].damageOnHit;

                // show damage on the parts
                ShowLocalizedDamage(prtHit.mat);
                if (prtHit.health <= 0)
                {
                    // Instead of destroying this enemy, disable the damaged part
                    prtHit.go.SetActive(false);
                }

                // Check to see if the whole ship is destroyed
                // Let's assume it is completely destroyed
                bool allDestroyed = true;

                // if any of the part is not destroyed
                foreach (Part prtPart in parts)
                {
                    if (!Destroyed(prtPart))
                    {
                        allDestroyed = false;
                        break;
                    }
                }

                // if all are destroyed
                if (allDestroyed)
                {
                    // notify main the ship is destroyed
                    Main.S.ShipDestroyed(this);

                    // destroy the gameobject
                    Destroy(this.gameObject);
                }
                Destroy(other);
                break;
        }
    }


    // These two functions find a Part in this.parts by name or GameObject
    Part FindPart(string s)
    {
        foreach (Part prtPart in parts)
        {
            if (prtPart.name == s)
            {
                return prtPart;
            }
        }

        return null;
    }

    Part FindPart(GameObject go)
    {
        foreach (Part part in parts)
        {
            if (part.go == go)
            {
                return part;
            }
        }
        return null;
    }



    // These functions return true if the Part has be destroyed
    bool Destroyed(GameObject go)
    {
        return (Destroyed(FindPart(go)));
    }

    bool Destroyed(string s)
    {
        return (Destroyed(FindPart(s)));
    }

    bool Destroyed(Part prt)
    {
        // if no real part was passed in, return true, meaning yes, it is destroyed
        if (prt == null)
        {
            return true;
        }

        return prt.health <= 0;
        // Returns the result of the comparison, prt.health<=0
        // if prt.health is 0 or less, return true.
    }

    // This changes the color of just one part to red instead of the whole ship
    void ShowLocalizedDamage(Material m)
    {
        m.color = Color.red;
        remainingDamageFrame = showDamageForFrame;
    }
}
