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

	    if (Input.GetKeyUp(KeyCode.Return))
	    {
	        SceneManager.LoadScene("_Scene_0");
	    }
		
	}
}
