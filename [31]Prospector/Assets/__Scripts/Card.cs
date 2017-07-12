using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour {
    // This will be define later

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}



public class Decorator
{
    // This class stores information about each decorator or pip from DeckXML

    public string type; // For card pips, type = "pip"

    public Vector3 loc; // The location of the Sprite on the Card

    public float scale = 1f; // The Scale of the Sprite

    public bool flip = false; // Whether to flip the Sprite vertically
}



public class CardDefinition
{
    // This class stores information for each rank of card

    public string face; // Sprite to use for each face card

    public int rank; //The rank (1-13) of this card

    public List<Decorator> pips = new List<Decorator>(); //Pips used

    // Because decorators (from the XML) are used the same way on every card in
    // the deck, pips only stores information about the pips on numbered cards.
}