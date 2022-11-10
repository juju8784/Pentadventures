using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rogue Drop Table", menuName = "Rogue Drops")]
public class Loot : ScriptableObject
{
    public Vector2 copperMinMax;
    public Vector2 silverMinMax;
    public Vector2 electrumMinMax;
    public Vector2 goldMinMax;
    public Vector2 platinumMinMax;

    public List<int> GetRandomLoot()
    {
        List<int> loot = new List<int>();

        if (copperMinMax.y > 0)
        {
            loot.Add(Random.Range((int)copperMinMax.x, (int)copperMinMax.y));
        }
        if (silverMinMax.y > 0)
        {
            loot.Add(Random.Range((int)silverMinMax.x, (int)silverMinMax.y));
        }
        if (electrumMinMax.y > 0)
        {
            loot.Add(Random.Range((int)electrumMinMax.x, (int)electrumMinMax.y));
        }
        if (goldMinMax.y > 0)
        {
            loot.Add(Random.Range((int)goldMinMax.x, (int)goldMinMax.y));
        }
        if (platinumMinMax.y > 0)
        {
            loot.Add(Random.Range((int)platinumMinMax.x, (int)platinumMinMax.y));
        }

        return loot;
    }

}
