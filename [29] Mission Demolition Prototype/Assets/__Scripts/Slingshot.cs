using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static public Slingshot S;

    // fields set in the unity Inspector pane
    public GameObject prefabProjectile;

    public float velocityMult = 4f;

    public bool ___________________________________;

    // fields set dynamically
    public GameObject launchPoint;

    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

	// Use this for initialization

    void Awake()
    {

        // Set the slingshot singleton S
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);

        launchPos = launchPointTrans.position;
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // If Slingshot is not in amingMode, don't run this code
        if(!aimingMode) return;

        // Get the current mouse position in 2d screen coordinates
	    Vector3 mousePos2d = Input.mousePosition;

        // Convert the mouse position to 3d world coordinates
	    mousePos2d.z = -Camera.main.transform.position.z;
	    Vector3 mousePos3d = Camera.main.ScreenToWorldPoint(mousePos2d);

        // Find the delta from the launchPos to the mousePos3D
	    Vector3 mouseDelta = mousePos3d - launchPos;

        // Limit mouseDelta to the radius of the Slingshot SphereCollider
	    float maxMagnitude = this.GetComponent<SphereCollider>().radius;
	    if (mouseDelta.magnitude > maxMagnitude)
	    {
	        mouseDelta.Normalize();
	        mouseDelta *= maxMagnitude;
	    }

        // MOve the projectile to this new position
	    Vector3 projPos = launchPos + mouseDelta;
	    projectile.transform.position = projPos;

	    if (Input.GetMouseButtonUp(0))
	    {
	        // The mouse has been released
	        aimingMode = false;
	        projectile.GetComponent<Rigidbody>().isKinematic = false;
	        projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMult;
	        FollowCam.S.poi = projectile;
	        projectile = null;
            MissionDemolition.ShotFired();
	    }
	}

    // 
    void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter");
        launchPoint.SetActive(true);
    }

    //
    void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit");
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        // The Player has pressed the mouse button while over slingshot
        aimingMode = true;

        // Instantiate a Projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        
        // Start it at the launchPoint
        projectile.transform.position = launchPos;

        // Set it to isKinemati for now
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
}
