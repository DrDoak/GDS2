﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(HitboxMaker))]

public class Turret : MonoBehaviour {

	public bool DrawLOS = true;
	public GameObject TurretHead;
	public GameObject Projectile;
	public float ProjSpeed = 10f;
	public float ProjDamage = 10f;
	public float ProjStun = 0.5f;
	public float ProjDuration = 10f;
	public int ProjPenetration = 0;
	public Vector2 ProjKB = new Vector2(10f,0f);
	public ElementType ProjElement = ElementType.PHYSICAL;
	public float TimeBetweenVolleys = 2.0f;
	public int ShotsInVolley = 3;
	public float TimeBetweenShots = 0.2f;
	bool m_firing;
	public GameObject m_target;
	int m_shotsFiredInVolley = 0;

	float m_sinceLastVolley = 0f;
	float m_sinceLastShot = 0f;
	LineRenderer m_line;

	public void SetTarget(GameObject target) {
		m_target = target;
	}
	// Use this for initialization
	void Start () {
		m_line = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		m_sinceLastVolley += Time.deltaTime;
		if (m_target != null) {
			trackTarget ();
			if (m_sinceLastVolley > TimeBetweenVolleys)
				beginVolley ();
		} else {
			m_line.SetPosition (0, transform.position);
			m_line.SetPosition (1, transform.position);
		}
		if (m_firing) {
			fireVolley ();
		}
	}

	void fireVolley() {
		if (m_shotsFiredInVolley < ShotsInVolley) {
			m_sinceLastShot += Time.deltaTime;
			if (m_sinceLastShot > TimeBetweenShots)
				fireShot ();
		} else {
			m_firing = false;
		}
	}
	void trackTarget() {
		Vector3 pos = m_target.transform.position;
		orientTurretToPoint (pos);
		if (DrawLOS) {
			Vector3 currentPos = transform.position;
			Vector3 targetPos = m_target.transform.position;
			float ang = Mathf.Atan2 (targetPos.y - currentPos.y, targetPos.x - currentPos.x);
			m_line.SetPosition (0, transform.position);
			float range = ProjSpeed * ProjDuration;
			m_line.SetPosition (1, new Vector3 (currentPos.x + Mathf.Cos (ang) * range, currentPos.y + Mathf.Sin (ang) * range,0f));
		}
	}

	void orientTurretToPoint(Vector3 targetPoint) {
		Vector3 currentPos = transform.position;
		orientToDiff (new Vector2 (targetPoint.x - currentPos.x, targetPoint.y - currentPos.y));
	}

	void beginVolley() {
		m_sinceLastVolley = 0f;
		m_sinceLastShot = 0f;
		m_firing = true;
		m_shotsFiredInVolley = 0;
	}

	void fireShot() {
		m_shotsFiredInVolley ++;
		m_sinceLastShot = 0f;
		Vector3 currentPos = transform.position;
		Vector2 d = new Vector2 (m_target.transform.position.x - currentPos.x, m_target.transform.position.y - currentPos.y);
		Projectile p = GetComponent<HitboxMaker> ().CreateProjectile (Projectile, Vector2.zero, d.normalized ,
			ProjSpeed, ProjDamage, ProjStun, ProjDuration, ProjKB, false, ProjElement);
		p.PenetrativePower = ProjPenetration;
	}
	void orientToDiff(Vector2 diff) {
		TurretHead.transform.rotation = Quaternion.Euler (new Vector3(0f,0f,Mathf.Rad2Deg * Mathf.Atan2 (diff.y, diff.x)));
	}
}