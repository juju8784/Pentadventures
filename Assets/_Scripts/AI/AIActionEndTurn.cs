using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActionEndTurn : AIAction
{
    public override void RunAction()
    {
        //need isa work
        //GameManager.instance.combatManager.EndUnitsTurn();
        ActionFinished = true;
        //stateMachine.aiTurnManager.UpdateTurn(stateMachine, false);
        stateMachine.aiTurnManager.NextTurn(stateMachine);
    }
}
