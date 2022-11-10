using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class StateMachine : MonoBehaviour
{
    public GameObject itemDrop;

    public AIEnemyController EnemyController;
    public Health health;
    public StatsHolder stats;

    public WorldAITurn aiTurnManager;

    public string StartingStateName;
    //Keeps track of the current state
    /*[ReadOnly]*/ public string currentStateName;
    private AIState currentState;

    //Lists of the different states, actions, and decisions
    private List<AIState> states;
    private List<AIAction> actions;
    private List<AIDecision> decisions;

    //Variable for turn management
    //OBSOLETE
    //public bool isMyTurn;

    #region Helper Functions

    //Adds a state to the states list
    public void AddState(AIState state)
    {
        if (!states.Contains(state))
            states.Add(state);
    }

    //Adds an action to the action list
    public void AddAction(AIAction action)
    {
        if (!actions.Contains(action))
            actions.Add(action);
    }

    //Adds a decision to the decision list
    public void AddDecision(AIDecision decision)
    {
        if (!decisions.Contains(decision))
            decisions.Add(decision);
    }
    public void ResetAll()
    {
        for (int i = 0; i < states.Count; i++)
            states[i].ActivateState(false);
        for (int i = 0; i < actions.Count; i++)
            ActivateAction(actions[i].ActionName, false);
        for (int i = 0; i < decisions.Count; i++)
            ActivateDecision(decisions[i].DecisionName, false);
    }

    //Takes in the action name and returns the AIAction if it exists
    public AIAction GetAction(string actionName)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].ActionName.Equals(actionName))
                return actions[i];
        }
        return null;
    }

    //Takes in the decision name and returns the AIDecision if it exists
    public AIDecision GetDecision(string decisionName)
    {
        for (int i = 0; i < decisions.Count; i++)
        {
            if (decisions[i].DecisionName.Equals(decisionName))
                return decisions[i];
        }
        return null;
    }
    #endregion

    // Start is called before the first frame update
    public virtual void Start()
    {
        #region General Variables
        if (aiTurnManager)
        {
            aiTurnManager.AddAITurn(this);
        }
        else
        {
            aiTurnManager = GameManager.instance.aiTurns;
        }
        #endregion

        #region States, Actions, Decisions Setup
        states = new List<AIState>();
        actions = new List<AIAction>();
        decisions = new List<AIDecision>();

        AIState[] stateArray = GetComponents<AIState>();
        AIAction[] actionArray = GetComponents<AIAction>();
        AIDecision[] decisionArray = GetComponents<AIDecision>();

        for (int i = 0; i < stateArray.Length; i++)
            AddState(stateArray[i]);

        for (int i = 0; i < actionArray.Length; i++)
            AddAction(actionArray[i]);

        for (int i = 0; i < decisionArray.Length; i++)
            AddDecision(decisionArray[i]);

        #endregion

        //Sets the starting state and activates it
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].stateName.Equals(StartingStateName))
            {
                currentStateName = states[i].stateName;
                currentState = states[i];
                states[i].ActivateState();
                return;
            }
        }
    }

    //This will run all of the actions and decisions based on the current state
    private void Update()
    {
        if (GameManager.instance.paused == false)
        {
            //Only runs if it is my turn
            if (aiTurnManager.IsMyTurn(this) && !GameManager.instance.isCombat)
            {
                RunAllDecisions();

                //Changes the state if necessary
                for (int i = 0; i < currentState.decisions.Length; i++)
                {
                    AIDecision currentDecision = GetDecision(currentState.decisions[i].decisionName);
                    // Grabs the state name
                    string inputState = "";
                    if (currentDecision.CurrentValue)
                        inputState = currentState.decisions[i].trueState;
                    else
                        inputState = currentState.decisions[i].falseState;

                    //If the state name is nothing, continues if false
                    if (inputState.Equals(""))
                    {
                        if (i == currentState.decisions.Length - 1)
                        {
                            inputState = currentStateName;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    bool success = SwitchState(inputState);
                    if (success)
                        break;
                }
                //Runs actions after making a decision on what to do
                RunAllActions();
                //StartCoroutine("RunAllActions");

            }   
        }
    }


    //Runs the specified action if the name exists
    public void RunAction(string actionName)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].ActionName.Equals(actionName))
            {
                actions[i].RunAction();
            }
        }
    }

    //Runs the specified decision if the name exists
    public void RunDecision(string decisionName)
    {
        for (int i = 0; i < decisions.Count; i++)
        {
            if (decisions[i].DecisionName.Equals(decisionName))
            {
                decisions[i].RunDecision();
                return;
            }
        }
    }

    //Runs all active actions
    public void RunAllActions()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].isActiveAction && !actions[i].ActionFinished)
                actions[i].RunAction();
        }
    }

    //Runs all active decisions
    public void RunAllDecisions()
    {
        for (int i = 0; i < decisions.Count; i++)
        {
            if (decisions[i].isActiveDecision)
                decisions[i].RunDecision();
        }
    }

    //Returns the value of the decision. If it doesn't find the decision, it returns false
    public bool CheckDecisionResult(string decisionName)
    {
        for (int i = 0; i < decisions.Count; i++)
        {
            if (decisions[i].DecisionName.Equals(decisionName))
            {
                return decisions[i].CurrentValue;
            }
        }

        Debug.Log("CheckDecisionResult FAILED: Invalid DecisionName");
        return false;
    }

    //Switches the current state to stateName. If it succeeds,
    //it returns true and false otherwise
    public bool SwitchState(string stateName)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].stateName.Equals(stateName))
            {
                currentState.ActivateState(false);
                states[i].ActivateState();
                currentStateName = states[i].stateName;
                currentState = states[i];
                return true;
            }
        }
        return false;
    }


    //Activates the specified action. You can also specify a bool to deactivate
    public void ActivateAction(string actionName, bool boolState)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].ActionName.Equals(actionName))
                actions[i].isActiveAction = boolState;
        }
    }

    //Activates the specified decision. You can also specify a bool to deactivate
    public void ActivateDecision(string decisionName, bool boolState = true)
    {
        for (int i = 0; i < decisions.Count; i++)
        {
            if (decisions[i].DecisionName.Equals(decisionName))
                decisions[i].isActiveDecision = boolState;
        }
    }

    public void StartTurn()
    {
        stats.TurnReset();
        SwitchState(StartingStateName);
    }
}
