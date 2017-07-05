using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy {

    // Enemy_3 will move following a Bezier curve, which is a linear interpolation between more than 2 points.
    public Vector3[] points;

    public float birthTime;
    public float lifeTime = 10;


	// Use this for initialization
	void Start () {

        // Initialize points
	    points = new Vector3[3];

	    // Set the birthTime
	    birthTime = Time.time;

	    // Define entry and exit points, and the middle point
        // The start position has already been set by Main.SpawnEnemy()
	    points[0] = Pos;

        // Set xMin and xMax the same way that Main.SpawnEnemy() does
	    float xMin = Utils.CamBounds.min.x - Main.S.EnemySpawnPadding;
	    float xMax = Utils.CamBounds.max.x + Main.S.EnemySpawnPadding;

        // Pick a random middle position in the bottom half of the screen
	    Vector3 v = Vector3.zero;
	    v.x = Random.Range(xMin, xMax);
	    v.y = Random.Range(Utils.CamBounds.min.y, 0);
	    points[1] = v;

        // Pick a random final position above the top of the screen
	    v = Vector3.zero;
	    v.x = Random.Range(xMin, xMax);
	    v.y = Pos.y;
	    points[2] = v;


	}
	
	// Update is called once per frame
	void Update () {

        // Calculate the float u with the time elapsed
	    float u = (Time.time - birthTime) / lifeTime;
	    u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);

	    // If lifeTime is finished, destroy it
	    if (u>=1)
	    {
	        Destroy(gameObject);
            return;
	    }

	    // If not, update the position based on the linear interpolation method
	    Vector3 res = Utils.Bezier(u, points);
	    gameObject.transform.position = res;
	}
}
