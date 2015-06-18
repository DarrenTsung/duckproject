using UnityEngine;
using System.Collections;

public class PlayerHealthComponent : HealthComponent {
	public const uint CONTAINER_SIZE = 20;

	protected float healthRegenSpeed = 0.2f;
	protected uint healthRegenAmount = 1;
	protected float healthRegenDelay = 1.5f;

	protected Timer healthRegenTimer, healthRegenDelayTimer;

	protected override void Start() {
		base.Start();

		healthRegenTimer = TimerManager.Instance.MakeTimer();
		healthRegenTimer.SetTime(healthRegenSpeed);

		healthRegenDelayTimer = TimerManager.Instance.MakeTimer();
	}

	public uint NumberOfContainers() {
		return (uint)(baseHealth / CONTAINER_SIZE);
	}

	public uint ContainersRemaining() {
		return (uint)Mathf.Ceil((float)health / (float)CONTAINER_SIZE);
	}

	public float PercentFilledForContainer(int index) {
		float healthOver = (float)(health - (index * CONTAINER_SIZE));
		return Mathf.Clamp(healthOver / (float)CONTAINER_SIZE, 0.0f, 1.0f);
	}

	protected void Update() {
		if (healthRegenDelayTimer.IsFinished()) {
			if (healthRegenTimer.IsFinished()) {
				AddToHealth(healthRegenAmount);
				healthRegenTimer.SetTime(healthRegenSpeed);
			}
		}
	}

	protected void AddToHealth(uint amount) {
		// if health is at 43 and the CONTAINER_SIZE is 20,
		// maxOfCurrentContainer is 60
		uint maxOfCurrentContainer = CONTAINER_SIZE * ContainersRemaining();
		health += amount;
		health = (uint)Mathf.Min(health, maxOfCurrentContainer);
	}

	public override void TakeDamage(int damage) {
		base.TakeDamage(damage);

		healthRegenDelayTimer.SetTime(healthRegenDelay);
	}
}
