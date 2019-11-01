using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //health UI variables
    public GameObject healthUI;
    public Slider slider;

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
        this.currentState = TurnState.IDLE;
        this.observerList = new List<Observer>(); 
        this.startcolor = this.GetComponent<Renderer>().material.color;
        
        //if playerCardList is empty, generate new list of cards
        if (this.player.Cards.Count == 0){
            this.generateCards();
        }

        player.currentHP = player.baseHP;
        slider.value = CalculateHealth();
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
        Vector3 enemyPosition = new Vector3(enemyToAttack.transform.position.x - 1.5f, this.startPosition.y, enemyToAttack.transform.position.z);
        while (MoveTowardsPlayer(enemyPosition)){
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
         this.GetComponent<Renderer>().material.color = Color.yellow;
    }
    public void dehighlight(){
         this.GetComponent<Renderer>().material.color = startcolor;
    }


    //methods that must be implement to inherit from interface 'Subject' (Observable design pattern)
    public void registerObserver(Observer o) { 
        if (observerList == null)
        {
            Debug.Log("PLAYER OBSERVER LIST IS NULL");
        }
        else
        {
            Debug.Log("PLAYER OBSERVER LIST ISNT NULL");
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

    float CalculateHealth(){
        return player.currentHP / player.baseHP;
    }

    //methods to manipulate HP
    public void subtractHP(float input){
        this.player.currentHP -= input;
        if (this.player.currentHP <= 0){
            this.player.currentHP = 0;
            this.currentState = TurnState.DEAD;
        }

        //health UI call
        slider.value = CalculateHealth();

        if (player.currentHP < player.baseHP){
            healthUI.SetActive(true);
        }

        if (player.currentHP > player.baseHP){
            player.currentHP = player.baseHP;
        }
    }

    public void addHP(float input){
        this.player.currentHP += input;
        
        //health UI call
        slider.value = CalculateHealth();

        if (player.currentHP < player.baseHP){
            healthUI.SetActive(true);
        }

        if (player.currentHP > player.baseHP){
            player.currentHP = player.baseHP;
        }
    }

    //methods to populate Card List
    
    //default generator if no inputs are given
    public void generateCards(){
        for (int i = 0; i < 30; i++)
        {
            CardAttack1 tmp = new CardAttack1();
            this.player.Cards.Add(tmp);
        } 
    }

    //sets Card List to incoming list if given
    public void generateCards(List<BaseCard> input){
        this.player.Cards.Clear();
        this.player.Cards = input;
    }

    public void usedCard(int i){
        this.player.Cards.RemoveAt(i);
    }

    public void usedCard(BaseCard cardInput){
        this.player.Cards.Remove(cardInput);
        Debug.Log("Used card: " + cardInput.name);
    }
}