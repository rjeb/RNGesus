using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public BaseEnemy enemy;

    private BattleStateMachine BSM;
    private Vector3 startPosition;
    public bool moved = false;

    //TimeForAction variables
    private bool actionStarted = false;
    public GameObject playerToAttack;
    private float animSpeed = 5f;

    public enum TurnState
    {
        WAITING,
        CHOOSEACTION,
        ACTION,
        IDLE,
        MOVED,
        DEAD
    }

    public TurnState currentState;

    // Start is called before the first frame update
    void Start()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();   
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {   
            //Idle state before taking turns
            case (TurnState.WAITING):
                currentState = TurnState.CHOOSEACTION;
                break;
            case (TurnState.CHOOSEACTION):
                chooseAction();
                currentState = TurnState.IDLE;
                break;
            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;
            //idle state after selecting
            case (TurnState.IDLE):
                break;
            //Idle state after taken turn
            case (TurnState.MOVED):
                break;
            case (TurnState.DEAD):
                break;

        }
    }

    void chooseAction(){
    
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = enemy.name;
        myAttack.Type = "Enemy";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.PlayerCharacters[Random.Range(0, BSM.PlayerCharacters.Count)];
        BSM.collectActions(myAttack);
    }

    private IEnumerator TimeForAction(){
        if (actionStarted){
            yield break;
        }
        actionStarted = true;

        //animate the enemy near the hero to attack
        Vector3 playerPosition = new Vector3(playerToAttack.transform.position.x + 1.5f, playerToAttack.transform.position.y, playerToAttack.transform.position.z);
        while (MoveTowardsEnemy(playerPosition)){
            yield return null;
        }
        //wait
        yield return new WaitForSeconds(1.5f);
        //do damage

        //animate back to start position
        Vector3 firstPosition = startPosition;
         while (MoveTowardsStart(firstPosition)){
            yield return null;
        }
        //remove this performer from list in BSM
        BSM.PerformList.RemoveAt(0);
        //reset BSM to WAIT
        BSM.battleStates = BattleStateMachine.PerformAction.WAIT;
        //end coroutine
        actionStarted = false;
        //determine if all enemies have moved
        moved = true;
        currentState = TurnState.MOVED;
        //switch turns if all enemies have moved
        if (BSM.allEnemiesMoved()){
            BSM.switchTurns();
        }
    }

    private bool MoveTowardsEnemy(Vector3 target){

        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target){

        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

}
