using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManagementGH : MonoBehaviour
{
    [SerializeField] QuestBoardObject questInfoBlock;
    //[SerializeField] CardsInitializer cardInit;

    public List<GameObject> activeblockObjects;
    public List<GameObject> inactiveblockObjects;
    public List<GameObject> completedblockObjects;
    public void Start()
    {
        if (inactiveblockObjects.Count == 0)
        {
            Transform[] temp = gameObject.GetComponentsInChildren<Transform>(false);

            foreach (Transform item in temp)
            {
                if (item.gameObject.tag == "ActiveQuestSlot")
                {
                    activeblockObjects.Add(item.gameObject);
                }
                if (item.gameObject.tag == "NewQuestSlot")
                {
                    inactiveblockObjects.Add(item.gameObject);
                }
                if (item.gameObject.tag == "CompletedQuestSlot")
                {
                    completedblockObjects.Add(item.gameObject);
                }
            }
        }
        InitializeUI();
    }

    public void AcceptQuest(QuestPanel q)
    {
        questInfoBlock.ActivateQuest(q.block);
        InitializeUI();
    }

    void InitializeUI()
    {
        // initialize new quests
        for (int i = 0; i < questInfoBlock.inactives.Count; i++)
        {
            if (i < inactiveblockObjects.Count)
            {
                inactiveblockObjects[i].GetComponent<QuestPanel>().ModifyPanel(questInfoBlock.inactives[i]);
                inactiveblockObjects[i].SetActive(true);
            }
        }
        // setactive(false) to new quests stuff
        for (int i = questInfoBlock.inactives.Count; i < inactiveblockObjects.Count; i++)
        {
            if (i < inactiveblockObjects.Count)
            {
                inactiveblockObjects[i].SetActive(false);
            }
        }
        // initialize active quests
        if (questInfoBlock.active.Count > 0)
        {
            for (int i = 0; i < questInfoBlock.active.Count; i++)
            {
                if (i < activeblockObjects.Count)
                {
                    activeblockObjects[i].GetComponent<QuestPanel>().ModifyPanel(questInfoBlock.active[i]);
                    activeblockObjects[i].SetActive(true);
                }
            }
        }
        // setactive(false) to active quests stuff
        for (int i = questInfoBlock.active.Count; i < activeblockObjects.Count; i++)
        {
            if (i < activeblockObjects.Count)
            {
                activeblockObjects[i].SetActive(false);
            }
        }
        // initialize completed quests
        if (questInfoBlock.completed.Count > 0)
        {
            for (int i = 0; i < questInfoBlock.completed.Count; i++)
            {
                if (i < completedblockObjects.Count)
                {
                    completedblockObjects[i].GetComponent<QuestPanel>().ModifyPanel(questInfoBlock.completed[i]);
                    completedblockObjects[i].SetActive(true);
                }
            }
        }
        // setactive(false) to completed quests stuff
        for (int i = questInfoBlock.completed.Count; i < completedblockObjects.Count; i++)
        {
            if (i < completedblockObjects.Count)
            {
                completedblockObjects[i].SetActive(false);
            }
        }

    }
}
