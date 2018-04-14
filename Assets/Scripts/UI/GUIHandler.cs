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

	void Awake () {
		if (Instance == null)
			Instance = this;
		else if (Instance != this) {
			Destroy (gameObject);
		}
		gameManager = FindObjectOfType<GameManager> ();

		attemptNumber = 1;
		mainMenu = false;
	}
	void Update() {
		if (CurrentTarget != null) {
			var P1Controller = CurrentTarget.GetComponent<Attackable> ();

			//P1EnergyBar.value = P1Controller.energy;
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
//			Debug.Log (Screen.width + ", " + Screen.height);
			var centeredStyle = GUI.skin.GetStyle("Label");
			centeredStyle.fontSize = 32;
			centeredStyle.alignment = TextAnchor.UpperCenter;
			int w = 1000;
			int h = 100;
			GUI.Label (new Rect (Screen.width/2-w/2, Screen.height/2-h/2, w, h), textMessage, centeredStyle);
		}
	}

	public static void CreatePropertyList(List<Property> pList, string userName) {
		Instance.InternalPropertyList (pList, userName);
	}

	void InternalPropertyList(List<Property> pList, string userName) {
		GameObject gMenu = Instantiate (MenuProperty, transform.Find ("PauseCanvas"), false);
		gMenu.transform.Find ("UserName").GetComponent<TextMeshProUGUI> ().SetText (userName);
		foreach (Property p in pList) {
			Property mp = (Property)GameManager.Instance.gameObject.GetComponentInChildren (p.GetType());
			GameObject selection = Instantiate (SelectionProperty, gMenu.transform.Find ("PropList"), false);
			selection.transform.Find ("Image").GetComponent<Image>().sprite = mp.icon;
			selection.transform.Find ("Title").GetComponent<TextMeshProUGUI>().SetText( mp.PropertyName);
			selection.transform.Find ("Description").GetComponent<TextMeshProUGUI>().SetText( mp.Description);
		}
	}
}