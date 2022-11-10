using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wallet", menuName = "Wallet")]
public class Wallet : ScriptableObject
{
    public static Wallet Instance { get; private set; }

    public int copper = 0;
    public int silver = 0;
    public int electrum = 0;
    public int gold = 0;
    public int platinum = 0;

    public void AddGold(int i)
    {
        gold += i;
    }
    public void AddCopper(int i)
    {
        copper += i;
    }
    public void AddSilver(int i)
    {
        silver += i;
    }
    public void AddElectrum(int i)
    {
        electrum += i;
    }
    public void AddPlatinum(int i)
    {
        platinum += i;
    }

    public void AddLoot(List<int> loot)
    {
        if (loot.Count > 0)
        {
            AddCopper(loot[0]);
            if (loot.Count > 1)
            {
                AddSilver(loot[1]);
                if (loot.Count > 2)
                {
                    AddElectrum(loot[2]);
                    if (loot.Count > 3)
                    {
                        AddGold(loot[3]);
                        if (loot.Count > 4)
                        {
                            AddPlatinum(loot[4]);
                        }
                    }
                }
            }
        }
    }

    public void RemGold(int i)
    {
        gold -= i;
    }
    public void RemCopper(int i)
    {
        copper -= i;
    }
    public void RemSilver(int i)
    {
        silver -= i;
    }
    public void RemElectrum(int i)
    {
        electrum -= i;
    }
    public void RemPlatinum(int i)
    {
        platinum -= i;
    }


    public void Reset()
    {
        copper = 0;
        silver = 0;
        electrum = 0;
        gold = 0;
        platinum = 0;
    }

}
