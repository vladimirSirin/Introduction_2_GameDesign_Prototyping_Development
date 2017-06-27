using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolator2 : MonoBehaviour
{


    public Transform c0, c1;
    public float timeDuration = 1;

    // Set checkToCalculate to sart moving
    public bool checkTocalculate = false;

    public bool moving = false;
    public float timeStart;

    public bool ______________________________;

    public Vector3 p01;
    public Quaternion r01;
    public Vector3 s01;
    public Color c01;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	    if (checkTocalculate)
	    {
	        checkTocalculate = false;

	        moving = true;
	        timeStart = Time.time;
	    }

	    if (moving)
	    {
	        float u = (Time.time - timeStart) / timeDuration;
	        float v = 1 - u;
	        if (u>=1)
	        {
	            moving = false;
	            u = 1;
	        }

	        p01 = v * c0.position + u * c1.position;
	        r01 = Quaternion.Slerp(c0.rotation, c1.rotation, u);
	        s01 = v * c0.localScale + u * c1.localScale;
	        c01 = v * c0.GetComponent<Renderer>().material.color + u * c1.GetComponent<Renderer>().material.color;

            // Apply those transforms.
	        transform.position = p01;
	        transform.rotation = r01;
	        transform.localScale = s01;
	        transform.GetComponent<Renderer>().material.color = c01;
	    }
		
	}
}
