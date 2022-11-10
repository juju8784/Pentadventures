using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManagementGH : MonoBehaviour
{
    [SerializeField] PartyInfoBlock pInfoBlock;
    [SerializeField] CardsInitializer cardInit;
    //List<GameObject> children = new List<GameObject>();

    public List<GameObject> activeblockObjects;
    public List<GameObject> inactiveblockObjects;
    public List<GameObject> blockObjects;
    public void Start()
    {
        if (activeblockObjects.Count == 0)
        {
            Transform[] temp = gameObject.GetComponentsInChildren<Transform>(false);

            foreach (Transform item in temp)
            {
                if (item.gameObject.tag == "ActiveSlot")
                {
                    activeblockObjects.Add(item.gameObject);
                }
                if (item.gameObject.tag == "DisabledSpot")
                {
                    inactiveblockObjects.Add(item.gameObject);
                }
                if (item.gameObject.tag == "Block")
                {
                    blockObjects.Add(item.gameObject);
                }
            }
        }
        InitializeUI();
    }

    void InitializeUI()
    {
        
        for (int i = 0; i < pInfoBlock.members.Count; i++)
        {
            cardInit.StartBlock(blockObjects[i], pInfoBlock.members[i]);
            blockObjects[i].transform.position = activeblockObjects[i].transform.position;
        }
        if (pInfoBlock.inactives.Count > 0)
        {
            for (int i = 0; i < pInfoBlock.inactives.Count; i++)
            {
                cardInit.StartBlock(blockObjects[pInfoBlock.members.Count + i], pInfoBlock.inactives[i]);
                blockObjects[pInfoBlock.members.Count + i].transform.position = inactiveblockObjects[i].transform.position;
            }
        }
        for (int i = pInfoBlock.inactives.Count + pInfoBlock.members.Count; i < blockObjects.Count; i++)
        {
            if (blockObjects[i])
            {
                blockObjects[i].SetActive(false);
            }
        }
        
    }
    public void UpdateUI()
    {
        if (blockObjects.Count > 0)
        {
            for (int i = 0; i < pInfoBlock.members.Count; i++)
            {
                cardInit.UpdateButton(blockObjects[i], pInfoBlock.members[i]);
                //blockObjects[i].transform.position = activeblockObjects[i].transform.position;
            }
            if (pInfoBlock.inactives.Count > 0)
            {
                for (int i = 0; i < pInfoBlock.inactives.Count; i++)
                {
                    cardInit.UpdateButton(blockObjects[pInfoBlock.members.Count + i], pInfoBlock.inactives[i]);
                    //blockObjects[pInfoBlock.members.Count + i].transform.position = inactiveblockObjects[i].transform.position;
                }
            }
        }
        
    }
}
