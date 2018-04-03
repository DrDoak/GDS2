using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
	Attackable player;

	internal void Start()
	{
		foreach (BasicMovement playerObj in Object.FindObjectsOfType<BasicMovement>())
			if (playerObj.IsCurrentPlayer)
				player = playerObj.GetComponent<Attackable>();
	}

	internal void Update() 
	{
		if (Input.GetKeyDown (KeyCode.Minus))
			player.DamageObj(.5f);
		else if (Input.GetKeyDown (KeyCode.Equals))
			player.DamageObj(-.5f);
	}
}
