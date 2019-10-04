using System.Collections;
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

    public enum PlayerGUI{
        WAITING,
        INPUT,
        SELECTING,
        ACTION,
        DONE,
    }

    public PerformAction battleStates;
    public Turn turn;
    public PlayerGUI playerInput;

    public GameObject hitObject;
    public GameObject selectedPlayer;
    public List<GameObject> selectedTargets;
    public bool cardsLoaded, playerSelected, targetsSelected;
    public BaseCard selectedCard;
    public List<BaseCard> CardInfo = new List<BaseCard>();

    public List<HandleTurn> PerformList = new List<HandleTurn>();
    public List<GameObject> PlayerCharacters = new List<GameObject>();
    public List<GameObject> EnemyCharacters = new List<GameObject>();
    public List<GameObject> CardButtons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        this.turn = Turn.ENEMY;
        this.battleStates = PerformAction.WAIT;
        this.playerInput = PlayerGUI.WAITING;

        this.cardsLoaded = false;
        this.playerSelected = false;

        EnemyCharacters.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        PlayerCharacters.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        CardButtons.AddRange(GameObject.FindGameObjectsWithTag("CardButton"));
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
        switch (playerInput){
            case (PlayerGUI.WAITING):
               if (this.turn == Turn.PLAYER){
                   if (playerSelected != true){
                        for (int i = 0; i < this.PlayerCharacters.Count; i++){
                            if(PlayerCharacters[i].GetComponent<PlayerStateMachine>().moved != true){
                                this.playerSelected = true;
                                selectedPlayer = PlayerCharacters[i];

                                List<BaseCard> cardList = selectedPlayer.GetComponent<PlayerStateMachine>().player.Cards;
                                this.CardInfo.Clear();
                                for (int j = 0; j < selectedPlayer.GetComponent<PlayerStateMachine>().player.Cards.Count; j++){
                                    //TODO add cards from player onto the GUI
                                    this.CardInfo.Add(selectedPlayer.GetComponent<PlayerStateMachine>().player.Cards[j]);
                                }
                                cardsLoaded = true;
                                break;
                            }
                        }
                    }
                   this.playerInput = PlayerGUI.INPUT;
               }
                break;
            case (PlayerGUI.INPUT):
               break;
            case (PlayerGUI.SELECTING):
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100)){
                    Debug.DrawLine(ray.origin, hit.point);
                    hitObject = hit.transform.gameObject;
                    //hit.transform.gameObject.GetComponent<EnemyStateMachine>().enabled = false;
                    EnemyStateMachine tmp = hitObject.GetComponent<EnemyStateMachine>();
                    if (tmp != null){
                        selectEnemy(EnemyCharacters.IndexOf(hitObject));
                    }
                }
                break;
            case (PlayerGUI.ACTION):
                this.performAction();
                selectedPlayer.GetComponent<PlayerStateMachine>().moved = true;
                if(this.allPlayersMoved() == true){
                    this.playerInput = PlayerGUI.INPUT;
                    this.playerSelected = false;
                    this.cardsLoaded = false;
                    this.targetsSelected = false;
                    this.switchTurns();
                }
                else{
                    this.playerSelected = false;
                    this.cardsLoaded = false;
                    this.targetsSelected = false;
                    this.playerInput = PlayerGUI.WAITING;
                }
               break;
            case (PlayerGUI.DONE):
               break;
        }
    }

    public void collectActions(HandleTurn input){
        PerformList.Add(input);
    }

    public void switchTurns(){
        if (this.turn == Turn.PLAYER){
            this.turn = Turn.ENEMY;
            for (int i = 0; i < this.EnemyCharacters.Count; i++)
            {
                this.EnemyCharacters[i].GetComponent<EnemyStateMachine>().currentState = EnemyStateMachine.TurnState.WAITING;
                this.EnemyCharacters[i].GetComponent<EnemyStateMachine>().moved = false;
            }
        }
        else if(this.turn == Turn.ENEMY){
            this.turn = Turn.PLAYER;
            for(int i = 0; i < this.EnemyCharacters.Count; i++){
                this.PlayerCharacters[i].GetComponent<PlayerStateMachine>().currentState = PlayerStateMachine.TurnState.WAITING;
                this.PlayerCharacters[i].GetComponent<PlayerStateMachine>().moved = false;
                this.playerSelected = false;
                this.cardsLoaded = false;
                this.targetsSelected = false;
                this.playerInput = PlayerGUI.WAITING;
            }
        }
    }

    public bool allPlayersMoved(){
        bool allMoved = true;
        for (int i = 0; i < this.PlayerCharacters.Count; i++)
        {
            if (this.PlayerCharacters[i].GetComponent<PlayerStateMachine>().moved == false)
            {
                allMoved = false;
                return allMoved;
            }
        }
        return allMoved;
    }

    public bool allEnemiesMoved(){
        bool allMoved = true;
        for(int i = 0; i < this.EnemyCharacters.Count; i++){
            if (this.EnemyCharacters[i].GetComponent<EnemyStateMachine>().moved == false){
                allMoved = false;
                return allMoved;
            }
        }
        return allMoved;
    }

    public void selectCard(int cardNum){
        if (this.playerInput == PlayerGUI.INPUT && this.playerSelected == true){
            this.selectedCard = CardInfo[cardNum];
            this.playerInput = PlayerGUI.SELECTING;
        }
    }

    public void selectEnemy(int enemyNum){
        if (this.playerInput == PlayerGUI.SELECTING){
            selectedTargets.Clear();
            selectedTargets.Add(this.EnemyCharacters[enemyNum]);
            this.playerInput = PlayerGUI.ACTION;  
        }
    }



    private void performAction(){
    
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = selectedPlayer.GetComponent<PlayerStateMachine>().player.name;
        myAttack.Type = "Player";
        myAttack.AttackersGameObject = selectedPlayer;
        myAttack.AttackersTarget = selectedTargets[0];
        this.collectActions(myAttack);
    }

}
