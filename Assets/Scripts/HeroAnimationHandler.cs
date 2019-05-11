using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimationHandler : MonoBehaviour {

    public GameObject Hero;
    public Animator anim;
    private HeroStateMachine currentState;


    // Use this for initialization
    void Start () {

        anim = GetComponent<Animator>();
        
    }
	
	// Update is called once per frame
	void Update () {

        currentState = Hero.GetComponent<HeroStateMachine>();


        if (currentState.currentState == HeroStateMachine.TurnState.Action)
        {
            anim.SetTrigger("Attack");
        }

        if (currentState.currentState == HeroStateMachine.TurnState.Processing || currentState.currentState == HeroStateMachine.TurnState.Waiting)
        {
            anim.SetTrigger("Stationary");
        }
    }
}
