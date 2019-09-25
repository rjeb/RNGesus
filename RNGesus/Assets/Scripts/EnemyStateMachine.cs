using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public BaseEnemy enemy;

    public enum TurnState
    {
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        MOVED,
        DEAD
    }

    public TurnState currentState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (TurnState.ADDTOLIST):
                break;
            case (TurnState.WAITING):
                break;
            case (TurnState.SELECTING):
                break;
            case (TurnState.ACTION):
                break;
            case (TurnState.MOVED):
                break;
            case (TurnState.DEAD):
                break;

        }
    }
}
