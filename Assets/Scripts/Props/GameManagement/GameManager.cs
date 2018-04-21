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

	public GameObject IconPropertyPrefab;
	public Sprite UnknownPropertyIcon;
	public GameObject CurrentPlayer;
	public GameObject HealthBarPrefab;

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
			m_instance = this;
		}
		else if (m_instance != this)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);
		FindPlayer ();
		Reset ();

	}
	public void FindPlayer() {
		foreach (BasicMovement bm in FindObjectsOfType<BasicMovement>()) {
			if (bm.IsCurrentPlayer) {
				SetPlayer (bm);
				break;
			}
		}
	}
	public void LoadRoom(string roomName) {
		Debug.Log ("---LOADING ROOM: " + roomName);
		GetComponent<SaveObjManager>().ResaveRoom ();
		SceneManager.LoadScene (roomName, LoadSceneMode.Single);
	}

	public void SetPlayer(BasicMovement bm) {
		GetComponent<CameraFollow> ().target = bm.GetComponent<PhysicsSS>();
		GetComponent<GUIHandler> ().CurrentTarget = bm.gameObject;
		CurrentPlayer = bm.gameObject;
	}

	public void AddPropIcon(Property p) { 
		if (!m_iconList.ContainsKey(p.GetType().ToString()) ){
			System.Type sysType = p.GetType ();
			Property mp = (Property)GetComponentInChildren (sysType);
			GameObject go = Instantiate (IconPropertyPrefab);
			go.transform.SetParent(transform.GetChild(0).Find ("PropList"),false);
			if (mp != null) {
				go.GetComponent<Image> ().sprite = mp.icon;
			} else {
				go.GetComponent<Image> ().sprite = mp.icon;
			}
			m_iconList [p.GetType().ToString()] = go;
		}
	}
	public Property GetPropInfo(Property p ) {
		System.Type sysType = p.GetType ();
		return (Property)GetComponentInChildren (sysType);
	}

	public void RemovePropIcon(Property p) {
		if (m_iconList.ContainsKey (p.GetType().ToString())) {
			Destroy (m_iconList [p.GetType().ToString()]);
			m_iconList.Remove (p.GetType().ToString());
		}
	}

	public static void Reset() {
		SaveObjManager.charContainer = new CharacterSaveContainer ();
		Instance.GetComponent<SaveObjManager>().resetRoomData ();
		Debug.Log ("Resetting");
	}
}
