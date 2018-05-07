using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGFinish : MonoBehaviour {

	public GameObject HomePlayer;
	// Use this for initialization
	/*void Start () {

	}

	// Update is called once per frame
	void Update () {

	}*/

	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 1, 0, .5f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}

	internal void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<MGPlayer> () && other.gameObject != HomePlayer) {
			OnFinish (other.GetComponent<MGPlayer> ());
		}
	}

	void OnFinish(MGPlayer player) {
		FindObjectOfType<MGUI> ().UpdatePlayerWins (player.PlayerName);
		List<string> winMessages = new List<string> ();
		winMessages.Add (player.PlayerName + ", A Winner is You!");
		winMessages.Add (player.PlayerName + ", You're Winner!");
		winMessages.Add (player.PlayerName + ", You have prooved the Justice of our Culture!");
		winMessages.Add (player.PlayerName + ", Congraturations! You sucsess!");
		winMessages.Add (player.PlayerName + ", You have completed a Great Game!");
		winMessages.Add (player.PlayerName + ", Unbelievably Powerfull you are!");

		FindObjectOfType<MGUI> ().SetMessage (winMessages[Random.Range(0,winMessages.Count - 1)],5f);
		foreach (MGPlayer p in FindObjectsOfType<MGPlayer>()) {
			p.transform.position = p.SpawnPoint;
			p.GetComponent<Attackable> ().DamageObj (-100f);
			p.Bombs = 3;
			p.Blocks = 3;
			p.Barrels = 3;
		}
	}
}