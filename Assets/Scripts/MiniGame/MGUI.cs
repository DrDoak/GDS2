using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MGUI : MonoBehaviour {

	public TextMeshProUGUI p1deaths;
	int p1DeathCount = 0;
	public TextMeshProUGUI p2deaths;
	int p2DeathCount = 0;

	public TextMeshProUGUI p1Score;
	int p1ScoreCount = 0;
	public TextMeshProUGUI p2Score;
	int p2ScoreCount = 0;

	public TextMeshProUGUI congratsMessage;
	float messageDisplayTime;

	// Use this for initialization
	void Start () {
		p1Score = transform.Find ("P1Score").GetComponent<TextMeshProUGUI> ();
		p2Score = transform.Find ("P2Score").GetComponent<TextMeshProUGUI> ();
		p1deaths = transform.Find ("P1Deaths").GetComponent<TextMeshProUGUI> ();
		p2deaths = transform.Find ("P2Deaths").GetComponent<TextMeshProUGUI> ();
		congratsMessage = transform.Find ("Congrats").GetComponent<TextMeshProUGUI> ();
		messageDisplayTime = 10f;
	}
	
	// Update is called once per frame
	void Update () {
		if (messageDisplayTime > 0f) {
			messageDisplayTime -= Time.deltaTime;
			if (messageDisplayTime <= 0f) {
				congratsMessage.alpha = 0f;
			}
		}
	}

	public void UpdatePlayerDeath(string player) {
		if (player == "Player 1") {
			p1DeathCount += 1;
			p1deaths.SetText ("Player 1 Deaths: " + p1DeathCount);
		} else {
			p2DeathCount += 1;
			p2deaths.SetText ("Player 2 Deaths: " + p2DeathCount);
		}
	}

	public void UpdatePlayerWins(string player) {
		if (player == "Player 1") {
			p1ScoreCount += 1;
			p1Score.SetText ("Player 1 Wins: " + p1ScoreCount);
		} else {
			p2ScoreCount += 1;
			p2Score.SetText ("Player 2 Wins: " + p2ScoreCount);
		}
	}


	public void SetMessage(string message, float displayTime = 5f) { 
		if (congratsMessage.alpha == 0f) {
			congratsMessage.alpha = 1f;
			congratsMessage.SetText (message);
			messageDisplayTime = displayTime;
		} else {
			messageDisplayTime = 0.1f;
		}
	}
}
