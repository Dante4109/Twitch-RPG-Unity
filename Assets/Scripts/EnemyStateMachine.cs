using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public BaseEnemy enemy;

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING, 
        ACTION,
        DEAD
    }

    public TurnState currentState;
    //for the progressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    

    // Use this for initialization
    void Start()
    {
        currentState = TurnState.PROCESSING;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentState);
        switch (currentState)
        {

            case (TurnState.PROCESSING):
                UpgradeProgressbar();

                break;


            case (TurnState.CHOOSEACTION):
                UpgradeProgressbar();

                break;

            case (TurnState.WAITING):

                break;

            case (TurnState.ACTION):

                break;

            case (TurnState.DEAD):

                break;
        }
    }

    void UpgradeProgressbar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        
        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.CHOOSEACTION;
        }


    }

    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = enemy.name;
        myAttack.AttacksGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];
        BSM.CollectActions(myAttack);

    }

}

