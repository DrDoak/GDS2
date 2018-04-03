using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterRoutine : MonoBehaviour {
	[SerializeField]
	private List<FighterTask> m_tasks;
	public List<FighterTask> Tasks { get { return m_tasks; } set { m_tasks = value; } }

	[SerializeField]
	private List<float> m_taskDurations;
	public List<float> TaskDurations { get { return m_taskDurations; } set { m_taskDurations = value; } }

	protected float TaskTimer = 0;
	protected int ActiveTask = 0;

	public void Init(Fighter player, AIFighter fighter) {
		foreach (FighterTask task in Tasks) {
			task.Init(player, fighter);
		}
	}

	virtual public void Advance() {
		AdvanceCurrentTask();
		TaskTimer += Time.deltaTime;
	}

	void AdvanceCurrentTask() {
		if (ActiveTask == -1 || Tasks.Count == 0)
			return;
		Tasks[ActiveTask].Advance();
		if (TaskHaltedOrInactive(ActiveTask))
			MoveToNextTask();
	}

	bool TaskHaltedOrInactive(int taskIndex) {
		return TaskTimer > TaskDurations[taskIndex] || !Tasks[taskIndex].Active;
	}

	void MoveToNextTask() {
		TaskTimer = 0;
		ActiveTask = (ActiveTask + 1) % Tasks.Count;
		Tasks[ActiveTask].Activate();
	}
}
