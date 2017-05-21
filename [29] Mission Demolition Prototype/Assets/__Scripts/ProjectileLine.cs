using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.VR.WSA.Persistence;

public class ProjectileLine : MonoBehaviour
{

    static public ProjectileLine S;

    // Fields set in the Unity Inspector Pane
    public float minDist = 0.1f;
    public bool __________________________________;

    // Fields set dynamically
    public LineRenderer Line;

    private GameObject _poi;
    public List<Vector3> Points;

    void Awake()
    {
        // Set the singleton
        S = this;

        // Get a reference to the LineRender
        Line = this.GetComponent<LineRenderer>();

        // Disable the LineRenderer until it is needed
        Line.enabled = false;

        // Initialize the points list
        Points = new List<Vector3>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // This is a property that is, a method masquerading as a field
    public GameObject poi
    {
        get { return (_poi); }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                // When POI is set to something new, it resets everything
                Line.enabled = false;
                Points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    // This can be used to clear the line directly
    public void Clear()
    {
        _poi = null;
        Line.enabled = false;
        Points = new List<Vector3>();
    }

    public void AddPoint()
    {
        // This is called to add a point to the line
        Vector3 pt = _poi.transform.position;
        if (Points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            // If the point isn't far enough from the last point, it returns
            return;
        }

        if (Points.Count == 0)
        {
            // If this is the launch point
            Vector3 launchPos = Slingshot.S.launchPoint.transform.position;
            Vector3 launchPosDiff = pt - launchPos;

            // It adds an extra bit of line to aid aiming later
            Points.Add(pt + launchPosDiff);
            Points.Add(pt);
            Line.positionCount = 2;

            // Sets the first two points
            Line.SetPosition(0, Points[0]);
            Line.SetPosition(1, Points[1]);

            // Enable the LineRenderer
            Line.enabled = true;
        }
        else
        {
            // Normal Behavior of adding a point
            Points.Add(pt);
            Line.positionCount = Points.Count;
            Line.SetPosition(Points.Count - 1, lastPoint);
            Line.enabled = true;


        }
    }

    // Returns the location of the most recently added point
    public Vector3 lastPoint
    {
        get
        {
            if (Points == null)
            {
                // If there are no points, returns Vector3.zero
                return (Vector3.zero);
            }
            return (Points[Points.Count - 1]);
        }
    }

    void FixedUpdate()
    {
        if (poi == null)
        {
            // If there is no poi, search for one
            if (FollowCam.S.poi != null)
            {
                if (FollowCam.S.poi.tag == "Projectile")
                {
                    poi = FollowCam.S.poi;
                }
                else
                    return; // Return if we didnt find a poi
            }
            else
            {
                 return; // Return if we didnt find a poi
            }

        }

        // If there is a poi, it's loc is added every fixed Update
        AddPoint();
        if (poi.GetComponent<Rigidbody>().IsSleeping())
        {
            // Once the poi is sleeping, it is cleared
            poi = null;
        }
    }
}

