using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAttack1 : BaseCard
{

    public CardAttack1()
    {
        this.name = "Rush";
        this.value = 50;
        this.Type = "Attack";
        this.desc = "Target 1 enemy and deal 10 damage";
    }

    public override void useCard()
    {Debug.Log("You selected the "); // ensure you picked right object
        switch(this.UserType){
            case("Player"):
                this.CardTargets[0].GetComponent<EnemyStateMachine>().subtractHP(this.value);
                break;
            case("Enemy"):
                this.CardTargets[0].GetComponent<PlayerStateMachine>().subtractHP(this.value);
                break;
        }
    }

};
