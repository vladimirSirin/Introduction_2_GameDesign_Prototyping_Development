using System.Collections;
using System.Collections.Generic;
using UnityEngine;





// Enemy_1 extends the Enemy Class
public class Enemy_1 : Enemy {
    // Because Enemy_1 extends Enemy, the ______ bool won't work
    // the same way in the Inspector Pane. :/


    // # seconds for a full sine wave
    public float waveFrequency = 2f;

    // # sine wave width in meters
    public float waveWidth = 4f;

    public float waveRotY = 45;

    private float _x0 = -12345; // The initial x value of position
    private float _birthTime;



	// Use this for initialization
	void Start ()
    {
        //Set _x0 to the initial x position of Enemy_1
        // This works fine because the position will already
        // been set by Main.SpawnEnemy() before Start() runs
        // (though Awake() would have been too early!).
        // This is also good because there is no Start() method
        // on Enemy
        _x0 = Pos.x;
        _birthTime = Time.time;

    }
	


    // Override the Move function on Enemy
    public override void Move()
    {
        // Because pos is a property, you can't directly set pos.x
        // so get the pos as an editable Vector3
        Vector3 tempPos = Pos;
        // theta adjusts based on time
        float age = Time.time - _birthTime;
        float theta = Mathf.PI * 2 * age / waveFrequency;
        tempPos.x = _x0 + waveWidth * Mathf.Sin(theta);
        Pos = tempPos;

        // rotate a bit about y
        Vector3 rotVector3 = new Vector3(0, Mathf.Sin(theta) * waveRotY, 0);
        transform.rotation = Quaternion.Euler(rotVector3);


        // base.Move() still handles the movement down in y
        base.Move();
    }
}
