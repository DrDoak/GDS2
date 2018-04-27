using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	GameObject m_menuUI;
	GameObject m_saveScreen;
	GameObject m_loadScreen;
	GameObject menu;

	void Awake() {
		m_menuUI = transform.GetChild (0).gameObject;
		m_saveScreen = transform.GetChild (1).gameObject;
		m_loadScreen = transform.GetChild (2).gameObject;

		m_menuUI.SetActive(true);
		m_saveScreen.SetActive (false);
		m_loadScreen.SetActive (false);
		PauseGame.CanPause = false;
	}
		
	public void Resume()
	{
		m_menuUI.SetActive(false);
		m_saveScreen.SetActive (false);
		m_loadScreen.SetActive (false);
		PauseGame.CanPause = true;
	}
			
	//-------------------------------------------------
	public void MenuNew() {
		SaveObjManager.Instance.resetRoomData ();
		SceneManager.LoadScene ("LB_BottomPoint");
	}
	public void MenuLoad() {
		m_menuUI.SetActive(false);
		m_loadScreen.SetActive (true);
		m_loadScreen.GetComponent<SaveLoadMenu> ().Refresh ();
	}

	public void MenuExit() {
		Application.Quit();
	}
	public void ReturnToMenu() {
		m_menuUI.SetActive(true);
		m_saveScreen.SetActive (false);
		m_loadScreen.SetActive (false);
	}
}