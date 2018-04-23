using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveObjManager : MonoBehaviour{
	private static SaveObjManager m_instance;

	static SceneTrigger [] sceneTriggers;
	bool second = false;
	string curRoom;
	static string saveBase = "SaveData/";
	static string savePath = "SaveData/Debug";
	static string saveFolder = "Debug/";
	List<string> registeredPermItems;

	public bool SetDirectory(string directory) {
		if (IsAlphaNum (directory) && directory.Length < 16) {
			saveFolder = directory + "/";
			savePath = saveBase + saveFolder;
			if (!Directory.Exists (savePath)) {
				Directory.CreateDirectory (savePath);
			}
			return true;
		}
		return false;
	}
	public static SaveObjManager Instance
	{
		get { return m_instance; }
		set { m_instance = value; }
	}
	
	bool IsAlphaNum(string str)
	{
		if (string.IsNullOrEmpty(str))
			return false;

		for (int i = 0; i < str.Length; i++)
		{
			if (!(char.IsLetter(str[i])) && (!(char.IsNumber(str[i]))))
				return false;
		}

		return true;
	}

	void Awake()
	{
		if (m_instance == null) {
			m_instance = this;
		} else if (m_instance != this) {
			Destroy (gameObject);
			return;
		}
		registeredPermItems = new List<string> ();
		SceneManager.sceneLoaded += onRoomLoad;
		curRoom = SceneManager.GetActiveScene ().name;
	}
	public void saveCurrentRoom() {}
	public void resetRoomData() {
		foreach (string file in Directory.GetFiles(savePath))
		{
			File.Delete(file);      
		}
		registeredPermItems = new List<string> ();
	}
	public List<string> loadRegisteredIDs() {
		List<string> ids = new List<string> ();
		foreach (string file in Directory.GetFiles(savePath))
		{
			//File.Delete(file);      
		}
		return ids;
	}
	public void onRoomLoad(Scene scene, LoadSceneMode mode) {
		//curRoomInfo = getRoom(roomName);
		curRoom = SceneManager.GetActiveScene ().name;;
		sceneTriggers = GameObject.FindObjectsOfType<SceneTrigger> ();
		recreateItems (curRoom);
		GameManager.Instance.FindPlayer ();
		//registerPersItems (curRoom);
	}

	public void recreateItems(string RoomName) {
		//Debug.Log ("Recreating items for room: " + RoomName);
		LoadRoom (savePath + RoomName);
	}

	public static void MoveItem(GameObject go,string newRoom, Vector3 newPos) {
		Instance.m_moveItem (go,newRoom,newPos);
	}
	void m_moveItem(GameObject go,string newRoom, Vector3 newPos) {
		//Debug.Log ("Moving item to " + newRoom + " at position: " + newPos);
		PersistentItem item = go.GetComponent<PersistentItem> ();
		item.targetID = null;
		item.StoreData ();
		//Debug.Log ("moving item: " + item.name + " to " + newRoom);
		refreshPersItems();
		DelCharData (item.data);
		CharacterSaveContainer cc = LoadChars(savePath + newRoom);
		item.pos = newPos;
		string json = JsonUtility.ToJson(new CharacterSaveContainer());
		cc.actors.Add (item.data);
		Save (savePath + newRoom, cc);
		ResaveRoom ();
	}

	public static void MoveItem(GameObject gm,string newRoom, string newID,RoomDirection newDir) {
		Instance.m_moveItem (gm, newRoom, newID, newDir);
	}

	void m_moveItem(GameObject go,string newRoom, string newID,RoomDirection newDir) {
		//Debug.Log ("Moving " + go.name + " to " + newRoom + " at position: " + newID + " direction: " + newDir);
		PersistentItem item = go.GetComponent<PersistentItem> ();
		//Remove the current data.
		item.StoreData ();
		refreshPersItems();
		DelCharData (item.data);

		//load the list of things in the new room and add the current character to it
		CharacterSaveContainer cc = LoadChars(savePath + newRoom);
		item.data.targetID = newID;
		item.data.targetDir = newDir;
		cc.actors.Add (item.data);
		//Debug.Log ("What is this anyways?: " + item.data.targetID + " : " + item.data.targetDir);
		Save (savePath+newRoom, cc);
		ResaveRoom ();
		LoadChars (savePath + curRoom);
	}

	public static bool CheckRegistered(GameObject go) {
		return m_instance.m_checkRegister (go);
	}

	bool m_checkRegister(GameObject go) {
		PersistentItem c = go.GetComponent<PersistentItem> ();
		string xID = (((int)(go.transform.position.x/2))*2).ToString ();
		string yID = (((int)(go.transform.position.y/2))*2).ToString ();
		string id = go.name + "-" + SceneManager.GetActiveScene ().name + xID + yID;
		//Debug.Log ("Checking if character: " + c + " registered id is: " + c.data.regID);
		//Debug.Log ("incoming ID: " + c.data.regID);
		if (c.data.regID != "Not Assigned") {
			id = c.data.regID;
		}
		if (registeredPermItems.Contains(id) ){
			if (c.recreated) {
				//Debug.Log ("Recreated entity.");
				c.recreated = false;
				return false;
			} else {
				//Debug.Log ("already registered, removing");
				return true;
			}
		}
		//Debug.Log ("new entity. " + id + " Adding to registry");
		registeredPermItems.Add(id);
		c.data.regID = id;
		//Debug.Log ("saved ID is: " + c.data.regID);
		//Debug.Log ("Length of registry is: " + registeredPermItems.Count);
		return false;
	}

	public void refreshPersItems() {
		PersistentItem [] cList = Object.FindObjectsOfType<PersistentItem>();
		charContainer.actors.Clear ();
		foreach (PersistentItem c in cList) {
			c.StoreData ();
			//Debug.Log ("Adding character: " + c);
			charContainer.actors.Add(c.data);
		}
	}
	public void ResaveRoom() {
		//Debug.Log ("Resaved characters: " + charContainer.actors.Count);
		Save (savePath + curRoom, charContainer);
	}
	//-----------------------------------------------------------
	//-----------------------------------------------------------

	public static CharacterSaveContainer charContainer = new CharacterSaveContainer();

	public delegate void SerializeAction();
	public static event SerializeAction OnLoaded;
	public static event SerializeAction OnBeforeSave;

	//public const string playerPath = "Prefabs/Player";

	//Loading---------------
	public static void LoadRoom(string path) {		
		charContainer = LoadChars(path);	
		//Debug.Log ("items to recreate: " + charContainer.actors.Count);
		foreach (CharData data in charContainer.actors) {
			PersistentItem pi = RecreatePersistentItem (data, data.prefabPath,
				data.pos, Quaternion.identity);
			pi.registryCheck ();
		}
		//OnLoaded();
		//ClearActorList();
	}
	public static void ClearActorList() {
		charContainer.actors.Clear();
	}
	private static CharacterSaveContainer LoadChars(string path) {
		if (File.Exists(path+ ".txt"))
		{
			string json = File.ReadAllText(path+ ".txt");
			//Debug.Log ("Chars from path: " + path + " : " + json);
			return JsonUtility.FromJson<CharacterSaveContainer>(json);
		} else {
			//Debug.Log("no save data found, creating new file");
			CharacterSaveContainer cc = new CharacterSaveContainer();
			SaveActors(path,cc);
			return cc;
		} 

	}
	public static PersistentItem RecreatePersistentItem(string path, Vector3 position, Quaternion rotation) {
		Debug.Log ("re-instantiating object: " + path);
		GameObject prefab = Resources.Load<GameObject>(path);
		Debug.Log (prefab);
		GameObject go = GameObject.Instantiate(prefab, position, rotation) as GameObject;
		PersistentItem actor = go.GetComponent<PersistentItem>() ?? go.AddComponent<PersistentItem>();
		actor.recreated = true;
		return actor;
	}
	public static PersistentItem RecreatePersistentItem(CharData data, string path, Vector3 position, Quaternion rotation) {
		PersistentItem actor = null;
		if (data.targetID != null) {
			Vector3 nv = data.pos;
			bool found = false;
			foreach (SceneTrigger rm in sceneTriggers) {
				//Debug.Log ("COMPARE: " + rm.TriggerID + " : " + data.targetID);
				if (rm.TriggerID == data.targetID) {
					if (data.targetDir == RoomDirection.LEFT) {
							nv = rm.transform.position - new Vector3 (rm.GetComponent<BoxCollider2D> ().size.x + 3f, 0f);
					} else if (data.targetDir == RoomDirection.RIGHT) {
							nv = rm.transform.position + new Vector3 (rm.GetComponent<BoxCollider2D> ().size.x + 3f, 0f);
					} else if (data.targetDir == RoomDirection.UP) {
							nv = rm.transform.position + new Vector3 (0f, rm.GetComponent<BoxCollider2D> ().size.y + 3f, 0f);
					} else if (data.targetDir == RoomDirection.DOWN) {
							nv = rm.transform.position - new Vector3 (0f, rm.GetComponent<BoxCollider2D> ().size.x + 3f, 0f);
					}
					found = true;
					break;
				}
			}
			if (found) {
				actor = RecreatePersistentItem(path, nv, rotation);
			} else {
				actor = RecreatePersistentItem(path, nv, rotation);
			}
		} else {
			actor = RecreatePersistentItem(path, data.pos, rotation);
		}
		actor.data = data;
		actor.LoadData ();
		return actor;
	}
	public static void AddCharData(CharData data) {
		//Debug.Log ("Adding character");
		charContainer.actors.Add(data);
	}
	public static void DelCharData(CharData data) {
		charContainer.actors.Remove (data);
		////Debug.Log (charContainer.actors.Count);
	}

	//Saving --------------------
	public static void Save(string path, CharacterSaveContainer actors) {
		//OnBeforeSave();
		//ClearSave(path);
		SaveActors(path, actors);
		//actors.actors.Clear ();
	}
	private static void SaveActors(string path, CharacterSaveContainer actors) {
		string json = JsonUtility.ToJson(actors);
		//Debug.Log ("jsoN: " + json);
		//Debug.Log ("save to path: " + path+ ".txt");
		//Debug.Log("Saving: " + json.ToString() + " to path: " + path);
		StreamWriter sw = File.CreateText(path + ".txt");
		sw.Close();
		File.WriteAllText(path+ ".txt", json);
	}
}