using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {


    // Set up the variables for the PowerUp

	// Use this for initialization
	void Awake () {

        // Find the cube references and textmesh

        // Set up the random velocity 

        // Set up the rotation per second for update

        // Invoke the repeating of the check offscreen function
		
	}
	
	// Update is called once per frame
	void Update () {

        // Update the rotation of the PowerUp

        // Fade out and destroy the PowerUp based on the lifetime
		
	}


    // This function set the type of the powerUp and render the color of it
    public void SetType(WeaponType wt)
    {
        
    }


    // This function destroy and invoke the Hero weapon change based on the collision/Absorbtion of the PowerUp
    public void AbsorbedBy(GameObject go)
    {
        
    }


    // Check if the PowerUp is offscreen, if it is destroy is.
    void CheckOffscreen()
    {
        
    }
}
