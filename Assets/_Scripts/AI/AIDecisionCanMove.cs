using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecisionCanMove : AIDecision
{
    public override bool RunDecision()
    {
        CurrentValue = stateMachine.stats.CanMove();
        return CurrentValue;
    }
}
