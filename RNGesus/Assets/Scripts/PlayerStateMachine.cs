using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public BasePlayerCharacter player;

  
    private BattleStateMachine BSM;
    private Vector3 startPosition;
    public bool moved = false;

    //TimeForAction variables
    private bool actionStarted = false;
    public GameObject enemyToAttack;
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
        currentState = TurnState.IDLE;
        for (int i = 0; i < 3; i++)
        {
            CardAttack1 Card1 = new CardAttack1();
            player.Cards.Add(Card1);
        } 
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
                //chooseAction();
                //currentState = TurnState.IDLE;
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
        myAttack.Attacker = player.name;
        myAttack.Type = "Player";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTarget = BSM.EnemyCharacters[Random.Range(0, BSM.EnemyCharacters.Count)];
        BSM.collectActions(myAttack);
    }

    private IEnumerator TimeForAction(){
        if (actionStarted){
            yield break;
        }
        actionStarted = true;

        //animate the player near the enemy to attack
        yield return new WaitForSeconds(1.0f);
        Vector3 enemyPosition = new Vector3(enemyToAttack.transform.position.x - 1.5f, enemyToAttack.transform.position.y, enemyToAttack.transform.position.z);
        while (MoveTowardsPlayer(enemyPosition)){
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

        moved = true;
        currentState = TurnState.MOVED;
        //switch if all players have moved
        if (BSM.allPlayersMoved())
        {
            BSM.switchTurns();
        }
    }

    private bool MoveTowardsPlayer(Vector3 target){

        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target){

        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }


}