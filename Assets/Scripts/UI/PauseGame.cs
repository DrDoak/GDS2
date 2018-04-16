using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
	private static PauseGame m_instance;

	public static bool isPaused = false;
	GameObject m_pauseMenuUI;

	[SerializeField] public Scene startMenu;
	public static PauseGame Instance
	{
		get { return m_instance; }
		set { m_instance = value; }
	}

	void Awake() {
		if (m_instance == null)
		{
			m_instance = this;
		}
		else if (m_instance != this)
		{
			Destroy(gameObject);
			return;
		}
		m_pauseMenuUI = transform.GetChild (0).gameObject;
		m_pauseMenuUI.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Pause"))
		{
			if (isPaused)
				Resume();
			else
				Pause();
		}
	}
	public static void Resume() {
		m_instance.mResume ();
	}

	public void mResume()
	{
		m_pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		isPaused = false;
		GUIHandler.ClosePropertyLists ();
	}

	public static void Pause(bool drawMenu = true) {
		m_instance.mPause (drawMenu);
	}

	void mPause(bool drawMenu = true)
	{
		if (drawMenu) {
			m_pauseMenuUI.SetActive (true);
			Vector3 v = new Vector3 (200f, -200f);
			GUIHandler.CreatePropertyList (GameManager.Instance.CurrentPlayer.GetComponent<PropertyHolder> ().GetStealableProperties (), "Your Properties", v,false);
		}
		Time.timeScale = 0f;
		isPaused = true;
		//Vector3 v = new Vector3 (150f, 300f);
	}

	public void LoadMenu()
	{
		Time.timeScale = 1f;
		Debug.Log("Loading Menu...");
		SceneManager.LoadScene("StartMenu"); //get rid of this hardcode
	}

	public void QuitGame()
	{
		Debug.Log("Quiting Game...");
		Application.Quit();
	}
}
