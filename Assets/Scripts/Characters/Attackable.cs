using System.Collections;
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
	public bool ResistStun = false;
	public bool ResistKnockback = false;
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
	private PhysicsSS m_movementController;
	private Fighter m_fighter;

	// public AudioClip Hit;

	internal void Awake()
	{
		m_movementController = GetComponent<PhysicsSS>();
		m_fighter = GetComponent<Fighter>();
		m_health = Mathf.Min (m_health, MaxHealth);
		m_currDeathTime = DeathTime;
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
	public void SetResistence(ElementType element, float percentage, bool overflow = false, bool isTimed = false, float duration = 0f, 
		bool resistStun = false, bool resistKnockback = false) {
		RemoveResistence (element);
		Resistence r = new Resistence ();
		r.Element = element;
		r.Percentage = percentage;
		r.Duration = duration;
		r.ResistStun = resistStun;
		r.ResistKnockback = resistKnockback;
		r.Timed = isTimed;
		Resistences.Add (r);
	}

	public void AddResistence(ElementType element, float percentage, bool overflow = false, bool isTimed = false, float duration = 0f, 
			bool resistStun = false, bool resistKnockback = false) {
		Resistence r;
		if (GetResistence (element) != null)
			r = GetResistence (element);
		else {
			r = new Resistence ();
			r.Element = element;
		}
		r.Percentage += percentage;
		r.Duration += duration;
		r.ResistStun = (r.ResistStun || resistStun);
		r.ResistKnockback = (r.ResistKnockback || resistKnockback);
		if (r.Timed)
			r.Timed = isTimed;
		Resistences.Add (r);
	}

	public void RemoveResistence(ElementType element) {
		foreach (Resistence r in Resistences) {
			if (r.Element == element) {
				Resistences.Remove (r);
				return;
			}
		}
	}
	public Resistence GetResistence(ElementType element) {
		foreach (Resistence r in Resistences) {
			if (r.Element == element) {
				return r;
			}
		}
		return null;
	}

	private void ApplyHitToPhysicsSS(Hitbox hb)
	{
		if (!m_movementController)
			return;

		if (hb.IsFixedKnockback)
		{
			m_movementController.AddToVelocity(hb.Knockback);
			return;
		}

		Vector3 hitVector = transform.position - hb.transform.position;
		float angle = Mathf.Atan2(hitVector.y,hitVector.x); //*180.0f / Mathf.PI;
		Vector2 force = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		force.Scale(new Vector2(hb.Knockback.magnitude, hb.Knockback.magnitude));
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
		if (r != null) {
			DamageObj (hb.Damage - (hb.Damage * (r.Percentage/100f)));
			if (!r.ResistKnockback)
				ApplyHitToPhysicsSS(hb);
			if (hb.Stun > 0f && m_fighter) {
				if (r.ResistStun)
					return HitResult.BLOCKED;
				m_fighter.RegisterStun (hb.Stun, false, hb);
			}
		} else {
			DamageObj(hb.Damage);
			ApplyHitToPhysicsSS(hb);
			if (hb.Stun > 0 && m_fighter)
				m_fighter.RegisterStun(hb.Stun, true, hb);
		}
		return HitResult.HIT;
	}

	internal void OnTriggerEnter2D(Collider2D other)
	{
		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnAttack ());
	}

	public void DamageObj(float damage)
	{
		m_health = Mathf.Max(Mathf.Min(MaxHealth, m_health - damage), 0);
		Alive = (m_health > 0);
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
