using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileNode
{
    public TileNode parent;
    public BaseTile tile;
    public float givenCost;
    public float heuristicCost;
    public float finalCost;
}


public class BaseTile : MonoBehaviour
{
    //New Format
    public Hex hex;
    public HexGrid grid;

    //Tile accessors
    public BaseTile[] neighbors = new BaseTile[6];
    public BaseTile TopLeft
    {
        get
        {
            return neighbors[0];
        }
        set
        {
            neighbors[0] = value;
        }
    }
    public BaseTile Top
    {
        get
        {
            return neighbors[1];
        }
        set
        {
            neighbors[1] = value;
        }
    }
    public BaseTile TopRight
    {
        get
        {
            return neighbors[2];
        }
        set
        {
            neighbors[2] = value;
        }
    }
    public BaseTile BottomRight
    {
        get
        {
            return neighbors[3];
        }
        set
        {
            neighbors[3] = value;
        }
    }
    public BaseTile Bottom
    {
        get
        {
            return neighbors[4];
        }
        set
        {
            neighbors[4] = value;
        }
    }
    public BaseTile BottomLeft
    {
        get
        {
            return neighbors[5];
        }
        set
        {
            neighbors[5] = value;
        }
    }

    public float weight;
    public List<GameObject> entities = new List<GameObject>();
    private int count;
    public UnityEvent OnEntityChange;

    public enum ColorID
    {
        Original,
        Ice,
        Highligthed,
        Selected,
        Entity,
        AttackRange,
        SpecialAttack,
        Unwalkable
    }
    public TileColor colorManager;
    public Material[] colors = new Material[4];

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (!colorManager)
        {
            colorManager = GetComponent<TileColor>();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (count != entities.Count)
        {
            OnEntityChange.Invoke();
            count = entities.Count;
        }
    }

    public virtual void AddEntity(Entity entity)
    {
        entities.Add(entity.gameObject);
    }

    public virtual void RemoveEntity(Entity entity)
    {
        if (entities.Contains(entity.gameObject))
        {
            entities.Remove(entity.gameObject);
        }
    }

    public virtual void EntityChange()
    {
        colorManager.SetTileState(ColorID.Entity, (entities.Count >= 1));
    }

    private void OnTriggerEnter(Collider other)
    {
    }
    private void OnTriggerExit(Collider other)
    {
    }

    //can choose the position, but if left alone it will choose random
    //returns if it succeeded or not
    public virtual bool AddNeighbor(BaseTile tile, int position)
    {
        if (position >= 0 && position <= 5 && neighbors[position] == null)
        {
            neighbors[position] = tile;
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void RemoveNeighbor(int position)
    {
        neighbors[position] = null;
    }

    //Outdated
    public List<BaseTile> GreedyPath(BaseTile destination)
    {
        List<BaseTile> directions = new List<BaseTile>();
        BaseTile[] currNeighbors = new BaseTile[6];
        directions.Add(this);

        while (directions[directions.Count - 1] != destination)
        {
            currNeighbors = directions[directions.Count - 1].neighbors;
            BaseTile closestTile = null;
            float bestDistance = -1;

            //This is going to be messy for now and is going to need more optimizations
            for (int i = 0; i < 6; i++)
            {
                if (currNeighbors[i])
                {
                    if ((currNeighbors[i].entities.Count > 0 && currNeighbors[i] != destination) || currNeighbors[i].weight == -1)
                    {
                        //continue;
                    }
                    float distance = Vector3.Distance(currNeighbors[i].transform.position, destination.transform.position);
                    if (distance < bestDistance || !closestTile)
                    {
                        closestTile = currNeighbors[i];
                        bestDistance = distance;
                    }
                }
            }
            if (!closestTile)
            {
                break;
            }

            directions.Add(closestTile);

            //Fail safe for this greedy path
            if (directions.Count >= 30)
            {
                break;
            }
        }

        directions.RemoveAt(0);

        return directions;
    }

    //Doesn't crash if there is no valid path
    public List<BaseTile> GreedyPath(BaseTile destination, int distanceLimit, bool ignoreWeight = false)
    {
        List<BaseTile> directions = new List<BaseTile>();
        BaseTile[] currNeighbors = new BaseTile[6];
        directions.Add(this);

        while (directions[directions.Count - 1] != destination && directions.Count <= distanceLimit)
        {
            currNeighbors = directions[directions.Count - 1].neighbors;
            BaseTile closestTile = null;
            float bestDistance = -1;

            //This is going to be messy for now and is going to need more optimizations
            for (int i = 0; i < 6; i++)
            {
                if (currNeighbors[i])
                {
                    if (!ignoreWeight)
                    {
                        if ((currNeighbors[i].entities.Count > 0 && currNeighbors[i] != destination) || currNeighbors[i].weight == -1)
                        {
                            continue;
                        }
                    }
                    float distance = currNeighbors[i].TileDistance(destination);
                    if (distance < bestDistance || !closestTile)
                    {
                        closestTile = currNeighbors[i];
                        bestDistance = distance;
                    }
                }
            }

            if (!closestTile)
            {
                break;
            }

            directions.Add(closestTile);
        }

        directions.RemoveAt(0);

        return directions;
    }

    //Hopefully better pathing than the greedy pathing
    public List<BaseTile> AStarPath(BaseTile destination, int travelLimit)
    {
        List<TileNode> open = new List<TileNode>();
        List<BaseTile> results = new List<BaseTile>();
        Dictionary<BaseTile, TileNode> visited = new Dictionary<BaseTile, TileNode>();

        TileNode start = new TileNode();
        start.tile = this;
        start.givenCost = 0;
        start.heuristicCost = HexMathLib.Distance(hex, destination.hex);
        start.finalCost = start.givenCost + start.heuristicCost * start.tile.weight;
        open.Add(start);
        visited[this] = start;

        while (open.Count > 0)
        {
            //Start at the beginning of the list
            TileNode current = open[0];
            open.Remove(current);

            if (current.tile == destination)
            {
                while(current != null)
                {
                    results.Add(current.tile);
                    current = current.parent;
                }
                results.Reverse();
                break;
            }

            foreach (BaseTile successor in current.tile.neighbors)
            {
                if (successor)
                {
                    if (successor.weight == -1 || successor.entities.Count > 0)
                    {
                        continue;
                    }
                    float tempGivenCost = current.givenCost + successor.weight;

                    //If we've already visited it create a path
                    if (visited.ContainsKey(successor))
                    {
                        TileNode node = visited[successor];
                        if (tempGivenCost < node.givenCost)
                        {
                            open.Remove(node);
                            node.parent = current;
                            node.givenCost = tempGivenCost;
                            node.finalCost = node.givenCost + node.heuristicCost * node.tile.weight;
                            visited[successor] = node;
                            open.Add(node);
                        }
                    }
                    else
                    {
                        TileNode successorNode = new TileNode();
                        successorNode.tile = successor;
                        successorNode.parent = current;
                        successorNode.givenCost = tempGivenCost;
                        successorNode.heuristicCost = HexMathLib.Distance(successor.hex, destination.hex);
                        successorNode.finalCost = successorNode.givenCost = successorNode.heuristicCost * successor.weight;
                        open.Add(successorNode);
                        visited[successor] = successorNode;
                    }
                }
            }
        }
        if (results.Count > 0)
        {
            results.RemoveAt(0);
        }
        
        return results.Count > travelLimit ? results.GetRange(0, travelLimit) : results;
    }

    public int TileDistance(BaseTile destination)
    {
        return HexMathLib.Distance(hex, destination.hex);
    }

    public List<BaseTile> LinePath(BaseTile destination)
    {
        List<BaseTile> results = grid.GetTiles(HexMathLib.DrawLine(hex, destination.hex));
        results.RemoveAt(0);
        return results;
    }
    public List<BaseTile> BreadthFirst(int range)
    {
        List<Hex> hexCircle = HexMathLib.HexSpiral(hex, range);
        List<BaseTile> circle = grid.GetTiles(hexCircle);

        return circle;
    }

    public List<BaseTile> Ring(int range)
    {
        List<BaseTile> ring = new List<BaseTile>();

        ring = grid.GetTiles(HexMathLib.HexRing(hex, range));

        return ring;
    }

    public void ChangeColor(ColorID color)
    {
        GetComponent<MeshRenderer>().material = colors[(int)color];
    }

    public void Deactivate()
    {
        if (entities.Count > 0)
        {
            entities[0].GetComponent<Entity>().Deactivate();
        }
    }
    public void Activate()
    {
        if (entities.Count > 0)
        {
            entities[0].GetComponent<Entity>().Activate();
        }
    }
}
