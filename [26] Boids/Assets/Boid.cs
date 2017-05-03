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
    // 1: The Awake() function is called by Unity at the moment that this GameObject is instantiated.
    // This means that it is called beofre Start()
    void Awake()
    {
        // Define the Boids list if it is still null
        if (Boids == null)
        {
            Boids = new List<Boid>();
        }
        // ^2: All instances of the Boid class can access the shared static List<Boid> Boid.
        // The first Boid to be created will initialize a new List<Boid>, and then the others
        // will just add themselves to the list

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
        // ^3: Each Boid maintains its own list of neighbors and collision risks.
        // The neighbors are other Boids that are within BoidSpawner.S.nearDist of this Boid
        // The collision risks are those that are within BoidSpawner.S.collisionDist

        // Make this.tranform a child of the Boids GameObjetc
        this.transform.parent = GameObject.Find("Boids").transform;
        //^4: Making all the Boids children of the same GameObject will help keep the Hierarchy pane organized
        // It places them all underneath a single parent GameObject named Bolds.
        // If you want to see them all listed in the hierarchy, you just need to click the disclosure triangle
        // next to the parent object Boids.

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
    // 5: Every Update(), each Boid needs to find its neighbors and react to their locations using
    // collision avoidance, velocity matching and flock centering.
	void Update () {
		
        // Get the list of nearby Boids (this Boid's neighbors)
	    List<Boid> Neighbors = GetNeighbors(this);
        // ^6: GetNeighbors function is defined later in the code, it finds other Boids that are nearby

        // Initialize newVelocity and newPosition to the current values
	    NewVelocity = Velocity;
	    NewPosition = this.transform.position;

        // Velocity matching: this sets the velocity of the Boid to be similar to that of its neighbors
	    Vector3 neighborVel = GetAverageVelocity(Neighbors);
        // Utilizes the fields set on the Boidspawner.S singleton
	    NewVelocity = neighborVel * BoidSpawner.S.VelocityMatchingAmt;

        // Flock Centering: Move toward middle of neighbors
	    Vector3 neighborCenterOffset = GetAveragePosition(Neighbors) - this.transform.position;
	    NewVelocity += neighborCenterOffset * BoidSpawner.S.FlockCenteringAmt;

        // Collision avoidance: Avoid running into Boids that are too close
	    Vector3 dist;
	    if (CollisionRisk.Count > 0)
	    {
	        dist = GetAveragePosition(CollisionRisk) - this.transform.position;
	        NewVelocity += dist * BoidSpawner.S.CollisionAvoidanceAmt;
	    }

        // Mouse attraction, move toward the mouse position no matter how far away
	    dist = BoidSpawner.S.MousePos - this.transform.position;
	    if (dist.magnitude > BoidSpawner.S.MouseAvoidanceDist)
	    {
	        NewVelocity += dist * BoidSpawner.S.MouseAttractionAmt;
	    }

        // Avoid the mouse if it is too close
	    else
	    {
	        NewVelocity += dist.normalized * BoidSpawner.S.MouseAvoidanceDist * BoidSpawner.S.MouseAvoidanceAmt;
	    }

        // newVelocity and newPosition are ready, but wait unitl LateUpdate() 
        // to set them so that this Boid does not move before others have 
        // had a chance to calculate their new value
	}

    // By allowing all Boids to Update() themselves before any Boids move, we avoid
    // race conditions that could be caused by some Boids moving before others have decided where to go

    // 7: LateUpdate is called after Update has been called on every object

    void LateUpdate()
    {
        // Adjust the current velocity based on newVelocity using a linear interpolation ( see Appendix B, "Useful Concept")
        Velocity = (1 - BoidSpawner.S.VelocityLerpAmt) * Velocity + BoidSpawner.S.VelocityLerpAmt * NewVelocity;

        // Make sure the velocity is in the max and min range of it
        if (Velocity.magnitude > BoidSpawner.S.MaxVelocity)
        {
            Velocity = Velocity.normalized * BoidSpawner.S.MaxVelocity;
        }
        if (Velocity.magnitude < BoidSpawner.S.MinVelocity)
        {
            Velocity = Velocity.normalized * BoidSpawner.S.MinVelocity;
        }

        // Decide on the new Position
        NewPosition = this.transform.position + Time.deltaTime * Velocity;

        // Keep everything in the xz plane
        NewPosition.y = 0;

        // Look from the old position at the newPosition to orient the model
        this.transform.LookAt(NewPosition);

        // Actually move to the newPosition
        this.transform.position = NewPosition;
    }

    // Find which  Boids are near enough to be considered neighbors
    // boi is Boid of interest, the Boid on which we are focusing

    // 8: GetNeighbors will find the nearby Boids around the boi

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

    // Get the average position of the Boids in a List<Boid>
    // 9: GetAveragePosition will return the average position of a group of Boids

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
    // 10: GetAverageVelocity will return the average velocity Vector3 of a group of Boids
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
