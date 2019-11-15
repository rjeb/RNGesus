using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAttack3 : BaseCard
{

    public CardAttack3()
    {
        this.name = "The First Stone";
        this.value = Random.Range(60, 120);
        this.Type = "Attack";
        this.desc = "Target 1 enemy and deal 60 - 120 damage, and half to yourself as recoil";
        this.numTarget = 1;
    }

    public override void useCard()
    {
        Debug.Log("You selected the "); // ensure you picked right object
        switch (this.UserType)
        {
            case ("Player"):
                this.CardTargets[0].GetComponent<EnemyStateMachine>().subtractHP(this.value);
                this.CardTargets[0].GetComponent<EnemyStateMachine>().damagedExplode();
                this.UserGameObject.GetComponent<PlayerStateMachine>().subtractHP(this.value / 2);
                break;
            case ("Enemy"):
                this.CardTargets[0].GetComponent<PlayerStateMachine>().subtractHP(this.value);
                this.CardTargets[0].GetComponent<PlayerStateMachine>().damagedExplode();
                this.UserGameObject.GetComponent<EnemyStateMachine>().subtractHP(this.value / 2);
                break;
        }
    }

};
