using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatBlock : MonoBehaviour
{
    public CharaStatBlock blocktemp;
    public Image artwork;
    public TextMeshProUGUI charaName;
    public List<Sprite> stars = new List<Sprite>();

    public List<GameObject> statBlock = new List<GameObject>();
    public List<GameObject> extraStatBlock = new List<GameObject>();
    //public GameObject objectInUI;
    StatsHolder stats;
    ChangeText temp;
    CharaStatBlock block;

    public void StartBlock(GameObject objectInUI, StatsHolder charaStats) //, Transform position)
    {
        block = Instantiate(blocktemp);
        // getting the right children to alter
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
        extraStatBlock.Clear();
        stats = charaStats; 
        foreach (Transform to in transformTemp)
        {
            if (to.gameObject.tag == "Stats")
            {
                if (!to.gameObject.GetComponent<ChangeText>())
                {
                    statBlock.Add(to.gameObject);
                }
            }
            if (to.gameObject.tag == "ExtraStats")
            {
                if (!to.gameObject.GetComponent<ChangeText>())
                {
                    extraStatBlock.Add(to.gameObject);
                }
            }
            if (to.gameObject.tag == "StarLvl")
            {
                if (to.gameObject.GetComponent<Image>())
                {
                    if(stats.StarLevel == 0)
                    {
                        stats.SetStats();
                    }
                    to.gameObject.GetComponent<Image>().sprite = stars[stats.StarLevel-1];
                }
            }
        }
        charaName.text = block.characterName;


        InitializeUI();
    }

    public void UIUpdateVitality(int newHealth)
    {
        temp = statBlock[0].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(newHealth.ToString());
        }
        else
        {
            Debug.Log("statBlock[0] get children was null.");
        }
    }

    public void InitializeUI()
    {

        #region Stats

        temp = statBlock[0].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            stats.SetStats();
            //temp.Change(block.vitality.ToString());
            temp.Change(stats.BaseVitality.ToString());
        }
        else
        {
            Debug.Log("statBlock[0] get children was null.");
        }

        temp = statBlock[1].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            //temp.Change(block.dexterity.ToString());
            temp.Change(stats.BaseDexterity.ToString());
        }
        else
        {
            Debug.Log("statBlock[1] get children was null.");
        }

        temp = statBlock[2].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            //temp.Change(block.strength.ToString());
            temp.Change(stats.BaseStrength.ToString());
        }
        else
        {
            Debug.Log("statBlock[2] get children was null.");
        }

        temp = statBlock[3].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            //temp.Change(block.intelligence.ToString());
            temp.Change(stats.BaseIntelligence.ToString());
        }
        else
        {
            Debug.Log("statBlock[3] get children was null.");
        }

        temp = statBlock[4].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            //temp.Change(block.wisdom.ToString());
            temp.Change(stats.BaseWisdom.ToString());
        }
        else
        {
            Debug.Log("statBlock[4] get children was null.");
        }

        #endregion

        #region ExtraStats

        temp = extraStatBlock[0].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            stats.SetStats();
            temp.Change(stats.CalculateEvasion().ToString());
        }
        else
        {
            Debug.Log("extraStatBlock[0] get children was null.");
        }

        temp = extraStatBlock[1].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(stats.CalculatePhyDamage().ToString());
        }
        else
        {
            Debug.Log("extraStatBlock[1] get children was null.");
        }

        temp = extraStatBlock[2].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(stats.HitRate.ToString());
        }
        else
        {
            Debug.Log("extraStatBlock[2] get children was null.");
        }

        temp = extraStatBlock[3].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change((stats.CalculatePhyDamage() / stats.HitRate).ToString() );
        }
        else
        {
            Debug.Log("extraStatBlock[3] get children was null.");
        }

        temp = extraStatBlock[4].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(stats.CalculateCritChance().ToString());
        }
        else
        {
            Debug.Log("extraStatBlock[4] get children was null.");
        }

        temp = extraStatBlock[5].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(stats.APointsRecoveryRate.ToString());
        }
        else
        {
            Debug.Log("extraStatBlock[5] get children was null.");
        }

        temp = extraStatBlock[6].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(stats.CalculateMovement().ToString());
        }
        else
        {
            Debug.Log("extraStatBlock[6] get children was null.");
        }

        temp = extraStatBlock[7].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(stats.CalculateArmor().ToString());
        }
        else
        {
            Debug.Log("extraStatBlock[7] get children was null.");
        }

        temp = extraStatBlock[8].GetComponentInChildren<ChangeText>();
        if (temp)
        {
            temp.Change(stats.CalculateMR().ToString());
        }
        else
        {
            Debug.Log("extraStatBlock[8] get children was null.");
        }

        #endregion

    }
}
