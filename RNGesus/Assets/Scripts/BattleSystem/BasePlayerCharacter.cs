using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasePlayerCharacter
{
    public string name;

    public float baseHP;
    public float currentHP;

    public float baseMP;
    public float currentMP;

    public List<BaseCard> Cards;

    int strenth;
    int magic;
    int dexterity;
    int agility;
}
