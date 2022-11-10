using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestPanel : MonoBehaviour
{
    public TextMeshProUGUI panelTitle;
    public TextMeshProUGUI panelDescription;
    public TextMeshProUGUI currentGoal;
    public TextMeshProUGUI claimText;
    public GameObject claimButton;
    public QuestInfoBlock block;

    public void ModifyPanel(QuestInfoBlock quest)
    {
        block = quest;
        panelTitle.text = block.title;
        panelDescription.text = block.description;
        if (currentGoal)
        {
            currentGoal.text = block.goal.currentAmount.ToString();
        }
        if (claimButton)
        {
            if (quest.claimed)
            {
                claimButton.SetActive(false);
                claimText.gameObject.SetActive(true);
            }
            else
            {
                claimButton.GetComponent<Button>().onClick.AddListener(quest.Claim);
            }
        }
    }


}
