﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : MonoBehaviour
{
    public enum Turn{
        PLAYER,
        ENEMY
    }

    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION
    }

    public PerformAction battleStates;
    public Turn turn;

    public List<HandleTurn> PerformList = new List<HandleTurn>();
    public List<GameObject> PlayerCharacters = new List<GameObject>();
    public List<GameObject> EnemyCharacters = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        turn = Turn.ENEMY;
        battleStates = PerformAction.WAIT;
        EnemyCharacters.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        PlayerCharacters.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }

    // Update is called once per frame
    void Update()
    {
        switch (battleStates)
        {
            case (PerformAction.WAIT):
                if(PerformList.Count > 0){
                    battleStates = PerformAction.TAKEACTION;
                }
                break;
            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(PerformList[0].Attacker);
                if(PerformList[0].Type == "Enemy"){
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine> ();
                    ESM.playerToAttack = PerformList[0].AttackersTarget;
                    ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                }
                if(PerformList[0].Type == "Player"){
                    PlayerStateMachine PSM = performer.GetComponent<PlayerStateMachine>();
                    PSM.enemyToAttack = PerformList[0].AttackersTarget;
                    PSM.currentState = PlayerStateMachine.TurnState.ACTION;
                }
                if (PerformList.Count == 0) {
                    battleStates = PerformAction.WAIT;
                }
                break;
            case (PerformAction.PERFORMACTION):
                break;
        }
    }

    public void collectActions(HandleTurn input){
        PerformList.Add(input);
    }
}
