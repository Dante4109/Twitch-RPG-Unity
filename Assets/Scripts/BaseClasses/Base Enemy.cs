using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BaseEnemy : BaseClass
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
}


