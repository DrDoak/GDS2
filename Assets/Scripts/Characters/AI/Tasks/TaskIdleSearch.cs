using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskIdleSearch : FighterTask {

	private int m_lastObservable;
	private Observer m_observer;
	private Attackable m_attackable;
	private AIFighter m_fighter;

	public override void Init(Fighter player, AIFighter fighter, FighterRoutine routine) {
		base.Init (player, fighter, routine);
		m_lastObservable = 0;
		Debug.Log (Fighter);
		m_observer = Fighter.GetComponent<Observer> ();
		Debug.Log (Fighter.GetComponent<Observer> ());
		m_attackable = Fighter.GetComponent<Attackable> ();
	}

	override public void Advance() 
	{
		if (Fighter.CurrentTarget != null) {
			Debug.Log (Fighter.CurrentTarget);
			NextTask ();
			return;
		}
		if (m_observer.VisibleObjs.Count != m_lastObservable) {
			m_lastObservable = m_observer.VisibleObjs.Count;
			foreach (Observable o in m_observer.VisibleObjs) {
				if (o.GetComponent<Attackable> () && m_attackable.CanAttack (o.GetComponent<Attackable> ().Faction)) {
					Fighter.CurrentTarget = o.GetComponent<Attackable> ();
					Debug.Log (Fighter.CurrentTarget);
					NextTask ();
					return;
				}
			}
		}
	}
}
