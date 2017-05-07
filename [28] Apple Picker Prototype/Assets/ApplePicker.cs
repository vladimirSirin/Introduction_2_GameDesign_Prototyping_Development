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

    // Use this for initialization
    void Start () {

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

        // Restart the game if the baskets have been run out of
        if (BasketList.Count == 0)
        {
            SceneManager.LoadScene("_Scene_0");
        }
    }
}
