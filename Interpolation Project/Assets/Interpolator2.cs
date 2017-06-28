using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum EasingType
{
    Linear,
    EaseIn,
    EaseOut,
    EaseInOut,
    Sin,
    SinIn,
    SinOut
}

public class Interpolator2 : MonoBehaviour
{


    public Transform c0, c1;
    public float timeDuration = 1;
    public EasingType easingType = EasingType.Linear;
    public float easingMod = 2;
    public bool loopMove = true; // Causes the move to repeat



    // ExtraInterpolation
    public float uMin = 0;
    public float uMax = 1;

    // Set checkToCalculate to sart moving
    public bool checkTocalculate = false;

    public bool moving = false;
    public float timeStart;

    public bool ______________________________;

    public Vector3 p01;
    public Quaternion r01;
    public Vector3 s01;
    public Color c01;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	    if (checkTocalculate)
	    {
	        checkTocalculate = false;

	        moving = true;
	        timeStart = Time.time;
	    }

	    if (moving)
	    {
	        float u = (Time.time - timeStart) / timeDuration;

	        if (u>=1)
	        {
                // Use LoopMove to make the interpolation starts again when it ends
                if (loopMove)
	            {
	                timeStart = Time.time;
	            }
	            else
	            {
	                moving = false;
	            }
	            u = 1;
	        }

            // Adjust u to the range from uMIn to uMax
	        u = (1 - u) * uMin + u * uMax;

            // Use the Easing function to adjust the [u]


            // This is the standard linear interpolation function
	        float v = 1 - u;
            p01 = v * c0.position + u * c1.position;
	        r01 = Quaternion.Slerp(c0.rotation, c1.rotation, u);
	        s01 = v * c0.localScale + u * c1.localScale;
	        c01 = v * c0.GetComponent<Renderer>().material.color + u * c1.GetComponent<Renderer>().material.color;

            // Apply those transforms.
	        transform.position = p01;
	        transform.rotation = r01;
	        transform.localScale = s01;
	        transform.GetComponent<Renderer>().material.color = c01;
	    }
		
	}

    public float EaseU(float u, EasingType eType, float eMod)
    {
        float u2 = u;

        // Switch based on the Easing type of the interpolations
        switch (eType)
        {
                case EasingType.Linear:
                    u2 = u;
                break;

                case EasingType.EaseIn:
                    u2 = Mathf.Pow(u, eMod);
                break;

                case EasingType.EaseOut:
                    u2 = 1 - Mathf.Pow(1 - u, eMod);
                break;

                case EasingType.EaseInOut:
                    if (u <= 0.5f)
                    {
                        u2 = 0.5f * Mathf.Pow(u * 2, eMod);
                    }
                    else
                    {
                        u2 = 0.5f + 0.5f * (1 - Mathf.Pow(1 - (2 * (u - 0.5f)), eMod));
                    }
                break;

                case EasingType.Sin:
                // Try eMode valus of 0.16f and -0.2f for EasingType.Sin
                    u2 = u + eMod * Mathf.Sin(2 * Mathf.PI * u);
                break;

                case EasingType.SinIn:
                // eMod is ignored for SinIn
                    u2 = 1 - Mathf.Cos(0.5f * u * Mathf.PI);
                break;

                case EasingType.SinOut:
                // eMod is Ignored
                    u2 = Mathf.Sin(0.5f * u * Mathf.PI);
                break;      
        }
        return u2;
    }
}
