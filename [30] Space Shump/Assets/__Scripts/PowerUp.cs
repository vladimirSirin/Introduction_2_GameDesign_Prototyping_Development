using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {


    // Set up the variables for the PowerUp
    public Vector2 driftMaxMin = new Vector2(15, 90);
    public Vector2 rotMaxMin = new Vector2(0.25f, 2f);
    public float lifeTime = 6f;
    public float fadeTime = 4f;
    public bool ______________________________________;
    public WeaponType type;
    public Vector3 rotPerSecond;
    public GameObject cube;
    public float birthTime;
    public TextMesh letter;

	// Use this for initialization
	void Awake () {

        // Find the cube references and textmesh
	    cube = transform.Find("Cube").gameObject;
	    letter = gameObject.GetComponent<TextMesh>();

	    // Set up the random velocity 
	    Vector3 vel = Random.onUnitSphere; // Get random xyz velocity
	    vel.z = 0; // Flatten the vel to the xy plane

        vel.Normalize(); // Make the length of the vel 1
	    vel *= Random.Range(driftMaxMin.x, driftMaxMin.y);
        // Above sets the velocity length to something between the x and y
	    gameObject.GetComponent<Rigidbody>().velocity = vel;
        // Values of driftMinMax

	    // Set up the rotation per second for update
	    gameObject.transform.rotation = Quaternion.identity;

	    // Quaternion.identity is equal to no rotation

	    // Set up the rotPerSecond for the cube child using rotMinMax x and y
        rotPerSecond = new Vector3(Random.Range(rotMaxMin.x, rotMaxMin.y), Random.Range(rotMaxMin.x, rotMaxMin.y), Random.Range(rotMaxMin.x, rotMaxMin.y));

	    // Invoke the repeating of the check offscreen function
        InvokeRepeating("CheckOffscreen", 2f, 2f);

	    // Set the birth time of the PowerUp for further
	    birthTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        // Update the rotation of the PowerUp
        // Manually rotate the Cube Child every Update()
        // Multiplying it by Time.time causes the rotation to be time-based
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        // Fade out and destroy the PowerUp based on the lifetime
        // Give the default values. a PowerUp will exist for 10 seconds and then fade out over 4 seconds
	    float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
	    // For lifeTime seconds, u will be <= 0, then it will transition to 1 over fadeTime seconds
	    // If u >=1, means the time is expanded, destroy the object
	    if (u > 1)
	    {
	        Destroy(gameObject);
            return;
	    }

        // Use u to determine the alpha value of the Cube and letter
	    if (u >= 0)
	    {
	        Color c = gameObject.GetComponent<Renderer>().material.color;
	        c.a = 1f - u;
	        gameObject.GetComponent<Renderer>().material.color = c;

	        c = gameObject.GetComponent<TextMesh>().color;
	        c.a = 1f - u * 0.5f;
	        letter.color = c;

	    }



	}


    // This function set the type of the powerUp and render the color of it, differs from those on Weapon and Projectile
    public void SetType(WeaponType wt)
    {
        // Grab the weapon definition from Main
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        // Sets the color of the Cube child
        cube.GetComponent<Renderer>().material.color = def.color;
        // We could colorize the letter too
        letter.text = def.letter;
        // Finally actually set the type
        type = wt;
    }


    // This function destroy and invoke the Hero weapon change based on the collision/Absorbtion of the PowerUp
    public void AbsorbedBy(GameObject go)
    {
        // This function is called by the Hero class when a PowerUp is collected 
        // WE could tween into the target and shrink it in size, but for now just destroy this.gameObject
        Destroy(this.gameObject);
    }


    // Check if the PowerUp is offscreen, if it is destroy is.
    void CheckOffscreen()
    {
        // If the PowerUp has drifted entirely off screen
        Vector3 off = Utils.ScreenBoundsCheck(cube.GetComponent<Collider>().bounds, BoundsTest.offScreen);

        // Destroy the gameobject
        if (off != Vector3.zero)
        {
            Destroy(this.gameObject);
        }
    }
}
