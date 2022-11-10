using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvGHManager : MonoBehaviour
{
    public static InvGHManager instance { get; private set; }
    [SerializeField] PartyManagementGH partyManagementGH;
    public PartyInfoBlock partyInfoBlock;
    public List<List<CharaStatBlock>> characters = new List<List<CharaStatBlock>>();
    public Wallet wally;
    public WalletUI wallyUI;

    [SerializeField] List<CharaStatBlock> Alessia = new List<CharaStatBlock>();
    [SerializeField] List<CharaStatBlock> Erin = new List<CharaStatBlock>();
    [SerializeField] List<CharaStatBlock> Galahad = new List<CharaStatBlock>();
    [SerializeField] List<CharaStatBlock> Leonidas = new List<CharaStatBlock>();
    [SerializeField] List<CharaStatBlock> Thomas = new List<CharaStatBlock>();
    [SerializeField] List<CharaStatBlock> Vivienne = new List<CharaStatBlock>();
    [SerializeField] List<CharaStatBlock> Savannah = new List<CharaStatBlock>();
    [SerializeField] List<CharaStatBlock> Soma = new List<CharaStatBlock>();

    private void Start()
    {
        instance = this;
        characters.Add(Alessia);
        characters.Add(Erin);
        characters.Add(Galahad);
        characters.Add(Leonidas);
        characters.Add(Savannah);
        characters.Add(Soma);
        characters.Add(Thomas);
        characters.Add(Vivienne);
    }

    public bool CheckForavilability(int starlevel)
    {
        switch (starlevel)
        {
            case 1:
                if (wally.copper >= 100)
                {
                    if(wally.silver >= 10)
                    {
                        return true;
                    }
                }
                break;
            case 2:
                if (wally.copper >= 200)
                {
                    if(wally.silver >= 50)
                    {
                        if (wally.electrum >= 20)
                        {
                            return true;
                        }
                    }
                }
                break;
            case 3:
                if (wally.copper >= 300)
                {
                    if (wally.silver >= 150)
                    {
                        if (wally.electrum >= 40)
                        {
                            if (wally.gold >= 10)
                            {
                                if (wally.platinum >= 2)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                break;
            case 4:
                if (wally.copper >= 500)
                {
                    if (wally.silver >= 300)
                    {
                        if (wally.electrum >= 100)
                        {
                            if (wally.gold >= 100)
                            {
                                if (wally.platinum >= 50)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }
        return false;
    }
    public void Upgrade(CharaStatBlock chara)
    {
        int nextIndex = chara.starLevel;
        CharaStatBlock temp = characters[chara.managerIndex][nextIndex];
        for (int i = 0; i < partyInfoBlock.members.Count; i++)
        {
            if (partyInfoBlock.members[i] == chara)
            {
                partyInfoBlock.members[i] = temp;
            }
        }
        for (int i = 0; i < partyInfoBlock.inactives.Count; i++)
        {

            if (partyInfoBlock.inactives[i] == chara)
            {
                partyInfoBlock.inactives[i] = temp;
            }

        }
        switch (chara.starLevel)
        {
            case 1:
                wally.RemCopper(100);
                wally.RemSilver(10);
                break;
            case 2:
                wally.RemCopper(100);
                wally.RemSilver(50);
                wally.RemElectrum(20);
                break;
            case 3:
                wally.RemCopper(300);
                wally.RemSilver(150);
                wally.RemElectrum(40);
                wally.RemGold(10);
                wally.RemPlatinum(2);
                break;
            case 4:
                wally.RemCopper(500);
                wally.RemSilver(300);
                wally.RemElectrum(100);
                wally.RemGold(100);
                wally.RemPlatinum(50);
                break;
            default:
                break;
        }
        wallyUI.UpdateText();
        partyManagementGH.UpdateUI();
    }
    public CharaStatBlock UpgradeII(CharaStatBlock chara)
    {
        int nextIndex = chara.starLevel;
        CharaStatBlock temp = characters[chara.managerIndex][nextIndex];
        for (int i = 0; i < partyInfoBlock.members.Count; i++)
        {
            if (partyInfoBlock.members[i] == chara)
            {
                partyInfoBlock.members[i] = temp;
            }
        }
        for (int i = 0; i < partyInfoBlock.inactives.Count; i++)
        {

            if (partyInfoBlock.inactives[i] == chara)
            {
                partyInfoBlock.inactives[i] = temp;
            }
        }
        switch (chara.starLevel)
        {
            case 1:
                wally.RemCopper(100);
                wally.RemSilver(10);
                break;
            case 2:
                wally.RemCopper(100);
                wally.RemSilver(50);
                wally.RemElectrum(20);
                break;
            case 3:
                wally.RemCopper(300);
                wally.RemSilver(150);
                wally.RemElectrum(40);
                wally.RemGold(10);
                wally.RemPlatinum(2);
                break;
            case 4:
                wally.RemCopper(500);
                wally.RemSilver(300);
                wally.RemElectrum(100);
                wally.RemGold(100);
                wally.RemPlatinum(50);
                break;
            default:
                break;
        }
        wallyUI.UpdateText();
        partyManagementGH.UpdateUI();
        return temp;
    }

}
