using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIState : MonoBehaviour
{
    public string stateName;

    [Tooltip("DO NOT TOUCH THIS. EVER")]
    /*[ReadOnly]*/
    public bool isActive = false;
    public bool isActiveState
    {
        get { return isActive; }
        set { isActive = value; }
    }

    public string[] actionNames;
    public Decision[] decisions;

    protected StateMachine enemyStateMachine;


    private void OnGUI()
    {
        enemyStateMachine = GetComponent<StateMachine>();
        if (enemyStateMachine == null)
        {
            enemyStateMachine = gameObject.AddComponent<StateMachine>();
        }
    }

    private void Awake()
    {
        enemyStateMachine = GetComponent<StateMachine>();
    }

    private void Update()
    {

    }

    //Will activate this state along with all of its actions and decisions
    //You can also deactivate them by using false as the parameter
    public virtual void ActivateState(bool boolState = true)
    {
        isActiveState = boolState;
        for (int i = 0; i < actionNames.Length; i++)
        {
            enemyStateMachine.ActivateAction(actionNames[i], boolState);
        }

        for (int i = 0; i < decisions.Length; i++)
        {
            enemyStateMachine.ActivateDecision(decisions[i].decisionName, boolState);
        }
    }
}

//Used for holding the decisions
//Leave either the true or false state blank to stay in the current state
[Serializable]
public class Decision
{
    public string decisionName;

    //The state that the controller will be directed to if true
    public string trueState;

    //The state that the controller will be directed to if false
    public string falseState;

    public Decision()
    {
        decisionName = "";
        trueState = "";
        falseState = "";
    }

    public Decision(string dn, string ts, string fs)
    {
        decisionName = dn;
        trueState = ts;
        falseState = fs;
    }
}