using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeText : MonoBehaviour
{

    TextMeshProUGUI tmp;
    // This script should be put in Objects with TextMeshPro components only
    // otherwise it will most probably crash

    public void Change(string text)
    {
        tmp = gameObject.GetComponent<TextMeshProUGUI>();
        if (tmp)
        {
            tmp.text = text;
        }
        else
        {
            Debug.Log("The text was null, figure it out");
        }
    }
}
