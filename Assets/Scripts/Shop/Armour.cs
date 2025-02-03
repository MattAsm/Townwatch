using UnityEngine;
[CreateAssetMenu(fileName = "newArmour", menuName = "ShopItems/Armour")]
public class Armour : ShopItems
{
    public int DefenceValue;
   // public int sellPrice;

    [Header("ArmourType")]
    public ArmourType armourType;
    //public WeaponType weaponType;
    //Later add armour requiring weapon type
}

public enum ArmourType{ Head, Torse, Hands, Legs, Feet};
