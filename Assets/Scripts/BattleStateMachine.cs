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
                    ESM.HeroToAttack = PerformList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.Action;
                }

                if (PerformList [0].Type == "Hero")
                {
                  
                }
                battlestates = PerformAction.PerformAction; 


                break;

            case (PerformAction.PerformAction):

                break;
        }

        switch (HeroInput)
        {
            case (HeroGui.Activate):

            break;

            case (HeroGui.Waiting):

                break;

            case (HeroGui.Input1):

                break;

            case (HeroGui.Input2):

                break;

            case (HeroGui.Done):

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

            Text buttonText = newButton.transform.FindChild("Text").gameObject.GetComponent<Text>();
            buttonText.text = cur_enemy.enemy.name;

            button.EnemyPrefab = enemy;

            newButton.transform.SetParent(Spacer);
        }
    }
}

