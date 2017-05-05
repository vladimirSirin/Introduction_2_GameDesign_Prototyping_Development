using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour {
    
    // Prefab for instantiate apples
    public GameObject ApplePrefab;
    
    // Speed at which the AppleTree moves in meters/second
    public float Speed = 1f;

    // Distance where AppleTree turn around
    public float LeftAndRightEdge = 10f;

    // Chance that the AppleTree turn around
    public float ChanceToChangeDirection = 0.1f;

    // Rate at which Apples will be instantiated
    public float SecondsBetweenAppleDrops = 1f;


	// Use this for initialization
	void Start () {

        // Dropping Apples every second
		
	}
	
	// Update is called once per frame
	void Update () {

        // Basic Movement

        // Changing Direction
		
	}
}
