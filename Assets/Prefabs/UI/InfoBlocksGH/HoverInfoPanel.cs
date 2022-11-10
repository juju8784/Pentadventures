using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HoverInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI charaClass;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI passive;
    [SerializeField] private TextMeshProUGUI passiveName;
    [SerializeField] private TextMeshProUGUI special;
    [SerializeField] private TextMeshProUGUI specialName;
    
    public void ChangeText(SlotStatsSheetHolder slotHolder)
    {
        name.text = slotHolder.charaStat.characterName;
        charaClass.text = slotHolder.charaStat.characterClass;
        description.text = slotHolder.charaStat.description;
        if(slotHolder.charaStat.passiveAbility == "")
        {
            passiveName.gameObject.SetActive(false);
            passive.text = "";
            specialName.transform.localPosition = passiveName.transform.localPosition;
            special.transform.localPosition = passive.transform.localPosition;
        }
        else
        {
            passiveName.gameObject.SetActive(true);
            passive.text = slotHolder.charaStat.passiveAbility;
            specialName.transform.localPosition = new Vector3(0, -58);
            special.transform.localPosition = new Vector3(0, -161);
        }

        if (slotHolder.charaStat.specialAbility == "")
        {
            specialName.gameObject.SetActive(false);
            special.text = "";
        }
        else
        {
            specialName.gameObject.SetActive(true);
            special.text = slotHolder.charaStat.specialAbility;
        }
    }
}
