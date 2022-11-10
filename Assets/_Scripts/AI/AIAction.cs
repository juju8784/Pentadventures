using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAction : MonoBehaviour
{
    public string ActionName;

    [SerializeField] private bool isActive = false;
    public bool ActionFinished = false;
    public bool isActiveAction
    {
        get { return isActive; }
        set
        {
            isActive = value;
            ActionFinished = false;
            OnActiveChange();
        }
    }

    protected StateMachine stateMachine;

    private void OnGUI()
    {
        stateMachine = GetComponent<StateMachine>();
        if (stateMachine == null)
        {
            stateMachine = gameObject.AddComponent<StateMachine>();
        }

    }

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        stateMachine = GetComponent<StateMachine>();
    }


    protected virtual void Update()
    {
        //Override this if you want it to happen always
    }

    //Runs the action
    public virtual void RunAction()
    {
        //Override this method if you want the action to be uninterruptable
    }

    //Runs when isActiveAction is changed
    public virtual void OnActiveChange()
    {
        //Override this method in any actions that you see fit
    }
}
