using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    public bool _______________________________;

    public PT_XMLReader xmlr;
    public List<string> cardName;
    public List<Card> cards;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    // InitDeck is called by Prospector when it is ready
    public void InitDeck(string deckXMLText)
    {
        ReadDeck(deckXMLText);
    }


    // ReadDeck parses the XML file passed to it into CardDefinitions
    public void ReadDeck(string deckXMLText)
    {
        // Create a new PT_XMLReader
        xmlr = new PT_XMLReader();

        // Use that PT_XMLReader to parse DeckXML
        xmlr.Parse(deckXMLText);

        // This prints a test line to show you how xmlr can be used
        // For more information read about the XML in the Useful Concept Appendix
        string s = "xml[0] decorator[0]";
        s += "type=" + xmlr.xml["xml"][0]["decorator"][0].att("type");
        s += " x=" + xmlr.xml["xml"][0]["decorator"][0].att("x");
        s += " y=" + xmlr.xml["xml"][0]["decorator"][0].att("y");
        s += " scale=" + xmlr.xml["xml"][0]["decorator"][0].att("scale");
        //print(s); // Comment out this line, since we are done with the test



        // Read decorators for all Cards
        // Init the list of decorators

        // Grab a PT_XMLHashList of all <decorator>s in the XML file




        // Read pip location for each card number
        // Init the List of the Cards

        // Grab a PT_XMLHashList of all the <card>s in the XML file



        // Face cards (Jack, Queen, & King) have a face attribute
        // cDef.face is the best name of the face Sprite
        // e.g., FaceCard_11 is the base name for the Jack face Sprites
        // the Jack of Clubs is FaceCard_11C, hearts is FaceCard_11H, etc.



    }
}
