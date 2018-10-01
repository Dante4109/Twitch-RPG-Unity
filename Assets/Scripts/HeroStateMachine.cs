using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour {

    private BattleStateMachine BSM;
    public BaseHero hero;

    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    public TurnState currentState;
    //for the progressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Image ProgressBar; 

    // Use this for initialization
	void Start ()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine> ();
        currentState = TurnState.PROCESSING;	
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

            case (TurnState.ADDTOLIST):
                BSM.HeroesToManage.Add(this.gameObject);
                currentState = TurnState.WAITING;

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
            float calc_cooldown = cur_cooldown / max_cooldown;
            ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
            if (cur_cooldown >= max_cooldown)
            {
                currentState = TurnState.ADDTOLIST;
            }


        }

	}

