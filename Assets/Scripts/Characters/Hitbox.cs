using UnityEngine;
using System.Collections.Generic;

public class Hitbox : MonoBehaviour {

	[SerializeField]
	private float m_damage = 10.0f;
	public float Damage { get { return m_damage; } set { m_damage = value; } }

	[SerializeField]
	private float m_duration = 1.0f;
	public float Duration { get { return m_duration; } set { m_duration = value; } }

	[SerializeField]
	private bool m_hasDuration = true;

	[SerializeField]
	private bool m_isFixedKnockback = false;
	public bool IsFixedKnockback { get { return m_isFixedKnockback; } set { m_isFixedKnockback = value; } }

	[SerializeField]
	private Vector2 m_knockback = new Vector2(0.0f,40.0f);
	public Vector2 Knockback { get { return m_knockback; } set { m_knockback = value; } }

	[SerializeField]
	private float m_stun = 0.0f;
	public float Stun { get { return m_stun; } set { m_stun = value; } }

	[SerializeField]
	private bool m_isRandomKnockback = false;
	public bool IsRandomKnockback { get { return m_isRandomKnockback; } set { m_isRandomKnockback = value; } }

	private List<string> m_hitTypes;
	public List<string> HitTypes { get { return m_hitTypes; } set { m_hitTypes = value; } }
	public FactionType Faction;

	[HideInInspector]
	public GameObject Creator { get; set; }

	[SerializeField]
	private GameObject m_followObj;

	[SerializeField]
	private Vector2 m_followOffset;

	[SerializeField]
	private List<Collider2D> m_upRightDownLeftColliders;

	private PhysicsSS m_creatorPhysics;
	private Vector4 m_knockbackRanges;
	protected List<Attackable> m_collidedObjs = new List<Attackable> ();
	protected List<Attackable> m_overlappingControl = new List<Attackable> (); 

	virtual public void Init()
	{
		m_creatorPhysics = Creator.GetComponent<PhysicsSS>();
		if (m_isRandomKnockback)
			RandomizeKnockback ();
		m_hasDuration = m_duration > 0;
		Tick();
	}

	virtual internal void Update()
	{
		Tick();
	}

	protected void Tick()
	{
		if (m_followObj != null)
			FollowObj();
		SwitchActiveCollider(m_creatorPhysics.FacingLeft);
		if (m_hasDuration)
			MaintainOrDestroyHitbox();
	}

	public void SetScale(Vector2 scale)
	{
		transform.localScale = scale;
	}

	public void SetFollow(GameObject obj, Vector2 offset)
	{
		m_followObj = obj;
		m_followOffset = offset;
	}

	public void SetKnockbackRanges (float minX, float maxX,float minY, float maxY)
	{
		IsRandomKnockback = true;
		IsFixedKnockback = true;
		m_knockbackRanges = new Vector4 (minX, maxX, minY, maxY);
	}

	public bool HasHitTypes()
	{
		return HitTypes != null && HitTypes.Count > 0;
	}

	private void MaintainOrDestroyHitbox()
	{
		if (m_duration <= 0.0f)
			GameObject.Destroy (gameObject);
		Duration -= Time.deltaTime;
	}

	private void FollowObj()
	{
		transform.position = new Vector3(m_followObj.transform.position.x + m_followOffset.x, m_followObj.transform.position.y + m_followOffset.y,0);
	}

	private void RandomizeKnockback ()
	{
		m_knockback.x = Random.Range (m_knockbackRanges.x, m_knockbackRanges.y);
		m_knockback.y = Random.Range (m_knockbackRanges.z, m_knockbackRanges.w);
	}

	protected void OnAttackable(Attackable atkObj)
	{
		if (!atkObj || atkObj.gameObject == Creator || m_collidedObjs.Contains (atkObj) || !atkObj.CanAttack(Faction))
			return;
		if (IsRandomKnockback)
			RandomizeKnockback();
		atkObj.TakeHit(this);
		m_collidedObjs.Add (atkObj);
		if (!m_overlappingControl.Contains (atkObj))
			m_overlappingControl.Add (atkObj);
	}

	internal void OnTriggerEnter2D(Collider2D other)
	{
		OnAttackable (other.gameObject.GetComponent<Attackable> ());
	}

	internal void OnTriggerExit2D(Collider2D other)
	{
		/*
		 * TODO: Delay removal of collided object to avoid stuttered collisions 
		 */
		/*
		if (other.gameObject.GetComponent<Attackable> () && collidedObjs.Contains(other.gameObject.GetComponent<Attackable>())) {
			collidedObjs.Remove (other.gameObject.GetComponent<Attackable> ());
		}
		*/
		if (other.gameObject.GetComponent<Attackable> () 
			&& m_overlappingControl.Contains(other.gameObject.GetComponent<Attackable>())) {
			m_overlappingControl.Remove (other.gameObject.GetComponent<Attackable> ());
		}
	}

	private void SwitchActiveCollider(bool FacingLeft)
	{
		if (m_upRightDownLeftColliders.Count == 0)
			return;
		var dirIndex = ConvertDirToUpRightDownLeftIndex(FacingLeft);
		// Or'd check on enabled in case collider falls under several categories
		for (var i = 0; i < m_upRightDownLeftColliders.Count; i++)
		{
			m_upRightDownLeftColliders[i].enabled |= (i == dirIndex);
			Debug.Log (m_upRightDownLeftColliders [i].enabled);
		}
	}

	private int ConvertDirToUpRightDownLeftIndex(bool FacingLeft)
	{
		if (FacingLeft)
			return 3;
		return 1;
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, .8f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
}