using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RayCastBuyable : MonoBehaviour
{
    public Camera cam;
    public float RaycastDist = 25f;

    public TMP_Text itemName;
    public TMP_Text Description;
    public TMP_Text Price;
    public GameObject descBack;

    public TMP_Text shopkeep;
    public List<string> CantAfford;

    public CharacterManager player;

    public ShopManager shopManager;
    private void Start()
    {
        player = CharacterManager.instance;

            itemName.enabled = false;
            Description.enabled = false;
            Price.enabled = false;
            descBack.SetActive(false);
    }
    void Update()
    {
            RayCastDesc();
    }
    public void RayCastDesc()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, RaycastDist);
        if (hit.collider != null)
        {
            PurchasableItem shopItem = hit.collider.GetComponent<PurchasableItem>();

            if(shopItem != null)
            {
               ShowDesc(shopItem.itemName, shopItem.price, shopItem.description, shopItem.rarity);
                if(player.Money >= shopItem.price && Input.GetMouseButtonDown(0))
                {
                    //Now we need code to only purchase 1 weapon at a time. Maybe add this later
                    player.Money -= shopItem.price;
                    shopItem.OnBuy();
                    shopManager.bought(shopItem.gameObject);
                }
                else if(player.Money < shopItem.price && Input.GetMouseButtonDown(0))
                {
                    shopkeep.text = CantAfford[Random.Range(0, CantAfford.Count)];
                }
            }
            else
            {
               HideDesc();
            }
        }
        else
        {
            HideDesc();
        }
    }

    public void ShowDesc(string name, int price, string description, Rarity rarity)
    {
        FollowMouse();
        itemName.enabled = true;
        itemName.text = name;

        Description.enabled = true;
        Description.text = $"{description} {RarityDescriptionColor(rarity)}";

        Price.enabled = true;
        Price.text = $"{price.ToString()} Gold";

        descBack.SetActive(true);
    }

    public void HideDesc()
    {
           itemName.enabled = false;
           Description.enabled = false;
           Price.enabled = false;
           descBack.SetActive(false);     
    }

    public void FollowMouse()
    {
        if (Input.mousePosition.x + 500 <= Screen.width)
        {
            descBack.transform.position = Input.mousePosition + new Vector3(275, -105, 0);
        }
        else 
        {
            descBack.transform.position = Input.mousePosition + new Vector3(-275, -105, 0);
        }

    }

    public string RarityDescriptionColor(Rarity rarity)
    {
       
        switch (rarity)
        {
            case Rarity.Common:
               string rarityColor = $"<color=green> ({rarity})</color>";
                return rarityColor;
            case Rarity.Uncommon:
                rarityColor = $"<color=blue> ({rarity})</color>";
                return rarityColor;
            case Rarity.Rare:
                rarityColor = $"<color=purple> ({rarity})</color>";
                return rarityColor;
            case Rarity.Epic:
                rarityColor = $"<color=orange> ({rarity})</color>";
                return rarityColor;
            case Rarity.Legendary:
               rarityColor = $"<color=red> ({rarity})</color>";
                return rarityColor;
            case Rarity.Saint:
                rarityColor = $"<color=#FF00FF> ({rarity})</color>";
                return rarityColor;
            case Rarity.Demon:
                rarityColor = $"<color=#7C007C> ({rarity})</color>";
                return rarityColor;
            default:
                return null;
        }
        
    }
}
