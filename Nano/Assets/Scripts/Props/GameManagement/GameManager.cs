/*
 * Singleton game manager intended to be persistent across all scenes.
 * Singleton maintained by creating a static instance of the class recursively.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	private static GameManager m_instance;

	public static GameManager Instance
	{
		get { return m_instance; }
		set { m_instance = value; }
	}

	private SessionPersistentData m_data;
	private GameProgress m_progress;

	void Awake()
	{
		if (m_instance == null)
		{
			//Instantiation logic should go entirely in here.
			m_instance = this;
			m_data = new SessionPersistentData();
			m_progress = new GameProgress();

			SceneManager.sceneLoaded += OnSceneLoaded;
		}
		else if (m_instance != this)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		string lastScene = m_data.LastScene;

		if (lastScene != null)
			Debug.Log("Last scene was:" + lastScene);
		else
			Debug.Log("No last scene. Should only see this message once on game load");

		//TODO: There needs to be a cleaner way to remove/toggle lighting in the hubworld.
		//		It may be more accessible to move this logic to a Light Controller?
		//		Also, obviously this needs to be looped.  Temporary for demo purposes.
		if (scene.name == "HubWorld")
		{
			//check lights and remove them according to game progress
			GameObject l0 = GameObject.Find("/ground0/Lantern");
			GameProgress.HubWorldDoorStatus l0s = m_progress.GetDoorState(0);
			if (l0s != GameProgress.HubWorldDoorStatus.completed)
				Destroy(l0);

			GameObject l1 = GameObject.Find("/ground1/Lantern");
			GameProgress.HubWorldDoorStatus l1s = m_progress.GetDoorState(1);
			if (l1s != GameProgress.HubWorldDoorStatus.completed)
				Destroy(l1);
		}
	}

	public void SwitchToScene(string to, Vector2 toCoords)
	{
		m_data.LastScene = SceneManager.GetActiveScene().name;
		m_data.ToCoords = toCoords;
		//TODO: Async version instead
		SceneManager.LoadScene(to);
	}

	public Vector2 GetPlayerSpawnPosition()
	{
		return m_data.ToCoords;
	}
}
