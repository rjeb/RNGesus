using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAttack1 : BaseCard
{

    public CardAttack1()
    {
        this.name = "Rush";
        this.defaultVal = 10;
        this.Type = "Attack";
    }

    void Roll(int roll)
    {
        this.value = roll * 10;
    }

};
