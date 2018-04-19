﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransferMenu : MonoBehaviour {
	
	public GameObject SelectionPrefab;
	public GameObject GhostSelectionPrefab;

	TextMeshProUGUI m_remainingText;
	TextMeshProUGUI m_infoText;

	Image m_centerImage;
	public Sprite ArrowLeft;
	public Sprite ArrowRight;
	public Sprite XImage;

	public GameObject m_selectedButton;
	GameObject m_currentGhost;

	int m_propSelected = 0;
	int m_listSelected = 0;
	int m_transfersRemaining  = 2;

	public float m_timeSinceExit = 0.0f;

	public Color m_highLightedColor;
	public Color m_deactiveColor;
	public Color m_normalColor;
	public Color m_highlightDeactiveColor;

	bool exiting = false;
	bool starting = false;

	const float SCROLL_SPEED = 1000f;
	private class PropertyMenu {
		public PropertyHolder holder;
		public int MaxSlots;
		public List<Property> propertyList;
		public string holderName;
		public List<GameObject> propertyButtons;
		public GameObject MenuPrefab;
		public int NumProps () { return propertyButtons.Count;  }
		public TextMeshProUGUI slotText;
		public PropertyMenu() {
			propertyList = new List<Property>();
			propertyButtons = new List<GameObject>();
		}
	}

	List<PropertyMenu> m_propMenus;
	PropertyMenu m_CurrentMenu;
	PropertyMenu m_OtherMenu;

	// Use this for initialization
	void Start () {
		m_infoText = transform.Find ("Info").GetComponent<TextMeshProUGUI>();
		m_remainingText = transform.Find ("Remaining").GetComponent<TextMeshProUGUI>();
		m_centerImage = transform.Find ("CenterImage").GetComponent<Image> ();

		m_propMenus = new List<PropertyMenu> ();

		PropertyMenu pm = new PropertyMenu ();
		pm.MenuPrefab = transform.Find ("MenuPropYou").gameObject;
		Transform pList = pm.MenuPrefab.transform.Find ("PropList");
		for (int i = 0; i < pList.childCount; i++) {
			pm.propertyButtons .Add (pList.GetChild (i).gameObject);
		}
		pm.slotText = pm.MenuPrefab.transform.Find ("Remaining").GetComponent<TextMeshProUGUI> ();
		m_propMenus.Add (pm);

		PropertyMenu pm2 = new PropertyMenu ();
		pm2.MenuPrefab = transform.Find ("MenuPropTarget").gameObject;
		pList =  pm2.MenuPrefab.transform.Find ("PropList");
		for (int i = 0; i < pList.childCount; i++) {
			pm2.propertyButtons .Add (pList.GetChild (i).gameObject);
		}
		pm2.slotText = pm2.MenuPrefab.transform.Find ("Remaining").GetComponent<TextMeshProUGUI> ();
		m_propMenus.Add (pm2);

		init (m_transfersRemaining);

		gameObject.SetActive (false);
		exiting = (m_transfersRemaining <= 0);
	}

	public void init(int transfers, bool resetPos = true) {
		m_OtherMenu = m_propMenus [1];
		foreach (PropertyMenu pm in m_propMenus) {
			foreach (GameObject go in pm.propertyButtons) {
				if (CanSelectProperty(go.GetComponent<ButtonProperty>().SelectedProperty, m_OtherMenu))
					go.GetComponent<Image> ().color = m_normalColor;
				else
					go.GetComponent<Image> ().color = m_deactiveColor;
			}
			m_OtherMenu = m_propMenus [0];
		}
		if (resetPos) { 
			m_listSelected = 0;
			m_propSelected = 0;
		}
		m_CurrentMenu = m_propMenus [m_listSelected];
		int otherMenu = (int)((m_listSelected + 1) % 2);

		m_OtherMenu = m_propMenus [otherMenu];
		m_propSelected = Mathf.Min (m_CurrentMenu.NumProps () - 1, m_propSelected);
		if (m_propSelected  < 0) {
			SwapLists ();
			m_propSelected = 0;
			m_propSelected = Mathf.Min (m_CurrentMenu.NumProps () - 1, m_propSelected);
		}
		if (m_propSelected < 0) {
			m_infoText.text = "No Properties Available";
			ExitMenu ();
			return;
		}
		m_timeSinceExit = 0.0f;
		m_transfersRemaining = transfers;
		m_remainingText.text = "Transfers Remaining: \n" + transfers.ToString ();
		m_selectedButton = m_CurrentMenu.propertyButtons [m_propSelected];
		HighlightKey (m_selectedButton);
		PauseGame.SlowToPause ();
		starting = true;
	}

	public void Clear() {
		foreach (PropertyMenu pm in m_propMenus) {
			foreach (GameObject go in pm.propertyButtons) {
				Destroy (go);
			}
			pm.propertyButtons.Clear ();
		}
	}

	public void AddPropertyHolder(PropertyHolder ph, int coordIndex) {
		PropertyMenu pm;
		pm = m_propMenus [coordIndex];
		pm.holderName = ph.gameObject.name;
		pm.MaxSlots = ph.MaxSlots;
		pm.propertyList = ph.GetStealableProperties ();
		pm.holder = ph;

		AddPropertyList (pm.propertyList, pm.holderName, coordIndex);
		int left = pm.MaxSlots - pm.NumProps ();
		pm.slotText.text = left.ToString () + " / " + pm.MaxSlots + "\n Free";
	}

	void SwapLists() {
		PropertyMenu temp = m_CurrentMenu;
		m_CurrentMenu = m_OtherMenu;
		m_OtherMenu = temp;
		m_listSelected = (int)((m_listSelected + 1) % 2);
	}

	void Update () {
		if (exiting) {
			m_timeSinceExit += Time.unscaledDeltaTime;
			if (m_timeSinceExit > 0.5f) {
				GetComponent<RectTransform> ().Translate (new Vector3 (0, 1000f * Time.unscaledDeltaTime, 0f));
				if (GetComponent<RectTransform> ().localPosition.y > 600) {
					PauseGame.Resume ();
					exiting = false;
				}
			}
		}
		if (starting) {
			if (GetComponent<RectTransform> ().localPosition.y > 0) {
				GetComponent<RectTransform> ().Translate (new Vector3 (0, -1000f * Time.unscaledDeltaTime, 0f));
			} else {
				starting = false;
			}
		}
	}

	void OnGUI () {
		if (m_selectedButton == null) {
			return;
		}

		Event e = Event.current;
		if (e.type == EventType.KeyDown)
		{
			if (e.keyCode == KeyCode.W) {
				SelectProperty (0, -1);
			} else if (e.keyCode == KeyCode.S) {
				SelectProperty (0, 1);
			} else if (e.keyCode == KeyCode.D) {
				SelectProperty (1, 0);
			} else if (e.keyCode == KeyCode.A) {
				SelectProperty (-1, 0);
			} else if (e.keyCode == KeyCode.J) {
				OnPropertySelect ();
			} else if (e.keyCode == KeyCode.Escape) {
				m_remainingText.text = "Ending Transfer";
				ExitMenu ();
			}
		}
	}

	void OnPropertySelect() {
		if (m_selectedButton == null)
			return;
		Property p = m_selectedButton.GetComponent<ButtonProperty> ().SelectedProperty;
		if (!CanSelectProperty(p,m_OtherMenu))
			return;
		if (m_currentGhost != null)
			Destroy (m_currentGhost);
		Clear ();
		//something.SetTransferLists(m_propMenus [0].propertyList, new List<Ability>(), m_propMenus [1].propertyList)
		AddPropertyHolder (m_propMenus [0].holder, 0);
		AddPropertyHolder (m_propMenus [1].holder, 1);
		m_transfersRemaining--;
		if (m_transfersRemaining == 0)
			m_remainingText.text = "No Transfers \nRemaining";
		if (m_transfersRemaining > 0) {
			init (m_transfersRemaining,false);
		} else {
			ExitMenu ();
		}
	}

	bool CanSelectProperty(Property p, PropertyMenu other) {
		if (other.NumProps() >= other.MaxSlots) {
			return false;
		}
		string pName = p.PropertyName;
		if (other.holder.HasProperty(pName)) {
			return false;
		}
		return true;
	}

	void ExitMenu() {
		if (m_currentGhost != null)
			Destroy (m_currentGhost);
		exiting = true;
		m_selectedButton = null;

	}

	void AddPropertyList(List<Property> pList, string userName, int menuIndex = 0) {
		PropertyMenu pm;
		pm = m_propMenus[menuIndex];

		pm.MenuPrefab.transform.Find ("UserName").GetComponent<TextMeshProUGUI> ().SetText (userName);
		foreach (Property p in pList) {
			pm.propertyButtons.Add(AddProperty(pm.MenuPrefab,p));
		}
	}

	private GameObject AddProperty(GameObject go, Property p) {
		Property mp = GameManager.Instance.GetPropInfo (p);
		GameObject selection = Instantiate (SelectionPrefab, go.transform.Find ("PropList"), false);
		selection.name = mp.PropertyName;
		selection.GetComponent<ButtonProperty> ().SelectedProperty = mp;
		selection.transform.Find ("Image").GetComponent<Image>().sprite = mp.icon;
		selection.transform.Find ("Title").GetComponent<TextMeshProUGUI>().SetText( mp.PropertyName);
		selection.transform.Find ("Description").GetComponent<TextMeshProUGUI>().SetText( mp.Description);
		return selection;
	}

	private void AddGhostProperty (GameObject go, Property mp) {
		GameObject selection = Instantiate (GhostSelectionPrefab, go.transform.Find ("PropList"), false);
		selection.name = mp.PropertyName;
		selection.GetComponent<ButtonProperty> ().SelectedProperty = mp;
		selection.transform.Find ("Image").GetComponent<Image>().sprite = mp.icon;
		selection.transform.Find ("Title").GetComponent<TextMeshProUGUI>().SetText( mp.PropertyName);
		selection.transform.Find ("Description").GetComponent<TextMeshProUGUI>().SetText( mp.Description);
		m_currentGhost = selection;
	}

	void SelectProperty (int xInput, int yInput) {
		if (m_selectedButton != null) {
			if (CanSelectProperty(m_selectedButton.GetComponent<ButtonProperty>().SelectedProperty, m_OtherMenu))
				m_selectedButton.GetComponent<Image> ().color = m_normalColor;
			else
				m_selectedButton.GetComponent<Image> ().color = m_deactiveColor;
		}
		if (m_currentGhost != null)
			Destroy (m_currentGhost);
					
		if (xInput == -1 && m_listSelected == 1) {
			if (m_propMenus[0].NumProps() > 0) {
				m_propSelected = Mathf.Min (m_propMenus[0].NumProps() - 1, m_propSelected);
				SwapLists ();
			}
		} else if (xInput == 1 && m_listSelected == 0) {
			if (m_propMenus[1].NumProps() > 0) {
				m_propSelected = Mathf.Min (m_propMenus[1].NumProps() - 1, m_propSelected);
				SwapLists ();
			}
		}

		if (yInput == -1 && m_propSelected > 0) {
			m_propSelected -= 1;
		} else if (yInput == 1 && m_propSelected < (m_CurrentMenu.NumProps() - 1 ) ){
			m_propSelected += 1;
		}
		HighlightKey(m_CurrentMenu.propertyButtons[m_propSelected]);
	}

	void HighlightKey(GameObject button) {
		m_selectedButton = button;
		Property p = button.GetComponent<ButtonProperty> ().SelectedProperty;
		if (CanSelectProperty(p,m_OtherMenu))
			m_selectedButton.GetComponent<Image> ().color = m_highLightedColor;
		else
			m_selectedButton.GetComponent<Image> ().color = m_highlightDeactiveColor;
		if (m_OtherMenu.NumProps() >= m_OtherMenu.MaxSlots) {
			m_centerImage.sprite = XImage;
			if (m_listSelected == 1) {
				m_infoText.text = "CANNOT TRANSFER! You have no free slots";
			} else {
				m_infoText.text = "CANNOT TRANSFER! Transfer Target has no free slots";
			}
			return;
		}

		string pName = p.PropertyName;

		//Debug.Log (pName + " : " + m_OtherMenu.holderName + " : " + m_OtherMenu.holder.HasProperty(pName));
		if (m_OtherMenu.holder.HasProperty(pName)) {
			m_centerImage.sprite = XImage;
			if (m_listSelected == 1) {
				m_infoText.text = "CANNOT TRANSFER! You already have this property";
			} else {
				m_infoText.text = "CANNOT TRANSFER! Transfer Target already has this property";
			}
			return;
		}

		if (m_listSelected == 0) {
			m_centerImage.sprite = ArrowRight;
			m_infoText.text = "Press 'J' to implant " + pName;
			AddGhostProperty (m_propMenus[1].MenuPrefab,p);
		} else {
			m_centerImage.sprite = ArrowLeft;
			m_infoText.text = "Press 'J' to steal " + pName;
			AddGhostProperty (m_propMenus[0].MenuPrefab,p);
		}
	}
}