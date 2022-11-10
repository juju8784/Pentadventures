using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // In game tracking of quests progress and completion status. 

    // Quest Manager is reached throught accessing the Game Manager instance

    [SerializeField] QuestBoardObject questContainer;
    [SerializeField] PanelForQuest panel;
    [SerializeField] GameObject dialogPrefab;

    GameObject dialog;

    List<QuestInfoBlock> actives = new List<QuestInfoBlock>();

    public void Start()
    {
        actives = questContainer.active;
    }

    public void SpawnDialog()
    {
        dialog = Instantiate(dialogPrefab);
        Canvas canvas = FindObjectOfType<Canvas>();
        dialog.transform.SetParent(canvas.transform);
        dialog.transform.SetSiblingIndex(dialog.transform.GetSiblingIndex() - 5);
        dialog.transform.localPosition = Vector3.zero;
        //dialog.transform.position = Camera.main.WorldToScreenPoint(dialog.transform.position);
    }

    public void RogueDie()
    {
        for (int i = 0; i < actives.Count; i++)
        {
            QuestInfoBlock item = actives[i];
            if (item.goal.encounterType == EncounterType.Enemy) 
            { 
                if (item.goal.entityType == EnemyType.Rogue)
                {
                    if (item.goal.goalType == GoalType.Kill)
                    {
                        if (StarLevel.Any == item.goal.starLevel)
                        {
                            item.goal.IncreaseGoalAmount();
                            if (item.goal.IsReached())
                            {
                                questContainer.CompleteQuest(item);
                                SpawnDialog();
                                i--;
                            }
                            panel.UpdateBoard();
                        }
                    }
                }
            }
        }
    }
    public void Rogue1StarDie()
    {
        for (int i = 0; i < actives.Count; i++)
        {
            QuestInfoBlock item = actives[i];
            if (item.goal.encounterType == EncounterType.Enemy)
            {
                if (item.goal.entityType == EnemyType.Rogue)
                {
                    if (item.goal.goalType == GoalType.Kill)
                    {
                        if (item.goal.starLevel == StarLevel.One)
                        {
                            item.goal.IncreaseGoalAmount();
                            if (item.goal.IsReached())
                            {
                                questContainer.CompleteQuest(item);
                                SpawnDialog();
                                i--;
                            }
                            panel.UpdateBoard();
                        }
                    }
                }
            }
        }
    }
    public void Rogue2StarDie()
    {
        for (int i = 0; i < actives.Count; i++)
        {
            QuestInfoBlock item = actives[i];
            if (item.goal.encounterType == EncounterType.Enemy)
            {
                if (item.goal.entityType == EnemyType.Rogue)
                {
                    if (item.goal.goalType == GoalType.Kill)
                    {
                        if (item.goal.starLevel == StarLevel.Two)
                        {
                            item.goal.IncreaseGoalAmount();
                            if (item.goal.IsReached())
                            {
                                questContainer.CompleteQuest(item);
                                SpawnDialog();
                                i--;
                            }
                            panel.UpdateBoard();
                        }
                    }
                }
            }
        }
    }
    public void Rogue3StarDie()
    {
        for (int i = 0; i < actives.Count; i++)
        {
            QuestInfoBlock item = actives[i];
            if (item.goal.encounterType == EncounterType.Enemy)
            {
                if (item.goal.entityType == EnemyType.Rogue)
                {
                    if (item.goal.goalType == GoalType.Kill)
                    {
                        if (item.goal.starLevel == StarLevel.Three)
                        {
                            item.goal.IncreaseGoalAmount();
                            if (item.goal.IsReached())
                            {
                                questContainer.CompleteQuest(item);
                                SpawnDialog();
                                i--;
                            }
                            panel.UpdateBoard();
                        }
                    }
                }
            }
        }
    }


    public void Rogue4StarDie()
    {
        for (int i = 0; i < actives.Count; i++)
        {
            QuestInfoBlock item = actives[i];
            if (item.goal.encounterType == EncounterType.Enemy)
            {
                if (item.goal.entityType == EnemyType.Rogue)
                {
                    if (item.goal.goalType == GoalType.Kill)
                    {
                        if (item.goal.starLevel == StarLevel.Four)
                        {
                            item.goal.IncreaseGoalAmount();
                            if (item.goal.IsReached())
                            {
                                questContainer.CompleteQuest(item);
                                SpawnDialog();
                                i--;
                            }
                            panel.UpdateBoard();
                        }
                    }
                }
            }
        }
    }
    public void Rogue5StarDie()
    {
        for (int i = 0; i < actives.Count; i++)
        {
            QuestInfoBlock item = actives[i];
            if (item.goal.encounterType == EncounterType.Enemy)
            {
                if (item.goal.entityType == EnemyType.Rogue)
                {
                    if (item.goal.goalType == GoalType.Kill)
                    {
                        if (item.goal.starLevel == StarLevel.Five)
                        {
                            item.goal.IncreaseGoalAmount();
                            if (item.goal.IsReached())
                            {
                                questContainer.CompleteQuest(item);
                                SpawnDialog();
                                i--;
                            }
                            panel.UpdateBoard();
                        }
                    }
                }
            }
        }
    }

    public void PlayerHealed(int healAmount)
    {
        for (int i = 0; i < actives.Count; i++)
        {
            QuestInfoBlock item = actives[i];
            if (item.goal.entityType == EnemyType.None)
            {
                if (item.goal.goalType == GoalType.Misc)
                {
                    item.goal.IncreaseGoalAmount(healAmount);
                    if (item.goal.IsReached())
                    {
                        questContainer.CompleteQuest(item);
                        SpawnDialog();
                        i--;
                    }
                        panel.UpdateBoard();
                }
            }
        }
    }

    public void FindNewCharacter()
    {
        for (int i = 0; i < actives.Count; i++)
        {
            QuestInfoBlock item = actives[i];
            if (item.goal.encounterType == EncounterType.NewCharacter)
            {
                if (item.goal.entityType == EnemyType.None)
                {
                    if (item.goal.goalType == GoalType.Encounter)
                    {
                        item.goal.IncreaseGoalAmount();
                        if (item.goal.IsReached())
                        {
                            questContainer.CompleteQuest(item);
                            SpawnDialog();
                            i--;
                        }
                            panel.UpdateBoard();
                    }
                }
            }
        }
    }

}
