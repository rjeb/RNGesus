using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleStateMachine : MonoBehaviour, Observer
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
        IDLE
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
    public GameObject TextUI;

    //link to different scenes
    public string overWorldScene;
    public string titleScene;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        instantiateScene();

        //populate all lists with corresponding GameObjects
        EnemyCharacters.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        PlayerCharacters.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        CardButtons.AddRange(GameObject.FindGameObjectsWithTag("CardButton"));
        TextUI = GameObject.FindGameObjectWithTag("TextUI");

        yield return null;

        //add BattleStateMachine as an observer for all enemies and players
        for(int i = 0; i < EnemyCharacters.Count; i++){
            EnemyCharacters[i].GetComponent<EnemyStateMachine>().registerObserver(this);
        }

        for(int i = 0; i < PlayerCharacters.Count; i++){
            PlayerCharacters[i].GetComponent<PlayerStateMachine>().registerObserver(this);
        }
        
        this.turn = Turn.PLAYER;
        this.battleStates = PerformAction.WAIT;
        this.playerInput = PlayerGUI.WAITING;

        this.cardsLoaded = false;
        this.playerSelected = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        //switch on battleState
        switch (battleStates)
        {
            //exit waiting state when the perform list is populated(either player or enemy have chosen a move)
            case (PerformAction.WAIT):
                if(PerformList.Count > 0){
                    battleStates = PerformAction.TAKEACTION;
                }
                break;
            //perform the action until perform list is empty again (then switch to waiting state)
            case (PerformAction.TAKEACTION):
                GameObject performer = GameObject.Find(PerformList[0].AttackersGameObject.name);
                if(PerformList[0].Type == "Enemy"){
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine> ();
                    ESM.playerToAttack = PerformList[0].AttackersTargets[0];
                    ESM.currentState = EnemyStateMachine.TurnState.ACTION;
                }
                if(PerformList[0].Type == "Player"){
                    PlayerStateMachine PSM = performer.GetComponent<PlayerStateMachine>();
                    PSM.enemyToAttack = PerformList[0].AttackersTargets[0];
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
            //waiting on choosing the acting player
            case (PlayerGUI.WAITING):
                if (playerSelected != true){
                    for (int i = 0; i < this.PlayerCharacters.Count; i++){
                        if(PlayerCharacters[i].GetComponent<PlayerStateMachine>().moved != true && PlayerCharacters[i].GetComponent<PlayerStateMachine>().currentState != PlayerStateMachine.TurnState.DEAD){
                            this.playerSelected = true;
                            selectedPlayer = PlayerCharacters[i];
                            selectedPlayer.GetComponent<PlayerStateMachine>().highlight();

                            List<BaseCard> cardList = selectedPlayer.GetComponent<PlayerStateMachine>().player.Cards;
                            this.CardInfo.Clear();
                            for (int j = 0; j < selectedPlayer.GetComponent<PlayerStateMachine>().player.Cards.Count; j++){
                                //TODO add cards from player onto the GUI
                                this.CardInfo.Add(selectedPlayer.GetComponent<PlayerStateMachine>().player.Cards[j]);
                            }
                            //call method to load images for selected player
                            this.loadImages();
                            cardsLoaded = true;
                            break;
                        }
                    }
                }
                this.playerInput = PlayerGUI.INPUT;
                break;
            //waiting on player input(choose card)
            case (PlayerGUI.INPUT):
               detectHitObjectUI();
               break;
            //waiting on chosen targets for card
            case (PlayerGUI.SELECTING):
                /*
                //highlight enemy on mouse over
                if (this.detectHitObject() == true){
                        EnemyStateMachine tmp = hitObject.GetComponent<EnemyStateMachine>();
                        if (tmp != null){
                            tmp.highlight();
                            //tmp.dehighlight();
                        }
                    }
                */
                if ( Input.GetMouseButtonDown (0)){ 
                    if (this.detectHitObject2D() == true){
                        if (selectedCard.Type == "Attack" || selectedCard.Type == "Special" || selectedCard.Type == "Debuff"){
                            EnemyStateMachine tmp = hitObject.GetComponent<EnemyStateMachine>();
                            if (tmp != null){
                                selectEnemy(EnemyCharacters.IndexOf(hitObject));
                            }
                        }
                        else if (selectedCard.Type == "Heal" || selectedCard.Type == "Special" || selectedCard.Type == "Defend"){
                            PlayerStateMachine tmp = hitObject.GetComponent<PlayerStateMachine>();
                            if (tmp != null){
                                selectPlayer(PlayerCharacters.IndexOf(hitObject));
                            }
                        }
                    }
                 }
                break;
            //sending an action to the perform action queue based on selected parameters(player, card, enemy(s))
            case (PlayerGUI.ACTION):
                this.performAction();
                selectedPlayer.GetComponent<PlayerStateMachine>().moved = true;
                this.playerSelected = false;
                this.cardsLoaded = false;
                this.targetsSelected = false;
                this.playerInput = PlayerGUI.IDLE;
               break;
            //all players have moved so switch turns
            case (PlayerGUI.DONE):
               break;
            case (PlayerGUI.IDLE):
                break;
        }
    }

    //add an action-to-do to the list of actions
    public void collectActions(HandleTurn input){
        PerformList.Add(input);
    }

    //switch the turn between player and enemy turn
    public void switchTurns(){
        if (this.turn == Turn.PLAYER){
            for (int i = 0; i < this.EnemyCharacters.Count; i++)
            {
                if (this.EnemyCharacters[i].GetComponent<EnemyStateMachine>().currentState != EnemyStateMachine.TurnState.DEAD){
                    this.EnemyCharacters[i].GetComponent<EnemyStateMachine>().currentState = EnemyStateMachine.TurnState.WAITING;
                    this.EnemyCharacters[i].GetComponent<EnemyStateMachine>().moved = false;
                }
            }
            this.playerSelected = false;
            this.cardsLoaded = false;
            this.targetsSelected = false;
            this.turn = Turn.ENEMY;
            this.playerInput = PlayerGUI.IDLE;
        }
        else if(this.turn == Turn.ENEMY){
            for(int i = 0; i < this.PlayerCharacters.Count; i++){
                if (this.PlayerCharacters[i].GetComponent<PlayerStateMachine>().currentState != PlayerStateMachine.TurnState.DEAD){
                    this.PlayerCharacters[i].GetComponent<PlayerStateMachine>().currentState = PlayerStateMachine.TurnState.WAITING;
                    this.PlayerCharacters[i].GetComponent<PlayerStateMachine>().moved = false;
                }
            }
            this.playerSelected = false;
            this.cardsLoaded = false;
            this.targetsSelected = false;
            this.playerInput = PlayerGUI.WAITING;
            this.turn = Turn.PLAYER;
        }
    }

    //returns true if all players have moved
    public bool allPlayersMoved(){
        bool allMoved = true;
        for (int i = 0; i < this.PlayerCharacters.Count; i++)
        {
            if (this.PlayerCharacters[i].GetComponent<PlayerStateMachine>().moved == false && PlayerCharacters[i].GetComponent<PlayerStateMachine>().currentState != PlayerStateMachine.TurnState.DEAD)
            {
                allMoved = false;
                return allMoved;
            }
        }
        return allMoved;
    }

    //returns true if all enemies have moved
    public bool allEnemiesMoved(){
        bool allMoved = true;
        for(int i = 0; i < this.EnemyCharacters.Count; i++){
            //dead enemies are not considered when checking for enemies not moved
            if (this.EnemyCharacters[i].GetComponent<EnemyStateMachine>().moved == false && EnemyCharacters[i].GetComponent<EnemyStateMachine>().currentState != EnemyStateMachine.TurnState.DEAD){
                allMoved = false;
                return allMoved;
            }
        }
        return allMoved;
    }

    //returns true if all player characters are in deadState
    public bool allPlayersDead(){
        bool allDead = true;
        for(int i = 0; i < this.PlayerCharacters.Count; i++){
            if (this.PlayerCharacters[i].GetComponent<PlayerStateMachine>().currentState != PlayerStateMachine.TurnState.DEAD){
                allDead = false;
                return allDead;
            }
        }
        return allDead;
    }

    //returns true if all enemy characters are in deadState
    public bool allEnemiesDead(){
        bool allDead = true;
        for(int i = 0; i < this.EnemyCharacters.Count; i++){
            if (this.EnemyCharacters[i].GetComponent<EnemyStateMachine>().currentState != EnemyStateMachine.TurnState.DEAD){
                allDead = false;
                Debug.Log("They aren't all dead");
                return allDead;
            }
        }
        Debug.Log("They are all dead");
        return allDead;
    }

    //select the card with index cardNum
    public void selectCard(int cardNum){
        if (this.playerInput == PlayerGUI.INPUT && this.playerSelected == true){
            this.selectedCard = CardInfo[cardNum];
            this.playerInput = PlayerGUI.SELECTING;
        }
    }

    public void displayCard(int cardNum){
        //return if Deck is smaller than the selected Card number
        if (cardNum > selectedPlayer.GetComponent<PlayerStateMachine>().player.Cards.Count - 1){
            return;
        }
        if (this.playerInput == PlayerGUI.INPUT && this.playerSelected == true){
            this.displayText(CardInfo[cardNum].name + ": " + CardInfo[cardNum].desc);
        }
    }

    //load card Images from selectedPlayers
    public void loadImages(){
        List<BaseCard> tmpCards = selectedPlayer.GetComponent<PlayerStateMachine>().player.Cards;
        if (tmpCards.Count >= 3){
            for (int i = 0; i < 3; i++){
                Sprite cardSprite1 = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Art/Sprites/UI/CardAttack.png", typeof(Sprite));
                Sprite cardSprite2 = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Art/Sprites/UI/CardDefend.png", typeof(Sprite));
                Sprite cardSprite3 = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Art/Sprites/UI/CardHeal.png", typeof(Sprite));
                Sprite cardSprite4 = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Art/Sprites/UI/CardDebuff.png", typeof(Sprite));
                Sprite cardSprite5 = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Art/Sprites/UI/CardMiracle.png", typeof(Sprite));
                //Sprite cardSprite = Resources.Load<Sprite>("Images/CardImages/Attack1");
                if(tmpCards[i].Type == "Attack"){
                    CardButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = cardSprite1;
                }
                else if(tmpCards[i].Type == "Defend"){
                    CardButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = cardSprite2;
                }
                else if(tmpCards[i].Type == "Heal"){
                    CardButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = cardSprite3;
                }
                else if(tmpCards[i].Type == "Debuff"){
                    CardButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = cardSprite4;
                }
                else if(tmpCards[i].Type == "Special"){
                    CardButtons[i].GetComponent<UnityEngine.UI.Image>().sprite = cardSprite5;
                }
            }
        }
    }

    //select the enemy with index cardNum
    public void selectEnemy(int enemyNum){
            selectedTargets.Add(this.EnemyCharacters[enemyNum]);
            if (selectedTargets.Count >= selectedCard.numTarget){
                this.playerInput = PlayerGUI.ACTION;
            }
    }

    //select the player with index cardNum
    public void selectPlayer(int playerNum){
            selectedTargets.Add(this.PlayerCharacters[playerNum]);
            if (selectedTargets.Count >= selectedCard.numTarget){
                this.playerInput = PlayerGUI.ACTION;
            }
    }

    //add action to queue based on current selected player, card, and enemy
    private void performAction(){
        HandleTurn myAttack = new HandleTurn();
        myAttack.Attacker = selectedPlayer.GetComponent<PlayerStateMachine>().player.name;
        myAttack.Type = "Player";
        myAttack.AttackersGameObject = selectedPlayer;
        myAttack.AttackersTargets  = selectedTargets;
        myAttack.cardToUse = selectedCard;
        myAttack.populateCard();
        this.collectActions(myAttack);
    }

    //detects if mouse is pointing to a 3D object and saves it in variable hitObject
    public bool detectHitObject(){
        RaycastHit hit; 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        if ( Physics.Raycast (ray,out hit,100.0f)) {
            this.hitObject = hit.transform.gameObject;
            Debug.Log("You selected the " + hitObject.name); // ensure you picked right object
            return true;
        }
        else{
            return false;
        }
    }

    //detects if mouse is pointing to a 3D object and saves it in variable hitObject
    public bool detectHitObject2D(){
        //Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
        //RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        for (int i = 0; i < hits.Length; i++){
            //If something was hit, the RaycastHit2D.collider will not be null.
            if (hits[i].collider != null)
            {
                this.hitObject = hits[i].transform.gameObject;
                Debug.Log("You selected the " + hitObject.name); // ensure you picked right object
                return true;
            }
        }
        return false;
    }

    public bool detectHitObjectUI(){

        //detect if mouse is over a UI element
        if ((EventSystem.current.IsPointerOverGameObject()) && EventSystem.current.currentSelectedGameObject != null){
            Debug.Log("mouse over UI");
            return true;
        }
        else{
            return false;
        }
    }

    public void instantiateScene(){
        //set up lists
        List<string> playerStrings = CharacterManager.getCharacters();
        List<string> enemyStrings = CharacterManager.getEnemys();

        //populate Lists
        for(int i = 0; i < playerStrings.Count; i++){
            Instantiate((GameObject)AssetDatabase.LoadAssetAtPath(playerStrings[i], typeof(GameObject)), new Vector3(-3 + (0.5f * i), 1, 0 + (1 * i)), Quaternion.identity);
        }

        for(int i = 0; i < enemyStrings.Count; i++){
            Instantiate((GameObject)AssetDatabase.LoadAssetAtPath(enemyStrings[i], typeof(GameObject)), new Vector3(5 + (-1 * i), 2, 0 + (1 * i)), Quaternion.identity);
        }
    }

    public void populateLists(){

        //clear all lists
        EnemyCharacters.Clear();
        PlayerCharacters.Clear();
        CardButtons.Clear();

        //populate all lists with corresponding GameObjects
        EnemyCharacters.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        PlayerCharacters.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        CardButtons.AddRange(GameObject.FindGameObjectsWithTag("CardButton"));
        TextUI = GameObject.FindGameObjectWithTag("TextUI");

        //add BattleStateMachine as an observer for all enemies and players
        for(int i = 0; i < EnemyCharacters.Count; i++){
            EnemyCharacters[i].GetComponent<EnemyStateMachine>().registerObserver(this);
        }

        for(int i = 0; i < PlayerCharacters.Count; i++){
            PlayerCharacters[i].GetComponent<PlayerStateMachine>().registerObserver(this);
        }
    }

    //display information to the TextUI
    public void displayText(string input){
        TextUI.GetComponentInChildren<Text>().text = input;
    }


    //methods that must be implemented because this implements interface observer
    public void updateFromSubject(){
        Debug.Log("Recieved empty update from subject");
    }

    public void updateFromSubject(object o){
        if (o.GetType() == typeof(string)){
        //different case based on string passed in
            switch((string)o){
                case("PlayerActionDone1"):
                    //deselect the player and move on to the next state, allowing the next player to be selected
                    selectedTargets.Clear(); //deselect Targeted enemies from moved player
                    if (allEnemiesDead() == true){
                        SceneManager.LoadScene(overWorldScene);
                    }
                    this.selectedPlayer.GetComponent<PlayerStateMachine>().dehighlight();
                    this.playerInput = PlayerGUI.WAITING;
                    
                    //if every player has moved, proceed to switch turns to Enemy Turn
                    if(this.allPlayersMoved() == true){
                        this.switchTurns();
                    }
                    break;
                case ("EnemyActionDone1"):
                    //deselect the player and move on to the next state, allowing the next player to be selected
                    if (allPlayersDead() == true)
                    {
                        SceneManager.LoadScene(titleScene);
                    }
                                    //if every player has moved, proceed to switch turns to Enemy Turn
                    if(this.allEnemiesMoved() == true){
                        this.switchTurns();
                    }
                    break;
            }
        }
    }
}
