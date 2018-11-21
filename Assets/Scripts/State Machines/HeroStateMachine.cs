using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour
{

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
    //dead
    private bool alive = true;
    //heroPanel
    private HeroPanelStats stats;
    public GameObject HeroPanel;
    private Transform HeroPanelSpacer;

    // Use this for initialization
    void Start()
    {

        //find spacer
        HeroPanelSpacer = GameObject.Find("BattleCanvas").transform.Find("HeroPanel").transform.Find("HeroPanelSpacer");
        //create panel, fill in info 
        CreateHeroPanel();

        startPosition = transform.position;
        cur_cooldown = Random.Range(0, 2.5f); //manipulate with stats for character later 
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        currentState = TurnState.Processing;
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
                {
                    CheckIfDead();
                }



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
        DoDamage();
        
        //animate back to startposition 
        Vector3 firstPositon = startPosition;
        while (MoveTowardsStart(firstPositon)) { yield return null; }

        BSM.PerformList.RemoveAt(0);

        //reset BSM -> Wait
        if (BSM.battlestates != BattleStateMachine.PerformAction.Win && BSM.battlestates != BattleStateMachine.PerformAction.Lose)
        {
            BSM.battlestates = BattleStateMachine.PerformAction.Wait;
            //reset this enemy state
            cur_cooldown = 0f;
            currentState = TurnState.Processing;
        }
        else
        {
            currentState = TurnState.Waiting;
        }
        
        //end coroutine
        actionStarted = false;
    }

    private bool MoveTowardsEnemy(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }


    private bool MoveTowardsStart(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }


    public void TakeDamage(float getDamageAmount)
    {
        hero.curHP -= getDamageAmount;
        if (hero.curHP <= 0)
        {
            hero.curHP = 0;
            currentState = TurnState.Dead;
        }
        UpdateHeroPanel();
    }

    void DoDamage()
    {
        float calc_damage = hero.curATK + BSM.PerformList[0].chosenAttack.attackDamage;
        EnemyToAttack.GetComponent<EnemyStateMachine>().TakeDamage(calc_damage);
    }


   public void CreateHeroPanel()
    {
        HeroPanel = Instantiate(HeroPanel) as GameObject;
        stats = HeroPanel.GetComponent<HeroPanelStats>();
        stats.HeroName.text = hero.theName;
        stats.HeroHP.text = "HP: " + hero.curHP;
        stats.HeroMP.text = "MP: " + hero.curMP;

        ProgressBar = stats.ProgressBar;
        HeroPanel.transform.SetParent(HeroPanelSpacer, false);
    }

    //update stats on damage/heal 
    void UpdateHeroPanel()
    {
        stats.HeroHP.text = "HP: " + hero.curHP;
        stats.HeroMP.text = "MP: " + hero.curMP;
    }


    public void CheckIfDead()
    {
        if (!alive)
        {
            return;
        }
        else
        {
            //change tag
            this.gameObject.tag = "DeadHero";
            //not attackable by enemy
            BSM.HerosInBattle.Remove(this.gameObject);
            //not managable
            BSM.HeroesToManage.Remove(this.gameObject);
            //deactivate the selector
            Selector.SetActive(false);
            //reset gui 
            BSM.AttackPanel.SetActive(false);
            BSM.EnemySelectPanel.SetActive(false);
            //reset item from performlist
            if(BSM.HerosInBattle.Count > 0)
            {
                for (int i = 0; i < BSM.PerformList.Count; i++)
                {
                    if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                    {
                        BSM.PerformList.Remove(BSM.PerformList[i]);
                    }

                    if (BSM.PerformList[i].AttackersTarget == this.gameObject)
                    {
                        BSM.PerformList[i].AttackersTarget = BSM.HerosInBattle[Random.Range(0, BSM.HerosInBattle.Count)];
                    }
                }

            }
            //change color / play animation
            this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
            //reset heroinput
            BSM.battlestates = BattleStateMachine.PerformAction.CheckAlive;
            alive = false;

        }
    }
}




    


	

