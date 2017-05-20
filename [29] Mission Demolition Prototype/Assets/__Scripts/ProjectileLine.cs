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

    //public void AddPoint()
    //{
    //    // This is called to add a point to the line
    //    Vector3 pt = _poi.transform.position;
    //    if (Points.Count>0 && (pt - lastPoint).magnitude<minDist)
    //    {
    //        //
    //    }
    //}
}

