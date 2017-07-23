using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Suits
    public Sprite suitClub;
    public Sprite suitDiamond;
    public Sprite suitHeart;
    public Sprite suitSpade;

    public Sprite[] faceSprites;
    public Sprite[] rankSprites;

    public Sprite cardBack;
    public Sprite cardBackGold;
    public Sprite cardFront;
    public Sprite cardFrontGold;

    // prefabs
    public GameObject prefabSprite;
    public GameObject prefabCard;


    public bool _______________________________;

    public PT_XMLReader xmlr;
    public List<string> cardNames;
    public List<Card> cards;
    public List<Decorator> decorators;
    public List<CardDefinition> cardDefs;
    public Transform deckAnchor;
    public Dictionary<string, Sprite> dictSuits;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    // InitDeck is called by Prospector when it is ready
    public void InitDeck(string deckXMLText)
    {
        // This creates an anchor for all the card GameObjects in the Hierarchy
        if (GameObject.Find("_Deck") == null)
        {
            GameObject anchorGO = new GameObject("_Deck");
            deckAnchor = anchorGO.transform;
        }


        // Initialize the Dictionary of SuitSprites with necessary sprites
        dictSuits = new Dictionary<string, Sprite>();
        dictSuits.Add("C", suitClub);
        dictSuits.Add("D", suitDiamond);
        dictSuits.Add("S", suitSpade);
        dictSuits.Add("H",suitHeart);

        
        ReadDeck(deckXMLText);
        MakeCards();
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
        decorators = new List<Decorator>();
        // Grab a PT_XMLHashList of all <decorator>s in the XML file
        PT_XMLHashList decoHashList = xmlr.xml["xml"][0]["decorator"];

        // Initialize the decorators in the xml and add them into the decorators list.
        Decorator deco;
        // For each decorator to work, we need four variables: the type of the suit/letter as string, 
        // the location of the suit/letter as vector3, the size as float, and whether it is flipped as bool
        for (int i = 0; i < decoHashList.Count; i++)
        {
            // Make a new decorator
            deco = new Decorator();

            // Find the four variable attribution of the [i] element
            string type = decoHashList[i].att("type");
            Vector3 location = new Vector3();
            location.x = float.Parse(decoHashList[i].att("x"));
            location.y = float.Parse(decoHashList[i].att("y"));
            location.z = float.Parse(decoHashList[i].att("z"));

            float size = float.Parse(decoHashList[i].att("scale"));
            bool flipped = (decoHashList[i].att("flip") == "1");

            // Assign them to the new decorator
            deco.flip = flipped;
            deco.loc = location;
            deco.scale = size;
            deco.type = type;

            // Add them to the new decorator list
            decorators.Add(deco);
        }


        // Read pip location for each card number
        // Init the List of the Cards
        cardDefs = new List<CardDefinition>();

        // Grab a PT_XMLHashList of all the <card>s in the XML file
        PT_XMLHashList xCardDefs = xmlr.xml["xml"][0]["card"];

        // For each cardDefinition, we need 3 infos, the rank of a int number
        // the face if it is a faced card, and a list of decorators as pip
        for (int i = 0; i < xCardDefs.length; i++)
        {
            // Make a new cardDef
            CardDefinition cardDef = new CardDefinition();

            // Find the variables of the [i] element as a card definition
            cardDef.rank = int.Parse(xCardDefs[i].att("rank"));

            // Iterate through all the pips in the hashlist and creates decorators
            // After initialize the new decorators with data,
            // add them into the pip list of the cardDef
            PT_XMLHashList xPips = xCardDefs[i]["pip"];
            if (xPips != null)
            {

                for (int j = 0; j < xPips.length; j++)
                {
                    // Make a new decorator for use
                    Decorator decorator = new Decorator();

                    // type, location, scale and flip
                    decorator.type = "pip";
                    decorator.loc.x = float.Parse(xPips[j].att("x"));
                    decorator.loc.y = float.Parse(xPips[j].att("y"));
                    decorator.loc.z = float.Parse(xPips[j].att("z"));

                    if (xPips[i].HasAtt("scale"))
                    {
                        decorator.scale = float.Parse(xPips[i].att("scale"));
                    }

                    decorator.flip = (xPips[j].att("flip") == "1");

                    // add them into the pip list of the cardDef
                    cardDef.pips.Add(decorator);
                }

            }

            // Face cards (Jack, Queen, & King) have a face attribute
            // cDef.face is the best name of the face Sprite
            // e.g., FaceCard_11 is the base name for the Jack face Sprites
            // the Jack of Clubs is FaceCard_11C, hearts is FaceCard_11H, etc.
            if (xCardDefs[i].HasAtt("face"))
            {
                cardDef.face = xCardDefs[i].att("face");
            }

            cardDefs.Add(cardDef);

        }






    }


    // Get the proper CardDefinition based on Rank (1 to 14 is Ace to king)
    public CardDefinition GetCardDefinitionByRank(int rank)
    {
        // iterate through all the card definitions
        // If the rank of the cd matches rank, return it
        foreach (CardDefinition cardDef in cardDefs)
        {
            if (cardDef.rank == rank)
            {
                return cardDef;
            }

        }
        
        // if no such rank, return null
        return (null);

    }


    // Make the Card gameobjects
    public void MakeCards()
    {
        // Prepare all the possible combinations into the cardName list
        // If the combination exsits in the cardDefs, make the card
        cardNames = new List<string>();
        List<string> letters = new List<string>{"C","S","H","D"};
        for (int index = 0; index < 13; index++)
        {
            foreach (string s in letters)
            {
                cardNames.Add(s+(index+1));
            }
        }

        // Create a list for the cards to put them in
        cards = new List<Card>();

        // Several variables that will be used several  times
        GameObject tGO = null;
        SpriteRenderer tSR = null;

        // Iterate through the cardNames and see if the combination works
        for (int i = 0; i < cardNames.Count; i++)
        {

            // Instantiate the card prefabs for each card in the list
            GameObject cardGO = Instantiate(prefabCard);

            // Set the transfrom.parent of the new card to the anchor, so they can move relatively
            cardGO.transform.parent = deckAnchor;

            // Get the card component
            Card card = cardGO.GetComponent<Card>();

            // Set the location so they are all in nice rows
            cardGO.transform.localPosition = new Vector3((i%13)*3, i/13*4,0);

            // Assign the basic variables of the card
            card.name = cardNames[i];
            card.suit = card.name[0].ToString();
            card.rank = int.Parse(card.name.Substring(1));
            if (card.suit == "D" || card.suit == "H")
            {
                card.color = Color.red;
                card.colS = "Red";
            }

            // Get the card definition, to prepare for decorators
            card.def = GetCardDefinitionByRank(card.rank);

            // Add the decorators in a correct way
            foreach (Decorator deco in decorators)
            {
                // Check if it is a suit or a rank, and deal them differently
                if (deco.type == "suit")
                {
                    // Instantiate the game object of decorator
                    tGO = Instantiate(prefabSprite);
                    // Get the renderer component and render it
                    tSR = tGO.GetComponent<SpriteRenderer>();
                    tSR.sprite = dictSuits[card.suit];
                }
                else // if it is not a suit, then it is a rank
                {
                    // Instantiate it too
                    tGO = Instantiate(prefabSprite);
                    // Get the renderer too
                    tSR = tGO.GetComponent<SpriteRenderer>();
                    tSR.sprite = rankSprites[card.rank];

                    // Set the color of the rank to match the suit
                    tSR.color = card.color;
                }

                // Make the deco sprites above the card when rendering
                tSR.sortingOrder = 1;

                // Make the decorator sprites at a correct position
                tGO.transform.parent = card.transform;
                tGO.transform.localPosition = deco.loc;

                // Make the decorator sprites at a correct size and is flip
                tGO.transform.localScale = Vector3.one * deco.scale;
                if (deco.flip)
                {
                    tGO.transform.localRotation = Quaternion.Euler(0, 0, 180);
                }

                // Name the gameobject so it can be found easily
                // Add the decoGo into the list of the GOs
                tGO.name = deco.type;
                card.decoGos.Add(tGO);
            }

            // Add the pips in a correct way
            // For each of the pips in the definition
            // Same way as the decorators, only without suit/letter identify
            foreach (Decorator pip in card.def.pips)
            {
                tGO = Instantiate(prefabSprite);
                tSR = tGO.GetComponent<SpriteRenderer>();
                tSR.sprite = dictSuits[card.suit];

                tSR.color = card.color;
                tSR.sortingOrder = 1;

                tGO.transform.parent = card.transform;
                tGO.transform.localPosition = pip.loc;

                if (pip.flip)
                {
                    tGO.transform.localRotation = Quaternion.Euler(0, 0, 180);
                }

                if (pip.scale != 1)
                {
                    tGO.transform.localScale = pip.scale * Vector3.one;
                }

                card.pipGos.Add(tGO);
            }

            // Handle Face Cards
            if (card.def.face != "")
            {
                // if this card has a face in the card definition
                // instantiate the gameobject and render this as a face card
                tGO = Instantiate(prefabSprite);
                tSR = tGO.GetComponent<SpriteRenderer>();

                // Genrate the right name for Get face
                tSR.sprite = GetFace(card.def.face + card.suit);

                tSR.sortingOrder = 1;

                tGO.transform.parent = card.transform;
                tGO.transform.localPosition = Vector3.zero;
                tGO.name = "face";

            }

            // Handle the card back
            // The card_back will be able to cover everything else on the card
            tGO = Instantiate(prefabSprite);
            tSR = tGO.GetComponent<SpriteRenderer>();
            tSR.sprite = cardBack;

            tGO.transform.parent = card.transform;
            tGO.transform.localPosition = Vector3.zero;

            // This is a higher sortingOrder than anything else
            tSR.sortingOrder = 2;
            tGO.name = "back";
            card.back = tGO;

            // Default to face-up
            card.faceUp = true; // use the property faceUp of card

            

            // Add them into the list       
            cards.Add(card);
        }

    } // This is the closing brace for MakeCards()

    // the get face function
    // given a name of the face and find the correct face sprite
    public Sprite GetFace(string faceName)
    {
        foreach (Sprite faceSprite in faceSprites)
        {
            if (faceSprite.name == faceName)
            {
                return faceSprite;
            }
        }

        return null;
    }


    // function to shuffle the cards
    public static void ShuffleDeck(ref List<Card> oCards)
    {
        // Create a temp list of cards to hold the list info.
        List<Card> tCards = new List<Card>();

        int ndx; // This will hold the index of the card to be moved
        tCards = new List<Card>(); // Initialize the temporary List
        
        // Repeat as long as there are cards in the original list
        while (oCards.Count > 0)
        {
            // Pick the index of a random card
            ndx = Random.Range(0, oCards.Count);

            // Add the card to the temp list
            tCards.Add(oCards[ndx]);

            // Add remove the card from the original list
            oCards.RemoveAt(ndx);
        }

        // Replace the original list with the temp list
        oCards = tCards;

        // Because oCards is a reference variable, the original that was
        // passed in is changed as well.
    }
}
