using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDetector : MonoBehaviour
{
    public BaseTile parentTile;
    public int position;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseTile>())
        {
            parentTile.AddNeighbor(other.GetComponent<BaseTile>(), position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BaseTile>())
        {
            parentTile.RemoveNeighbor(position);
        }
    }
}
