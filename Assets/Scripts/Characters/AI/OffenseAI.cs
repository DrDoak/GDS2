﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Fighter))]
[RequireComponent (typeof (BasicMovement))]
public class OffenseAI : MonoBehaviour {

	public List<AttackInfo> allAttacks;
	AttackInfo currentAttack;
	public BasicMovement CurrentTarget;

	public float baseSpacing = 1.0f;
	public float baseReactionSpeed = 1.0f;
	public float baseDecisionMaking = 1.0f;
	public float baseAggression = 0.5f;

	float spacing;
	float reactionSpeed;
	float decisionMaking;
	float aggression;
	Fighter m_fighter;
	BasicMovement playable;

	public string currentAction = "wait";

	void Start () {
		spacing = baseSpacing;
		reactionSpeed = baseReactionSpeed;
		decisionMaking = baseDecisionMaking;
		aggression = baseAggression;
		allAttacks = new List<AttackInfo>();
		foreach (AttackInfo ai in GetComponents<AttackInfo> ()) {
			if (ai.name != "sheath" && ai.name != "unsheath") {
				allAttacks.Add (ai);
			}
		}
		m_fighter = GetComponent<Fighter> ();
		playable = GetComponent<BasicMovement> ();
	}

	void Update () {
		if (CurrentTarget != null && !playable.IsCurrentPlayer) {
			if (currentAction == "wait") {
				decideNextAction ();
			} else if (currentAction == "moveToTarget") {
				playable.MoveToPoint (CurrentTarget.transform.position);
				decideNextAction ();
			} else if (currentAction == "attack") {
				if (!m_fighter.IsAttacking()) {
					decideNextAction ();
				}
			}
		}
	}

	public void decideNextAction() {
		Vector3 otherPos = CurrentTarget.transform.position;
		float xDiff = Mathf.Abs(transform.position.x - otherPos.x);
		float yDiff = Mathf.Abs(transform.position.y - otherPos.y);
		if (Random.value < (aggression * 0.1f)) {
			foreach (AttackInfo ainfo in allAttacks) {
				if ((ainfo.AIPredictionHitbox.x + ainfo.AIPredictionOffset.x) +
				   (ainfo.AIPredictionHitbox.x + ainfo.AIPredictionOffset.x) * Random.Range (0f, 1f - spacing) > xDiff &&
				   (ainfo.AIPredictionHitbox.y + ainfo.AIPredictionOffset.y) +
					(ainfo.AIPredictionHitbox.y + ainfo.AIPredictionOffset.y) * Random.Range (0f, 1f - spacing) > yDiff && Random.value > 0.5f) {
					m_fighter.TryAttack (ainfo.AttackName);
					currentAction = "attack";
					allAttacks.Reverse ();
					break;
				}
			}
		}
		currentAction = "moveToTarget";
	}

	public void commitToAction() {}


	public void setTarget(BasicMovement c) {
		CurrentTarget = c;
	}
}