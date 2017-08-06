using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// An enum to handle all the possible scoring events
public enum ScoreEvent
{
    draw,
    mine,
    mineGold,
    gameWin,
    gameLoss
}

public class Prospector : MonoBehaviour
{

    public static int SCORE_FROM_PREV_ROUND = 0;
    public static int HIGH_SCORE = 0;

    public float reloadDelay = 5.0f; // Time delay before reloading the level

    public static Prospector S;

    // Fields for deck building and layouting
    public Deck deck;
    public TextAsset deckXML;
    public Vector3 layoutCentre;
    public float xOffset = 3;
    public float yOffset = -2.5f;
    public Transform layoutAnchor;

    // Fields for game logic and layout
    public CardProspector target;
    public List<CardProspector> tableau;
    public List<CardProspector> discardPile;
    public List<CardProspector>  drawPile;
    public Layout layout;
    public TextAsset layoutXML;

    // Fields to track score info
    public int score = 0;   // the current total score
    public int chainNum = 0;    // the num of cards in this mine chain
    public int chainScore = 0;  // the total score in current chain
    public int chainGoldNum = 0;  // The multiplier of goldCard

    // Fields for the floatingScore and scoreboard (UI)
    private static readonly float _SCREEN_W = Screen.width;
    private static readonly float _SCREEN_H = Screen.height;

    public Vector3 fsPosMid = new Vector3(0.5f, 0.90f, 0);
    public Vector3 fsPosRun = new Vector3(0.5f, 0.75f, 0);
    public Vector3 fsPosMid2 = new Vector3(0.5f, 0.5f, 0);
    public Vector3 fsPosEnd;

    public FloatingScore fsRun;

    // The GameOver and RoundResult UI
    public Text gameOverUI;
    public Text roundResultUI;
    public Text highScoreUI;


    void Awake()
    {
        S = this; // Set up a Singleton for Prospector

        // Check for high score in PlayerPrefs
        if (PlayerPrefs.HasKey("ProspectorHighScore"))
        {
            HIGH_SCORE = PlayerPrefs.GetInt("ProspectorHighScore");
        }

        // Add the score from last round, which will be >0 if it was a win
        score += SCORE_FROM_PREV_ROUND;


        // Add reset the SCORE_FROM_PREV_ROUND
        SCORE_FROM_PREV_ROUND = 0;

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

	    Scoreboard.S.score = score;

	    // Normalize the points
	    fsPosMid.x *= _SCREEN_W;
	    fsPosMid.y *= _SCREEN_H;
	    fsPosRun.x *= _SCREEN_W;
	    fsPosRun.y *= _SCREEN_H;
	    fsPosMid2.x *= _SCREEN_W;
	    fsPosMid2.y *= _SCREEN_H;
	    fsPosEnd = Scoreboard.S.transform.position;

        // Find the text game objects
	    gameOverUI = GameObject.Find("GameOver").GetComponent<Text>();
	    roundResultUI = GameObject.Find("RoundResult").GetComponent<Text>();
	    highScoreUI = GameObject.Find("HighScore").GetComponent<Text>();

        // Setting the high score if there is any
	    highScoreUI.text = "High Score: " + Utils.AddCommasToNumber(HIGH_SCORE);

        // Set the game over feedbacks invisible at beginning
       ShowResultUI(false);

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

    // Convert the layoutId into the CardProspector in this slot
    CardProspector FindCardByLayoutID(int layoutID)
    {
        foreach (CardProspector cd in tableau)
        {
            // search through all cards in the tableau list<>
            if (cd.layoutId == layoutID)
            {
                // If the card has the same ID, return it
                return cd;
            }
        }

        // If it is not found, return null
        return null;
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
            cp.layoutId = slotDef.id;
            cp.slotDef = slotDef;

            cp.SetSortingLayerName(slotDef.layerName); // Set the sorting layers
            
            tableau.Add(cp); // Add this CarProspector to the List<> tableau

        }

        // Iterate through the hiddenby list of the slotDef and find the cards
        foreach (CardProspector cd in tableau)
        {
            foreach (int id in cd.slotDef.hiddenBy)
            {
                cp = FindCardByLayoutID(id);
                cd.hiddenBy.Add(cp);
            }
        }


        // Set up the initial tableau
        MoveToTarget(Draw());
        UpdateDrawPile();

    }

    // the CardClicked function
    public void CardClicked(CardProspector card)
    {
        switch (card.state)
        {
            case CardState.target:
                // Click the target card does nothing
                break;
            
            case CardState.drawpile:
                // Click the drawpile will replace the target card with a new one and move it to discardPile
                MoveToDiscard(target);

                MoveToTarget(Draw());

                UpdateDrawPile();
                ScoreManager(ScoreEvent.draw);
                break;

            case CardState.tableau:
                // Click a card in the tableau will check if it's a valid play
                bool validPlay = card.faceUp; // Check if it is hidden ( faceUp or not)

                // Check if it is adjacent to the target
                if (!AdjacentRank(card, target))
                {
                    validPlay = false;
                }

                if (validPlay == false) return; // Return if it is not a valid move

                // It is a valid move
                MoveToDiscard(target); // remove the origin target
                tableau.Remove(card); // Remove it from the tableau list
                MoveToTarget(card); // Make it the new target
                SetTableauFaces(); // Update tableau card face-
                
                ScoreManager(ScoreEvent.mine);
                break;
        }

        // Check whether the game is over
        CheckForGameOver();
    }

    // Moves the current target to the discardPile
    void MoveToDiscard(CardProspector cd)
    {
        // Set the card state to discard
        cd.state = CardState.discard;

        // Set the parent and localposition to the discardpile
        cd.transform.parent = layoutAnchor;
        float x = layout.multiplier.x * layout.discardPile.x;
        float y = layout.multiplier.y * layout.discardPile.y;

        cd.transform.localPosition = new Vector3(x, y, -layout.discardPile.layerId + 0.5f);

        // Set the faceUp
        cd.faceUp = true;

        // Add it to the list of discardPile
        discardPile.Add(cd);

        // Place it on top of the pile for depth sorting
        cd.SetSortingLayerName(layout.discardPile.layerName);
        cd.SetSortingOrder(-100+discardPile.Count);
    }

    // Move the top card of the DrawPile to the target
    void MoveToTarget(CardProspector cd)
    {
        // Set the card state to target
        cd.state = CardState.target;

        // Set the parent and localpotion to the target position
        cd.transform.parent = layoutAnchor;
        cd.transform.localPosition = new Vector3(
            layout.multiplier.x * layout.discardPile.x,
            layout.multiplier.y * layout.discardPile.y,
            -layout.discardPile.layerId);

        // Set the faceUp
        cd.faceUp = true;

        // Set it to target
        target = cd;

        // Place it on the target layer and set the order
        cd.SetSortingLayerName(layout.discardPile.layerName);
        cd.SetSortingOrder(0);
    }

    // Arrange all the cards of the drawPile to show how many are left
    void UpdateDrawPile()
    {
        // go through all the left cards in the drawPile
        for (int i = 0; i < drawPile.Count; i++)
        {
            // Set the position
            CardProspector cd = drawPile[i];
            cd.transform.parent = layoutAnchor;
            cd.transform.localPosition = new Vector3(
                layout.multiplier.x * (layout.drawPile.x + i*layout.drawPile.stagger.x),
                layout.multiplier.y * (layout.drawPile.y + i*layout.drawPile.stagger.y),
                -layout.drawPile.layerId + i * 0.1f);

            // Set the face down
            cd.faceUp = false;

            // Set the state as drawpile
            cd.state = CardState.drawpile;
            
            // Sorting the order and layers
            cd.SetSortingLayerName(layout.drawPile.layerName);
            cd.SetSortingOrder(-10*i);
        }
    }

    // Check whether two cards are adjacent to each other by value (Handles A-to-King wrapround)
    public bool AdjacentRank(CardProspector c0, CardProspector c1)
    {
        // If either card is face down, it's not adjacent
        if (!c0.faceUp || !c1.faceUp) return false;

        // If they are 1 apart, they are adjacent
        if (Mathf.Abs(c0.rank - c1.rank) == 1)
        {
            return true;
        }

        // If one is A and the other is King, they are adjacent
        if (c0.rank == 13 && c1.rank == 1) return true;
        if (c0.rank == 1 && c1.rank == 13) return true;

        // Otherwise, return false;
        return false;   
    }

    // This turns cards in the Mine face-up or face-down
    void SetTableauFaces()
    {
        foreach (CardProspector cd in tableau)
        {
            bool fup = true;
            // Check if the cd list of hiddenby is empty (not needed since only those have elements will have this list
            // if (cd.hiddenBy.Count != 0)
                // if it is not empty, check the state of the cards in the list
            foreach (CardProspector tCd in cd.hiddenBy)
            {
                if (tCd.state == CardState.tableau)
                {
                    fup = false;
                }
            }

            cd.faceUp = fup;
        }

        
    }

    // Define the function of CheckForGameOver()
    void CheckForGameOver()
    {
        // Check if the Tableau is gone, if so game is over
        if (tableau.Count == 0)
        {
            // Call gameover with a win
            GameOver(true);
        }

        // Check if the drawpile is gone, if not, the game is not over
        if (drawPile.Count > 0)
        {
            return;
        }

        // If drawpile is gone, check if there is solid move
        if (drawPile.Count == 0)
        {
            foreach (CardProspector card in tableau)
            {
                if (AdjacentRank(card, target))
                {
                    return; // If there is a valid move, game is no over
                }
            }

            // Since there are no valid play, the game is over
            // Call gameOver with a lose
            GameOver(false);
        }
    }

    // Define the GameOver function, call when the game is over
    void GameOver(bool won)
    {
        if (won)
        {
            ScoreManager(ScoreEvent.gameWin);
        }
        else
        {
            ScoreManager(ScoreEvent.gameLoss);
        }

        // Invoke the reloadlevel function
        // Delay will give the score time to travel
        Invoke("ReloadLevel", reloadDelay);
    }

    // ScoreManager handles all of the scoring
    void ScoreManager(ScoreEvent sEvt)
    {

        List<Vector3> fsPts;

        switch (sEvt)
        {
            case ScoreEvent.draw:
                // Add the chained score into the total one and reset it to zero;
                score += chainScore * Mathf.RoundToInt(Mathf.Pow(2, chainGoldNum));
                chainScore = 0;
                chainNum = 0;
                chainGoldNum = 0;
                break;

            case ScoreEvent.mine:
                // +1 on the chainNum
                chainNum++;
                // Add score on the chainScore
                chainScore += chainNum;
                break;

            case ScoreEvent.mineGold:
                // raise the multiplier based on the gold card encountered in this chain
                chainGoldNum++;
                chainNum++;
                break;

            case ScoreEvent.gameWin:
                // Clear the total score of this round, add the score to PREV_ROUND for next round
                SCORE_FROM_PREV_ROUND = score;
                break;

            case ScoreEvent.gameLoss:
                // Clear the other ones, add all to the total score
                score += chainScore;
                break;
        }

        // Handle the statements of game win and loss
        switch (sEvt)
        {
            case ScoreEvent.gameWin:
                // if it is a win, add the score to the next round
                // static fields are NOT reset by Application.LoadLevel()
                print("You Won this round! Round score: " + score);

                gameOverUI.text = "Round Over";
                roundResultUI.text = "You have won this round!\n Round Score: " + score;
                ShowResultUI(true);
                break;

            case ScoreEvent.gameLoss:
                // If it is a loss, check against the high score
                gameOverUI.text = "Game Over";
                if (Prospector.HIGH_SCORE <= score)
                {
                    print("You got the new high score! High score: " + score);
                    PlayerPrefs.SetInt("ProspectorHighScore", score);
                    roundResultUI.text = "You got the high score!\n High Score: " + score;
                }
                else
                {
                    print("Your final score is: "+score);
                    roundResultUI.text = "Your final score was: " + score;
                    ShowResultUI(true);

                }
                break;

            default:
                print("score:" + score+" chainNum:" + chainNum+" chainScore:"+chainScore);
                break;
        }

        // Handle the scoring show to player
        switch (sEvt)
        {
                case ScoreEvent.draw:
                case ScoreEvent.gameWin:
                case ScoreEvent.gameLoss:
                    // adding the chain score to the scoreboard
                    if (fsRun != null)
                    {
                        // Check if there is a started run.
                        // If there is, make the bezier list for score moving
                        fsPts = new List<Vector3>();
                        fsPts.Add(fsPosRun);
                        fsPts.Add(fsPosMid2);
                        fsPts.Add(fsPosEnd);
                        fsRun.reportFinishTo = Scoreboard.S.gameObject;
                        fsRun.Init(fsPts, 0, 1);
                    
                        // Change the font too.
                        fsRun.fontSize = new List<float>(new float[]{28, 36, 4});
                        fsRun = null; // Clear fsRun so it is created again
                    }
                    break;

                case ScoreEvent.mine:
                case ScoreEvent.mineGold:
                    // When mining, adding up the score in fsRun
                    FloatingScore fs;

                    // Moving it from the mousePosition to fsPosRun
                    fsPts = new List<Vector3>();
                    Vector3 p0 = Input.mousePosition;
                    //p0.x /= Screen.width;
                    //p0.y /= Screen.height;
                    fsPts.Add(p0);
                    fsPts.Add(fsPosMid);
                    fsPts.Add(fsPosRun);

                    fs = Scoreboard.S.CreateFloatingScore(chainNum, fsPts);

                    fs.fontSize = new List<float>(new float[]{4, 50, 28});

                    if (fsRun == null)
                    {
                        fsRun = fs;
                        fsRun.reportFinishTo = null;
                    }
                    else
                    {
                        fs.reportFinishTo = fsRun.gameObject;
                    }
                    break;



        }

    }

    // function to reload the level
    void ReloadLevel()
    {
        // Reload the scene, resetting the game
        SceneManager.LoadScene("__Prospector_Scene_0");
    }

    // Function to show or hide the UI text of game over / round / highscore
    void ShowResultUI(bool show)
    {
        gameOverUI.gameObject.SetActive(show);
        roundResultUI.gameObject.SetActive(show);
    }
}
