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

public class CardBartok : Card
{
	// These static fields are used to set value that will be the same
	// for all instances of Cardbartok
	public static float MOVE_DURATION = 0.5f;
	public static string MOVE_EASING = Easing.InOut;
	public static float CARD_HEIGHT = 3.5f;
	public static float CARD_WIDTH = 2f;

	public CBState state = CBState.drawpile;

	// Fields to store info the card will use to move and rotate
	public List<Vector3> bezierPts;
	public List<Quaternion> bezierRots;
	public float timeStart, timeDuration; // declares 2 fields.

	// When the card is done moving, it will call reportFinishTo.SendMesage()
	public GameObject reportFinishTo = null;

	// MoveTo tells the card to interpolate to a new position and rotation
	public void MoveTo(Vector3 ePos, Quaternion eRot)
	{
		// Make new intepolation lists for the card.
		// Position and Rotation will each have only 2 points
		bezierPts = new List<Vector3>();
		bezierPts.Add(transform.localPosition); // current Position
		bezierPts.Add(ePos); // New position
		bezierRots = new List<Quaternion>();
		bezierRots.Add(transform.rotation); // current Rotation
		bezierRots.Add(eRot); // new rotation

		// If timeStart is 0, then it's set to start immediately.
		// Otherwise, it starts at timeStart. This way, if timeStart is already set
		// it wont be overwritten
		if (timeStart == 0)
		{
			timeStart = Time.time;
		}
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}