using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{

    // Prefab for instantiate apples
    public GameObject ApplePrefab;

    // Speed at which the AppleTree moves in meters/second
    public float Speed = 1f;

    // Distance where AppleTree turn around
    public float LeftAndRightEdge = 10f;

    // Chance that the AppleTree turn around
    public float ChanceToChangeDirection = 0.0f;

    // Rate at which Apples will be instantiated
    public float SecondsBetweenAppleDrops = 1f;


    // Use this for initialization
    void Start()
    {

        // Dropping Apples every second
        InvokeRepeating("DropApple", 2f, SecondsBetweenAppleDrops);

    }

    // Drop apple function
    void DropApple()
    {
        GameObject apple = Instantiate(ApplePrefab);
        apple.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        // Basic Movement
        Vector3 pos = this.transform.position;
        pos.x += Time.deltaTime * Speed;
        transform.position = pos;

        // Changing Direction
        if (this.transform.position.x < -LeftAndRightEdge)
        {
            Speed = Mathf.Abs(Speed);
        }
        else if (this.transform.position.x > LeftAndRightEdge)
        {
            Speed = -Mathf.Abs((Speed));
        }
    }

    void FixedUpdate()
    {
        // CHange direction randomly
        if (Random.value < ChanceToChangeDirection)
        {
            Speed *= -1; // Change direction
        }
    }
}
