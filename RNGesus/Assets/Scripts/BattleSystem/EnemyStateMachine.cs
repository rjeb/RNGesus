using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour, Subject
{
    public BaseEnemy enemy;

    private BattleStateMachine BSM;
    private Vector3 startPosition;
    public bool moved = false;

    //selection variables
    private Color startcolor;

    //TimeForAction variables
    private bool actionStarted = false;
    public GameObject playerToAttack;
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
        this.BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();   
        this.startPosition = transform.position;
        this.observerList = new List<Observer>(); 
        this.startcolor = this.GetComponent<Renderer>().material.color;
        
        //if enemyCardList is empty, generate new list of cards
        if (this.enemy.Cards.Count == 0){
            this.generateCards();
        }

        if (observerList == null)
        {
            Debug.Log("OBSERVER LIST IS NULL");
        }
        else
        {
            Debug.Log("OBSERVER LIST ISNT NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {   
            //Idle state before taking turns
            case (TurnState.WAITING):
                if (BSM.turn == BattleStateMachine.Turn.PLAYER){
                    currentState = TurnState.IDLE;
                }
                else{
                    currentState = TurnState.CHOOSEACTION;
                }
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

    //method to randomly generate enemy action
    void chooseAction(){
    
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = enemy.name;
        myAttack.Type = "Enemy";
        myAttack.AttackersGameObject = this.gameObject;

        //if hand is full, select from 3 cards, else select from remaining cards
        if (this.enemy.Cards.Count >= 3){
            myAttack.cardToUse = this.enemy.Cards[Random.Range(0, 3)];
        }
        else{
            myAttack.cardToUse = this.enemy.Cards[Random.Range(0, this.enemy.Cards.Count - 1)];
        }
        myAttack.AttackersTargets.Add(BSM.PlayerCharacters[Random.Range(0, BSM.PlayerCharacters.Count)]);
        myAttack.populateCard();
        BSM.collectActions(myAttack);
    }

    //coroutine to move to attack
    private IEnumerator TimeForAction(){
        if (actionStarted){
            yield break;
        }
        actionStarted = true;

        //animate the enemy near the hero to attack
        yield return new WaitForSeconds(1.0f);
        Vector3 playerPosition = new Vector3(playerToAttack.transform.position.x + 1.5f, this.startPosition.y, playerToAttack.transform.position.z);
        while (MoveTowardsEnemy(playerPosition)){
            yield return null;
        }
        //wait
        yield return new WaitForSeconds(1.5f);
        //do damage
        BSM.PerformList[0].cardToUse.useCard();
        this.usedCard(BSM.PerformList[0].cardToUse);
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
        
        //notify BSM that this enemy has moved
        this.notifyObservers("EnemyActionDone1");

    }
    
    //methods to move Enemy
    private bool MoveTowardsEnemy(Vector3 target){

        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    private bool MoveTowardsStart(Vector3 target){

        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }
    
    //methods to change the color of the player character
    public void highlight(){
         this.GetComponent<Renderer>().material.color = Color.grey;
    }

    public void dehighlight(){
         this.GetComponent<Renderer>().material.color = startcolor;
    }

    //methods that must be implement to inherit from interface 'Subject' Observable Design Pattern
    public void registerObserver(Observer o) {
        if (observerList == null)
        {
            Debug.Log("ENEMY OBSERVER LIST IS NULL");
        }
        else
        {
            Debug.Log("ENEMY OBSERVER LIST ISNT NULL");
        }
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
        this.enemy.currentHP -= input;
        if (this.enemy.currentHP <= 0){
            this.enemy.currentHP = 0;
            this.currentState = TurnState.DEAD;
        }
    }

    public void addHP(float input){
        this.enemy.currentHP += input;
    }

    //methods to populate Card List
    
    //default generator if no inputs are given
    public void generateCards(){
        for (int i = 0; i < 30; i++)
        {
            CardAttack1 tmp = new CardAttack1();
            this.enemy.Cards.Add(tmp);
        } 
    }

    //sets Card List to incoming list if given
    public void generateCards(List<BaseCard> input){
        this.enemy.Cards.Clear();
        this.enemy.Cards = input;
    }

    public void usedCard(int i){
        this.enemy.Cards.RemoveAt(i);
    }

    public void usedCard(BaseCard cardInput){
        this.enemy.Cards.Remove(cardInput);
        Debug.Log("Used card: " + cardInput.name);
    }

}
