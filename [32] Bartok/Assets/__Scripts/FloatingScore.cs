using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// An enum to track the possible states of a FloatingScore
// SXC: Divide the states of the floatingScore UI and manage them separately
public enum FSState
{
    idle,
    pre,
    active,
    post
}


// FloatingScore can move itself on screen following a Bezier Curve
public class FloatingScore : MonoBehaviour {
    public FSState state = FSState.idle;
    [SerializeField]
    private int _score = 0; // The score field
    public string scoreString;

    public Text fsText;

    void Awake()
    {
        fsText = gameObject.GetComponent<Text>();
        gameObject.transform.SetParent(FindObjectOfType<Canvas>().transform);
    }

    // The score property also sets scoreString when set
    public int score
    {
        get { return (_score); }
        set
        {
            _score = value;
            scoreString = Utils.AddCommasToNumber(_score);
            fsText.text = scoreString;
        }
    }

    public List<Vector3> bezierPts; // Bezier points for movement
    public List<float> fontSize; // Bezier points for font scaling
    public float timeStart = -1f;
    public float timeDuration = 1f;
    public string easingCurve = Easing.InOut; // Uses Easing in Utils.cs

    // The GameObject that will receive the SendMessage when this is done moving
    public GameObject reportFinishTo = null;

    // Setup the FloartingScore and movement
    // Note the use of parameter defaults for eTimeS & eTimeD
    // SXC: Initialize to deal with the Pre state
    public void Init(List<Vector3> bPts, float eTimeS = 0, float eTimeD = 1)
    {
        bezierPts = new List<Vector3>(bPts);

        if (bPts.Count == 1) // If there is only one point
        {
            // Then.. just go there
            transform.position = bPts[0];
            return;
        }

        // If eTimes is the default, just start at the current time
        if (eTimeS == 0)
        {
            eTimeS = Time.time;
        }
        timeStart = eTimeS;
        timeDuration = eTimeD;

        state = FSState.pre; // Set it to the pre state, ready to start moving
    }

    // SXC: Adding up the score of one Run
    public void FSCallback(FloatingScore fs)
    {
        // When this callback is called by Sendmessage,
        // add the score from the calling FloatingScore
        score += fs.score;
    }

    // Update is called once per frame
    // SXC: Update is used to deal with the active and post states
    void Update()
    {
        // If this is not moving, just return
        if(state == FSState.idle) return;
        
        // Get u from the current time and duration
        // u rages from 0 to 1 (usually)
        float u = (Time.time - timeStart) / timeDuration;

        // Use Easing class from utils to curve the u value
        float uC = Easing.Ease(u, easingCurve);
        if (u < 0) // if u <0, then we should not move yet
        {
            state = FSState.pre;
            // Set the position to the starting point
            transform.position = bezierPts[0];
        }
        else if(u >= 1) // It should be stopped
        {
            // Set the state to post, since it shall not be moving
            state = FSState.post;
            // Set uC = 1 so we dont overshoot
            uC = 1;

            // If the callback object exist, send the message to adding up the score
            if (reportFinishTo != null)
            {
                reportFinishTo.SendMessage("FSCallback", this); 
                // Call the callback method to add up the score

                // Destory this gameobject since the message is sent
                Destroy(gameObject);
            }
            else
            {
                // If there is nothing to callback
                // ... Then dont destroy this, just let it stay still
                state = FSState.idle;
            }
        }
        else
        {
            // if 0<= u <1, use the bezier pts and algorithm to update the position since it is moving
            state = FSState.active;

            // Use Bezier curve to move this to the right point
            Vector3 tPos = Utils.Bezier(uC, bezierPts);
            transform.position = tPos;

            // If font size has values in it
            if (fontSize.Count != 0)
            {
                // Then adjust the fontSize of this GUIText
                int tSize = Mathf.RoundToInt(Utils.Bezier(uC, fontSize));
                fsText.fontSize = tSize;
            }

        }
    }

}