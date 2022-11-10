using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public BaseTile currentTile;

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!GameManager.instance.paused)
        {
            Run();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        CollisionRun(other);
    }

    protected virtual void OnTriggerExit(Collider collider)
    {
       // ExitRun(collider);
    }

    protected virtual void Run()
    {
        //implement in your specific entity
    }

    //override this if necessary in your specific entity
    protected virtual void CollisionRun(Collider collider)
    {
        if (collider.GetComponent<BaseTile>())
        {
            BaseTile newTile = collider.GetComponent<BaseTile>();
            RemoveCurrentTile();
            AddCurrentTile(newTile);
        }
    }

    //override this if necessary in your specific entity
    protected virtual void ExitRun(Collider collider)
    {
        if (collider.GetComponent<BaseTile>() == currentTile)
        {
            RemoveCurrentTile();
        }
    }
    
    protected void RemoveCurrentTile()
    {
        if (currentTile)
        {
            currentTile.entities.Remove(gameObject);
            currentTile = null;
        }
    }

    protected void AddCurrentTile(BaseTile tile)
    {
        if (currentTile == tile)
        {
            return;
        }
        RemoveCurrentTile();
        currentTile = tile;
        currentTile.entities.Add(gameObject);
    }

    public virtual void Deactivate()
    {
        //Implement in the children
        gameObject.SetActive(false);
    }

    public virtual void Activate()
    {
        //Implement in the children
        gameObject.SetActive(true);
    }
}
