using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    static public FollowCam S; // a followCam Singleton

    // Fields set in the Unity Inspector pane
    public float easing = 0.05f;

    public Vector2 minXY;
    public bool _____________________________________;

    // Fields set dynamically
    public GameObject poi; // The point of interest

    public float camZ; // The desired Z pos of the camera

    // Awake
    void Awake()
    {
        S = this;
        camZ = this.transform.position.z;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{

	    Vector3 destination;

        // If there is no POI, return to P:[0,0,0]


        // if there's only one line following an if, it doesn't need braces
	    if (poi == null) // return if there is no poi
	        destination = Vector3.zero;
	    else
	    {
            // Get the position of the poi
            destination = poi.transform.position;

            // IF pos is a projectile, check to see if it's at rest
	        if (poi.tag == "Projectile")
	        {
	            // If it is sleeping , that is not moving
	            if (poi.GetComponent<Rigidbody>().IsSleeping())
	            {
	                // return to default view in the next update
	                poi = null;
	                return;
	            }
	        }
        }


        // LImit the X and Y to minimum values
	    destination.x = Mathf.Max(minXY.x, destination.x);
	    destination.y = Mathf.Max(minXY.y, destination.y);

        // INterpolate from the current camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);

        // Retain a destination.z of camZ
	    destination.z = camZ;

        // Set the camera to the destination
	    transform.position = destination;

        // Set the orthographicSize of the Camera to keep Ground in view
	    this.GetComponent<Camera>().orthographicSize = destination.y + 10;

	}
}
