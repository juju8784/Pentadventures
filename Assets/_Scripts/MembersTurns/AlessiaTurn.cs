using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlessiaTurn : PlayerTurn
{
    List<GameObject> members;
    int specialCount = 3;
    public override void StartTurn()
    {
        base.StartTurn();
        members = GameManager.instance.partyManager.active.GetMembers();
        if (gameObject.GetComponent<Health>().dmgReduction > 0)
        {
            specialCount--;
            if (specialCount == 0)
            {
                specialCount = 3;
                ResetDmgReduction();
            }
        }
    }

    public override void Attack()
    {
        range = 1;
        base.Attack();
    }

    public override void Special()
    {
        if (!stats)
        {
            stats = GetComponent<StatsHolder>();
        }
        if (stats.SubtractActionPoints(stats.block.actionsCost[1]))
        {
            for (int i = 0; i < members.Count; i++)
            {
                //List<BaseTile> t = player.currentTile.GreedyPath(members[i].GetComponent<TestCharacterController>().currentTile);
                if (player.currentTile.TileDistance(members[i].GetComponent<TestCharacterController>().currentTile) <= 2)
                {
                    Vector3 offset = new Vector3(0, 1.6f, 0.7f);
                    GameObject dmgRedText = Instantiate(textSpawnPrefab, members[i].transform.position + offset, Quaternion.identity);
                    dmgRedText.transform.SetParent(canvas.transform);
                    dmgRedText.GetComponent<TextMeshProUGUI>().text = "10% damage reduction";
                    dmgRedText.GetComponent<TextMeshProUGUI>().color = Color.green;
                    dmgRedText.transform.position = Camera.main.WorldToScreenPoint(dmgRedText.transform.position);
                    Destroy(dmgRedText, 2.5f);
                    members[i].GetComponent<PlayerHealth>().dmgReduction = 10;
                }
            }
        }
    }

    void ResetDmgReduction()
    {
        members = GameManager.instance.partyManager.active.GetMembers();
        for (int i = 0; i < members.Count; i++)
        {
            members[i].GetComponent<PlayerHealth>().dmgReduction = 0;
        }
    }

    public override void Death()
    {
        ResetDmgReduction();
        base.Death();
    }

    public override void ResetOnEndCombat()
    {
        specialCount = 3;
        ResetDmgReduction();
        base.ResetOnEndCombat();
        //if(QuestGiver.giver.questsList[1].isActive == true)
        //{
        //    QuestGiver.giver.questsList[1].goal.PlayerSurvived();
        //}
    }
}
