using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeactivateNoMoreActionPoints : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // just in case
    [SerializeField] private StatsHolder stats;
    public Button button;
    public int ID;
    public int APRequired;
    public int APLeft;
    void Start()
    {
        if (!button)
        {
            button = GetComponent<Button>();
        }
        List<GameObject> t = GameManager.instance.partyManager.active.GetMembers();
        foreach (var item in t)
        {
            if(item.name == (prefab.name + "(Clone)"))
            {
                stats = item.GetComponent<StatsHolder>();
                break;
            }
        }
        
        if (stats)
        {
            //stats.SetStats();
            APRequired = stats.block.actionsCost[ID];
            APLeft = stats.APointsLeft;
        }
        else
        {
            Debug.LogError("Button is Missing Stat Block.");
        }
    }

    void Update()
    {
        if (stats)
        {
            APLeft = stats.APointsLeft;
            if (APRequired > APLeft)
            {
                button.interactable = false;
            }
        }
        else
        {
            Debug.LogError("Button is Missing Stat Block.");
        }

    }

}
