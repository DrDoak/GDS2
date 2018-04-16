/*
 * Singleton game manager intended to be persistent across all scenes.
 * Singleton maintained by creating a static instance of the class recursively.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	private static GameManager m_instance;
	public GameObject FXExplosionPrefab;
	public GameObject FXPropertyPrefab;
	public GameObject FXPropertyGetPrefab;

	public GameObject FXHitPhysical;
	public GameObject FXHitFire;
	public GameObject FXHitLightning;
	public GameObject FXHitBiological;
	public GameObject FXHitPsychic;
	public GameObject FXHitBlock;
	public GameObject FXHeal;

	public GameObject IconPropertyPrefab;
	public GameObject TextPropertyPrefab;
	public Sprite UnknownPropertyIcon;
	public GameObject CurrentPlayer;

	Dictionary<string,GameObject> m_iconList;

	public static GameManager Instance
	{
		get { return m_instance; }
		set { m_instance = value; }
	}

	private SessionPersistentData m_data;
	private GameProgress m_progress;

	void Awake()
	{
		m_iconList = new Dictionary<string,GameObject>();
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

		foreach (BasicMovement bm in FindObjectsOfType<BasicMovement>()) {
			if (bm.IsCurrentPlayer) {
				SetPlayer (bm);
				break;
			}
		}
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

	public void SetPlayer(BasicMovement bm) {
		GetComponent<CameraFollow> ().target = bm.GetComponent<PhysicsSS>();
		GetComponent<GUIHandler> ().CurrentTarget = bm.gameObject;
		CurrentPlayer = bm.gameObject;
	}

	public void AddPropIcon(Property p) { 
		if (!m_iconList.ContainsKey(p.PropertyName) ){
			System.Type sysType = p.GetType ();
			Property mp = (Property)GetComponentInChildren (sysType);
			GameObject go = Instantiate (IconPropertyPrefab);
			go.transform.SetParent(transform.GetChild(0).Find ("PropList"),false);
			if (mp != null) {
				go.GetComponent<Image> ().sprite = mp.icon;
			} else {
				go.GetComponent<Image> ().sprite = mp.icon;
			}
			m_iconList [p.PropertyName] = go;
		}
	}
	public void RemovePropIcon(Property p) {
		if (m_iconList.ContainsKey (p.PropertyName)) {
			Destroy (m_iconList [p.PropertyName]);
			m_iconList.Remove (p.PropertyName);
		}
	}

	public void AddPropertyText(Property p, Vector3 v) {
		GameObject go = Instantiate (TextPropertyPrefab);
		go.GetComponent<RectTransform> ().position = v;
		go.GetComponent<RectTransform> ().rotation = Quaternion.identity;
		System.Type sysType = p.GetType ();
		Property mp = (Property)GetComponentInChildren (sysType);
		if (mp != null) {
			go.GetComponent<PropertyText> ().PropertyName = mp.PropertyName;
			go.GetComponent<PropertyText> ().Description = mp.Description;
		} else {
			go.GetComponent<PropertyText> ().PropertyName = "Unregistered Property";
			go.GetComponent<PropertyText> ().Description = "Properties unknown, likely dangerous";
		}
	}
}
