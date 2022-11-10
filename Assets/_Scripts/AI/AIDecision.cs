using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class AIDecision : MonoBehaviour
{
    public string DecisionName;

    [SerializeField] private bool isActive = false;
    public bool isActiveDecision
    {
        get { return isActive; }
        set { isActive = value; }
    }
    protected StateMachine stateMachine;

    //Keeps the current value of the decision
    [Tooltip("DONT TOUCH")]
    /*[ReadOnly]*/ public bool CurrentValue;


    private void OnGUI()
    {
        stateMachine = GetComponent<StateMachine>();
        if (stateMachine == null)
        {
            stateMachine = gameObject.AddComponent<StateMachine>();
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    //Runs the decision, then returns the outcome
    public virtual bool RunDecision()
    {
        return CurrentValue;
    }
}
