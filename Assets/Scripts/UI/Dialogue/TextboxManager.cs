﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextboxManager : MonoBehaviour {

	private static TextboxManager m_instance;

	public static TextboxManager Instance
	{
		get { return m_instance; }
		set { m_instance = value; }
	}
	//public delegate void optionResponse(int r);
	public GameObject textboxPrefab;
	public GameObject textboxStaticPrefab;
	public GameObject textboxFullPrefab;
	public DialogueSound nextSoundType;

	//Color TextboxColor;
	float timeAfter = 2f;
	float textSpeed = 0.03f;

	void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
		else if (m_instance != this)
		{
			Destroy(gameObject);
			return;
		}
	}

	// Use this for initialization
	void Start () {
		//TextboxColor = new Color (1.0f, 0.0f, 0.0f, 0.5f);
	}

	public static void StartSequence(string text,GameObject speaker = null) {
		DialogueSequence ds = Instance.parseSequence (text);
		ds.Speaker = speaker;
		ds.advanceSequence ();
	}

	public DialogueSequence parseSequence(string text,int startingChar = 0,int indLevel = 0,DialogueSequence parentSeq = null) {
		DialogueSequence newSeq = new DialogueSequence ();
		newSeq.parentSequence = parentSeq;
		List<DialogueUnit> subDS = new List<DialogueUnit> ();

		DialogueUnit ds = new DialogueUnit {};
		subDS.Add (ds);
		string lastText = "";
		string lastAnim = "none";
		int i = startingChar;
		int currIndent = 0;
		while (i < text.Length) {
			char lastC = text.ToCharArray () [i];
			newSeq.rawText += lastC;
			if (lastText.Length == 0 && lastC == '\t') {
				currIndent += 1;
			} else {
				if (currIndent < indLevel) {
					break;
				}
				if (lastText.Length == 0 && lastC == ' ') {
				} else if (lastText.Length == 0 && lastC == '-') {
					DialogueSequence newS = parseSequence (text, i, indLevel + 1, newSeq);
					i += newS.numChars;
				} else if (lastC == '`') {
					lastText += lastC;
				} else if (lastC == '\n' || lastC == '|') {
					if (lastText.Length > 0) {
						if (lastAnim == "none") {
							ds.addTextbox (lastText);
						} else {
							ds.addTextbox (lastText, lastAnim);
						}
					}
					currIndent = 0;
					lastText = "";
				} else {
					lastText += lastC;
				}
			}
			newSeq.numChars += 1;
			i += 1;
		}
		if (lastAnim == "none") {
			ds.addTextbox (lastText);
		} else {
			ds.addTextbox (lastText,lastAnim);
		}
		subDS.Add (ds);
		newSeq.allDUnits = subDS;
		return newSeq;
	}

	public static void SetSoundType(DialogueSound ds) {
		m_instance.nextSoundType = ds;
	}
	public static Textbox addTextbox(string text,GameObject targetObj,bool full = false) {
		return m_instance.addTextbox (text, targetObj, true, m_instance.textSpeed,Color.black,full);
	}
	public Textbox addTextbox(string text,GameObject targetObj,bool typeText,float textSpeed, Color tbColor, bool full) {
		Vector2 newPos = new Vector2();
		GameObject newTextbox;
		if (full) {
			newTextbox = Instantiate (textboxFullPrefab);
		} else if (targetObj != null) {
			newPos = findPosition (targetObj.transform.position);
			newTextbox = Instantiate (textboxPrefab, newPos, Quaternion.identity);
		} else {
			newTextbox = Instantiate (textboxStaticPrefab);
		}

		Textbox tb = newTextbox.GetComponent<Textbox> ();
		tb.m_sound = nextSoundType;
		/*if (!type) {
			//Debug.Log ("displaying Textbox: " + text);
			newTextbox.GetComponent<DestroyAfterTime> ().duration = textSpeed * 1.2f * text.Length + timeAfter;
			newTextbox.GetComponent<DestroyAfterTime> ().toDisappear = true;
		}*/

		tb.setTypeMode (typeText);			
		tb.setText(text);
		//tb.transform.position = newPos;
		tb.setTargetObj (targetObj);
		tb.pauseAfterType = timeAfter;
		tb.timeBetweenChar = textSpeed;
		RectTransform[] transforms = newTextbox.GetComponentsInChildren<RectTransform> ();
		if (text.Length > 200) {
			Vector2 v = new Vector2 ();
			foreach (RectTransform r in transforms) {
				v.y = r.sizeDelta.y * 2f;
				v.x = r.sizeDelta.x;
				if (text.Length > 300) {
					v.x = r.sizeDelta.x * 1.5f;
				}
				r.sizeDelta = v;
			}
		}

		//textboxes.Add (newTextbox);
		//tb.setColor (tbColor);
		return tb;
	}

	public Vector2 findPosition(Vector2 startLocation) {
		//Vector2 newPos;
		float targetY = startLocation.y + 5f;
		//newPos.y = targetY;
		return new Vector2 (startLocation.x, targetY);
	}
	public void setPauseAfterType(float time) {
		timeAfter = time;
	}
	public void setTextSpeed(float time ){
		textSpeed = time;
	}
	public void removeTextbox(GameObject go) {
		//textboxes.Remove (go);
	}
}

/*
 * else if (!specialGroup && false && lastText.Length < 18) { //&& lastC == ':' 
					ds = new DialogueUnit ();
					subDS.Add (ds);
					//Debug.Log (targetCharName);
					lastText = "";
				}*/