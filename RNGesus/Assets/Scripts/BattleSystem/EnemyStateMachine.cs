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
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();   
        startPosition = transform.position;
        observerList = new List<Observer>(); 
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

    //method to randomly generate enemy action
    void chooseAction(){
    
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = enemy.name;
        myAttack.Type = "Enemy";
        myAttack.AttackersGameObject = this.gameObject;
        myAttack.AttackersTargets.Add(BSM.PlayerCharacters[Random.Range(0, BSM.PlayerCharacters.Count)]);
        myAttack.cardToUse = new CardAttack1();
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
    
    //methods to move Enemy
    private bool MoveTowardsEnemy(Vector3 target){

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

    //methods that must be implement to inherit from interface 'Subject' Observable Design Pattern
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
        this.enemy.currentHP -= input;
    }

    public void addHP(float input){
        this.enemy.currentHP += input;
    }

}
