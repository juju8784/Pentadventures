using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodes : MonoBehaviour
{
    public PartyStatsHolder player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                player.AddMovementLeft(200);
            }
        }    
    }
}
