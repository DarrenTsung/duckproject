using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Timer {
	private float timeLeft;
	public delegate void HandleTimerFinished();
	public event HandleTimerFinished TimerFinished;
	
	public Timer () {
		timeLeft = -1.0f;
	}
	
	public void SetTime(float time) {
		timeLeft = time;
	}
	
	public void Update(float deltaTime) {
		float previousTime = timeLeft;
		timeLeft -= deltaTime;
		if (previousTime > 0.0 && timeLeft <= 0.0f) {
			if (TimerFinished != null) {
				TimerFinished();
			}
		}
	}
	
	public bool IsFinished() {
		return timeLeft <= 0.0f;
	}
}


public class TimerManager : Singleton<TimerManager> {

	protected TimerManager () {}

	protected List<Timer> timers, timersToAdd;

	public Timer MakeTimer() {
		Timer t = new Timer();
		timersToAdd.Add(t);
		return t;
	}

	protected void Awake () {
		timers = new List<Timer> ();
		timersToAdd = new List<Timer> ();
	}

	protected void Update () {
		timers.AddRange(timersToAdd);
		timersToAdd.Clear();
		foreach (Timer t in timers) {
			t.Update(Time.deltaTime);
		}
	}
}
