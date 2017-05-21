using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

// enum
public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{

    static public MissionDemolition S; // a Singleton

    // Fields set in the Unity Inspector Pane
    public GameObject[] Castles; // the array of castles

    public Text GtLevel; // the gt_level Text
    public Text GtScore; // the gt_score text
    public Vector3 CastlePos; // the Place to put the castle

    public bool ______________________________________;

    // Fields set dynamically
    public int Level; // The current level

    public int LevelMax; // The number of level
    public int ShotsTaken; // the shots taken
    public GameObject Castle; // the current castle
    public GameMode Mode = GameMode.idle;
    public string Showing = "Slingshot"; // Follow camera mode

	// Use this for initialization
	void Start () {

        // Define the singleton
	    S = this;
	    Level = 0;
	    LevelMax = Castles.Length;

	    StartLevel();

	}
	
	// Update is called once per frame
	void Update () {

        // Update the Text every frame
        ShowGT();

        // check for level end
	    if (Mode == GameMode.playing && Goal.GoalMet == true)
	    {
	        // Change Mode to stop checking for level end
            Mode = GameMode.levelEnd;
            // Zoom Out
            SwitchView("Both");
            // Start the next level in 2 seconds
            Invoke("NextLevel", 2f);
	    }
	}

    // Define the function of Startlevel
    void StartLevel()
    {
        // Get rid of the old castle if one exists
        if (Castle != null)
        {
            Destroy(Castle);
        }

        // Destroy old projectiles if they exist
        GameObject[] projectileArray = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (var pro in projectileArray)
        {
            Destroy(pro);
        }

        // Instantiate the new castle
        Castle = Instantiate(Castles[Level]) as GameObject;
        Castle.transform.position = CastlePos;
        ShotsTaken = 0;


        // Reset the camera
        SwitchView("Both");
        ProjectileLine.S.Clear();

        // Reset the Goal
        Goal.GoalMet = false;
        ShowGT();

        Mode = GameMode.playing;
    }

    // Define the ShowGT function
    private void ShowGT()
    {
        // Show the data in the GUITexts
        GtLevel.text = "Level: " + (Level + 1) + "of" + LevelMax;
        GtScore.text = "ShotsTake: " + ShotsTaken;
    }

    // Define NExtLevel
    void NextLevel()
    {
        Level++;
        if (Level == LevelMax)
        {
            Level = 0;
        }
        StartLevel();
    }

    // Define OnGUI
    void OnGUI()
    {
        // Draw the GUI button for view switching at the top of the screen
        Rect buttonRect = new Rect((Screen.width / 2)-50,10,100,24);

        // Use switch to change the GUI.Button in, Slingshot, Castle and BOth situations.
        switch (Showing)
        {
            case "Slingshot":
                if (GUI.Button(buttonRect, "Show Castle"))
                {
                    SwitchView("Castle");
                }
                break;

            case "Castle":
                if (GUI.Button(buttonRect, "Show Both"))
                {
                    SwitchView("Both");
                }
                break;

            case "Both":
                if(GUI.Button(buttonRect, "Slingshot"))
                    SwitchView("Slingshot");
                break;

        }
    }

    // Static method that allows code anywhere to request a view change
    static public void SwitchView(String eView)
    {
        // Set the showing string to the new view
        S.Showing = eView;


        // Switch the camera
        switch (S.Showing)
        {
            case "Slingshot":
                FollowCam.S.poi = GameObject.Find("Slingshot");
                break;
            case "Both":
                FollowCam.S.poi = GameObject.Find("ViewBoth");
                break;
            case "Castle":
                FollowCam.S.poi = S.Castle;
                break;
                
        }
    }

    // Static Method that allows code anywhere to increment shotsTaken
    public static void ShotFired()

    {
        S.ShotsTaken++;
    }
}
