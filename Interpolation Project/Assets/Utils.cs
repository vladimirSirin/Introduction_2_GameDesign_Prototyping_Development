using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{

    // The standard Vector Lerp functions in Unity don't allow for extrapolation
    // (i.e., u is clamped to 0<=u<=1, so we write our own Lerp function

    static public Vector3 Lerp(Vector3 vFrom, Vector3 vTo, float u)
    {
        Vector3 res = (1 - u) * vFrom + u * vTo;
        return res;
    }


    // While most Bezier curves are 3 or 4 points, it is possible to have any number of points using this recursive function
    // This use the Lerp function above because the Vector3.Lerp function
    // doesn't allow extrapolation
    static public Vector3 Bezier(float u, List<Vector3> vList)
    {
        // If there is only one element in vList, return it
        if (vList.Count == 1)
        {
            return vList[0];
        }


        // Create vListR, which is all but the 0th element of vList
        // e.g., if vList = [0,1,2,3,4], vListR = [1,2,3,4]
        List<Vector3> vListR = vList.GetRange(1, vList.Count - 1);


        // And create vListL, which is all but the last element  of vList
        //e.g., if vList = [0,1,2,3,4], then vListL = [0,1,2,3]
        List<Vector3> vListL = vList.GetRange(0, vList.Count - 1);

        // The result is the Lerp of the Bezier of these two shorter Lists
        Vector3 res = Lerp(Bezier(u, vListL), Bezier(u, vListR), u);

        // ^ The Bezier function recursively calls itself here to split the
        // lists down until there is only one value in each
        return res;

    }

    // This version allow an array of a series of Vector3s input which is then converted into a list<Vector3>
    static public Vector3 Bezier(float u, params Vector3[] vecs)
    {
        return Bezier(u, new List<Vector3>(vecs));
    }

}
