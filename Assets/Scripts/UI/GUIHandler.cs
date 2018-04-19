using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GUIHandler : MonoBehaviour {

	public static GUIHandler Instance = null;

	public GameObject MenuProperty;
	public GameObject SelectionProperty;
	public TransferMenu MTransferMenu;

	[TextArea(1,10)]
	public string textMessage = "";

	public Slider P1HealthBar;
//	public Slider P1EnergyBar;
	public GameObject CurrentTarget;

	private bool displayTextMessage = false;
	private float displayTime;
	private float displayStart;
	private float displayTimePassed;

	private bool flashRed = false;
	private float flashTime;
	private float flashStart;
	private float flashTimePassed;

	private bool mainMenu = false;
	private float menuTime;
	private float menuStart;
	private float menuTimePassed;

	private GameManager gameManager;

	private int attemptNumber;
	private List<GameObject> PropertyLists;

    private Ability abiliityToUpdate;

	void Awake () {
		if (Instance == null)
			Instance = this;
		else if (Instance != this) {
			Destroy (gameObject);
		}
		gameManager = FindObjectOfType<GameManager> ();

		attemptNumber = 1;
		mainMenu = false;
		PropertyLists = new List<GameObject> ();
	}
	void Update() {
		if (CurrentTarget != null) {
			var P1Controller = CurrentTarget.GetComponent<Attackable> ();

			P1HealthBar.value = P1Controller.Health;
		}
	}

	public void displayText(string msg, float dTime) {
		displayTextMessage = true;
		textMessage = msg;
		displayTime = dTime;
		displayStart = Time.time;
		displayTimePassed = 0f;
	}
	// goes to main menu in 2 seconds
	private void GoToMainMenu(float wTime) {
		if (mainMenu == false) {
			mainMenu = true;
			menuTime = wTime;
			menuStart = Time.time;
			menuTimePassed = 0f;
		}
	}

	void OnGUI() {
		if (displayTextMessage) {
			var centeredStyle = GUI.skin.GetStyle("Label");
			centeredStyle.fontSize = 32;
			centeredStyle.alignment = TextAnchor.UpperCenter;
			int w = 1000;
			int h = 100;
			GUI.Label (new Rect (Screen.width/2-w/2, Screen.height/2-h/2, w, h), textMessage, centeredStyle);
		}
	}

	public static void CreateTransferMenu(PropertyHolder ph1, PropertyHolder ph2) {
		Instance.InternalTransferMenu (ph1 , ph2);
	}

	void InternalTransferMenu(PropertyHolder ph1, PropertyHolder ph2) {
		MTransferMenu.gameObject.SetActive (true);
		MTransferMenu.Clear ();

		MTransferMenu.AddPropertyHolder (ph1,0);
		MTransferMenu.AddPropertyHolder (ph2,1);
		MTransferMenu.init (ph1.NumTransfers);
	}

	public static void ClosePropertyLists() {
		foreach (GameObject go in Instance.PropertyLists) {
			Destroy (go);
		}
		Instance.PropertyLists.Clear ();
	}

    public static void SetAbility(Ability a)
    {
        Instance.abiliityToUpdate = a;
    }

    public static void UpdateAbility(Property p = null, Ability a = null)
    {
		if (Instance.abiliityToUpdate != null) {
			if (p)
				Instance.abiliityToUpdate.UpdateProperty (p);
			if (a)
				Instance.abiliityToUpdate.UpdateAbility (a);
			Instance.abiliityToUpdate.UseAbility ();
		}
    }
}