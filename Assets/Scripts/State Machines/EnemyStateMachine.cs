using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public BaseEnemy enemy;

    public enum TurnState
    {
        Processing,
        ChooseAction,
        Waiting, 
        Action,
        Dead
    }

    public TurnState currentState;
    //for the progressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    //this gameobject
    private Vector3 startPosition;
    public GameObject Selector;
    //timeforaction stuff
    private bool actionStarted = false;
    public GameObject heroToAttack;
    private float animSpeed = 10f;

    //alive
    private bool alive = true;

    // Use this for initialization
    void Start()
    {
        
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = TurnState.Processing;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentState);
        switch (currentState)
        {

            case (TurnState.Processing):
                UpgradeProgressbar();
                break;


            case (TurnState.ChooseAction):
                if(BSM.HerosInBattle.Count > 0)
                {
                    ChooseAction();
                }
                currentState = TurnState.Waiting;
                break;

            case (TurnState.Waiting):
                //idle state
                break;

            case (TurnState.Action):
                StartCoroutine(TimeForAction ());
                break;

            case (TurnState.Dead):
                if (!alive)
                {
                    return;
                }
                else
                {
                    //change tag of enemy
                    this.gameObject.tag = "DeadEnemy";
                    //not attackable by heroes
                    BSM.EnemiesInBattle.Remove(this.gameObject);
                    //disable the selector
                    Selector.SetActive(false);
                    //remove all inputs heroattacks
                    if (BSM.EnemiesInBattle.Count > 0)
                    {
                        for (int i = 0; i < BSM.PerformList.Count; i++)
                        {
                            if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                            {
                                BSM.PerformList.Remove(BSM.PerformList[i]);
                            }
                            if (BSM.PerformList[i].AttackersTarget == this.gameObject)
                            {
                                BSM.PerformList[i].AttackersTarget = BSM.EnemiesInBattle[Random.Range(0, BSM.EnemiesInBattle.Count)];
                            }
                        }
                    }
                    //change the color to gray / fade out 
                    //this.gameObject.GetComponent<SpriteRenderer>().material.color = new Color32(105, 105, 105, 255);
                    Destroy(this.gameObject);

                    alive = false;
                    //reset enemybutton
                    BSM.EnemyButtons();
                    //Announce enemy defeated
                    Debug.Log("");
                    //check alive
                    BSM.battlestates = BattleStateMachine.PerformAction.CheckAlive;
                }
            break;
        }
    }

    void UpgradeProgressbar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        
        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.ChooseAction;
        }


    }

    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn();

        myAttack.Attacker = enemy.name;
        myAttack.Type = "Enemy";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];

        int num = Random.Range(0, enemy.attacks.Count);
        myAttack.chosenAttack = enemy.attacks[num];
        Debug.Log(this.gameObject.name + " has choosen " + myAttack.chosenAttack.attackName + " and has inflicted " + myAttack.chosenAttack.attackDamage + " damage ");
        
        

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
        Vector3 heroPostion = new Vector3 (heroToAttack.transform.position.x-1.5f, heroToAttack.transform.position.y, heroToAttack.transform.position.z); 
        while (MoveTowardsEnemy(heroPostion)) { yield return null; } 


        //wait abit
        yield return new WaitForSeconds(0.5f);
        //do damage
        DoDamage();
        //animate back to startposition 
        Vector3 firstPositon = startPosition; 
        while (MoveTowardsStart (firstPositon)) { yield return null; }


        //remove this perform from the list in BSM
        BSM.PerformList.RemoveAt(0);

        //reset BSM -> Wait
        BSM.battlestates = BattleStateMachine.PerformAction.Wait;

        //end corutine
        actionStarted = false;
        
        //reset this enemy state
        cur_cooldown = 0f;
        currentState = TurnState.Processing;
    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    void DoDamage()
    {
        float calc_damage = enemy.curATK + BSM.PerformList[0].chosenAttack.attackDamage;
        heroToAttack.GetComponent<HeroStateMachine>().TakeDamage(calc_damage);
    }

    public void TakeDamage(float getDamageAmount)
    {
        enemy.curHP -= getDamageAmount;
        if(enemy.curHP <= 0)
        {
            enemy.curHP = 0;
            currentState = TurnState.Dead;
        }
    }
}


