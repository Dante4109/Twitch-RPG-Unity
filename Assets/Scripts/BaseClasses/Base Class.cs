using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseClass
{

    public string theName;

    public float baseHP;
    public float curHP;

    public float baseMP;
    public float curMP;

    public float baseATK;
    public float curATK;

    public float baseDEF;
    public float curDEF;

    public List<BaseAttack> attacks = new List<BaseAttack>();

}

