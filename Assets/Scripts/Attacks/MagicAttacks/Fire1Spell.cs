using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire1Spell : BaseAttack
{
    Fire1Spell()
    {
        attackName = "Fire1";
        attackDescription = "Level 1 Fire elemental attack.";
        attackDamage = 20f;
        attackCost = 10f;
    }
}
