using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    
    // A static field accessible by code anywhere
    static public bool GoalMet = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //
    void OnTriggerEnter(Collider other)
    {
        // When the trigger is hit by something
        // Check to see if it is a Projectile

        if (other.gameObject.tag == "Projectile")
        {
            // If so, get goalMet to true
            Goal.GoalMet = true;

            // Also get the alpha of the color to high opacity
            Color c = gameObject.GetComponent<Renderer>().material.color;
            c.a = 1;
            gameObject.GetComponent<Renderer>().material.color = c;
            
        }
    }
}
