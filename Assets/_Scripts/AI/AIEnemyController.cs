using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyController : Entity
{
    public List<BaseTile> directions = new List<BaseTile>();
    private StatsHolder stats;
    protected float fraction;
    public float speed;
    public bool stopped;
    private EnemyAnimationHandler anim;

    // Start is called before the first frame update
    protected override void Start()
    {
        if (currentTile)
        {
            directions.Add(currentTile);
        }
        stats = GetComponent<StatsHolder>();
        stopped = true;
        anim = GetComponent<EnemyAnimationHandler>();
    }

    public void AddDirections(List<BaseTile> path)
    {
        if (directions.Count == 0)
        {
            directions.Add(currentTile);
        }
        for(int i = 0; i < path.Count; i++)
        {
            if (path[i] != directions[directions.Count - 1])
            {
                directions.Add(path[i]);
            }
        }
        stopped = (directions.Count <= 1);
    }

    public bool AtDestination()
    {
        if (directions.Count == 0)
        {
            directions.Add(currentTile);
        }

        if (currentTile == directions[directions.Count - 1])
        {
            return true;
        }

        return false;
    }

    public void ResetDirections()
    {
        directions.Clear();
        directions.Add(currentTile);
    }

    protected override void Run()
    {
        if (directions.Count == 0)
        {
            directions.Add(currentTile);
        }
        else if (directions[0] != currentTile)
        {
            if (directions.Count >= 2)
            {
                if (directions[1] != currentTile)
                {
                    ResetDirections();
                }
            }
            else
            {
                ResetDirections();
            }
        }
        if (directions[0] == null)
        {
            ResetDirections();
            return;
        }
        bool active = false;
        if (GetComponent<Turn>())
        {
            active = GetComponent<Turn>().isActive;
        }
        if ((GameManager.instance.TurnManager.isPlayersTurn == false && !GameManager.instance.isCombat) || (active && GameManager.instance.isCombat))
        {
            // moves to the next tile is 'directions' has any other tile that is not currentTile
            if (directions.Count >= 2 && stats.CanMove())
            {
                if ((directions[1].transform.position.x == transform.position.x && directions[1].transform.position.y == transform.position.y) || fraction >= 1)
                {
                    directions.RemoveAt(0);
                    stats.Move(1);
                    fraction = 0;
                }
                else
                {
                    fraction += Time.deltaTime * speed;
                    Vector3 newPos = Vector3.Lerp(directions[0].transform.position, directions[1].transform.position, fraction);
                    newPos.y = transform.position.y;
                    transform.position = newPos;
                    transform.rotation = Quaternion.LookRotation(directions[1].transform.position - directions[0].transform.position, Vector3.up);
                }
            }
        }
        stopped = (directions.Count <= 1);
        if (GetComponent<EnemyAnimationHandler>())
        {
            anim.SetMovementAnimation(stopped);
        }
    }

    public void FinishActions()
    {
        if (directions.Count > 1)
        {
            Vector3 newPos = directions[1].transform.position;
            newPos.y = transform.position.y;
            transform.position = newPos;
            directions.RemoveAt(0);
            currentTile = directions[0];
            stopped = true;
        }
    }

    public override void Activate()
    {
        base.Activate();
        GetComponent<Enemy>().Activate();
    }

    public override void Deactivate()
    {
        GetComponent<Enemy>().Deactivate();
        base.Deactivate();
    }
}
