using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	public Slider healthSlider;
	public Image damageImage; //flashes color red across screen when player takes damage
	public float flashSpeed = 10f;
	public Color flashColor = new Color(1f, 1f, 1f);

	Attackable player;
	public float currentHealth;
	//bool isDead;
	bool damaged;

	void Start () {
		healthSlider = GetComponentInChildren<Slider> ();
		damageImage = GameObject.Find ("HealthIcon").GetComponentInChildren<Image> ();
		foreach (BasicMovement playerObj in Object.FindObjectsOfType<BasicMovement>()) {
			if (playerObj.IsCurrentPlayer) {
				player = playerObj.GetComponent<Attackable>();
			}
		}
	}
	 
	void Update () {
		CheckPlayerHealth ();
		UpdateHealthUI ();
		if (damaged) {
			damageImage.color = flashColor;
		}
		else {
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed*Time.deltaTime);
		}
		damaged = false;
	}

	void CheckPlayerHealth () {
		float oldHealth = currentHealth;
		currentHealth = player.Health;
		//isDead = player.alive;
		damaged = currentHealth < oldHealth;
	}

	void UpdateHealthUI () {
		healthSlider.value = currentHealth;
	}
}
