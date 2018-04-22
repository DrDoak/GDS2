using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
	private static PauseGame m_instance;

	public static bool isPaused = false;
	public static bool CanPause = true;

	GameObject m_pauseMenuUI;
	GameObject m_saveScreen;
	GameObject m_loadScreen;

	float m_slowingSpeed = 0.0f;
	float m_speedingSpeed = 0.0f;

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
		m_saveScreen = transform.GetChild (1).gameObject;
		m_loadScreen = transform.GetChild (2).gameObject;
		m_pauseMenuUI.SetActive(false);
		m_saveScreen.SetActive (false);
		m_loadScreen.SetActive (false);
	}
		
	void Update () {
		if (CanPause && Input.GetButtonDown ("Pause"))
		{
			if (isPaused)
				Resume();
			else
				Pause();
		}
	}

	void speedSlowManage() {
		if (m_slowingSpeed > 0.0f && Time.timeScale > 0f) {
			Time.timeScale = Mathf.Max (0f, Time.timeScale - (m_slowingSpeed * Time.unscaledDeltaTime));
			if (Time.timeScale == 0f) {
				m_slowingSpeed = 0f;
			}
		}
		if (m_speedingSpeed > 0.0f && Time.timeScale < 1f) {
			Time.timeScale = Mathf.Min (1f, Time.timeScale + (m_speedingSpeed * Time.unscaledDeltaTime));
			if (Time.timeScale == 1f) {
				m_speedingSpeed = 0f;
			}
		}
	}

	public static void Resume() {
		m_instance.mResume ();
		m_instance.m_slowingSpeed = 0f;
	}

	public void mResume()
	{
		m_pauseMenuUI.SetActive(false);
		m_saveScreen.SetActive (false);
		m_loadScreen.SetActive (false);
		Time.timeScale = 1f;
		isPaused = false;
	}

	public static void Pause(bool drawMenu = true) {
		m_instance.mPause (drawMenu);
	}
	public static void SlowToPause(float slowSpeed = 1.0f) {
		m_instance.m_slowingSpeed = slowSpeed;
		isPaused = true;
	}
	public static void SpeedToResume(float speedSpeed = 1.0f) {
		m_instance.m_speedingSpeed = speedSpeed;
		isPaused = false;
	}

	void mPause(bool drawMenu = true)
	{
		if (drawMenu) {
			m_pauseMenuUI.SetActive (true);
		}
		Time.timeScale = 0f;
		isPaused = true;
	}
//-------------------------------------------------
	public void MenuResume() {
		mResume ();
	}
	public void MenuNew() {
	}
	public void MenuSave() {
		m_pauseMenuUI.SetActive(false);
		m_saveScreen.SetActive (true);
	}
	public void MenuLoad() {
		m_pauseMenuUI.SetActive(false);
		m_loadScreen.SetActive (true);
	}
	public void MenuMainMenu() {
		Time.timeScale = 1f;
		Debug.Log("Loading Menu...");
		SceneManager.LoadScene("StartMenu"); //get rid of this hardcode
	}
	public void MenuExit() {
		Debug.Log("Quiting Game...");
		Application.Quit();
	}
	public void ReturnToPause() {
		m_pauseMenuUI.SetActive(true);
		m_saveScreen.SetActive (false);
		m_loadScreen.SetActive (false);
	}
}
