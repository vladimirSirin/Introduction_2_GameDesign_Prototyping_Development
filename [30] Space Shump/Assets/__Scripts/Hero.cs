using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    static public Hero S; // Singleton
    
    //  These fields control the movement of the help
    public float Speed = 30;

    public float PitchMult = 30;
    public float RollMult = -45;

    // ship status information
    public float ShieldLevel = 1;

    public bool _______________________________________________;
    public Bounds Bounds;

    // Awake
    void Awake()
    {
        S = this; // Set the Singleton
        Bounds = Utils.CombineBoundsOfChildren(this.gameObject);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Pull in Information from the input class
	    float xAxis = Input.GetAxis("Horizontal");
	    float yAxis = Input.GetAxis("Vertical");

	    // Change transform based on the input
	    Vector3 pos = this.transform.position;
	    pos.x += xAxis * Speed * Time.deltaTime;
	    pos.y += yAxis * Speed * Time.deltaTime;
	    transform.position = pos;

	    // Rotate the ship to make it feel more dynamic
	    transform.rotation = Quaternion.Euler(yAxis * PitchMult, xAxis * RollMult, 0);

	}
}
