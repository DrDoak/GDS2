using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Ability {

    public GameObject fireball;

    public override void UseAbility()
    {
        if (!fireball)
            fireball = Manager.GetObject(0);
        GameObject temp = GameObject.Instantiate(fireball, Player.transform.position + new Vector3(2,0), Player.transform.rotation);
    }

}
