using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour
{

    public GameObject BasketPrefab;
    public int NumBasket = 3;
    public float BasketBottomY = -14f;
    public float BasketSpacingY = 2f;
    public List<GameObject> BasketList;
    public GameObject AppleTree_A;
    public GameObject AppleTree_B;
    public GameObject AppleTree_Uni;
    static public int NumTrees = 1;
    public int DifficultyGate_01 = 50;
    public int DifficultyGate_02 = 100;
    public int DifficultyGate_03 = 240;

    // Use this for initialization
    void Start ()
    {

        Instantiate(AppleTree_A);

        BasketList.Clear();
        for (int i = 0; i < NumBasket; i++)
	    {
            GameObject basketGameObject = Instantiate(BasketPrefab);
	        Vector3 pos = Vector3.zero;

	        pos.y = BasketBottomY;
	        pos.y += i * BasketSpacingY;

	        basketGameObject.transform.position = pos;
            BasketList.Add(basketGameObject);
	    }
		
	}
	
	// Update is called once per frame

	void Update () {

	    if (Basket.NewScore > DifficultyGate_01 && NumTrees == 1)
	    {
	        Instantiate(AppleTree_B);
	        NumTrees = 2;
	    }
	    else if (Basket.NewScore > DifficultyGate_02 && NumTrees == 2)
	    {
	        Instantiate(AppleTree_Uni);
	        NumTrees = 3;
	    }

        else if (Basket.NewScore > DifficultyGate_03 && NumTrees == 3)
        {
            Instantiate(AppleTree_A);
            Instantiate(AppleTree_B);
            NumTrees = 4;
        }

    }

    // Public Function of AppleDestroyed
    public void AppleDestroyed()
    {
        GameObject[] tAppleArray = GameObject.FindGameObjectsWithTag("Apple");
        foreach (var apple in tAppleArray)
        {
            Destroy(apple);
        }

        // Remove one basket from the list
        int basketIndex = BasketList.Count - 1;
        GameObject basketRemove = BasketList[basketIndex];
        BasketList.Remove(basketRemove);
        Destroy(basketRemove);

        // Go to the Game_over scene if the baskets have been run out of
        if (BasketList.Count == 0)
        {
            SceneManager.LoadScene("_Scene_Over");
        }
    }
}
