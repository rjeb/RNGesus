using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurn
{
    public string Attacker; //name of Attacker
    public string Type; //Type of Attacker: Player, Enemy
    public GameObject AttackersGameObject; //who attacks
    public List<GameObject> AttackersTargets; // who is going to be attacked
    public BaseCard cardToUse; //card to use

    public HandleTurn(){
        this.AttackersTargets = new List<GameObject>();
    }

    //sets the cards variables using HandleTurns variables
    public void populateCard(){
        this.cardToUse.setUserGameObject(this.AttackersGameObject);
        this.cardToUse.setUserName(this.Attacker);
        this.cardToUse.setTargets(this.AttackersTargets);
        this.cardToUse.setUserType(this.Type);
    }


}
