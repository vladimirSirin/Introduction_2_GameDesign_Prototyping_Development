using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prospector : MonoBehaviour
{


    public static Prospector S;

    public Deck deck;

    public TextAsset deckXML;

    public Layout layout;
    public TextAsset layoutXML;

    public List<CardProspector>  drawPile;

    void Awake()
    {
        S = this; // Set up a Singleton for Prospector
    }

	// Use this for initialization
	void Start ()
	{

	    deck = GetComponent<Deck>(); // Get the Deck
        deck.InitDeck(deckXML.text); // Pass DeckXML to it
        Deck.ShuffleDeck(ref deck.cards); // This shuffle the deck

        // The ref keyword passes a reference to deck.cards, which allows
        // deck.cards to be modified by shuffle function

	    layout = GetComponent<Layout>(); // Get the layout
        layout.ReadLayout(layoutXML.text); // Pass layout.xml to it

	    drawPile = cardListToCardProspectorsList(deck.cards);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    // Card to CardProspector convertion function
    public List<CardProspector> cardListToCardProspectorsList(List<Card> Cards)
    {
        List<CardProspector> cardProspectorList = new List<CardProspector>();

        CardProspector tempCP;

        foreach (Card card in Cards)
        {
            tempCP = card as CardProspector;
            cardProspectorList.Add(tempCP);
        }

        return cardProspectorList;
    }
}
