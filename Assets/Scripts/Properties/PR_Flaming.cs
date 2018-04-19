using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Flaming : Property
{
    Resistence fireResist;

    Vector2 scl = new Vector2(1.0f, 1.0f);
    Vector2 off = new Vector2(0f, 0f);
    float dmg = 5.0f;
    float stun = 0.5f;
    float hd = -0.5f;
    Vector2 kb = new Vector2(0.0f, 0.0f);
    HitboxDoT fireSurround;

    float time_tracker = 0.0f;
    float flaming_period = 0.1f;
    float flaming_damage = 5.0f;
	GameObject fx;



    public override void OnAddProperty()
    {
        GetComponent<Attackable>().Faction = FactionType.HOSTILE;
        fireResist = GetComponent<Attackable>().AddResistence(ElementType.FIRE, 50.0f, false, false);
        fireSurround = GetComponent<HitboxMaker>().CreateHitboxDoT(scl, off, dmg, stun, hd, kb,false, true, ElementType.FIRE);
		fireSurround.GetComponent<Hitbox> ().Faction = FactionType.HOSTILE;
		fx = GetComponent<PropertyHolder> ().AddBodyEffect (FXBody.Instance.FXFlame);
    }

    public override void OnRemoveProperty()
    {
        GetComponent<Attackable>().RemoveResistence(fireResist);
        GetComponent<HitboxMaker>().ClearHitboxes();
		GetComponent<PropertyHolder> ().RemoveBodyEffect (fx);
    }

    public override void OnUpdate()
    {
       // if (Time.time > time_tracker)
        //{
          //  time_tracker += flaming_period;
			GetComponent<Attackable>().TakeHit(fireSurround);
			GetComponent<Attackable>().TakeHit(fireSurround);
			GetComponent<Attackable>().TakeHit(fireSurround);
        //}
    }


    public override void OnHitboxCreate(Hitbox hitboxCreated)
    {
        hitboxCreated.Element = ElementType.FIRE;
    }
}
