using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    // This static List holds all Boid instances and it shared amongst them
    public static List<Boid> Boids;

    // Note: This code does NOT use a Rigidbody, it handles velocity directly
    public Vector3 Velocity; // The current velocity

    public Vector3 NewVelocity; // The velocity for next frame
    public Vector3 NewPosition; // The position for next frame

    public List<Boid> Neighbors; // All nearby Boids
    public List<Boid> CollisionRisk; // All nearby Boids too close
    public Boid ClosestBoid; // The closest Boid

    // Initialize this Boid on Awake
    void Awake()
    {
        // Define the Boids list if it is still null
        if (Boids == null)
        {
            Boids = new List<Boid>();
        }

        // Add this Boid to Boids
        Boids.Add(this);

        // Give this Boid instance a random velocity and position
        Vector3 randPos = Random.insideUnitSphere * BoidSpawner.S.SpawnRadius;
        randPos.y = 0; // make sure it moves only in XZ
        this.transform.position = randPos;

        Velocity = Random.insideUnitSphere;
        Velocity *= BoidSpawner.S.SpawnVelocity;

        // Initialize the two lines
        Neighbors = new List<Boid>();
        CollisionRisk = new List<Boid>();

        // Make this.tranform a child of the Boids GameObjetc
        this.transform.parent = GameObject.Find("Boids").transform;
        //^4

        // Give the Boid a random color, but make sure it's not too dark
        Color randColor = Color.black;
        while (randColor.r + randColor.g + randColor.b < 1.0f)
        {
            randColor = new Color(Random.value, Random.value, Random.value);
        }

        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (var r in rends)
        {
            r.material.color = randColor;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    // 5
	void Update () {
		
        // Get the list of nearby Boids (this Boid's neighbors)
	    List<Boid> neighbors = GetNeighbors(this);
        // ^6

        // Initialize newVelocity and newPosition to the current values
	    NewVelocity = Velocity;
	    NewPosition = this.transform.position;

        // Velocity matching: this sets the velocity of the Boid to be similar to that of its neighbors



	}

    // Find which  Boids are near enough to be considered neighbors
    // boi is Boid of interest, the Boid on which we are focusing
    // 8
    public List<Boid> GetNeighbors(Boid boi)
    {
        float closestDist = float.MaxValue; // set the max float value
        Vector3 delta;
        float dist;
        Neighbors.Clear();
        CollisionRisk.Clear();

        foreach (var boid in Boids)
        {
            delta = boid.transform.position - boi.transform.position;
            dist = delta.magnitude;

            if (dist < closestDist)
            {
                closestDist = dist;
                ClosestBoid = boid;
            }

            if (dist < BoidSpawner.S.NearDist)
                Neighbors.Add(boid);

            if (dist < BoidSpawner.S.CollisionDist)
                CollisionRisk.Add(boid);
        }
        if (Neighbors.Count == 0)
        {
            Neighbors.Add(ClosestBoid);
        }

        return (Neighbors);
    }

    //Get the average position of the Boids in a List<Boid>
    public Vector3 GetAveragePosition(List<Boid> someBoids)
    {
        Vector3 sum = Vector3.zero;
        foreach (var boid in someBoids)
        {
            sum += boid.transform.position;
        }

        Vector3 centre = sum / someBoids.Count;
        return (centre);
    }

    // Get the Average velocity of the Boids in a List<Boid>
    public Vector3 GetAverageVelocity(List<Boid> someBoids)
    {
        Vector3 sum = Vector3.zero;
        foreach (var boid in someBoids)
        {
            sum += boid.Velocity;
        }

        Vector3 avgVector3 = sum / someBoids.Count;
        return (avgVector3);
    }
}
