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

    // List of the SpriteRenderer Components of this GameObject and its Children
    public SpriteRenderer[] spriteRenderers;

    // Adding the faceUp property
    public bool faceUp
    {
        get { return (!back.activeSelf); }
        set { back.SetActive(!value); }
    }

    // Virtual Methods can be overridden by subclass methods with the same name
    public virtual void OnMouseUpAsButton()
    {

        //print(name); // When Clicked, this outputs the card name
    }

	// Use this for initialization
	void Start () {
		SetSortingOrder(0); // Ensure that the card starts properly depth sorted
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // If SpriteRenderers is not yet defined, this function defines it
    public void PopulatesSpriteRenderers()
    {
        // If spriteRenderers is null or empty
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            // Get SpriteRenderer Components of this GameObject and its children
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }
    }

    // Sets the sortingLayerName on all SpriteRenderer Components
    public void SetSortingLayerName(string tSLN)
    {
        PopulatesSpriteRenderers();

        foreach (SpriteRenderer tSR in spriteRenderers)
        {
            tSR.sortingLayerName = tSLN;
        }
    }

    // Sets the sortingOrder of all SpriteRenderer Components
    public void SetSortingOrder(int sOrd)
    {
        PopulatesSpriteRenderers();

        // The white background of the card is on bottom (sOrd)
        // On top of that are all the pips, decorators, face, etc. (sOrd+1)
        // The back is on top so that when visible, it covers the rest (sOrd+2)

        // Iterate through all the spriteRenderers as tSR
        foreach (SpriteRenderer tSR in spriteRenderers)
        {
            // If gameObject is the card itself, it has to be the bottom one
            if (tSR.gameObject == this.gameObject)
            {
                tSR.sortingOrder = sOrd; // Set its order to sOrd
                continue; // And continue to the next iterate of the loop
            }

            // Each of the children of this GameObject are named
            // switch based on the name
            switch (tSR.gameObject.name)
            {
                case "back": // if the name is "back"
                    tSR.sortingOrder = sOrd + 2;
                    // ^ Set it to the highest layer to cover everything else
                    break;

                default: // If the name is not "back", face, decorators and pips
                    tSR.sortingOrder = sOrd + 1;
                    // ^ Set it to the middle layer to be above the background
                    break;
            }
        }
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
