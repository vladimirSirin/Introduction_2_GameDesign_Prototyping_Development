using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Shield : MonoBehaviour
{

    public float RotationsPerSeconds = 0.1f;
    public bool _________________________;
    public int LevelShown = 0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        // Read the current shield level from the Hero Singleton
	    int currLevel = Mathf.FloorToInt(Hero.S.ShieldLevel);

	    // IF it is different from levelSHown
	    if (currLevel != LevelShown)
	    {
	        LevelShown = currLevel;
	        // Adjust the Texture offset to show different shield levels
	        Material mat = this.GetComponent<Renderer>().material;
	        mat.mainTextureOffset = new Vector2(0.2f * LevelShown, 0);
        }



	    // Rotate the Shield a bit every second
	    this.transform.rotation = Quaternion.Euler(0, 0, ((0.1f * 360 * Time.time) % 360f));

	}
}
