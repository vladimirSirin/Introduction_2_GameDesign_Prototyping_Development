using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an enum of the weapon types in the game
// Includes a shield type to raise the shield
// Marked with [NI] below are not implemented in this book
public enum WeaponType
{
    none, // The default , no weapon
    blaster, // A simple blaster
    spread, // Two shots simultaneously
    missile, // Homing Missile [NI]
    phaser, // shots that move in waves [NI]
    laser, // Damge over time [NI]
    shield // Raise shieldlevel
}


// The weapon definition class allows you to set the properties
// of a specific weapon in the Inspector. Main has an array of WeaponDefinitions that makes it possible
// [System.Serializable] tells unity to try to view WeaponDefinition in the Inspector pane. It does not work
// for everything, but it will work for simple classes like this!
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;                       // the letter to show on the power-up
    public Color color = Color.white;           // The Color of collar and power-up
    public float damageOnHit = 0;               // Amount of damage caused
    public float continuousDamage = 0;          // Damage Per second, for lasers
    public float delayBetweenShots = 0;
    public float velocity = 20;                 // Speed of projectiles
}

// Note: weapon prefabs, colors, and so on. Are set in the class Main


public class Weapon : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
