using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseCard
{
    public string name;

    public float defaultVal;
    public float value;
    void Roll(int roll) { }

    public string User; //name of Attacker
    public string Type;
    public GameObject UserGameObject; //the user game object
    public List<GameObject> CardTargets; //who is being targeted by the card
}
