using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{

    public List<Vector2> itemLocations;
    public List<GameObject> itemsPlacement;

    [Header("Full Item Stock/ All items go here that can be purchased")]
    public List<GameObject> itemsFullStock; //Full stock of items

    public List<GameObject> playerLevelStock;
    public List<Rarity> levelToRarity;

    [Header("Items to appear in store, these items are generated")]
    public List<GameObject> itemsInStore;

    public CharacterManager player;
    public TMP_Text description;
    public GameObject descPlatform;


    private void Awake()
    {
    
        player = CharacterManager.instance;
        player.moveUiUnActive();
        player.moneyDisplay.IsActive();
    }
    void Start()
    {
        availableRarity(player.Level);
       foreach (GameObject stock in itemsFullStock) // Loop through items first
        {
            stock.GetComponent<PurchasableItem>().Awake();
            var purchasableItem = stock.GetComponent<PurchasableItem>().rarity;
            Debug.Log($"{purchasableItem} is {stock.GetComponent<PurchasableItem>().itemName}'s rarity");

          
            foreach (Rarity rarity in levelToRarity) 
            {
                if (levelToRarity.Contains(purchasableItem)) // Check if item rarity matches
                {
                 //   Debug.Log($"{stock.GetComponent<PurchasableItem>().rarity} rarity and {stock}");
                    playerLevelStock.Add(stock);
                }
            }
        }

        for (int x = playerLevelStock.Count - 1; x > 0; x--)
        {
            int j = Random.Range(0, x + 1);
            GameObject temp = playerLevelStock[x];
            playerLevelStock[x] = playerLevelStock[j];
            playerLevelStock[j] = temp;
        }

        for (int i = 0; i < itemsPlacement.Count; i++)
        {
            itemLocations.Add(itemsPlacement[i].transform.position);
        }
        OnShopOpen();

    }

   public void OnShopOpen()
    {
        for(int x = 0; x < itemsPlacement.Count; x++)
        {
            GameObject itemToInstantiate = playerLevelStock[x];
            GameObject instantiatedItem = Instantiate(itemToInstantiate, itemLocations[x], Quaternion.identity);
            itemsInStore.Add(instantiatedItem);
             
        }
    }

    public void bought(GameObject boughtItem)
    {
        GameObject prefab = boughtItem.transform.GetComponent<PrefabReference>().prefab;
        if (itemsInStore.Contains(boughtItem))
        {
            itemsInStore.Remove(boughtItem);
        }
        else
        {
            Debug.LogWarning("Item not found in itemsInStore.");
        } 
        if(playerLevelStock.Contains(prefab))
        {
            playerLevelStock.Remove(prefab);
        }
        else
        {
            Debug.Log("No such item in stock");
        }
        prefab.GetComponent<PrefabReference>().ifBoughtDestroy();
        Destroy(boughtItem);

    }

    public void OnStoreLeave()
    {
        player.round += 1;
        SceneManager.LoadScene(1);
    }


    public void availableRarity(int lvl)
    {
        levelToRarity.Clear();
        if (0 < lvl && lvl <= 5)
        {
            levelToRarity.Add(Rarity.Common);
            levelToRarity.Add(Rarity.Common);
        }
        else if (5 < lvl && lvl <= 10)
        {
            levelToRarity.Add(Rarity.Common);
            levelToRarity.Add(Rarity.Uncommon);
            levelToRarity.Add(Rarity.Uncommon);
        }
        else if (10 < lvl && lvl <= 15)
        {
            levelToRarity.Add(Rarity.Common);
            levelToRarity.Add(Rarity.Uncommon);
            levelToRarity.Add(Rarity.Rare);
            levelToRarity.Add(Rarity.Uncommon);
            levelToRarity.Add(Rarity.Rare);
        }
        else if (15 < lvl && lvl <= 20)
        {
            levelToRarity.Add(Rarity.Uncommon);
            levelToRarity.Add(Rarity.Rare);
            levelToRarity.Add(Rarity.Uncommon);
            levelToRarity.Add(Rarity.Rare);
            levelToRarity.Add(Rarity.Epic);
        }
        else if (20 < lvl && lvl <= 25)
        {
            levelToRarity.Add(Rarity.Rare);
            levelToRarity.Add(Rarity.Epic);
            levelToRarity.Add(Rarity.Rare);
            levelToRarity.Add(Rarity.Epic);
            levelToRarity.Add(Rarity.Legendary); 
        }
        else if (25 < lvl)
        {
            levelToRarity.Add(Rarity.Epic);
            levelToRarity.Add(Rarity.Legendary);
            levelToRarity.Add(Rarity.Epic);
            levelToRarity.Add(Rarity.Legendary);
            levelToRarity.Add(Rarity.Saint);
            levelToRarity.Add(Rarity.Demon);
        }
    }
}
