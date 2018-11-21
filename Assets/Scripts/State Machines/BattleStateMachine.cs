using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class BattleStateMachine : MonoBehaviour
{

    public enum PerformAction
    {
        Wait,
        TakeAction,
        PerformAction,
        CheckAlive,
        Win,
        Lose
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

    //GUI selection panels 
    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;
    public GameObject MagicPanel;

    //hero attacks
    public Transform actionSpacer;
    public Transform magicSpacer;
    public GameObject actionButton;
    public GameObject magicButton;
    private List<GameObject> attackButtons = new List<GameObject>();


    //enemy buttons
    private List<GameObject> enemyBtns = new List<GameObject>();

    

    void Start()
    {
        battlestates = PerformAction.Wait;
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HerosInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
        HeroInput = HeroGui.Activate;

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);
        MagicPanel.SetActive(false);
        //enemies to select 
        EnemyButtons();
    }

    // Update is called once per frame
    void Update()
    {
        switch (battlestates)
        {
            case (PerformAction.Wait):
                if (PerformList.Count > 0)
                {
                    battlestates = PerformAction.TakeAction;
                }



                break;

            case (PerformAction.TakeAction):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if (PerformList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    for (int i = 0; i < HerosInBattle.Count; i++)
                    {
                        if (PerformList[0].AttackersTarget == HerosInBattle[i])
                        {
                            ESM.heroToAttack = PerformList[0].AttackersTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.Action;
                            break;
                        }
                        else
                        {
                            PerformList[0].AttackersTarget = HerosInBattle[Random.Range(0, HerosInBattle.Count)];
                            ESM.heroToAttack = PerformList[0].AttackersTarget;
                            ESM.currentState = EnemyStateMachine.TurnState.Action;
                        }
                    }
                }



                if (PerformList[0].Type == "Hero")
                {
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
                    HSM.EnemyToAttack = PerformList[0].AttackersTarget;
                    HSM.currentState = HeroStateMachine.TurnState.Action;
                }

                battlestates = PerformAction.PerformAction;
                break;

            case (PerformAction.PerformAction):
                    //idle    
                break;

            case (PerformAction.CheckAlive):
                if (HerosInBattle.Count < 1)
                {
                    battlestates = PerformAction.Lose;
                    //lose the battle
                }
                else if (EnemiesInBattle.Count < 1)
                {
                    battlestates = PerformAction.Win;
                    //win the battle
                    //gain experience
                    //gain money 
                }
                else
                {
                    //call function
                    ClearAttackPanel();
                    HeroInput = HeroGui.Activate;
                }
                break;

            case (PerformAction.Win):
                {
                    Debug.Log("You won the battle!");
                        for (int i = 0; i < HerosInBattle.Count; i++)
                        {
                        HerosInBattle[i].GetComponent<HeroStateMachine>().currentState = HeroStateMachine.TurnState.Waiting;
                        }
                }
                break;

            case (PerformAction.Lose):
                {
                    Debug.Log("You lost the battle!");
                }
                break;


        }

        switch (HeroInput)
        {
            case (HeroGui.Activate):
                if (HeroesToManage.Count > 0)
                {
                    HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                    HeroChoice = new HandleTurn();

                    AttackPanel.SetActive(true);

                    //populate action buttons 
                    CreateAttackButtons();

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

    public void EnemyButtons()
    {
        //clean up
        foreach(GameObject enemyBtn in enemyBtns)
        {
            Destroy(enemyBtn); 
        }
        enemyBtns.Clear();

        //create buttons
        foreach (GameObject enemy in EnemiesInBattle)
        {

            GameObject newButton = Instantiate(enemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine>();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = cur_enemy.enemy.name;

            button.EnemyPrefab = enemy;

            newButton.transform.SetParent(Spacer);
            newButton.transform.localScale = new Vector3(1, 1, 1);// Note: Fix in inspector eventually... 

            enemyBtns.Add(newButton);
        }
    }

    public void Input1()//attack button
    {
        HeroChoice.Attacker = HeroesToManage[0].name;
        HeroChoice.AttackersGameObject = HeroesToManage[0];
        HeroChoice.Type = "Hero";
        HeroChoice.chosenAttack = HeroesToManage[0].GetComponent<HeroStateMachine>().hero.attacks[0];
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
        // clean the attackpanel
        ClearAttackPanel();
        EnemySelectPanel.SetActive(false);

        HeroesToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        HeroesToManage.RemoveAt(0);
        HeroInput = HeroGui.Activate;
    }

    void ClearAttackPanel()
    {
        EnemySelectPanel.SetActive(false);
        AttackPanel.SetActive(false);
        MagicPanel.SetActive(false);

        foreach (GameObject attackButton in attackButtons)
        {
            Destroy(attackButton);
        }
        attackButtons.Clear();
    }


    //creeate actionbuttons
    void CreateAttackButtons()
    {
        GameObject AttackButton = Instantiate(actionButton) as GameObject;
        Text AttackButtonText = AttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        AttackButtonText.text = "Attack";
        AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());
        AttackButton.transform.SetParent(actionSpacer, false);
        attackButtons.Add(AttackButton);

        GameObject MagicAttackButton = Instantiate(actionButton) as GameObject;
        Text MagicAttackButtonText = MagicAttackButton.transform.Find("Text").gameObject.GetComponent<Text>();
        MagicAttackButtonText.text = "Magic";
        MagicAttackButton.GetComponent<Button>().onClick.AddListener(() => Input3());
        MagicAttackButton.transform.SetParent(actionSpacer, false);
        attackButtons.Add(MagicAttackButton);

        if (HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks.Count > 0)
        {
            foreach (BaseAttack magicAtk in HeroesToManage[0].GetComponent<HeroStateMachine>().hero.MagicAttacks)
            {
                GameObject MagicButton = Instantiate(magicButton) as GameObject;
                Text MagicButtonText = MagicButton.transform.Find("Text").gameObject.GetComponent<Text>();
                MagicButtonText.text = magicAtk.attackName;
                AttackButton ATB = MagicButton.GetComponent<AttackButton>();
                ATB.magicAttackToPerform = magicAtk;
                MagicButton.transform.SetParent(magicSpacer, false);
                attackButtons.Add(MagicButton);
            }
        }

        else
        {
            MagicAttackButton.GetComponent<Button>().interactable = false;
        }
    }

    public void Input3()//switching to magic attacks 
    {
        AttackPanel.SetActive(false);
        MagicPanel.SetActive(true);
    }


    public void Input4(BaseAttack choosenMagic)//choosen magic attack
    {
        HeroChoice.Attacker = HeroesToManage[0].name;
        HeroChoice.AttackersGameObject = HeroesToManage[0];
        HeroChoice.Type = "Hero";

        HeroChoice.chosenAttack = choosenMagic;
        MagicPanel.SetActive(false);
        EnemySelectPanel.SetActive(true);
    }
}



