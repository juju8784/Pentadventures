using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New Quest Block", menuName = "QuestInfo Block")]
[System.Serializable]
public class QuestInfoBlock : ScriptableObject
{
    public bool isActive; 
    public string title; 
    public string description;

    public QuestGoal goal;
    public bool claimed = false;
    public int copper = 0;
    public int silver = 0;
    public int electrum = 0;
    public int gold = 0;
    public int platinum = 0;

    public Wallet wally;

    public void Claim()
    {
        wally.AddCopper(copper);
        wally.AddSilver(silver);
        wally.AddElectrum(electrum);
        wally.AddGold(gold);
        wally.AddPlatinum(platinum);
        claimed = true;
    }
}
