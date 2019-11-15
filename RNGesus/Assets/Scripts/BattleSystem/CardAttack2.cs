using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAttack2 : BaseCard
{

    public CardAttack2()
    {
        this.name = "Sodom & Gomorrah";
        this.value = 25;
        this.Type = "Attack";
        this.desc = "Deal 50 Damage spread across up to two targets";
        this.numTarget = 2;
    }

    public override void useCard()
    {
        Debug.Log("You selected the "); // ensure you picked right object
        switch (this.UserType)
        {
            case ("Player"):
                for (int i = 0; i < this.CardTargets.Count; i++)
                {
                    this.CardTargets[i].GetComponent<EnemyStateMachine>().subtractHP(this.value);
                    this.CardTargets[i].GetComponent<EnemyStateMachine>().damagedExplode();
                }
                //this.CardTargets[0].GetComponent<EnemyStateMachine>().subtractHP(this.value);
                break;
            case ("Enemy"):
                for (int i = 0; i < this.CardTargets.Count; i++)
                {
                    this.CardTargets[i].GetComponent<PlayerStateMachine>().subtractHP(this.value);
                    this.CardTargets[i].GetComponent<PlayerStateMachine>().damagedExplode();
                }
                //this.CardTargets[0].GetComponent<PlayerStateMachine>().subtractHP(this.value);
                break;
        }
    }

};
