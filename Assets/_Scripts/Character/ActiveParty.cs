using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;


public class ActiveParty : MonoBehaviour
{
    [ReadOnly]
    public List<GameObject> party;

    int MaxCount = 5;

    [ReadOnly]
    public int count = 0;

    public List<GameObject> GetMembers()
    {
        return party;
    }

    public int GetCount()
    {
        return count;
    }

    public bool InParty(GameObject character)
    {
        if (party.Contains(character))
            return true;
        else
            return false;
    }

    public bool AddMember(GameObject character)
    {
        if (count == MaxCount)
        {
            return false;
        }
        else if (party.Contains(character))
        {
            Debug.Log("Party Already has this character");
            return false;
        }
        else
        {
            party.Add(character);
            count++;
            return true;
        }
    }

    public bool RemoveMember(GameObject character)
    {
        if (InParty(character))
        {
            party.Remove(character);
            count--;
            return true;
        }
        else
        {
            Debug.Log("Member already is not in active party");
            return false;
        }
    }

    // Add more functions as needed
    // 

}
