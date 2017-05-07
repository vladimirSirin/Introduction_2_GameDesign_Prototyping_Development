using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{

    public static int Score = 1000;

    // Initialize the score from the PlayerPrefs if it exists
    void Awake()
    {
        if (PlayerPrefs.HasKey("ApplePickerHighScore"))
        {
            Score = PlayerPrefs.GetInt("ApplePickerHighScore");
        }

        // Set the high score to ApplePickerHighScore
        // Can create the ApplePickerHighScore key is created if not.
        PlayerPrefs.SetInt("ApplePickerHighScore", Score);
    }


	// Use this for initialization
	void Start ()
	{



	}
	
	// Update is called once per frame
	void Update () {

        Text gt = this.GetComponent<Text>();
        gt.text = "HighScore: " + Score;

        // Update the high score in the PlayerPrefs if current high score is higher
        if (Score > PlayerPrefs.GetInt("ApplePickerHighScore")) ;
	    {
	        PlayerPrefs.SetInt("ApplePickerHighScore", Score);
	    }

	}
}
