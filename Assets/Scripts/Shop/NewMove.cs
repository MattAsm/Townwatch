using System.Diagnostics.Contracts;
using UnityEngine;
[CreateAssetMenu(fileName = "newNewMove", menuName = "ShopItems/NewMove")]
public class NewMove : ShopItems
{
    public UnitMoves BuyableMove;
    public int MoveLevel; //If I can get move leveling to work roperly in time
}
