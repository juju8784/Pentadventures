using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeactiveWhilePlayerMoving : MonoBehaviour
{
    public Button button;
    public string CharaName;
    public TestCharacterController player;
    PlayerTurn turn;

    // Start is called before the first frame update
    void Start()
    {
        if (!button)
        {
            button = GetComponent<Button>();
        }

        List<GameObject> t = GameManager.instance.partyManager.active.GetMembers();
        
        foreach (var item in t)
        {
            if (item.name == (CharaName + "(Clone)"))
            {
                player = item.GetComponent<TestCharacterController>();
                break;
            }
        }
        
        turn = player.gameObject.GetComponent<PlayerTurn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            List<GameObject> t = GameManager.instance.partyManager.active.GetMembers();
            foreach (var item in t)
            {
                if (item.name == (CharaName + "(Clone)"))
                {
                    player = item.GetComponent<TestCharacterController>();
                    break;
                }
            }
        }
        if (!turn && player)
        {
            turn = player.gameObject.GetComponent<PlayerTurn>();
        }
        
        if(turn.attacking || turn.specialActive)
        {
            button.interactable = false;
        }
        else if (player.stopped)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
