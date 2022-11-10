using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a dummy script so that I can test functionality of other code that I write
/// </summary>
public class TestFunctionality : MonoBehaviour
{
    private TestCharacterController controller;

    public List<BaseTile> path;

    public int range = 2;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<TestCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            for (int i = 0; i < path.Count; i++)
            {
                path[i].colorManager.SetTileState(BaseTile.ColorID.AttackRange, false);
            }

            path = controller.currentTile.BreadthFirst(range);

            for (int i = 0; i < path.Count; i++)
            {
                path[i].colorManager.SetTileState(BaseTile.ColorID.AttackRange, true);
            }
        }
    }
}
