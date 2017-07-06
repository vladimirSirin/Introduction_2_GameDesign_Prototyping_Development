using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;


// Part is another serializable data storage class just like WeaponDefinition

public class Enemy_4 : Enemy {

    // Initialize points for moving
    public Vector3[] points;

    // Birth time for this Enemy_4
    public float timeStart;

    // Duration of each movement
    public float duration = 4;

	// Use this for initialization
	void Start () {

        // Initialize the data
        points = new Vector3[2];
	    points[0] = Pos;
	    points[1] = Pos;

	    // Initialize the movement
        InitMovement();

	}


    void InitMovement()
    {
        // Initialize the movement coordinates
        Vector3 p1 = Vector3.zero;
        float esp = Main.S.EnemySpawnPadding;
        Bounds cBounds = Utils.CamBounds;

        p1.x = Random.Range(cBounds.min.x + esp, cBounds.max.x - esp);
        p1.y = Random.Range(cBounds.min.y + esp, cBounds.max.y - esp);

        // Shift the points form [1] to [0]
        points[0] = points[1];
        points[1] = p1;

        // Reset the startTime
        timeStart = Time.time;
    }
	
	// Override the move function
	public override void Move() {

        // Use linear interpolation to move the enemy_4
	    float u = (Time.time - timeStart) / duration;



	    // If u > 1, initialize the movement again
	    if (u>=1)
	    {
	        InitMovement();
	        u = 0;
	    }

	    // Apply eased out to the [u]
	    u = 1 - Mathf.Pow(1 - u, 2);

        Pos = (1 - u) * points[0] + u * points[1];




	}
}
