using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{


    public float Speed = 10f; // the speed of the enemy in m/s
    public float FireRate = 0.3f; // seconds/shot
    public float Health = 10;
    public int Score = 100;

    public bool _________________________________;
    public Bounds Boundbox; // the bounds of the enemy and his children
    public Vector3 CentreoffVector3; // the dist of bounds.centre from position
    

    //
    void Awake()
    {
        InvokeRepeating("CheckOffscreen", 0f, 2.0f);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Move();
	}


    public virtual void Move()
    {
        Vector3 tempPos = Pos;
        tempPos.y -= Speed * Time.deltaTime;
        Pos = tempPos;
    }

    public Vector3 Pos
    {
        get { return this.transform.position; }
        set { this.transform.position = value; }
    }


    public void CheckOffscreen()
    {
        // Check if the Bounds params are zero
        if (Boundbox.size == Vector3.zero)
        {
            // If so, initialize the Boundbox with the utils functions
            Boundbox = Utils.CombineBoundsOfChildren(this.gameObject);
            // Calculate the offset of the position and Boundbox centre
            CentreoffVector3 = Boundbox.center - this.transform.position;
        }

        // Caliberate the offset
        Boundbox.center = CentreoffVector3 + this.transform.position;

        // Check if this is off screen
        Vector3 off = Utils.ScreenBoundsCheck(Boundbox, BoundsTest.offScreen);

        // if so, destroy it.
        if (off != Vector3.zero)
        {
            if (off.y < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}

