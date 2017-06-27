using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenosFollower : MonoBehaviour
{
    public GameObject poi;
    public float u = 0.1f;
    public Vector3 p0, p1, p01;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
        // Get the position of this and the poi
	    p0 = this.transform.position;
	    p1 = poi.transform.position;

        // Interpolate between the two
	    p01 = (1 - u) * p0 + u * p1;

        //Move this to the new position
	    gameObject.transform.position = p01;

	}
}
