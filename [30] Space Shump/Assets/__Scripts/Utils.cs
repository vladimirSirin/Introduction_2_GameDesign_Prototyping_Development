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

}
