using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison1Spell : BaseAttack
{

    Poison1Spell()
    {
        attackName = "Poison1";
        attackDescription = "Level 1: Inflict poison damage. Chance of poison 50%";
        attackDamage = 5f;
        attackCost = 5f;
    }
}

