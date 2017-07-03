using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy {

    // Enemy_2 use a Sine Wave to modify a 2-point linear Interpolation
    public Vector3[] points;

    public float birthTime;

    public float lifeTime = 10f;

    public float sinEccentricity = 0.6f;


	// Use this for initialization
	void Start () {

        // Initialize the points
        points = new Vector3[2];

        // Find Utils.camBounds
	    Vector3 cMin = Utils.CamBounds.min;
	    Vector3 cMax = Utils.CamBounds.max;

	    // Pick any point on the left side of the screen
	    Vector3 v = Vector3.zero;
	    v.x = cMin.x - Main.S.EnemySpawnPadding;
	    v.y = Random.Range(cMin.y, cMax.y);

	    // Pick any point on the right side of the screen
        Vector3 w = Vector3.zero;
	    w.x = cMax.x + Main.S.EnemySpawnPadding;
	    w.y = Random.Range(cMin.y, cMax.y);

	    points[0] = v;
	    points[1] = w;
	    //Possibly swap sides
	    if (Random.value < 0.5f)
	    {
	        points[0].x *= -1;
	        points[1].x *= -1;
	    }


	    // Set the birthTime to the current time
	    birthTime = Time.time;

	}
	
	// Override the move function
	public override void Move()
    {
		// Bezier Curves work based on a u value between 0 to 1
        float u = (Time.time - birthTime) / lifeTime;


        // If u > 1, then it has been longer than lifetime since birth Time
        if (u>=1)
        {
            Destroy(gameObject);
            return;
        }


        // Adjust u by adding an easing curve based on a Sine wave
        u = u + sinEccentricity * Mathf.Sin(2 * Mathf.PI * u);


        // Interpolation the two linear points
        Pos = (1 - u) * points[0] + u * points[1];


    }
}
