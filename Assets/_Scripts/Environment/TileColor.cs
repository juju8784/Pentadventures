using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColor : MonoBehaviour
{
    public BaseTile owner;
    //A priority of states. The higher indexes have a higher priority for color
    public int numStates;
    [SerializeField]private List<bool> tileState = new List<bool>();

    private void Start()
    {
        if (tileState.Count != numStates)
        {
            StateFix();
        }

        if (!owner)
        {
            owner = GetComponent<BaseTile>();
        }
    }

    private void Update()
    {
        if (!tileState[0])
        {
            tileState[0] = true;
        }
        for (int i = tileState.Count - 1; i >= 0; i--)
        {
            if (tileState[i])
            {
                owner.ChangeColor((BaseTile.ColorID)i);
                return;
            }
        }
    }

    public void SetTileState(BaseTile.ColorID color, bool state)
    {
        if ((int)color >= tileState.Count)
        {
            StateFix();
        }
        tileState[(int)color] = state;
    }

    public void SetTileState(int index, bool state)
    {
        if (index >= tileState.Count)
        {
            StateFix();
        }
        tileState[index] = state;
    }

    public bool GetTileState(BaseTile.ColorID color)
    {
        return tileState[(int)color];
    }

    public void ResetTiles()
    {
        for (int i = 1; i < tileState.Count; i++)
        {
            tileState[i] = false;
        }
    }

    private void StateFix()
    {
        while (tileState.Count < numStates)
        {
            tileState.Add(false);
        }
    }
}
