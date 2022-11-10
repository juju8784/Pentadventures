using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Quest Holder Block", menuName = "Quest Holder Container")]
public class QuestBoardObject : ScriptableObject
{
    public List<QuestInfoBlock> inactives = new List<QuestInfoBlock>();
    public List<QuestInfoBlock> active = new List<QuestInfoBlock>();
    public List<QuestInfoBlock> completed = new List<QuestInfoBlock>();

    public bool ActivateQuest(QuestInfoBlock quest)
    {
        if (RemoveFromInactiveList(quest))
        {
            quest.isActive = true;
            AddToActiveList(quest);
        }
        else
        {
            Debug.Log("Quest not found/non-existant");
        }
        return true;
    }

    public void CompleteQuest(QuestInfoBlock quest)
    {
        if (active.Contains(quest))
        {
            quest.isActive = false;
            RemoveFromActiveList(quest);
            AddToCompletedList(quest);
        }
        else
        {
            Debug.Log("Quest not active. Cannot complete");
        }
    }

    public void ResetAllQuests()
    {
        if (completed != null) 
        {
            while(completed.Count > 0)
            {
                completed[0].goal.currentAmount = 0;
                completed[0].claimed = false;
                AddToInactiveList(completed[0]);
                RemoveFromCompletedList(completed[0]);
            }
        }
        
        if (active != null)
        {
            while (active.Count > 0)
            {
                active[0].isActive = false;
                active[0].goal.currentAmount = 0;
                AddToInactiveList(active[0]);
                RemoveFromActiveList(active[0]);  
            }
            foreach (var item in inactives)
            {
                if(item.title == "Socialite")
                {
                    ActivateQuest(item);
                    break;
                }
            }
        }
    }

    public void AddToCompletedList(QuestInfoBlock pc)
    {
        if (!completed.Contains(pc))
        {
            completed.Add(pc);
        }
    }
    public bool RemoveFromCompletedList(QuestInfoBlock pc)
    {
        if (completed.Contains(pc))
        {
            completed.Remove(pc);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void AddToActiveList(QuestInfoBlock pc)
    {
        if (!active.Contains(pc))
        {
            active.Add(pc);
        }
    }
    public bool RemoveFromActiveList(QuestInfoBlock pc)
    {
        if (active.Contains(pc))
        {
            active.Remove(pc);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddToInactiveList(QuestInfoBlock pc)
    {
        if (!inactives.Contains(pc))
        {
            inactives.Add(pc);
        }
    }
    public bool RemoveFromInactiveList(QuestInfoBlock pc)
    {
        if (inactives.Contains(pc))
        {
            inactives.Remove(pc);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetPartyCount()
    {
        return active.Count;
    }

    public bool Contains(QuestInfoBlock pc)
    {
        return active.Contains(pc);
    }

}
