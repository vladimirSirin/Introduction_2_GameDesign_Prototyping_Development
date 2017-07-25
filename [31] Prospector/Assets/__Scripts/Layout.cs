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
    public string[] sortingLayerNames = new string[] 
    { "Row0", "Row1", "Row2", "Row3", "Discard", "Draw" };

    // This function is called to read in the LayoutXML.xml file
    public void ReadLayout(string xmlText)
    {
        
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
