using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    public float speed = 10f; // the speed of the enemy in m/s
    public float fireRate = 0.3f; // seconds/shot
    public float health = 10;
    public int score = 100;

    public bool _________________________________;
    public Bounds boundBox; // the bounds of the enemy and his children
    public Vector3 boundsCentreOffVector3; // the dist of bounds.centre from position
    

    // for hit feedback
    public int showDamageForFrame = 2; // # frame to show damage

    public int remainingDamageFrame = 0; // Damage frames left
    public Color[] originalColors;
    public Material[] originalMaterials;

    // Drop Powerup chance
    public float powerUpDropChance = 1f;

    void Awake()
    {
        originalMaterials = Utils.GetAllMaterial(gameObject);
        originalColors = new Color[originalMaterials.Length];
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            originalColors[i] = originalMaterials[i].color;
        }

        InvokeRepeating("CheckOffscreen", 0f, 2.0f);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	    if (remainingDamageFrame > 0)
	    {
	        remainingDamageFrame--;
	        if (remainingDamageFrame == 0)
	        {
	            UnShowDamage();
	        }
	    }
	}


    public virtual void Move()
    {
        Vector3 tempPos = Pos;
        tempPos.y -= speed * Time.deltaTime;
        Pos = tempPos;
    }

    public Vector3 Pos
    {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }


    public void CheckOffscreen()
    {
        // Check if the Bounds params are zero
        if (boundBox.size == Vector3.zero)
        {
            // If so, initialize the boundBox with the utils functions
            boundBox = Utils.CombineBoundsOfChildren(this.gameObject);
            // Calculate the offset of the position and boundBox centre
            boundsCentreOffVector3 = boundBox.center - this.transform.position;
        }

        // Caliberate the offset
        boundBox.center = boundsCentreOffVector3 + this.transform.position;

        // Check if this is off screen
        Vector3 off = Utils.ScreenBoundsCheck(boundBox, BoundsTest.offScreen);

        // if so, destroy it.
        if (off != Vector3.zero)
        {
            if (off.y < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        // Initialize the gameObject of the collision detected
        GameObject other = coll.gameObject;


        switch (other.tag)
        {
            // Check if the Enemy is just awaking or starting before
            case "ProjectileHero":
                Projectile proj = other.GetComponent<Projectile>();
                boundBox.center = transform.position + boundsCentreOffVector3;
                if (boundBox.extents == Vector3.zero || Utils.ScreenBoundsCheck(boundBox, BoundsTest.offScreen) != Vector3.zero)
                {
                    Destroy(other);

                    break;
                }
                else
                {
                    //Hurt the enemy
                    ShowDamage();
                    // Get the damage on hit from the W_DEFS
                    health -= Main.W_DEFS[proj.Type].damageOnHit;
                    if (health <= 0)
                    {
                        // Destory the enemy
                        Main.S.ShipDestroyed(this);
                        Destroy(this.gameObject);
                    }

                }
                Destroy(other);
                break;     
        }

    }

    void ShowDamage()
    {
        foreach (Material m in originalMaterials)
        {
            m.color = Color.red;
        }
        remainingDamageFrame = showDamageForFrame;
    }

    void UnShowDamage()
    {
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            originalMaterials[i].color = originalColors[i];
        }
    }
}

