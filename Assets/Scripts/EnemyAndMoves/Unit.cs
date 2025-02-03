using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "GameData/Enemy")]
public class Unit : ScriptableObject
{
    [Header("Basic Data")]
    public string enemyName;

    [Header("Stats")]
    public int Level;
    public int MaxHealth;
    public int Attack;
    public int Defence;
    public float Speed;

    [Header("Moveset")]
    public UnitMoves[] moves;

    [Header("AI Behaviour")]
    public bool Aggressive;
    public bool Defensive;
    public float SpecialMoveThreshold;

    [Header("Loot")]
    public int GoldReward;
    public int EXP;
}
