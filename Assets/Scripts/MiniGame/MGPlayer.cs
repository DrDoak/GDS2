using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MGPlayer : BasicMovement {
	public KeyCode LeftButton;
	public KeyCode RightButton;
	public KeyCode JumpButton;
	public KeyCode AttackButton;
	public KeyCode BombButton;
	public KeyCode BlockButton;
	public KeyCode BarrelButton;
	public KeyCode Help;
	public KeyCode Quit;

	public int Bombs = 3;
	public int Blocks = 6;
	public int Barrels = 3;
	public string PlayerName = "Player 1";

	string helpmessage = "Player 1: AD = Move. W = Jump \n S = attack." +
		"\n F/G/H = Throw Items." +
		"\n Player 2: Left/Right = Move. Up = Jump" +
		"\n Down = Attack. </>/? = Throw Items" +
		"\n Press Enter to toggle Help" +
		"\n Press Escape to Return to Menu.";

	public Vector3 SpawnPoint;
	new internal void Awake()
	{
		base.Awake ();
		SpawnPoint = transform.position;
	}


	new internal void Update()
	{
		base.Update ();
		if (GetComponent<Attackable> ().Alive == false && GetComponent<Attackable> ().m_currDeathTime < 1.0f) {
			GetComponent<Attackable> ().Alive = true;
			GetComponent<PhysicsSS> ().CanMove = true;
			transform.position = SpawnPoint;
			GetComponent<Attackable> ().m_currDeathTime = 0f;// GetComponent<Attackable> ().DeathTime;
			GetComponent<Attackable> ().DamageObj (-100f);
			GetComponent<Fighter> ().StunTime = 0f;
			FindObjectOfType<MGUI> ().UpdatePlayerDeath (PlayerName);
			Bombs = 3;
			Barrels = 3;
			Blocks = 6;
		}
	}

	protected override void PlayerMovement() {
		float hInp = 0f;
		if (Input.GetKey (LeftButton)) {
			hInp -= 1f;
		}
		if (Input.GetKey (RightButton)) {
			hInp += 1f;
		}
		if (Input.GetKey(AttackButton)) {
			GetComponent<Fighter>().TryAttack("default");
		}
		if (Input.GetKeyDown (BombButton)) {
			if (Bombs > 0) {
				GetComponent<Fighter> ().TryAttack ("bomb");
				Bombs -= 1;
			}
		}

		if (Input.GetKeyDown (BlockButton)) {
			if (Blocks > 0) {
				GetComponent<Fighter> ().TryAttack ("block");
				Blocks -= 1;
			}
		}
		if (Input.GetKeyDown (BarrelButton)) {
			if (Barrels > 0) {
				GetComponent<Fighter> ().TryAttack ("barrel");
				Barrels -= 1;
			}
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (FindObjectOfType<GameManager> ()) {
				Rect r = new Rect ();
				r.height = 1.0f;
				r.width = 1.0f;
				GameManager.Instance.GetComponent<Camera> ().rect = r;
			}
			SceneManager.LoadScene ("MainMenu");
		}
		if (Input.GetKeyDown (Help)) {
			FindObjectOfType<MGUI> ().SetMessage (helpmessage, 10f);
		}

		m_inputMove = new Vector2(hInp, 0f);
		m_jumpDown = Input.GetKeyDown (JumpButton);
		m_jumpHold = Input.GetKey (JumpButton);
		JumpMovement ();
		SetDirectionFromInput();
	}
}