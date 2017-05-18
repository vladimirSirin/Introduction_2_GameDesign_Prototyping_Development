using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour {

    // Fields set in the Unity Inspector Pane

    public int numClouds = 40; // The # of clouds to make
    public GameObject[] cloudPrefabs; // The prefabs for the clouds
    public Vector3 cloudPosMin; // Min position of each cloud
    public Vector3 cloudPosMax; // Max position of each cloud
    public float cloudScaleMin = 1; // Min scale of each cloud
    public float cloudScaleMax = 5; // Max scale of each cloud
    public float cloudSpeedMult = 0.5f; //Adjusts speed of clouds

    public bool _________________________________________;

    // Fields set dynamically
    public GameObject[] cloudInstances;

    void Awake()
    {
        // Make an array large enough to hold all the cloud instances
        cloudInstances = new GameObject[numClouds];

        // Find the CloudAnchor parent GameObject
        GameObject anchor = GameObject.Find("CloudAnchor");

        // Iterate through and make Cloud_s
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            // Pick an int between 0 and cloudPrefabs.Length - 1
            // Random.range will not ever pick as high as the top number
            int prefabNum = Random.Range(0, cloudPrefabs.Length);

            // Make an instance
            cloud = Instantiate(cloudPrefabs[prefabNum]) as GameObject;
            
            // Position Cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);

            // Scale Cloud
            float scaleU = Random.value;
            float scaleval = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

            // Smaller Clouds (with smaller scaleU) should be nearer the ground)
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);

            // Smaller Cloud should be further away
            cPos.z = 100 - 90 * scaleU;

            // Apply transformation to the cloud
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleval;

            // Make the CloudAnchor the parent of the cloud and put it into the cloudinstances array
            cloud.transform.parent = anchor.transform;
            cloudInstances[i] = cloud;


        }
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		// Iterate over each cloud that was created
        foreach (var cloud in cloudInstances)
        {
           // get the cloud scale and position
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;

            // Move larger cloud faster
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

            // If a cloud has moved too far to the left
            if (cPos.x < cloudPosMin.x)
            {
                cPos.x = cloudPosMax.x;
            }

            // Apply the new position to cloud
            cloud.transform.position = cPos;
        }
	}
}
