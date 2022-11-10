using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SaveState")]
public class SaveState : ScriptableObject
{
    public bool gameCreated = false;

    public void NewGame()
    {
        gameCreated = true;
    }
}
