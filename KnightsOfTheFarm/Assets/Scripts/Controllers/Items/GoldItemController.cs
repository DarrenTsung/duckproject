using UnityEngine;
using System.Collections;

public class GoldItemController : AbsorbableItemController {
	public int goldValue;
	protected Animator animator;
	protected Rigidbody2D myRigidbody;

	protected override void Awake() {
		base.Awake();

		animator = GetComponent<Animator>();
		myRigidbody = GetComponent<Rigidbody2D>();
	}

	protected void ApplyModifiersToGoldValue() {
		// apply any modifiers to the gold value here
		int multiplierModifier = 1;
		goldValue = goldValue * multiplierModifier;
	}

	protected string DisplayString() {
		return goldValue + " gold";
	}

	protected override bool IsAbsorbable() {
		if (!base.IsAbsorbable()) {
			return false;
		}

		// if the gold coin is still moving upwards, don't let the player collect
		if (myRigidbody.velocity.y > 0.0f) {
			return false;
		}

		return true;
	}

	public override void Absorb() {
		base.Absorb();

		// stop the gold from moving
		myRigidbody.velocity = Vector2.zero;

		// give player gold
		EventManager.Instance.OnGoldCollected.Invoke(goldValue);

		// play collection animation
		animator.SetTrigger("Collect");
	}

	public void Destroy() {
		Destroy(gameObject);
	}
}
