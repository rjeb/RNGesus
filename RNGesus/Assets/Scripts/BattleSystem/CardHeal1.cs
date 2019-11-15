using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHeal1 : BaseCard
{

    public CardHeal1()
    {
        this.name = "Baptism";
        this.value = Random.Range(30, 50);
        this.Type = "Heal";
        this.desc = "Target 1 player and heal 30 - 50 health";
        this.numTarget = 1;
    }

    public override void useCard()
    {
        Debug.Log("You selected the "); // ensure you picked right object
        switch (this.UserType)
        {
            case ("Player"):
                this.CardTargets[0].GetComponent<PlayerStateMachine>().addHP(this.value);
                this.CardTargets[0].GetComponent<PlayerStateMachine>().healExplode();
                break;
            case ("Enemy"):
                this.CardTargets[0].GetComponent<EnemyStateMachine>().addHP(this.value);
                this.CardTargets[0].GetComponent<EnemyStateMachine>().healExplode();
                break;
        }
    }

};
