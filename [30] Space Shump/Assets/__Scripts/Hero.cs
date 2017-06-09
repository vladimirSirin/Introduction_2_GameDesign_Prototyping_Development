using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    static public Hero S; // Singleton
    public float DelayRestart = 2.0f; // Set the time delay after hero died
    
    //  These fields control the movement of the help
    public float Speed = 30;

    public float PitchMult = 30;
    public float RollMult = -45;

    // ship status information
    [SerializeField]
    private float _shieldLevel = 1;

    public bool _______________________________________________;
    public Bounds Bounds;

    // Declare a delegate type, weaponFireDelegate
    public delegate void WeaponFireDelegate();

    // Create a WeaponFireDelegate instance of the delegate for hero to call/fire
    public WeaponFireDelegate FireDelegate;
    

    // Properties
    public float ShieldLevel
    {
        get { return _shieldLevel; }
        set
        {
            _shieldLevel = Mathf.Min(4, value);
            if (value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayGameRestart(DelayRestart);
            }
        }
    }

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

        // Keep the hero ship constrained to the screen bounds
	    Bounds.center = transform.position;
	    Vector3 off = Utils.ScreenBoundsCheck(Bounds, BoundsTest.onScreen);
	    if (off != Vector3.zero)
	    {
            pos -= off;
	        transform.position = pos;
	    }

        // When the player press the "jump" button, the hero will fire (which is call FireDelegate function)
	    float inputAxis = Input.GetAxis("Jump");
        // print("Input is" + inputAxis);
	    if (inputAxis == 1f && FireDelegate != null)
	    {
	        FireDelegate();
	    }

	}


    // The variable holds the object collide with the hero last time, make sure there is no duplication on hit
    private GameObject _lastTriggerGameObject = null;

    // On trigger Enter function
    void OnTriggerEnter(Collider other)
    {
        // Find the parent object with a tag of the collier
        GameObject parent = Utils.FindTaggedParent(other.gameObject);

        // If there is a tag, annouce the parent
        if (parent != null)
        {
            // Check if the collision go is the same as the last one
            if (parent == _lastTriggerGameObject)
            {
                return;
            }
            _lastTriggerGameObject = parent;

            // If it is the enemy. Decrease the sheild level of the hero and destroy the enemy
            if (parent.tag == "Enemy")
            {
                this.ShieldLevel--;
                Destroy(parent);
            }
            else
            {
                print("Triggered" + parent.name);
            }
        }

        //Otherwise annouce the original name
        else
        {
            print("Triggered" + other.gameObject.name);
        }
    }
}
