using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal 
{
    public GoalType goalType;
    public EnemyType entityType;
    public EncounterType encounterType;
    public StarLevel starLevel;
    public int requiredAmount; 
    public int currentAmount; 

    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }

    public void IncreaseGoalAmount()
    {
        currentAmount++;
    }

    public void IncreaseGoalAmount(int customAmount)
    {
        currentAmount += customAmount;
    }

}

public enum GoalType
{
    Kill, 
    Encounter, 
    Misc,
    Upgrade
}

public enum EnemyType
{
    None,
    Rogue,
    Mercenary,
    Any
}
public enum StarLevel
{
    Any,
    One,
    Two,
    Three,
    Four,
    Five
}
public enum EncounterType
{
    None,
    NewCharacter,
    Enemy,
    Any,
}