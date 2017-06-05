using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    static public Main S;

    public GameObject[] PrefabEnemies; // the array of the enemy prefabs

    public float EnemySpawnPerSecond = 0.5f;
    public float EnemySpawnPadding = 1.5f;
    public WeaponDefinition[] WeaponDefinitions;
    public WeaponType[] ActiveWeaponTypes;

    public bool _____________________________________;

    public float EnemySpawnRate; // Time between two enemy spawns

    // Awake
    void Awake()
    {
        S = this;

        // Initiliaze the camera bounds
        Utils.SetCameraBounds(this.GetComponent<Camera>());

        // Calculate the enemy spawn rate
        EnemySpawnRate = 1.0f / EnemySpawnPerSecond;

        // Invoke the spawn Enemy function to spawn
        Invoke("SpawnEnemy", EnemySpawnRate);


    }

	// Use this for initialization
	void Start () {
		
        ActiveWeaponTypes = new WeaponType[WeaponDefinitions.Length];
	    for (int i = 0; i < WeaponDefinitions.Length; i++)
	    {
	        ActiveWeaponTypes[i] = WeaponDefinitions[i].type;
	    }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // The spawn enemy function
    public void SpawnEnemy()
    {
        // Choose a random enemy type from the prefab array
        int idx = Random.Range(0, PrefabEnemies.Length);

        // Instantiate the Game Object
        GameObject go = Instantiate(PrefabEnemies[idx]);

        // Randomize the x position, and set the y,z position to be the max point of the camera boundbox
        Vector3 posVector3 = go.transform.position;
        posVector3.x = Random.Range(Utils.CamBounds.min.x + EnemySpawnPadding, Utils.CamBounds.max.x - EnemySpawnPadding);
        posVector3.y = Utils.CamBounds.max.y + EnemySpawnPadding;

        go.transform.position = posVector3;

        // Invoke another spawn
        Invoke("SpawnEnemy", EnemySpawnRate);

    }

    // The restart game function
    public void DelayGameRestart(float timeOfDelay)
    {
        // invoke the restart function
        Invoke("Restart", timeOfDelay);
    }

    public void Restart()
    {
        // Reload the level to restart
        SceneManager.LoadScene("__Scene_0");
    }
}
