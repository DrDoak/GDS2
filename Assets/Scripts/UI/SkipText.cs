using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipText : MonoBehaviour {
	public string SceneName = "";	
	public bool DisablePause = false;
	void Start() {
		PauseGame.CanPause = !DisablePause;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			TextboxManager.ClearAllSequences ();
			SceneManager.LoadScene (SceneName);
			TextboxManager.ClearAllSequences ();
		}
	}
}
