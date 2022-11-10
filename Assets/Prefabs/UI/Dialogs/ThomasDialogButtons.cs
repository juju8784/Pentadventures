using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThomasDialogButtons : MonoBehaviour
{
    public void Initiate(LostCharacter lost)
    {
        Button[] t = GetComponentsInChildren<Button>();
        t[0].onClick.AddListener(lost.JoinTheParty);
        t[1].onClick.AddListener(lost.Ignored);
    }
}
