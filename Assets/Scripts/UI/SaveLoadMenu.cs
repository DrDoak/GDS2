using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class SaveLoadMenu : MonoBehaviour {

	public bool MainMenu = false;
	TMP_Dropdown DropDown;
	TMP_InputField Input;
	List<string> m_savedProfiles;
	int m_selectedIndex = 0;
	// Use this for initialization
	void Awake () {
		DropDown = transform.Find ("Dropdown").GetComponent<TMP_Dropdown>();
		Debug.Log (DropDown);
		if (transform.Find ("ProfileInput") != null) {
			Input = transform.Find ("ProfileInput").gameObject.GetComponent<TMP_InputField> ();
			Debug.Log ("Found input field!");
			Debug.Log (Input);
		}
		if (m_selectedIndex != 0 && m_selectedIndex < DropDown.options.Count) {
			DropDown.value = m_selectedIndex;
		}
		Refresh ();
	}
	public void Refresh() {
		m_savedProfiles = new List<string> ();
		foreach (string dir in Directory.GetDirectories(SaveObjManager.saveBase))
		{
			string s = dir.Substring (SaveObjManager.saveBase.Length);
			//Debug.Log (dir + " : " + s);
			m_savedProfiles.Add (s);
		}
		DropDown.ClearOptions ();
		DropDown.AddOptions (m_savedProfiles);

	}
	public void OnProfileSelect(int index) {
		if (Input != null) {
			Debug.Log("setting input to: " + m_savedProfiles [DropDown.value]);
			Input.text = m_savedProfiles [DropDown.value];
		}
		m_selectedIndex = DropDown.value;
	}
	public void OnSave() {
		SaveObjManager.Instance.SaveProfile (Input.text);
		PauseGame.Instance.ReturnToPause ();
	}
	public void OnDelete() {
		SaveObjManager.Instance.DeleteProfile (Input.text);
		Refresh ();
	}
	public void OnLoad() {
		SaveObjManager.Instance.LoadProfile (m_savedProfiles[m_selectedIndex]);
		if (MainMenu) {
		} else {
			PauseGame.Resume ();
		}
	}
}
