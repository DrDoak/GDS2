﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum FactionType { NONE, ALLIES, ENEMIES, HOSTILE };

[System.Serializable]
public class Resistence {
	public ElementType Element;
	public float Duration = 0f;
	public float Percentage = 0f;
	public bool Timed;
	public float StunResist = 0f;
	public float KnockbackResist = 0f;
	public bool AvoidOverflow = false;
	public float OverflowAmount = 0f;
}

public class Attackable : MonoBehaviour
{
	private float m_health = 100.0f;
	public float Health { get { return m_health; } private set { m_health = value; } }
	public float MaxHealth = 100.0f;

	public bool Alive = true;
	public float DeathTime = 0.0f;
	private float m_currDeathTime;
	public FactionType Faction = FactionType.HOSTILE;

	public List<Resistence> Resistences  = new List<Resistence>();
	private Dictionary< ElementType, Resistence> m_fullResistences = new Dictionary< ElementType, Resistence>();
	private PhysicsSS m_movementController;
	private Fighter m_fighter;


	// public AudioClip Hit;

	internal void Awake()
	{
		m_movementController = GetComponent<PhysicsSS>();
		m_fighter = GetComponent<Fighter>();
		m_health = Mathf.Min (m_health, MaxHealth);
		m_currDeathTime = DeathTime;
		InitResistences ();
	}

	internal void InitResistences() {
		m_fullResistences.Clear ();
//		ElementType[] eList = new ElementType[ElementType.PHYSICAL, ElementType.FIRE,
//			ElementType.BIOLOGICAL, ElementType.LIGHTNING, ElementType.PSYCHIC];
		//RangeInt l = new RangeInt(1,4);
		for (int i=0; i < 5; i++) {
			Resistence r = new Resistence ();
			r.Element = (ElementType)i;
			m_fullResistences.Add ( (ElementType)i, r);
		}
		foreach (Resistence r in Resistences) {
			AddResistence (r);
		}
	}
	internal void Start() {
		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnCreation ());
	}

	private void CheckDeath()
	{
		Alive = m_health > 0;
		if (Alive)
			return;
		
		if (m_currDeathTime < 0.0f) {
			ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnDeath ());
			Destroy (gameObject);
		}
		m_currDeathTime -= Time.deltaTime;
	}

	private void CheckResistanceValidities()
	{
		foreach (Resistence r in Resistences)
		{
			if (r.Timed) {
				r.Duration -= Time.deltaTime;
				if (r.Duration <= 0.0f)
					Resistences.Remove(r);
			}
		}
	}

	internal void Update() {
		CheckDeath();
		CheckResistanceValidities();
		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnUpdate ());
	}
	public Resistence SetResistence(ElementType element, float percentage, bool overflow = false, bool isTimed = false, float duration = 0f, 
		float resistStun = 0f, float resistKnockback = 0f) {
		ClearResistence (element);
		Resistence r = newResist (element, percentage, overflow, isTimed, duration, resistStun, resistKnockback);
		AddResistence (r);
		return r;
	}

	private Resistence newResist(ElementType element, float percentage, bool overflow = false, bool isTimed = false, float duration = 0f, 
		float resistStun = 0f, float resistKnockback = 0f) {
		Resistence r = new Resistence ();
		r.Element = element;
		r.Percentage = percentage;
		r.Duration = duration;
		r.StunResist = resistStun;
		r.KnockbackResist = resistKnockback;
		r.Timed = isTimed;
		r.AvoidOverflow = overflow;
		return r;
	}
	public Resistence AddResistence(ElementType element, float percentage, bool overflow = false, bool isTimed = false, float duration = 0f, 
		float resistStun = 0f, float resistKnockback = 0f) {
		Resistence r = newResist (element, percentage, overflow, isTimed, duration, resistStun, resistKnockback);
		AddResistence (r);
		return r;
	}

	public void AddResistence(Resistence r) {
		Resistence fr = m_fullResistences[r.Element];
		float pDiff = r.Percentage;
		if (r.AvoidOverflow) {
			pDiff = Mathf.Min (100f - fr.Percentage, r.Percentage);
			r.OverflowAmount = Mathf.Max(0f, r.Percentage - (100f - fr.Percentage));
		}
		fr.Percentage += pDiff;
		fr.StunResist += r.StunResist;
		fr.KnockbackResist += r.KnockbackResist;
		if (!Resistences.Contains(r))
			Resistences.Add (r);
	}
	public void RemoveResistence(Resistence r) {
		if (!Resistences.Contains (r))
			return;
		Resistence fr = m_fullResistences[r.Element];
		fr.Percentage -= r.Percentage;
		fr.StunResist -= r.StunResist;
		fr.KnockbackResist -= r.KnockbackResist;
		Resistences.Remove (r);
		foreach (Resistence or in Resistences) {
			if (or.Element == r.Element && r.AvoidOverflow) {
				float pDiff = Mathf.Min (100f - fr.Percentage, or.OverflowAmount);
				or.OverflowAmount = Mathf.Max(0f, or.OverflowAmount - (100f - fr.Percentage));
				fr.Percentage += pDiff;
			}
		}
	}

	public void ClearResistence(ElementType element) {
		foreach (Resistence r in Resistences) {
			if (r.Element == element) {
				Resistences.Remove (r);
			}
		}
		Resistence re = new Resistence();
		re.Element = element;
		m_fullResistences [element] = re;
	}

	public Resistence GetResistence(ElementType element) {
		return m_fullResistences [element];
	}

	private void ApplyHitToPhysicsSS(Hitbox hb)
	{
		Resistence r = GetResistence (hb.Element);
		Vector2 kb = hb.Knockback - (hb.Knockback * Mathf.Min(1f,(r.KnockbackResist/100f)));
		if (!m_movementController)
			return;

		if (hb.IsFixedKnockback)
		{
			m_movementController.AddToVelocity(kb);
			return;
		}

		Vector3 hitVector = transform.position - hb.transform.position;
		float angle = Mathf.Atan2(hitVector.y,hitVector.x); //*180.0f / Mathf.PI;
		Vector2 force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		force.Scale(new Vector2(kb.magnitude, kb.magnitude));
		float counterF = m_movementController.Velocity.y * (1 / Time.deltaTime);
		if (counterF < 0)
			force.y = force.y - counterF;
		
		m_movementController.AddToVelocity(force);
	}

	public HitResult TakeHit(Hitbox hb)
	{
		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnHit (hb, hb.Creator));
		if (GetComponent<AIFighter>()) {
			GetComponent<AIFighter> ().OnHit (hb);
		}
		Resistence r =  GetResistence(hb.Element);
		float d;
		d = hb.Damage - (hb.Damage * (r.Percentage / 100f));
		d = DamageObj (d);

		ApplyHitToPhysicsSS(hb);
		float s = hb.Stun - (hb.Stun * Mathf.Min(1f,(r.StunResist/100f)));
		if (hb.Stun > 0f && m_fighter) {
			if (s <= 0f)
				return HitResult.BLOCKED;
			Debug.Log ("Registering Stun: " + s);
			m_fighter.RegisterStun (s, true, hb);
		}

		if (d == 0f) {
			return HitResult.NONE;
		} else if (d < 0f) {
			return HitResult.HIT;
		} else {
			return HitResult.HEAL;
		}
	}

	internal void OnTriggerEnter2D(Collider2D other)
	{
		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnAttack ());
	}

	public float DamageObj(float damage)
	{
		float healthBefore = m_health;
		m_health = Mathf.Max(Mathf.Min(MaxHealth, m_health - damage), 0);
		Alive = (m_health > 0);
		return m_health - healthBefore;
	}

	public bool CanAttack(FactionType otherFaction) {
		return (otherFaction == FactionType.HOSTILE || Faction == FactionType.HOSTILE || otherFaction != Faction);
	}

	public void SetFaction(FactionType f) {
		Faction = f;
		if (GetComponent<HitboxMaker> ())
			GetComponent<HitboxMaker> ().Faction = f;
	}
		
}
