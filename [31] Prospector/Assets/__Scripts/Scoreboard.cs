using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    public static Scoreboard S; // The Singleton for scoreboard
    public string _scoreString;
    private int _score;

    public GameObject prefabFloatingScore;

    public Text sbText;

    // the Scoreboard manages showing the score to the player
    // Define the properties of score and scoreString
    public int score
    {
        get { return _score; }
        set
        {
            _score = value;
            scoreString = Utils.AddCommasToNumber(value);
        }
    }

    public string scoreString
    {
        get { return _scoreString; }
        set
        {
            _scoreString = value;
           sbText.text = value;
        }
    }

    // When awake, initialize the singleton
    void Awake()
    {
        S = this;
        sbText = gameObject.GetComponent<Text>();
    }

    // When called by message, this adds the fs.score to this.score
    public void FSCallback(FloatingScore fs)
    {
        score += fs.score;
    }

    // This function instantiate a new FloatingScore gameobject and intialize it
    // it also returns a pointer to the FloatingScore create so that the
    // Calling function can do more with it (like, set fontSizes, etc)
    public FloatingScore CreateFloatingScore(int amt, List<Vector3> bezierPts)
    {
        GameObject tGo = Instantiate(prefabFloatingScore);
        FloatingScore fs = tGo.GetComponent<FloatingScore>();

        fs.Init(bezierPts);
        fs.score = amt;

        fs.reportFinishTo = this.gameObject; // set fs callback to this script
        return fs;
    }
}
