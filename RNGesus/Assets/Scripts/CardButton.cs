using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour
{
    BaseCard CardInfo;
    public GameObject UserGameObject; //the user game object
    public List<GameObject> CardTargets; //who is being targeted by the card

    private BattleStateMachine BSM;

    // Start is called before the first frame update
    void Start()
    {
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void selectTarget()
    {

    }
}
