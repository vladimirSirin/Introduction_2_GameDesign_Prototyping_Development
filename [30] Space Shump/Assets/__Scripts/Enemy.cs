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
}

