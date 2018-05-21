using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The SlotDef class is not a subclass of MonoBehaviour, so it doesn't need a separate C# file
[System.Serializable] // This makes slotDefs visible in the Unity Inspector pane
public class SlotDef
{
    public int id;
    public float x;
    public float y;
    public bool faceUp = false;
    public int layerId;
    public string layerName = "Default";

    public string type = "slot";
    public Vector2 stagger; // why use a vector2 for staggering?

    public List<int> hiddenBy = new List<int>();


}


public class Layout : MonoBehaviour
{

    public PT_XMLReader xlmr; // Just like Deck, this has a PT_XMLReader
    public PT_XMLHashtable xml; // This variable is for easier xml access
    public Vector2 multiplier; // Set the spacing of the tableau

    // Define the slot references
    public List<SlotDef> slotDefs;

    public SlotDef drawPile;
    public SlotDef discardPile;

    // This holds all of the possible names for the layers set by layerID
    public static string[] sortingLayerNames = { "Row0", "Row1", "Row2", "Row3", "Draw", "Discard" };

    // This function is called to read in the LayoutXML.xml file
    public void ReadLayout(string xmlText)
    {
        // Parse the XML
        xlmr = new PT_XMLReader();
        xlmr.Parse(xmlText);
        xml = xlmr.xml["xml"][0];

        // Read in the multiplier, which sets card spacing
        multiplier.x = float.Parse(xml["multiplier"][0].att("x"));
        multiplier.y = float.Parse(xml["multiplier"][0].att("y"));

        // Read in the slots
        SlotDef tempSD;

        // slotsX is used as a shortcut to all the <slot>s
        PT_XMLHashList slotsX = xml["slot"];

        // Loop, reading the attributes into the slots and put them into the right piles
        for (int i = 0; i < slotsX.Count; i++)
        {
            // make a new empty slot
            tempSD = new SlotDef();

            // Various attributes are parsed into numerical values
            if (slotsX[i].HasAtt("type"))
            {
                // If this <slot> has a type attribute parse it
                tempSD.type = slotsX[i].att("type");
            }
            else
            {
                // if not, set its type to "slot"' it's a tableau card
                tempSD.type = "slot";
            }

            // Deal with: x, y, layerId, layerName, faceUp and id will be dealt with
            // In a different way, since only slot needs these two
            tempSD.x = float.Parse(slotsX[i].att("x"));
            tempSD.y = float.Parse(slotsX[i].att("y"));
            tempSD.layerId = int.Parse(slotsX[i].att("layer"));

            // This converts the number of the layerID into a text layerName
            tempSD.layerName = sortingLayerNames[tempSD.layerId];

            // The layers are used to make sure that the correct cards are on top of the others,
            // IN unity 2d, all of our assets are effectively at the same Z depth, so the layer is used
            // to differentiate between them

            // Add them to different postions based on the type of the card definitions
            switch (tempSD.type)
            {
                // pull additional attributes based on the type of this <slot>
                case "slot":
                    tempSD.faceUp = (int.Parse(slotsX[i].att("faceup")) == 1);
                    tempSD.id = int.Parse(slotsX[i].att("id"));
                    if (slotsX[i].HasAtt("hiddenby"))
                    {
                        string[] hiding = slotsX[i].att("hiddenby").Split(',');
                        foreach (string s in hiding)
                        {
                            tempSD.hiddenBy.Add(int.Parse(s));
                        }
                    }
                    slotDefs.Add(tempSD);
                    break;

                case "drawpile":
                    tempSD.stagger.x = float.Parse(slotsX[i].att("xstagger"));
                    drawPile = tempSD;
                    break;

                case "discardpile":
                    discardPile = tempSD;
                    break;
            }

        }

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
