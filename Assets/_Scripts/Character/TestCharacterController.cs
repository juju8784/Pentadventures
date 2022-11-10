using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestCharacterController : Entity
{
    public float speed;
    //testing

    //stores the current position and the places the character needs to go
    public List<BaseTile> directions = new List<BaseTile>();
    public List<BaseTile> possibleDirection = new List<BaseTile>();
    public bool stopped;

    protected float fraction;

    private StatsHolder stats;

    public GraphicRaycaster gRaycaster;
    public EventSystem eSystem;

    bool active = false;
    bool movement = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        if (currentTile)
        {
            directions.Add(currentTile);
            possibleDirection.Add(directions[directions.Count - 1]);
        }
        stats = GetComponent<StatsHolder>();
    }

    //Runs first thing in the update function if game isn't paused
    protected override void Run()
    {
        if (GetComponent<PlayerTurn>())
        {
            active = GetComponent<PlayerTurn>().isActive;
        }
        else
        {
            active = false;
        }
        if (movement)
        {
            if ((GameManager.instance.TurnManager.isPlayersTurn && !GameManager.instance.isCombat) || (active && GameManager.instance.isCombat))
            {
                //Error checking
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
                if (possibleDirection.Count == 0)
                {
                    possibleDirection.Add(directions[directions.Count - 1]);
                }

                //Selected tile stuff
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //Only does these calculations if we can move
                if (Physics.Raycast(ray, out hit) && stats.CanMove())
                {
                    if (hit.transform.GetComponent<BaseTile>())
                    {
                        BaseTile hitTile = hit.transform.GetComponent<BaseTile>();

                        //If the tile is unwalkable
                        if (hitTile.weight == -1 || hitTile.entities.Count > 0)
                        {
                            RemoveHighlights(possibleDirection);
                            possibleDirection.Clear();
                        }
                        else
                        {
                            if (possibleDirection.Count >= 1)
                            {
                                RemoveHighlights(possibleDirection);
                            }
                            possibleDirection.Clear();

                            possibleDirection.AddRange(directions[directions.Count - 1].GreedyPath(hitTile, stats.GetMovementLeft()));
                            //possibleDirection.AddRange(directions[directions.Count - 1].AStarPath(hitTile, stats.GetMovementLeft()));

                            HighlightDirections(possibleDirection);

                            //Left click move try to move to the tile
                            if (Input.GetMouseButtonDown(0))
                            {
                                PointerEventData pEventData = new PointerEventData(eSystem);
                                pEventData.position = Input.mousePosition;

                                List<RaycastResult> rResults = new List<RaycastResult>();
                                if (!gRaycaster)
                                {
                                    gRaycaster = GameManager.instance.gRaycaster;
                                }
                                gRaycaster.Raycast(pEventData, rResults);

                                if (rResults.Count == 0)
                                {
                                    if (possibleDirection.Count == 0)
                                    {
                                        possibleDirection.Add(directions[directions.Count - 1]);
                                    }
                                    if (hit.transform.GetComponent<BaseTile>() == possibleDirection[possibleDirection.Count - 1])
                                    {
                                        AddDirections(possibleDirection);
                                        //Tells the stats that we have moved this amount
                                        if (stats.Move(directions.Count - 1))
                                        {
                                            RemoveHighlights(possibleDirection);
                                            possibleDirection.Clear();
                                            possibleDirection.Add(directions[directions.Count - 1]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Removes highlighted colors if mouse isn't over a tile
                        RemoveHighlights(possibleDirection);
                        possibleDirection.Clear();
                    }
                }
                if (directions.Count >= 2)
                {
                    //if the tile isn't in another section of the directions, we take off the selected color
                    if (!directions.GetRange(1, directions.Count - 1).Contains(directions[0]))
                    {
                        directions[0].colorManager.SetTileState(BaseTile.ColorID.Selected, false);
                    }
                    if ((directions[1].transform.position.x == transform.position.x && directions[1].transform.position.y == transform.position.y) || fraction >= 1)
                    {
                        directions.RemoveAt(0);
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
            else
            {
                RemoveHighlights(possibleDirection);
            }
        }
        else if (!GameManager.instance.TurnManager.isPlayersTurn || !active)
        {
            ResetDirections();
        }

        stopped = (directions.Count <= 1);
        if (GetComponent<CombatCharacterAnimManager>())
        {
            GetComponent<CombatCharacterAnimManager>().SetMovementTrigger(stopped);
        }
    }

    public void HighlightDirections(List<BaseTile> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].colorManager.SetTileState(BaseTile.ColorID.Highligthed, true);
        }
    }

    protected void AddDirections(List<BaseTile> newDirections)
    {
        directions.AddRange(possibleDirection.GetRange(0, possibleDirection.Count));
        for (int i = 1; i < directions.Count; i++)
        {
            directions[i].colorManager.SetTileState(BaseTile.ColorID.Highligthed, false);
            directions[i].colorManager.SetTileState(BaseTile.ColorID.Selected, true);
        }
    }

    public void RemoveHighlights(List<BaseTile> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i])
            {
                tiles[i].colorManager.SetTileState(BaseTile.ColorID.Highligthed, false);
            }
        }
    }

    public void DisableMovement()
    {
        movement = false;
    }

    public void EnableMovement()
    {
        movement = true;
    }
    protected void RemoveSelected(List<BaseTile> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i])
            {
                tiles[i].colorManager.SetTileState(BaseTile.ColorID.Selected, false);
            }
        }
    }

    protected void ResetColors(List<BaseTile> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].colorManager.ResetTiles();
        }
    }

    //Resets your directions ot your current tile
    public void ResetDirections()
    {
        RemoveHighlights(possibleDirection);
        RemoveSelected(directions);
        directions.Clear();
        possibleDirection.Clear();
        directions.Add(currentTile);
    }    
}
