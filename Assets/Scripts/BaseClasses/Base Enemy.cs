using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BaseEnemy
{
    public string name;

    public enum Type
    {
        GRASS,
        FIRE,
        WATER,
        ELECTRIC,
    }

    public enum Rarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        SUPERRARE
    }

    public Type EnemyType;
    public Rarity rarity;

    public float baseHP;
    public float curHP;

    public float baseMP;
    public float curMP;

    public float baseATK;
    public float curATK;

    public float baseDEF;
    public float curDEF;


}


