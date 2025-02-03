using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "newStatBoost", menuName = "ShopItems/StatBoost")]
public class StatBoost : ShopItems
{
    public int AttackBoost;
    public int DefenseBoost;
    public int HealthBoost;
    public int SpeedBoost;
}
