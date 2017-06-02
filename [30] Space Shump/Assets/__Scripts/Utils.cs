using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    // ================================== Bounds Functions ======================================\\

    // Creates bounds that encapsulate the two Bounds passed in.
    public static Bounds BoundsUnion(Bounds b0, Bounds b1)
    {
        // If the size of one of the bounds is vector3.zero , ignore that one
        if (b0.size == Vector3.zero && b1.size != Vector3.zero)
            return (b1);
        else if (b0.size != Vector3.zero && b1.size == Vector3.zero)
            return (b0);
        else if (b0.size == Vector3.zero && b1.size == Vector3.zero)
            return (b0);

        // Stretch b0 to include the b1.min and b1.max
        b0.Encapsulate(b1.min);
        b0.Encapsulate(b1.max);
        return (b0);
    }

    public static Bounds CombineBoundsOfChildren(GameObject go)
    {
        // Create a empty Bounds b
        Bounds b = new Bounds(Vector3.zero, Vector3.zero);
        // If this GameObject has a Renderer Component...
        if (go.GetComponent<Renderer>() != null)
            // Expand b to contain the Renderer Component
            BoundsUnion(b, go.GetComponent<Renderer>().bounds);
        // If this GameObject has a Collider Component...
        if (go.GetComponent<Collider>() != null)
            // Expand b to contain the Collider's Bounds
            BoundsUnion(b, go.GetComponent<Collider>().bounds);
        // Recursively iterate through each child of this GameObject.transform
        foreach (Transform t in go.transform)
            // Expand b to contain their bounds as well
            BoundsUnion(b, CombineBoundsOfChildren(t.gameObject));

        return (b);
    }

    // Make a static read-only public property camBounds
    static public Bounds CamBounds
    {
        get
        {
            // if _camBounds has not been set yet
            if (_camBounds.size == Vector3.zero)
            {
                // Set _cambounds using the default camera
                SetCameraBounds();
            }
            return (_camBounds);
        }
    }

    // This is the private field that camBounds use
    static private Bounds _camBounds;

    // This function is used by camBounds to set _camBounds and can also be called directly
    static public void SetCameraBounds(Camera cam = null)
    {
        // If there is no camera passed in, use the main camera
        if (cam == null)
        {
            cam = Camera.main;
        }
        // base on: the camera has no rotation, it is orthographic

        // Set the vector3 of the Top Left point and Bottom Right point based on the screen size
        Vector3 topleftVector3 = new Vector3(0, 0, 0);
        Vector3 botRightVector3 = new Vector3(Screen.width, Screen.height, 0);

        // Convert these into world position and size.
        Vector3 boundTLN = cam.ScreenToWorldPoint(topleftVector3);
        Vector3 boundBRN = cam.ScreenToWorldPoint(botRightVector3);

        // Clip and set the Z value of the position, (near and far of the camera clipping plane)
        boundTLN.z += cam.nearClipPlane;
        boundBRN.z += cam.farClipPlane;

        // Set the centre of the bounds
        Vector3 centre = (boundBRN + boundTLN) / 2;
        _camBounds = new Bounds(centre, Vector3.zero);

        // Encapsulates the Top left and Bottom right, thus the whole boundry
        _camBounds.Encapsulate(boundBRN);
        _camBounds.Encapsulate(boundTLN);
    }


}
