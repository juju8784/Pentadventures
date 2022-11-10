using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyUIManager : MonoBehaviour
{
    public PartyManager manager;
    CombatManagement combat;
    public List<GameObject> blockObjects;
    List<Animator> blockAnims = new List<Animator>();
    List<StatBlock> statBlocksScripts = new List<StatBlock>();
    List<GameObject> members;
    public List<Turn> turnCopy;

    bool toggle = true;
    bool managerOn = false;
    bool initUICalled = false;

    public void StartPartyUI()
    {
        if (!manager)
        {
            manager = GameObject.FindObjectOfType<PartyManager>();
        }
        combat = GameManager.instance.combatManagement;
        if (!managerOn)
        {
            manager.Initalize();
            managerOn = true;
        }
        if(blockObjects.Count == 0)
        {
            Transform[] temp = gameObject.GetComponentsInChildren<Transform>(false);

            foreach (Transform item in temp)
            {
               if (item.gameObject.tag == "Block")
               {
                   blockObjects.Add(item.gameObject);
               }
            }
        }
        if(blockObjects.Count > 5)
        {
            Debug.Log("Too many blocks. Something went wrong");
        }
        if (!manager.active)
        {
            manager.active = GameObject.FindObjectOfType<ActiveParty>();
        }
        if (members == null)
        {
            members = manager.active.GetMembers();
            for (int i = 0; i < members.Count; i++)
            {
                statBlocksScripts.Add(members[i].GetComponent<StatBlock>());
            }
        }
        if (!initUICalled)
        {
            InitializeUI();
        }
    }

    public void UpdatePartyUI()
    {
        turnCopy = combat.turns;

        if (turnCopy.Count > 0)
        {
            while (turnCopy[0].id != combat.currentTurnID)
            {
                turnCopy.Add(turnCopy[0]);
                turnCopy.RemoveAt(0);
            }

            statBlocksScripts.Clear();
            blockAnims.Clear();
            for (int i = 0; i < turnCopy.Count; i++)
            {
                statBlocksScripts.Add(turnCopy[i].GetComponent<StatBlock>());
            }
            for (int i = 0; i < blockObjects.Count; i++)
            {
                blockAnims.Add(blockObjects[i].GetComponent<Animator>());
            }
            //foreach(var item in blockAnims)
            //{
            //    item.Play("animate");
            //}
            for (int i = 0; i < statBlocksScripts.Count; i++)
            {
                if (i < 5)
                {
                    statBlocksScripts[i].StartBlock(blockObjects[i], turnCopy[i].GetComponent<StatsHolder>());
                    blockAnims[i].Play("animate");
                    //blockObjects[i].GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
                }
            }
            //blockObjects[0].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            for (int i = statBlocksScripts.Count; i < blockObjects.Count; i++)
            {
                if (blockObjects[i])
                {
                    blockObjects[i].SetActive(false);
                }
            }
        }
    }

    public void UpdatePartyUIOutOfCombat()
    {
        
        statBlocksScripts.Clear();
        for (int i = 0; i < members.Count; i++)
        {
            statBlocksScripts.Add(members[i].GetComponent<StatBlock>());
        }
        for (int i = 0; i < statBlocksScripts.Count; i++)
        {
            if (i < 5)
            {
                statBlocksScripts[i].StartBlock(blockObjects[i], members[i].GetComponent<StatsHolder>());
                //blockObjects[i].GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
        }
        //blockObjects[0].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        for (int i = statBlocksScripts.Count; i < blockObjects.Count; i++)
        {
            if (blockObjects[i])
            {
                blockObjects[i].SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (!GameManager.instance.paused)
        {
            if (Input.GetKeyDown(ControlsManager.Party))
            {
                toggle = !toggle;
            }
            ToggleOnOff();
        }
    }

    void ToggleOnOff()
    {
        if (toggle)  // on
        {
            for (int i = 0; i < statBlocksScripts.Count; i++)
            {
                if (i < 5)
                {
                    if (blockObjects[i])
                    {
                        blockObjects[i].SetActive(true);
                    }
                }
            }
        }
        else // off
        {
            foreach (var item in blockObjects)
            {
                item.SetActive(false);
            }
        }
    }

    public void InitializeUI()
    {

        // --------------------- error checks to make sure we got all the info we need
        initUICalled = true;
        if (members == null)
        {
            if (!managerOn)
            {
                manager.Initalize();
                managerOn = true;
            }
            members = manager.active.GetMembers();
            for (int i = 0; i < members.Count; i++)
            {
                statBlocksScripts.Add(members[i].GetComponent<StatBlock>());
            }
        }
        if (blockObjects.Count == 0)
        {
            Transform[] temp = gameObject.GetComponentsInChildren<Transform>(false);

            foreach (Transform item in temp)
            {
                if (item.gameObject.tag == "Block")
                {
                    blockObjects.Add(item.gameObject);
                }
            }
        }
        // -------------------------------  end of error checks


        for (int i = 0; i < statBlocksScripts.Count; i++)
        {
            statBlocksScripts[i].StartBlock(blockObjects[i], members[i].GetComponent<StatsHolder>()); //, blockObjects[0].transform);
        }
        for (int i = statBlocksScripts.Count; i < blockObjects.Count; i++)
        {
            if (blockObjects[i])
            {
                blockObjects[i].SetActive(false);
            }
        }
    }

}
