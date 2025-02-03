using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "newShopItems", menuName = "ShopItems/Basic")]
public class ShopItems : ScriptableObject
{
    public string ItemName;
    public string ItemDescription;
    public Sprite Icon;
    public int price;

    [Header("Rarity")]
    public Rarity rarity;
}