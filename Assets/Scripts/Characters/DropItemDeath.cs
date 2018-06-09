using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemDeath : MonoBehaviour {
	public GameObject ItemDropped;
	public bool StopMusic = true;
	public string DeathQuote = "";
	// Use this for initialization
	void Update() {
		if (ItemDropped != null && !GetComponent<Attackable>().Alive) {
			Instantiate (ItemDropped, transform.position, Quaternion.identity);
			ItemDropped = null;
			if (StopMusic) {
				AudioManager.instance.StopMusic ();
			}
			if (DeathQuote != "")
				TextboxManager.StartSequence (DeathQuote);
		}

	}
}