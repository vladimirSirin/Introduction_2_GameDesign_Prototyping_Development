using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour {

    // This is Singleton of the Boidspawner. There is only one instance
    // of Boidspawner, so we can store it in a static variable named S.
    public static BoidSpawner S;

    // These fields allow you to adjust the behavior of the Bolds as a group
    public int NumBoids = 100;
    public GameObject BoidPrefab;
    public float SpawnRadius = 100f;
    public float SpawnVelocity = 10f;
    public float MinVelocity = 0f;
    public float MaxVelocity = 30f;
    public float NearDist = 30f;
    public float CollisionDist = 5f;
    public float VelocityMatchingAmt = 0.01f;
    public float FlockCenteringAmt = 0.15f;
    public float CollisionAvoidanceAmt = -0.5f;
    public float MouseAttractionAmt = 0.01f;
    public float MouseAvoidanceAmt = 0.75f;
    public float MouseAvoidanceDist = 15f;
    public float VelocityLerpAmt = 0.25f;

    public bool ____________________;
    public Vector3 MousePos;

	// Use this for initialization
	void Start () {

        // Set the Singleton S to be this instance of BoidSpawner
	    S = this;
        // Instantiate numBoids (currently 100) Boids
	    for (int i = 0; i < NumBoids; i++)
	    {
	        Instantiate(BoidPrefab);
	    }

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void LateUpdate()
    {
        // Track the mouse position. This keeps it the same for all Boids
        Vector3 mousePos2D = new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.transform.position.y);
        MousePos = GetComponent<Camera>().ScreenToWorldPoint(mousePos2D);
    }
}
