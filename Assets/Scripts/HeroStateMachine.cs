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
    public GameObject Selector;

    // Use this for initialization
	void Start ()
    {
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

                break;

            case (TurnState.Action):

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

	}

