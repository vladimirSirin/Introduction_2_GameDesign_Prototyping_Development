using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Game : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyUp(KeyCode.Mouse0))
	    {
	        SceneManager.LoadScene("_Scene_0");
	    }

        if (Input.touchCount > 0)
        {
            SceneManager.LoadScene("_Scene_0");
        }

    }
}
