using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Move", menuName = "Create New Move")]
public class UnitMoves : ScriptableObject
{
    public string MoveName;
    public int Damage;
    public int Healing;
    public int Accuracy;
    public Sprite moveEffect;
    public int hits = 1;
    public string attackType;
    public AudioClip attackSound;
   [SerializeField] public EnumStatusEffect StatusCondition;
}