using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour {

	public string toScene;
	public Vector2 toCoords;

	private GameManager m_gm;

	void Start()
	{
		m_gm = GameObject.Find("_GM").GetComponent<GameManager>();
		//TODO: Check if door is a HubWorld door and lock appropriately based on player progress.
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "player")
		{
			m_gm.SwitchToScene(toScene, toCoords);
		}
	}
}
