using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

// This is actually outside the Utils class
public enum BoundsTest
{
    centre, //is the centre of the gameOBject on screen?
    onScreen, //Are the bounds entirely on screen?
    offScreen, // Are the bounds entirely off screen?
}


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
            b = BoundsUnion(b, go.GetComponent<Renderer>().bounds);
        // If this GameObject has a Collider Component...
        if (go.GetComponent<Collider>() != null)
            // Expand b to contain the Collider's Bounds
            b = BoundsUnion(b, go.GetComponent<Collider>().bounds);
        // Recursively iterate through each child of this GameObject.transform
        foreach (Transform t in go.transform)
            // Expand b to contain their bounds as well
            b = BoundsUnion(b, CombineBoundsOfChildren(t.gameObject));

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

    // Checks to see whether the bounds bnd are within the camera bounds
    public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest test = BoundsTest.centre)
    {
        return BoundsInBoundsCheck(CamBounds, bnd, test);
    }


    // Checks to see whether Bounds lilB are within Bound bigB
    public static Vector3 BoundsInBoundsCheck(Bounds bigB, Bounds lilB, BoundsTest test = BoundsTest.onScreen)
    {
        // The behaviour of the function will change based on the BoundsTest

        // Get the centre of the lilB
        Vector3 posLilBCenter = lilB.center;

        // Initialize the offset
        Vector3 off = Vector3.zero;

        // Switch between three cases, is centre within the bigB? is all of lilB within? is any of lilB within?
        switch (test)
        {
                case BoundsTest.centre:
                    // The centre test determine what off would have to be applied to lilB to move the its center back into BigB
                    if (bigB.Contains(posLilBCenter))
                    {
                        return Vector3.zero;
                    }

                    if (posLilBCenter.x > bigB.max.x)
                    {
                        off.x = posLilBCenter.x - bigB.max.x;
                    }
                    else if (posLilBCenter.x < bigB.min.x)
                    {
                        off.x = posLilBCenter.x - bigB.min.x;
                    }

                    if (posLilBCenter.y > bigB.max.y)
                    {
                        off.y = posLilBCenter.y - bigB.max.y;
                    }
                    else if (posLilBCenter.y < bigB.min.y)
                    {
                        off.y = posLilBCenter.y - bigB.min.y;
                    }

                    if (posLilBCenter.z > bigB.max.z)
                    {
                        off.z = posLilBCenter.z - bigB.max.z;
                    }
                    else if (posLilBCenter.z < bigB.min.z)
                    {
                        off.z = posLilBCenter.z - bigB.min.z;
                    }

                    return off;



                case BoundsTest.onScreen:
                    // The on screen test determine what off would have to be applied to lilB to keep all of lilB inside bigB
                    if (bigB.Contains(lilB.max) && bigB.Contains(lilB.min))
                    {
                        return Vector3.zero;
                    }

                    if (lilB.max.x > bigB.max.x)
                    {
                        off.x = lilB.max.x - bigB.max.x;
                    }
                    else if (lilB.min.x < bigB.min.x)
                    {
                        off.x = lilB.min.x - bigB.min.x;
                    }

                    if (lilB.max.y > bigB.max.y)
                    {
                        off.y = lilB.max.y - bigB.max.y;
                    }
                    else if (lilB.min.y < bigB.min.y)
                    {
                        off.y = lilB.min.y - bigB.min.y;
                    }

                    if (lilB.max.z > bigB.max.z)
                    {
                        off.z = lilB.max.z - bigB.max.z;
                    }
                    else if (lilB.min.z < bigB.min.z)
                    {
                        off.z = lilB.min.z - bigB.min.z;
                    }

                    return off;

                case BoundsTest.offScreen:
                // the off screen test deternmine what off would need to be applied to move any tiny part of lilB inside of bigB
                    if (bigB.Contains(lilB.max) || bigB.Contains(lilB.min))
                    {
                        return Vector3.zero;
                    }

                    if (lilB.min.x > bigB.max.x)
                    {
                        off.x = lilB.min.x - bigB.max.x;
                    }
                    else if (lilB.max.x < bigB.min.x)
                    {
                        off.x = lilB.max.x - bigB.min.x;
                    }

                    if (lilB.min.y > bigB.max.y)
                    {
                        off.y = lilB.min.y - bigB.max.y;
                    }
                    else if (lilB.max.y < bigB.min.y)
                    {
                        off.y = lilB.max.y - bigB.min.y;
                    }

                    if (lilB.min.z > bigB.max.z)
                    {
                        off.z = lilB.min.z - bigB.max.z;
                    }
                    else if (lilB.max.z < bigB.min.z)
                    {
                        off.z = lilB.max.z - bigB.min.z;
                    }

                    return off;
        }
        return (Vector3.zero);
    }

    //====================================================== Transform Functions ==================================================\\
    
    // This function will iteratively climb up on the transfrom parent tree
    // unitl it either find a object with a tag != untagged, or there is no parent to be found
    public static GameObject FindTaggedParent(GameObject go)
    {
        // If this gameobject has a tag, return the go
        if (go.tag != "Untagged")
        {
            return go;
        }

        // If there is no  parent anymore
        if (go.transform.parent == null)
        {
            return null;
        }

        // keep iterate the parents
        return FindTaggedParent(go.transform.parent.gameObject);
    }

    // The version of the function handles a transform instead of a game object
    public static GameObject FindTaggedParent(Transform t)
    {
        return FindTaggedParent(t.gameObject);
    }




    //======================================================== Materials Functions ==================================================\\

    // return a list of materials on this GameObject or its children
    public static Material[] GetAllMaterial(GameObject go)
    {
        List<Material> mats = new List<Material>();
        if (go.GetComponent<Renderer>() != null)
        {
            mats.Add(go.GetComponent<Renderer>().material);
        }

        foreach (Transform t in go.transform) // Every transform can have parent or child, you can loop the transform in this way
        {
            mats.AddRange(GetAllMaterial(t.gameObject));
        }
        return mats.ToArray();
    }
}
