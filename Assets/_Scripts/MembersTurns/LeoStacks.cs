using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeoStacks : MonoBehaviour
{
    public int specialMax = 5;
    public int maxStacks = 3;
    public int stacks;

    public void OnEnable()
    {
        stacks = 0;
        maxStacks = 3;
        specialMax = 5;
    }
    public bool AddStack(bool special = false)
    {
        // adds a stack to the list
        // if the limit of stacks is reached, call remove stacks;
        stacks++;
        if (special)
        {
            if(stacks == specialMax)
            {
                return true;
            }
        }
        else
        {
            if(stacks == maxStacks)
            {
                return true;
            }
        }

        return false;
    }
}
