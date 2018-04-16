using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Biological : Property
{

    float time_tracker = 0.0f;
    float damagetime_tracker = 0.0f;
    float damagetime_period = 3.0f;
    float heal_period = 0.1f;
    float heal_amount = 0.05f;

    public override void OnUpdate()
    {
        if(Time.time > time_tracker)
        {
            time_tracker += heal_period;
            if(Time.time > damagetime_tracker + damagetime_period)
            {
                GetComponent<Attackable>().DamageObj(heal_amount * -1.0f);
            }
            
        }
    }

    public override void OnHit(Hitbox hb, GameObject attacker)
    {
        damagetime_tracker = Time.time;
    }



}