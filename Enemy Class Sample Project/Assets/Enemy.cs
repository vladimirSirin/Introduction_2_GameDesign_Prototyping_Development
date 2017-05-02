using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Persistence;

public class Enemy : MonoBehaviour {

    // Declare the speed and shoot rate of the enemy
    public float speed = 10f;

    public float fireRate = 0.3f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}

    // Implement the move function
    public virtual void Move()
    {
        // to do
        Vector3 tempPosVector3 = pos;
        tempPosVector3.y -= speed * Time.deltaTime;
        pos = tempPosVector3;

    }

    // Implement the onCollisionEnter, what happens when it collides with something
    void OncollisionEnter(Collision collision)
    {
        // to do
        GameObject other = collision.gameObject;
        switch (other.tag)
        {
            case "Hero":
                // Currently not implemented, but this would destroy the hero
                break;

            case "HeroLaser":
                //Destroy this Enemy
                Destroy(this.gameObject);
                break;
        }
    }

    // Setup the position of the enemy as a property, a method that acts like a field
    public Vector3 pos
    {
        get { return ( this.transform.position); }
        set { this.transform.position = value; }
    }
}
