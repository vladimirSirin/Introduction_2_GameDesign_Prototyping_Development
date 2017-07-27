using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This is an enum, which define a type of variable that only has a few possible name values.
// The CardState variable type has one of four values:
// drawpile, tableau, target, discard

public enum CardState
{
    drawpile,
    tableau,
    target,
    discard
}


public class CardProspector : Card {    // Make sure CardProspector extends cards

    // This is how you use the enum CardState
    public CardState state = CardState.drawpile;

    // The hiddenBy list stores which other cards will keep this one face down
    public List<CardProspector> hiddenBy = new List<CardProspector>();

    // LayoutID matches this card to a Layout XML id if it's a tableau card
    public int layoutId;

    // The SlotDef class stores information pulled in from the LayoutXML<slot>
    public SlotDef slotDef;

}
