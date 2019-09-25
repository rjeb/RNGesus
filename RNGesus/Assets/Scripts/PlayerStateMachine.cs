using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public BasePlayerCharacter player;

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
    
    void Start()
    {
        currentState = TurnState.WAITING;
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
