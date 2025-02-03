using UnityEngine;


[CreateAssetMenu(fileName = "newMoveUpgrade", menuName = "ShopItems/MoveUpgrade")]
public class MoveUpgrade : ShopItems
{
    public UnitMoves MoveToUpgrade;
    public int DamageIncrease;
    public int HealingIncrease;
    public int NumOfHits;
}
//Look at this further... See if I can add UnitMove to this as move to upgrade