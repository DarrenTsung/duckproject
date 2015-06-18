using UnityEngine;
using System.Collections;

public class AbsorbableItemController : MonoBehaviour, IHitboxDelegate2D {
	HitboxController2D hitboxController;
	protected bool collected;

	protected virtual void Awake() {
		collected = false;

		hitboxController = GetComponentInChildren<HitboxController2D>();
		if (hitboxController == null) {
			Debug.LogError("AbsorbableItemController - has no hitbox controller in children!");
		}
	}

	protected void OnHit(GameTag hitboxTag, Collider2D other) {
		switch (hitboxTag) {
			case GameTag.ABSORPTION_HITBOX_TAG:
				if (IsAbsorbable()) {
					Absorb();
				}
				break;
			default:
				Debug.LogError("AbsorbableItemController: in gameObject: " + gameObject.name + ", hitboxTag not recognized.");
				break;
		}
	}

	public void OnHitboxEnter2D (GameTag hitboxTag, Collider2D other) {
		OnHit(hitboxTag, other);
	}

	public void OnHitboxStay2D (GameTag hitboxTag, Collider2D other) {
		OnHit(hitboxTag, other);
	}

	public void OnHitboxExit2D (GameTag hitboxTag, Collider2D other) {
		OnHit(hitboxTag, other);
	}

	protected virtual bool IsAbsorbable() {
		if (collected) {
			return false;
		}

		return true;
	}

	public virtual void Absorb() {
		collected = true;

		// implement in subclass
	}
}
