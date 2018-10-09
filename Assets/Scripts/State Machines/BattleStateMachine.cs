using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class BattleStateMachine : MonoBehaviour {

    public enum PerformAction
    {
            Wait,
            TakeAction,
            PerformAction

    }
    public PerformAction battlestates;
    
    public List<HandleTurn> PerformList = new List<HandleTurn>();

    public List<GameObject> HerosInBattle = new List<GameObject>();
    public List<GameObject> EnemiesInBattle = new List<GameObject>();

    public enum HeroGui
    {
        Activate,
        Waiting,
        Input1,
        Input2,
        Done
    }

    public HeroGui HeroInput;


    public List<GameObject> HeroesToManage = new List<GameObject>();
    private HandleTurn HeroChoice;

    public GameObject enemyButton;
    public Transform Spacer;

    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;
    
    
    // Use this for initialization
    void Start ()

    {
        battlestates = PerformAction.Wait;
        EnemiesInBattle.AddRange (GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        HeroInput = HeroGui.Activate;

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false); 
        //enemies to select 
        EnemyButtons();
    }
    
    // Update is called once per frame
	void Update ()
    {
	    switch (battlestates)
        {
            case (PerformAction.Wait):
                if(PerformList.Count > 0)
                {
                    battlestates = PerformAction.TakeAction;
                }

                

            break;

            case (PerformAction.TakeAction):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if (PerformList [0].Type == "Enemy") 
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.heroToAttack = PerformList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.Action; 
                }



                if (PerformList [0].Type == "Hero")
                {
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
                    HSM.EnemyToAttack = PerformList[0].AttackersTarget;
                    HSM.currentState = HeroStateMachine.TurnState.Action;
                }

                battlestates = PerformAction.PerformAction; 


                break;

            case (PerformAction.PerformAction):

                break;
        }

        switch (HeroInput)
        {
            case (HeroGui.Activate):
                if(HeroesToManage.Count > 0)
                {
                    HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                    HeroChoice = new HandleTurn();

                    AttackPanel.SetActive(true);
                    HeroInput = HeroGui.Waiting;
                }
                
            break;

            case (HeroGui.Waiting):

                break;

            case (HeroGui.Input1):

                break;

            case (HeroGui.Input2):

                break;

            case (HeroGui.Done):
                HeroInputDone();
                break;
        }

        
	}

    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
        
    }

    void EnemyButtons()
    {
      
        foreach (GameObject enemy in EnemiesInBattle)
        {
            
            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = cur_enemy.enemy.name;

            button.EnemyPrefab = enemy;

            newButton.transform.SetParent(Spacer);
        }
    }

    public void Input1()//attack button
    {
        HeroChoice.Attacker = HeroesToManage[0].name;
        HeroChoice.AttacksGameObject = HeroesToManage[0];
        HeroChoice.Type = "Hero";

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }

    public void Input2(GameObject chosenEnemy)//enemy selection
    {
        HeroChoice.AttackersTarget = chosenEnemy;
        HeroInput = HeroGui.Done;
    }

    void HeroInputDone()
    {
        PerformList.Add(HeroChoice);
        EnemySelectPanel.SetActive(false);
        HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HeroesToManage.RemoveAt(0);
        HeroInput = HeroGui.Activate;
    }
}

