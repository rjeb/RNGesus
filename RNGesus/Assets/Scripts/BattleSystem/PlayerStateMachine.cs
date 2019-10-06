using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour, Subject
{
    public BasePlayerCharacter player;

  
    private BattleStateMachine BSM;
    private Vector3 startPosition;
    public bool moved = false;

    //selection variables
     private Color startcolor;

    //TimeForAction variables
    private bool actionStarted = false;
    public GameObject enemyToAttack;
    private float animSpeed = 5f;

    //Subject variables
    List<Observer> observerList; 

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
        observerList = new List<Observer>(); 
        
        //generate 3 default cards for each player
        //TODO, add cards from a saved character state
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

    //method to randomly generate player action
    void chooseAction(){
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = player.name;
        myAttack.Type = "Player";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTargets.Add(BSM.EnemyCharacters[Random.Range(0, BSM.EnemyCharacters.Count)]);
        myAttack.cardToUse = new CardAttack1();
        BSM.collectActions(myAttack);
    }

    //coroutine to move to attack
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
        BSM.selectedCard.useCard();
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

        //notify the battle state machine that this player is done acting
        this.notifyObservers("PlayerActionDone1");
    }

    //methods to move player
    private bool MoveTowardsPlayer(Vector3 target){

        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target){

        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    //methods to change the color of the player character
    public void highlight(){
         startcolor = this.GetComponent<Renderer>().material.color;
         this.GetComponent<Renderer>().material.color = Color.grey;
    }
    public void dehighlight(){
         this.GetComponent<Renderer>().material.color = startcolor;
    }


    //methods that must be implement to inherit from interface 'Subject' (Observable design pattern)
    public void registerObserver(Observer o) { 
        observerList.Add(o);
    } 
  
    public void unregisterObserver(Observer o) { 
        observerList.Remove(o); 
    } 
 
    public void notifyObservers() 
    { 
        for (int i = 0; i < observerList.Count; i++){
            observerList[i].updateFromSubject();
        }
    } 

    public void notifyObservers(object o){
        for (int i = 0; i < observerList.Count; i++){
            observerList[i].updateFromSubject(o);
        }
    }

    //methods to manipulate HP
    public void subtractHP(float input){
        this.player.currentHP -= input;
    }

    public void addHP(float input){
        this.player.currentHP += input;
    }

}