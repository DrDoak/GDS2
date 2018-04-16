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
//			Debug.Log (Screen.width + ", " + Screen.height);
			var centeredStyle = GUI.skin.GetStyle("Label");
			centeredStyle.fontSize = 32;
			centeredStyle.alignment = TextAnchor.UpperCenter;
			int w = 1000;
			int h = 100;
			GUI.Label (new Rect (Screen.width/2-w/2, Screen.height/2-h/2, w, h), textMessage, centeredStyle);
		}
	}

	public static void CreatePropertyList(List<Property> pList, string userName, Vector3 position, bool clickable = true) {
		Instance.InternalPropertyList (pList, userName, position, clickable);
	}

	void InternalPropertyList(List<Property> pList, string userName, Vector3 position, bool clickable) {
		GameObject gMenu = Instantiate (MenuProperty);
		gMenu.GetComponent<RectTransform> ().anchorMax = new Vector2(0f,1f);
		gMenu.GetComponent<RectTransform> ().anchorMin = new Vector2(0f,1f);
		gMenu.GetComponent<RectTransform> ().anchoredPosition = position;
		gMenu.GetComponent<RectTransform> ().SetParent (transform.Find ("PauseCanvas"), false);
		Debug.Log ("Positions: " + position);
		gMenu.transform.Find ("UserName").GetComponent<TextMeshProUGUI> ().SetText (userName);
		foreach (Property p in pList) {
			Property mp = (Property)GameManager.Instance.gameObject.GetComponentInChildren (p.GetType());
			GameObject selection = Instantiate (SelectionProperty, gMenu.transform.Find ("PropList"), false);
			selection.GetComponent<ButtonProperty> ().SelectedProperty = p;
			selection.transform.Find ("Image").GetComponent<Image>().sprite = mp.icon;
			selection.transform.Find ("Title").GetComponent<TextMeshProUGUI>().SetText( mp.PropertyName);
			selection.transform.Find ("Description").GetComponent<TextMeshProUGUI>().SetText( mp.Description);
			selection.GetComponent<Button> ().enabled = clickable;
		}
		PropertyLists.Add (gMenu);
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
        if(p)
            Instance.abiliityToUpdate.UpdateProperty(p);
        if (a)
            Instance.abiliityToUpdate.UpdateAbility(a);
    }
}