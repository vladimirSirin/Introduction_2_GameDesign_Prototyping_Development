﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Apple : MonoBehaviour
{

    public static float BottomY = -20f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	    if (this.gameObject.transform.position.y < BottomY)
	    {
	        Destroy(this.gameObject);

	        Scene scene = SceneManager.GetActiveScene();
	        if (scene.name != "_Scene_Start" && scene.name != "_Scene_Over")
	        {
                // Get the reference from the main camera with the ApplePicker script
                ApplePicker apPicker = Camera.main.GetComponent<ApplePicker>();

                // Call the AppleDestroyed() function of the ApplePicker script
                apPicker.AppleDestroyed();
            }

	    }

	}
}
