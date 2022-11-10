using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDecisionInCombat : AIDecision
{
    public override bool RunDecision()
    {
        CurrentValue = GameManager.instance.isCombat;
        return CurrentValue;
    }
}
