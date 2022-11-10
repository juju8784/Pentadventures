using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    private TestCharacterController player;
    public Button endturnButton;

    // Start is called before the first frame update
    void Start()
    {
        if (!endturnButton)
        {
            endturnButton = GetComponent<Button>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            player = GameManager.instance.player.GetComponent<TestCharacterController>();
        }

        if (!player.stopped)
        {
            endturnButton.interactable = false;
        }
        else
        {
            endturnButton.interactable = true;
        }
    }
}
