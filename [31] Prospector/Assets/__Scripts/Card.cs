using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public string suit; // the suit of the card
    public int rank; // the rank of the card
    public Color color = Color.black; // the color of the card pips(symbols)
    public string colS = "Black"; // the name of the color

    // This list holds all of the Decorator GameObjects
    public List<GameObject> decoGos = new List<GameObject>();
    // This List holds all of the Pip GameObjects
    public List<GameObject> pipGos = new List<GameObject>();

    public GameObject back; // The GameObject of the back of the card
    public CardDefinition def; // the card definition parsed from DeckXML.xml


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


[System.Serializable]
public class Decorator
{
    // This class stores information about each decorator or pip from DeckXML

    public string type; // For card pips, type = "pip"

    public Vector3 loc; // The location of the Sprite on the Card

    public float scale = 1f; // The Scale of the Sprite

    public bool flip = false; // Whether to flip the Sprite vertically
}


[System.Serializable]
public class CardDefinition
{
    // This class stores information for each rank of card

    public string face; // Sprite to use for each face card

    public int rank; //The rank (1-13) of this card

    public List<Decorator> pips = new List<Decorator>(); //Pips used

    // Because decorators (from the XML) are used the same way on every card in
    // the deck, pips only stores information about the pips on numbered cards.
}