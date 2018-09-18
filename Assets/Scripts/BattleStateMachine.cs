using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleStateMachine : MonoBehaviour {

    public enum PerformAction
    {
            WAIT,
            TAKEACTION,
            PERFORMACTION

    }
    public PerformAction battlestates;

    public List<HandleTurn> PerformList = new List<HandleTurn>();

    public List<GameObject> HerosInBattle = new List<GameObject>();
    public List<GameObject> EnemiesInBattle = new List<GameObject>();
    

    

    // Use this for initialization
    void Start ()

    {
        battlestates = PerformAction.WAIT;
        EnemiesInBattle.AddRange (GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
    }
    
    // Update is called once per frame
	void Update ()
    {
	    switch (battlestates)
        {
            case (PerformAction.WAIT):
                if(PerformList.Count > 0)
                {
                    battlestates = PerformAction.TAKEACTION;
                }

                

            break;

            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if (PerformList [0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.HeroToAttack = PerformList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                }

                if (PerformList [0].Type == "Hero")
                {
                    
                }

                break;

            case (PerformAction.PERFORMACTION):

                break;
        }
	}

    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
    }
}

