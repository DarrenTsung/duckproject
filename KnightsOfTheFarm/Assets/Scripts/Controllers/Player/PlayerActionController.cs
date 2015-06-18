using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class PlayerActionController : MonoBehaviour, IInteractionZoneActor, IHitboxDelegate2D {
	protected Animator pAnimator;
	protected HashSet<int> objectIdsHit;
	protected PlayerStatusController pStatus;

	protected void Awake() {
		objectIdsHit = new HashSet<int>();
		pStatus = GetComponent<PlayerStatusController>();
		interactionZonesTouching = new HashSet<GameObject>();
	}

	protected void Start() {
		PlayerComponentManager pComponentManager = GetComponent<PlayerComponentManager>();
		pAnimator = pComponentManager.pAnimator;
	}

	public void HandleSlashPressed() {
		if (pStatus.CurrentlySlashing()) {
			return;
		}

		ResetSlash();
		pAnimator.SetTrigger("Slash");
	}

	protected void ResetSlash() {
		objectIdsHit.Clear();
	}

	protected void Update() {
		UpdateClosestInteractionZone();
	}

	protected void HitObject(Collider2D other) {
		int otherId = other.gameObject.GetInstanceID();
		if (objectIdsHit.Contains(otherId)) {
			return;
		}

		objectIdsHit.Add(otherId);

		// trigger hit detection on other object
		IHitController hitController = other.gameObject.GetComponentInParent<IHitController>();
		if (hitController != null) {
			hitController.HandleHit(gameObject);
		}
 	}

	protected void ProcessHit(GameTag hitboxTag, Collider2D other) {
		switch (hitboxTag) {
			case GameTag.FORWARD_SLASH_HITBOX_TAG:
			case GameTag.DOWNWARD_SLASH_HITBOX_TAG:
				HitObject(other);
				break;
			default:
				Debug.LogError ("PlayerActionController - hitbox not recognized: " + hitboxTag);
				break;
		}
	}

	public void OnHitboxEnter2D(GameTag hitboxTag, Collider2D other) {
		ProcessHit(hitboxTag, other);
	}

	public void OnHitboxStay2D(GameTag hitboxTag, Collider2D other) {
		ProcessHit(hitboxTag, other);
	}

	public void OnHitboxExit2D(GameTag hitboxTag, Collider2D other) {
		ProcessHit(hitboxTag, other);
	}
}
