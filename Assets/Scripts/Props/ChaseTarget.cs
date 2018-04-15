using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTarget : MonoBehaviour {

	public PhysicsSS Target;

	bool Available = true;
	bool m_targetingPoint = false;
	float m_delay = 0.0f;
	Vector3 m_currentTarget;
	Vector2 m_speed;
	float m_targetSpeed;

	[SerializeField] float ACCELERATION = 0.5f;
	[SerializeField] float CHASE_TOLERANCE = 0.2f;
	[SerializeField] float PURSUE_DISTANCE = 1000f;

	SpriteRenderer m_sprite;
	TrailRenderer trail;

	float timeOut = 0f;
	float targetTime = 0.6f;

	[SerializeField]
	bool m_isOrientToSpeed = false;

	[SerializeField]
	bool m_isDestroyOnProximity = false;
	[SerializeField]
	float m_destroyDistance = 0.5f;
	[SerializeField] private float m_spawnSpeed;

	[SerializeField]
	bool m_isDestroyWhenTargetGone = true;

	void Start () {
		m_sprite = GetComponent<SpriteRenderer> ();
		m_currentTarget = new Vector2 ();
		m_speed = new Vector2 ();

		trail = GetComponent<TrailRenderer> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (Target != null) {
			m_currentTarget = Target.transform.position;
			float d = Vector3.Distance (m_currentTarget, transform.position);
			if (d < PURSUE_DISTANCE) {
				chaseTarget ();
			}
			if (m_isDestroyOnProximity && d < m_destroyDistance) {
				Destroy (gameObject);
			}

		} else if (m_isDestroyWhenTargetGone) {
			Destroy (gameObject);
		}
	}

	void chaseTarget() {
		if (Vector3.Distance (transform.position, m_currentTarget) > CHASE_TOLERANCE) {
			m_speed.x += ACCELERATION * Time.deltaTime * Mathf.Sign (m_currentTarget.x - transform.position.x);
			m_speed.y += ACCELERATION * Time.deltaTime * Mathf.Sign (m_currentTarget.y - transform.position.y);
		} else {
			m_targetingPoint = false;
		}
		timeOut += Time.deltaTime;
		if (timeOut > targetTime) {
			m_targetingPoint = false;
		}
		m_speed *= 0.99f;
		if (m_isOrientToSpeed) {
			orientToSpeed (m_speed);
		}
		transform.Translate (m_speed,Space.World);
	}

	void orientToSpeed(Vector2 speed) {
		m_sprite.transform.rotation = Quaternion.Euler (new Vector3(0f,0f,Mathf.Rad2Deg * Mathf.Atan2 (speed.y, speed.x)));
	}
}