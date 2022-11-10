using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Party Block", menuName = "Party Info Block")]
public class PartyInfoBlock : ScriptableObject
{
    public List<CharaStatBlock> members = new List<CharaStatBlock>();
    //public List<GameObject> prefabs;
    public List<CharaStatBlock> inactives = new List<CharaStatBlock>();

    public bool AddMember(CharaStatBlock pc)
    {
        if(members.Count == 5)
        {
            return false;
        }
        if (members.Contains(pc))
        {
            members.Remove(pc);
        }
        members.Add(pc);
        return true;
    }
    public bool RemoveMember(CharaStatBlock pc)
    {
        if (members.Contains(pc))
        {
            members.Remove(pc);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AddInactiveMember(CharaStatBlock pc)
    {
        if (inactives.Count == 5)
        {
            return false;
        }
        if (inactives.Contains(pc))
        {
            inactives.Remove(pc);
        }
        inactives.Add(pc);
        return true;
    }
    public bool RemoveInactiveMember(CharaStatBlock pc)
    {
        if (inactives.Contains(pc))
        {
            inactives.Remove(pc);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetPartyCount()
    {
        return members.Count;
    }

    public bool Contains(CharaStatBlock pc)
    {
        return members.Contains(pc);
    }

    public GameObject GetPrefab(int index)
    {
        return members[index].prefab;
    }

}
