using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteractable : MonoBehaviour
{
    public SaveState canContinue;

    private void Start()
    {
        gameObject.SetActive(canContinue.gameCreated);
    }
}
