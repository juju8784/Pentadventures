using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardsInitializer : MonoBehaviour
{
    public Image artwork;
    public TextMeshProUGUI charaName;
    public List<GameObject> statBlock = new List<GameObject>();

    public List<Sprite> stars = new List<Sprite>();
    ChangeText temp;
    public Button upgradeButton;
    CharaStatBlock block;
    public void StartBlock(GameObject objectInUI, CharaStatBlock charaStatBlock) //, Transform position)
    {
        block = Instantiate(charaStatBlock);
        // getting the right children to alter
        objectInUI.GetComponent<SlotStatsSheetHolder>().charaStat = charaStatBlock;
        Transform[] transformTemp = objectInUI.GetComponentsInChildren<Transform>(true);
        // get current party members statsHolder from player prefs
        foreach (var to in transformTemp)
        {
            if (to.name == "Name")
            {
                charaName = to.GetComponent<TextMeshProUGUI>();
                break;
            }
        }
        statBlock.Clear();
        foreach (Transform to in transformTemp)
        {
            if (to.gameObject.tag == "Stats")
            {
                if (!to.gameObject.GetComponent<ChangeText>())
                {
                    statBlock.Add(to.gameObject);
                }
            }
            if (to.gameObject.tag == "UpButton")
            {
                upgradeButton = to.gameObject.GetComponent<Button>();
            }
            if (to.gameObject.tag == "StarLvl")
            {
                if (to.gameObject.GetComponent<Image>())
                {
                   to.gameObject.GetComponent<Image>().sprite = stars[block.starLevel - 1];
                }
            }
        }
        charaName.text = block.characterName;
        InitializeUI();
    }

    public void UpdateButton(GameObject objectInUI, CharaStatBlock charaStatBlock)
    {

        Transform[] transformTemp = objectInUI.GetComponentsInChildren<Transform>(true);
        block = Instantiate(charaStatBlock);

        statBlock.Clear();
        foreach (Transform to in transformTemp)
        {
            if (to.gameObject.tag == "Stats")
            {
                if (!to.gameObject.GetComponent<ChangeText>())
                {
                    statBlock.Add(to.gameObject);
                }
            }
            if (to.gameObject.tag == "UpButton")
            {
                upgradeButton = to.gameObject.GetComponent<Button>();
            }
            if (to.gameObject.tag == "StarLvl")
            {
                if (to.gameObject.GetComponent<Image>())
                {
                    to.gameObject.GetComponent<Image>().sprite = stars[block.starLevel - 1];
                }
            }
        }
        InitializeUI();
    }


    public void InitializeUI()
    {

        #region Stats

        temp = statBlock[0].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(block.vitality.ToString());
        }
        else
        {
            Debug.Log("statBlock[0] get children was null.");
        }

        temp = statBlock[1].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(block.dexterity.ToString());
        }
        else
        {
            Debug.Log("statBlock[1] get children was null.");
        }

        temp = statBlock[2].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(block.strength.ToString());
        }
        else
        {
            Debug.Log("statBlock[2] get children was null.");
        }

        temp = statBlock[3].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(block.intelligence.ToString());
        }
        else
        {
            Debug.Log("statBlock[3] get children was null.");
        }

        temp = statBlock[4].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(block.wisdom.ToString());
        }
        else
        {
            Debug.Log("statBlock[4] get children was null.");
        }

        #endregion

    }
}
