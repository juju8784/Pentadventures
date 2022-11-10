using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldAITurn : MonoBehaviour
{
    //this contains all of the ai and whether or not it's their turn
    public Dictionary<StateMachine, bool> aiTurns = new Dictionary<StateMachine, bool>();
    [SerializeField] private int count;
    [SerializeField] private int activeCount;
    [SerializeField] private StateMachine currentAITurn;
    private List<StateMachine> keys = new List<StateMachine>();
    private int currentTurnIndex;
    // Start is called before the first frame update
    void Start()
    {
        //Starts all of the turns off at false
        List<StateMachine> keys = aiTurns.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            aiTurns[keys[i]] = false;
        }
        count = aiTurns.Count;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.TurnManager.isPlayersTurn)
        {
            if (aiTurns.Count > 0)
            {
                List<bool> values = aiTurns.Values.ToList();
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i])
                    {
                        return;
                    }
                }
            }
            EndAITurn();
        }
    }

    //Sets all of the turns equal to true
    public void StartAITurn()
    {
        //foreach (var key in aiTurns.Keys)
        //{
        //    aiTurns[key] = true;
        //}
        //List<StateMachine> keys = aiTurns.Keys.ToList();
        //for (int i = 0; i < keys.Count; i++)
        //{
        //    aiTurns[keys[i]] = true;
        //    keys[i].stats.TurnReset();
        //}
        keys = aiTurns.Keys.ToList();
        keys.RemoveAll(key => key == null);
        if (keys.Count == 0)
        {
            EndAITurn();
            return;
        }
        aiTurns[keys[0]] = true;
        keys[0].stats.TurnReset();
        activeCount = 1;
        currentTurnIndex = 0;
        currentAITurn = keys[currentTurnIndex];
        currentAITurn.StartTurn();
    }

    public void EndAITurn()
    {
        GameManager.instance.TurnManager.EndAITurn();
    }

    public void AddAITurn(StateMachine ai)
    {
        if (ai)
        {
            aiTurns.Add(ai, false);
            count++;
        }
    }

    public void RemoveAITurn(StateMachine ai)
    {
        if (ai)
        {
            aiTurns.Remove(ai);
            count--;
        }
    }
    public void UpdateTurn(StateMachine ai, bool value)
    {
        if (aiTurns.ContainsKey(ai))
        {
            aiTurns[ai] = value;
            activeCount += (value) ? 1 : -1;
        }
        else
        {
            Debug.Log("AI not found in turn list when ending turn");
        }
    }

    public void NextTurn(StateMachine ai)
    {
        currentTurnIndex++;
        aiTurns[ai] = false;
        if (currentTurnIndex >= keys.Count)
        {
            EndAITurn();
            currentAITurn = null;
            return;
        }
        aiTurns[keys[currentTurnIndex]] = true;
        currentAITurn = keys[currentTurnIndex];
        currentAITurn.StartTurn();
    }

    public bool IsMyTurn(StateMachine ai)
    {
        if (!GameManager.instance.isCombat)
        {
            if (aiTurns.ContainsKey(ai))
            {
                return aiTurns[ai];
            }
            Debug.Log("AI not found in turn list");
        }
        return false;
    }

    public void EndAllAITurn()
    {
        List<StateMachine> keys = aiTurns.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
        {
            aiTurns[keys[i]] = false;
        }
        EndAITurn();
    }
}
