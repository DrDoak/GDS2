using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{

	public static bool isPaused = false;
	GameObject m_pauseMenuUI;

	[SerializeField] public Scene startMenu;


	void Awake() {
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

	public void Resume()
	{
		m_pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		isPaused = false;
	}

	void Pause()
	{
		m_pauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		isPaused = true;
		//Vector3 v = new Vector3 (150f, 300f);
		Vector3 v = new Vector3 (200f, -200f);
		GUIHandler.CreatePropertyList (GameManager.Instance.CurrentPlayer.GetComponent<PropertyHolder> ().GetStealableProperties (), "Your Properties", v);
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
