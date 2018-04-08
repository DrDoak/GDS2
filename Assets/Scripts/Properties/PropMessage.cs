﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ICustomMessageTarget : IEventSystemHandler
{
	// functions that can be called via the messaging system
	void OnCreation();
	void OnHit();
	void OnHitConfirm ();
	void OnSight(Observable observedObj);
	void OnDeath();
	void OnUpdate();
	void OnCollision();
	void OnAttack();
}