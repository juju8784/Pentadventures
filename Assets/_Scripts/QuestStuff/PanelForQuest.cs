using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelForQuest : MonoBehaviour
{
    [SerializeField] QuestBoardObject questBoard;

    public List<GameObject> blocks = new List<GameObject>();

    // UI toggle stuff
    [SerializeField] GameObject questPanel;
    bool showing;

    private void Start()
    {
        for (int i = 0; i < questBoard.active.Count; i++)
        {
            if (blocks[i])
            {
                blocks[i].SetActive(true);
                Transform[] t = blocks[i].gameObject.GetComponentsInChildren<Transform>(true);
                foreach (var item in t)
                {
                    if (item.tag == "Description")
                    {
                        item.GetComponent<TextMeshProUGUI>().text = questBoard.active[i].description;
                    }
                    if (item.tag == "Progress")
                    {
                        item.GetComponent<TextMeshProUGUI>().text = questBoard.active[i].goal.currentAmount.ToString();
                    }
                    if (item.tag == "TotalProgress")
                    {
                        item.GetComponent<TextMeshProUGUI>().text = questBoard.active[i].goal.requiredAmount.ToString();
                    }
                }
            }
        }
        for (int i = questBoard.active.Count; i < blocks.Count; i++)
        {
            if (blocks[i])
            {
                blocks[i].SetActive(false);
            }
        }
        showing = false;
        questPanel.SetActive(false);
    }

    public void UpdateBoard()
    {
        questPanel.SetActive(true);
        for (int i = 0; i < questBoard.active.Count; i++)
        {
            if (blocks[i])
            {
                blocks[i].SetActive(true);
                Transform[] t = blocks[i].gameObject.GetComponentsInChildren<Transform>(true);
                foreach (var item in t)
                {
                    if (item.tag == "Description")
                    {
                        item.GetComponent<TextMeshProUGUI>().text = questBoard.active[i].description;
                    }
                    else if (item.tag == "Progress")
                    {
                        item.GetComponent<TextMeshProUGUI>().text = questBoard.active[i].goal.currentAmount.ToString();
                    }
                    else if (item.tag == "TotalProgress")
                    {
                        item.GetComponent<TextMeshProUGUI>().text = questBoard.active[i].goal.requiredAmount.ToString();
                    }
                    //else if (item.name == "Slash")
                    //{
                    //    item.GetComponent<TextMeshProUGUI>().text = "/";
                    //}
                }
            }
        }
        //for (int i = 0; i < questBoard.completed.Count; i++)
        //{
        //    if (blocks[questBoard.active.Count - 1 + i])
        //    {
        //        blocks[questBoard.active.Count - 1 + i].SetActive(true);
        //        Transform[] t = blocks[i].gameObject.GetComponentsInChildren<Transform>(true);
        //        foreach (var item in t)
        //        {
        //            if (item.tag == "Description")
        //            {
        //                item.GetComponent<TextMeshProUGUI>().text = questBoard.completed[i].description;
        //            }
        //            else if (item.tag == "Progress")
        //            {
        //                item.GetComponent<TextMeshProUGUI>().text = "";
        //            }
        //            else if (item.tag == "TotalProgress")
        //            {
        //                item.GetComponent<TextMeshProUGUI>().text = "Complete";
        //            }
        //            else if(item.name == "Slash")
        //            {
        //                item.GetComponent<TextMeshProUGUI>().text = "";
        //            }
        //        }
        //    }
        //}
        for (int i = questBoard.active.Count; i < blocks.Count; i++)
        {
            if (blocks[i])
            {
                blocks[i].SetActive(false);
            }
        }
        questPanel.SetActive(showing);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            showing = !showing;
            questPanel.SetActive(showing);
        }
    }
}
