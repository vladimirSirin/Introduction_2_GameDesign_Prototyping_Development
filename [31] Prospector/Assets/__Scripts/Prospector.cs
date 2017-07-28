using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prospector : MonoBehaviour
{


    public static Prospector S;

    public Deck deck;

    public TextAsset deckXML;
    public Vector3 layoutCentre;
    public float xOffset = 3;
    public float yOffset = -2.5f;
    public Transform layoutAnchor;

    public CardProspector target;
    public List<CardProspector> tableau;
    public List<CardProspector> discardPile;

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
	    LayoutGame();
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

    // The Draw function will pull a single card from the drawpile and return it 
    CardProspector Draw()
    {
        CardProspector cd = drawPile[0]; // Pull the 0th CardProspector
        drawPile.RemoveAt(0); // Then remove it from List<> drawPile
        return cd;  // And return it
    }

    // LayoutGame() Position the initial tableau of cards, aka the "mine"
    void LayoutGame()
    {
        if (layoutAnchor == null)

        {
            GameObject tempGO = new GameObject();
            layoutAnchor = tempGO.transform;
            layoutAnchor.position = layoutCentre;
        }

        CardProspector cp;

        foreach (SlotDef slotDef in layout.slotDefs)
        {
            // ^Iterate through all the slotDefs in the layout.slotDefs as slotDef
            cp = Draw(); // Pull a card from the top of the drawPile

            // Set the state, hiddenBy, layoutId, slotDef, faceUp, position
            cp.faceUp = slotDef.faceUp; // Set its faceUp to the value in SlotDef
            cp.transform.parent = layoutAnchor; // Make its parent layoutAnchor
            // This replaces the previous parent: deck.deckAnchor, which appears
            // as _Deck in the Hierarchy when the scene is playing

            cp.transform.localPosition = new Vector3(
                layout.multiplier.x * slotDef.x,
                layout.multiplier.y * slotDef.y,
                -slotDef.layerId);
            // ^Set the localPosition of the card based on slotDef

            cp.state = CardState.tableau; // CardProspectors in the tableau have the state CardState.tableau
            cp.layoutId = slotDef.layerId;
            cp.slotDef = slotDef;
            
            tableau.Add(cp); // Add this CarProspector to the List<> tableau

        }
    }
}
