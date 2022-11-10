using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyStatsHolder : StatsHolder
{
    PartyUIManager uIManager;
    PartyManager pManager;

    public override void Start()
    {
        uIManager = GameObject.FindObjectOfType<PartyUIManager>();
        if (uIManager)
        {
            uIManager.StartPartyUI();
            pManager = uIManager.manager;
            CalculatePartyStats();
            ResetMovement();
        }
        else
        {
            Debug.LogError("uIManager not found in scene. PartyStatsHolder.cs");
        }
    }

    public void CalculatePartyStats()
    {
        TotalVitality = 0;
        TotalStrength = 0;
        TotalDexterity = 0;
        TotalIntelligence = 0;
        TotalWisdom = 0;
        movementRange = 0;
        List<GameObject> temp = pManager.active.GetMembers();
        foreach (GameObject unit in temp)
        {
            StatsHolder stats = unit.GetComponent<StatsHolder>();
            TotalVitality += stats.BaseVitality;
            TotalStrength += stats.BaseStrength;
            TotalDexterity += stats.BaseDexterity;
            TotalIntelligence += stats.BaseIntelligence;
            TotalWisdom += stats.BaseWisdom;
            StarLevel += stats.StarLevel;
            movementRange += stats.GetTotalMovementRange();
        }
        if (temp.Count > 0)
        {
            TotalVitality /= temp.Count;
            TotalStrength /= temp.Count;
            TotalDexterity /= temp.Count;
            TotalIntelligence /= temp.Count;
            TotalWisdom /= temp.Count;
            movementRange /= temp.Count;
        }
        else
        {
            Debug.LogError("active party has 0 members");
        }
    }
}
