using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToGHButtons : MonoBehaviour
{
    public void Initiate(ReturnToGuildHallTile lost)
    {
        Button[] t = GetComponentsInChildren<Button>();
        t[0].onClick.AddListener(lost.Return);
        t[1].onClick.AddListener(lost.Dismiss);
    }
}
