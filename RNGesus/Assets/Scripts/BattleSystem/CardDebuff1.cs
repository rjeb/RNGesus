using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDebuff1 : BaseCard
{

    public CardDebuff1()
    {
        this.name = "Vampire";
        this.value = Random.Range(20, 50);
        this.Type = "Debuff";
        this.desc = "Target 1 enemy and deal 20 - 50 damage and heal half of your damage as HP";
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
                this.UserGameObject.GetComponent<PlayerStateMachine>().addHP(this.value / 2);
                break;
            case ("Enemy"):
                this.CardTargets[0].GetComponent<PlayerStateMachine>().subtractHP(this.value);
                this.CardTargets[0].GetComponent<PlayerStateMachine>().damagedExplode();
                this.UserGameObject.GetComponent<PlayerStateMachine>().addHP(this.value / 2);
                break;
        }
    }

};
