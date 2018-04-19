/*
 * Honestly I'm not even sure if this should get its own class.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehavior : MonoBehaviour
{
	private GameManager m_gm;
	private Vector2 m_spawnPos;
	// Use this for initialization
	void Start()
	{
		m_gm = GameObject.FindObjectOfType<GameManager>();
		//m_spawnPos = m_gm.GetPlayerSpawnPosition();
		SpawnThisObject();
	}

	void SpawnThisObject()
	{
		gameObject.transform.position = m_spawnPos;
		return;
	}
}
