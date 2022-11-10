using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public Transform hexPrefab;
    public bool unwalkableTiles = false;
    public int gridHeight = 11;
    public int gridWidth = 11; 
    float hexWidth = 1.732f;
    float hexHeight = 2.0f;
    public float gap = 0.0f;
    public float middlePositionX = 0.0f;
    public float middlePositionY = 0.0f;
    public float middlePositionZ = 0.0f;
    Vector3 startPos; 

    void Start()
    {
        AddGap();
        CalcStartPos();
        CreateGrid();
    }

    public void Startup()
    {
        AddGap();
        CalcStartPos();
        CreateGrid();
    }

    void AddGap()
    {
        hexWidth += hexWidth * gap; 
        hexHeight += hexHeight * gap;
    }

    void CalcStartPos()
    {
        float offset = 0; 
        if(gridHeight / 2 % 2 != 0)
        {
            offset = hexWidth / 2; 
        }

        float x = -hexWidth * (gridWidth / 2) - offset;
        float z = hexHeight * 0.75f * (gridHeight / 2);
        startPos = new Vector3(x + middlePositionX, 0 + middlePositionY, z + middlePositionZ);
    }

    Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float offset = 0; 
        if(gridPos.y % 2 != 0 )
        {
            offset = hexWidth / 2;
        }
        
        float x = startPos.x + gridPos.x * hexWidth + offset;
        float z = startPos.z - gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, 0, z);
    }

    void CreateGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Transform hex = Instantiate(hexPrefab) as Transform;
                Vector2 gridPos = new Vector2(x, y);
                hex.position = CalcWorldPos(gridPos);
                hex.parent = this.transform;
                hex.name = "Hexagon" + x + "|" + y;
                
                //TODO: FIX THIS Issues with when start runs on the different components
                //if (unwalkableTiles)
                //{
                //    float chance = Random.Range(0, 1);
                //    if (chance <= 0.01)
                //    {
                //        BaseTile tile = hex.GetComponent<BaseTile>();
                //        tile.weight = -1;
                //        tile.colorManager.SetTileState(BaseTile.ColorID.Unwalkable, true);
                //    }
                //}
            }
        }

        if (unwalkableTiles)
        {
            BaseTile[] children = GetComponentsInChildren<BaseTile>();
            int child = Random.Range(0, children.Length / 4);
            children[child].weight = -1;
            children[child].colorManager.SetTileState(BaseTile.ColorID.Unwalkable, true);
        }
        
    }

    //For bug testing
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BaseTile[] children = GetComponentsInChildren<BaseTile>();
            int child = Random.Range(0, children.Length / 4);
            children[child].weight = -1;
            children[child].colorManager.SetTileState(BaseTile.ColorID.Unwalkable, true);
        }
    }

}
