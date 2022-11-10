using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Block", menuName = "CharacterInfo Block")]
public class CharaStatBlock : ScriptableObject
{
    public string characterName;
    public string description;
    public Sprite artwork;
    public GameObject prefab;
    public int managerIndex;

    public string characterClass;
    public string passiveAbility;
    public string specialAbility;

    // Stats Holder should import the stats from here
    public int strength;
    public int vitality;
    public int dexterity;
    public int intelligence;
    public int wisdom;
    public int spirit;
    public int actionPointRecovery;
    public int starLevel;

    public int[] actionsCost = new int[3];
    [SerializeField] GameObject upgradeDenied;

    //public List<Item> items = new List<Item>();

    public void SetStatsHolder(StatsHolder holder)
    {
        holder.BaseStrength = strength;
        holder.BaseDexterity = dexterity;
        holder.BaseIntelligence = intelligence;
        holder.BaseVitality = vitality;
        holder.BaseWisdom = wisdom;
        holder.BaseSpirit = spirit;
        holder.APointsRecoveryRate = actionPointRecovery;
        holder.StarLevel = starLevel;
    }


    public void Upgrade()
    {
        if (InvGHManager.instance.CheckForavilability(starLevel))
        {
            InvGHManager.instance.Upgrade(this);
        }
        else
        {
            GameObject t = Instantiate(upgradeDenied);
            t.transform.SetParent(FindObjectOfType<Canvas>(false).transform);
            t.transform.localPosition = Vector3.zero;
            Debug.Log("Cannot upgrade. Not enough Items");
        }
    }
    public CharaStatBlock Upgrade(CharaStatBlock changeThis)
    {
        if (InvGHManager.instance.CheckForavilability(starLevel))
        {
            changeThis = InvGHManager.instance.UpgradeII(this);
        }
        else
        {
            GameObject t = Instantiate(upgradeDenied);
            t.transform.SetParent(FindObjectOfType<Canvas>(false).transform);
            t.transform.localPosition = Vector3.zero;
            Debug.Log("Cannot upgrade. Not enough Items");
        }
        return changeThis;
    }
}
