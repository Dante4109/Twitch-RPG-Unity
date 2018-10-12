using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public BaseHero hero;

    public enum TurnState
    {
        Processing,
        AddToList,
        ChooseAction,
        Waiting,
        Action,
        Dead
    }

    public TurnState currentState;
    //for the progressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Image ProgressBar;
    //this gameobject
    public GameObject Selector;
    private Vector3 startPosition;
    public GameObject EnemyToAttack;
    //timeforaction 
    private bool actionStarted = false;
    private float animSpeed = 10f;
    

    // Use this for initialization
    void Start ()
    {
        startPosition = transform.position;
        cur_cooldown = Random.Range(0, 2.5f); //manipulate with stats for character later 
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine> ();
        currentState = TurnState.Processing;	
	}
    
    

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentState);
        switch (currentState)
        {
            
            case (TurnState.Processing):
                    UpgradeProgressbar();

                break;

            case (TurnState.AddToList):
                    BSM.HeroesToManage.Add(this.gameObject);
                    currentState = TurnState.Waiting;

                break;

            case (TurnState.Waiting):
                //idle
                break;

            case (TurnState.Action):
                    StartCoroutine(TimeForAction());
                break;

            case (TurnState.Dead):

                break;
        }
    }

        void UpgradeProgressbar()
        {
            cur_cooldown = cur_cooldown + Time.deltaTime;
            float calc_cooldown = cur_cooldown / max_cooldown;
            ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
            if (cur_cooldown >= max_cooldown)
            {
                currentState = TurnState.AddToList;
            }


        }

    private IEnumerator TimeForAction()
    {

        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        //animate the enemy near the hero to attack
        Vector3 enemyPostion = new Vector3(EnemyToAttack.transform.position.x + 1.5f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
        while (MoveTowardsEnemy(enemyPostion)) { yield return null; }


        //wait abit
        yield return new WaitForSeconds(0.5f);
        //do damage

        //animate back to startposition 
        Vector3 firstPositon = startPosition;
        while (MoveTowardsStart(firstPositon)) { yield return null; }


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
}


	

