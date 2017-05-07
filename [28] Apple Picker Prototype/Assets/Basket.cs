using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour {

    public Text ScoreText;


    // Use this for initialization
    void Start ()
	{
        // Get the reference from the text object
        GameObject scoreCounter = GameObject.Find("ScoreCounter");
        
        // Initialize it to 0 at start
	    ScoreText = scoreCounter.GetComponent<Text>();
	    ScoreText.text = "0";




	}
	
	// Update is called once per frame
	void Update () {

        // Get the current position of the mouse in 2d
	    Vector3 mousePosition = Input.mousePosition;

	    // Get and set the z value of the mouse which is how far to push it in 3d
	    mousePosition.z = -Camera.main.transform.position.z;

	    // Transform the 2d mouse position into a 3d one
	    Vector3 mousePos3d = Camera.main.ScreenToWorldPoint(mousePosition);

	    // Set the basket position x to the 3d mouse x position
	    Vector3 basketPosVector3 = this.gameObject.transform.position;
	    basketPosVector3.x = mousePos3d.x;
	    this.gameObject.transform.position = basketPosVector3;

	}

    // Collision Check with the Apple 
    void OnCollisionEnter(Collision coll)
    {
        // get the object that collide with the Basket
        GameObject collGameObject = coll.gameObject;

        // Check if it is the apple, if so, destroy it
        if (collGameObject.tag == "Apple")
        {
            Destroy(collGameObject);
        }

        // Parse the text from the scoreCounter
        int newScore = Int32.Parse(ScoreText.text);
        newScore += 100;
        ScoreText.text = newScore.ToString();

        // if the current score is higher than old one ,replace it
        if (HighScore.Score < newScore)
            HighScore.Score = newScore;
    }
}
