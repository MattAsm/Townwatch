using UnityEngine;
[CreateAssetMenu(fileName = "newWeapon", menuName = "ShopItems/Weapon")]
public class Weapon : ShopItems
{
    public int AttackPower;
    public int LifeSteal;
    public int Speed;

    [Header("Weapon Type")]
    public WeaponType weaponType;
}
public enum WeaponType { Sword, Staff, Knuckles, Bow};