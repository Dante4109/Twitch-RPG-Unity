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
    //this gameobject
    private Vector3 startposition;
    //timeforaction stuff
    private bool actionStarted = false;
    public GameObject HeroToAttack;
    private float animSpeed = 5f;




    // Use this for initialization
    void Start()
    {
        currentState = TurnState.PROCESSING;
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        startposition = transform.position;
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
                ChooseAction();
                currentState = TurnState.WAITING;
                break;

            case (TurnState.WAITING):
                //idle state
                break;

            case (TurnState.ACTION):
                StartCoroutine(TimeForAction ());

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
        myAttack.Type = "Enemy";
        myAttack.AttacksGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];
        
        BSM.CollectActions(myAttack);
    }

    private IEnumerator TimeForAction()
    {

        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        //animate the enemy near the hero to attack
        Vector3 heroPostion = new Vector3 (HeroToAttack.transform.position.x-1.5f, HeroToAttack.transform.position.y, HeroToAttack.transform.position.z); 
        while (MoveTowardsEnemy(heroPostion)) { yield return null; } 
       

        //wait abit
        //do damage

        //animate back to startposition

        //remove this perform from the list in BSM

        //reset BSM -> Wait

        actionStarted = false;
        //reset this enemy state
        cur_cooldown = 0f;
        currentState = TurnState.PROCESSING;
    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    
}



