using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseCard
{
    public string name;
    public float value;
    public string Type; //type of Card: Attack, Support, etc.
    public string desc; //description of the card

    public string User; //name of Attacker
    public string UserType; //Type of User: Player, Enemy
    public GameObject UserGameObject; //the user game object
    public List<GameObject> CardTargets; //who is being targeted by the card

    //Image associated with card
    public GameObject CardImage;

    //override this
    public virtual void useCard(){

    }


    public void setUserGameObject(GameObject input){
        this.UserGameObject = input;
    }

    public void setUserName(string input){
        this.User = input;
    }

    public void addTarget(GameObject input){
        this.CardTargets.Add(input);
    }

    public void addTargets(List<GameObject> inputs){
        for(int i = 0; i < inputs.Count; i++){
            if(CardTargets.Contains(inputs[i]) != true){
                CardTargets.Add(inputs[i]);
            }
        }

    }

    public void setTargets(List<GameObject> inputs){
        this.CardTargets = inputs;
    }

    public void clearTargets(){
        this.CardTargets.Clear();
    }

    public void setUserType(string input){
        this.UserType = input;
    }

}
