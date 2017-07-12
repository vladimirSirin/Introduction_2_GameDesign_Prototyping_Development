using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prospector : MonoBehaviour
{


    public static Prospector S;

    public Deck deck;

    public TextAsset deckXML;

    void Awake()
    {
        S = this; // Set up a Singleton for Prospector
    }

	// Use this for initialization
	void Start ()
	{

	    deck = GetComponent<Deck>(); // Get the Deck
        deck.InitDeck(deckXML.text); // Pass DeckXML to it

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
