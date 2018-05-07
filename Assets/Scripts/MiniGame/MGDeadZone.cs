using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGDeadZone : MonoBehaviour {

	void Start () {}
	void Update () {}

	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, .5f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}

	internal void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<MGPlayer> ()) {
			OnDeath (other.GetComponent<MGPlayer> ());
		} else if (other.GetComponent<Attackable> ()) {
			other.GetComponent<Attackable> ().DamageObj (200f);
		}
	}

	void OnDeath(MGPlayer player) {
		player.transform.position = player.SpawnPoint;
		player.GetComponent<Attackable> ().DamageObj (-100f);
		player.Bombs = 3;
		player.Barrels = 3;
		player.Blocks = 6;
		FindObjectOfType<MGUI> ().UpdatePlayerDeath (player.PlayerName);
	}
}
