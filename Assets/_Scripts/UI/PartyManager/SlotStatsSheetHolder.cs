using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotStatsSheetHolder : MonoBehaviour
{
    [SerializeField] public CharaStatBlock charaStat;

    public void SetChara(CharaStatBlock chara)
    {
        charaStat = chara;
    }

    public CharaStatBlock GetCharaInfo()
    {
        return charaStat;
    }

    public void Upgrade()
    {
        SetChara(charaStat.Upgrade(charaStat));
    }
}
