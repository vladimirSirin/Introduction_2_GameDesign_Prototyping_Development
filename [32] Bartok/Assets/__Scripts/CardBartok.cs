using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// CBState includes both states for the game and to ___states for movement
public enum CBState
{
    drawpile,
    toHand,
    hand,
    toTarget,
    target,
    to,
    idle
}

public class CardBartok : Card {
    // These static fields are used to set value that will be the same
    // for all instances of Cardbartok
    static public float MOVE_DURATION = 0.5f;
    static public string MOVE_EASING = Easing.InOut;
    static public float CARD_HEIGHT = 3.5f;
    public static float CARD_WIDTH = 2f;

    public CBState state = CBState.drawpile;

    // Fields to store info the card will use to move and rotate
	public List<Vector3> bezierPts;
	public List<Quaternion> bezierRots;
	public float timeStart, timeDuration; // declares 2 fields.


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
