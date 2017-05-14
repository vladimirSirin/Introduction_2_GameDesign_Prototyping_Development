using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Over : MonoBehaviour
{

    public Text FinalScoreText;

	// Use this for initialization
	void Start () {

        // Get the reference from the score game object
        GameObject YourScore = GameObject.Find("YourScore");

        // Set it to the score player get in the game
	    FinalScoreText = YourScore.GetComponent<Text>();
	    FinalScoreText.text = "Your Score: " + Basket.NewScore;

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            SceneManager.LoadScene("_Scene_0");
        }

	    if (Input.touchCount > 1)
	    {
	        SceneManager.LoadScene("_Scene_0");
        }

    }
}
