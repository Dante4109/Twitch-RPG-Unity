using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack : MonoBehaviour
{

    public string attackName;//name
    public string attackDescription;//description
    public float attackDamage;//Base Damage 15, melee lvl 10 stamina 25 = basedmg + lvl / stamina) 
    public float attackCost;//Mana Cost 

}
