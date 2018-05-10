using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

	GameObject m_menuUI;
	GameObject m_loadScreen;
	GameObject menu;

	void Awake() {
		m_menuUI = transform.GetChild (0).gameObject;
		m_loadScreen = transform.GetChild (1).gameObject;

		m_menuUI.SetActive(true);
		m_loadScreen.SetActive (false);
		PauseGame.CanPause = false;
		FindObjectOfType<GUIHandler> ().SetHUD (false);
		ReturnToMenu ();
	}
		
	public void Resume()
	{
		m_menuUI.SetActive(false);
		m_loadScreen.SetActive (false);
		PauseGame.CanPause = true;
	}
			
	//-------------------------------------------------
	public void MenuNew() {
		SaveObjManager.Instance.resetRoomData ();
		FindObjectOfType<GUIHandler> ().SetHUD (true);
		SceneManager.LoadScene ("LB_BottomPoint");
	}
	public void MenuMiniGame() {
		SaveObjManager.Instance.resetRoomData ();
		PauseGame.CanPause = false;
		SceneManager.LoadScene ("minigame");
	}
	public void MenuLoad() {
		m_menuUI.SetActive(false);
		m_loadScreen.SetActive (true);
		m_loadScreen.GetComponent<SaveLoadMenu> ().Refresh ();
		m_loadScreen.GetComponent<SaveLoadMenu> ().Reset ();
	}

	public void MenuExit() {
		Application.Quit();
	}
	public void ReturnToMenu() {
		m_menuUI.SetActive(true);
		m_loadScreen.SetActive (false);
		EventSystem.current.SetSelectedGameObject(transform.Find("MainMenu").Find("New Game").gameObject);
	}
}