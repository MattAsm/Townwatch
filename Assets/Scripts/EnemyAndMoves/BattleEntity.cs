using System.Collections;
using UnityEngine;

public abstract class BattleEntity : MonoBehaviour
{
    public string entityName;
    public int currentHealth;
    public int maxHealth;
    public int Level;
    public int Attack;
    public int Defence;
    public float Speed;
    public abstract IEnumerator TakeTurn();
}