﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(LineRenderer))]
public class LineHitbox : Hitbox {
	public float range = 1000;
	private LineRenderer line;
	public Vector3 aimPoint = new Vector3();
	bool foundPoint = false;
	Vector2 endPoint = new Vector2();

	void Start ()
	{
		line = GetComponent<LineRenderer> ();
		//line.SetVertexCount (2);
		line.positionCount = 2;
		line.startWidth = 0.2f;
		line.startColor = Color.red;
	}

	void Update ()
	{
		base.Tick ();
		RaycastHit2D [] hit_list;
		hit_list = Physics2D.RaycastAll (transform.position, aimPoint, range);
		if (foundPoint) {
			line.SetPosition (0, transform.position);
			line.SetPosition (1, endPoint);
		} else {
			line.SetPosition (0, transform.position);
			line.SetPosition (1, transform.position + new Vector3 ((aimPoint * range).x, (aimPoint * range).y, 0f));
			foreach (RaycastHit2D hit in hit_list) {
				if (hit) {
					Collider2D collider = hit.collider;
					/*string hitType = OnTriggerEnter2D (collider);
					if (hitType != "none") {
						if (hitType == "reflect")
							getReflected (hit.point);
						line.SetPosition (0, transform.position);
						line.SetPosition (1, hit.point);
						foundPoint = true;
						endPoint = new Vector3 (hit.point.x, hit.point.y, 0f);
						return;
					}*/
				}
			}
		}
	}

	public void getReflected(Vector2 hitPoint) {
		string fac = "player";
		float offsetX = Random.Range (-15, 15) / 100f;
		float offsetY = Random.Range (-15, 15) / 100f;
		Vector2 realD = new Vector2 (-aimPoint.x + offsetX, -aimPoint.y + offsetY);
		Vector2 realKB = new Vector2 (-Knockback.x, Knockback.y);
		realD = Vector2.ClampMagnitude (realD, 1.0f);

		Vector3 newPos = new Vector3(hitPoint.x - aimPoint.x, hitPoint.y - aimPoint.y, 0);
		GameObject go = Instantiate(gameObject,newPos,Quaternion.identity) as GameObject; 
		LineHitbox line = go.GetComponent<LineHitbox> ();
		line.setRange (range);
		line.Damage = Damage;
		line.setAimPoint (realD);
		line.Duration = Duration;
		line.Knockback = realKB;
		line.IsFixedKnockback = true;
		//line.setFaction (faction);
		line.Creator = gameObject;
		//line.reflect = hitboxReflect;
		line.Stun = Stun;
		//line.mAttr = mAttrs;
	}

	public void setRange(float r) {
		range = r;
	}
	public void setAimPoint(Vector2 aP) {
		aimPoint = aP;
	}
}