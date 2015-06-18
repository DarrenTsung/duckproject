using UnityEngine;
using System.Collections;

public class InteractionZoneController : MonoBehaviour {
	protected IInteractionZoneDelegate myDelegate;
	protected bool active;

	protected void Awake() {
		myDelegate = GetComponentInParent<IInteractionZoneDelegate>();
	}

	protected IInteractionZoneActor GetActor(Collider2D other) {
		IInteractionZoneActor actor = other.gameObject.GetComponentInParent<IInteractionZoneActor>();

		if (actor == null) {
			Debug.LogError("InteractionZone entered by GameObject without InteractionZoneActor script");
			return null;
		}

		return actor;
	}

	protected void OnTriggerEnter2D(Collider2D other) {
		IInteractionZoneActor actor = GetActor(other);
		actor.EnteredZone(gameObject);

		if (actor.IsClosestZone(gameObject)) {
			active = true;
			myDelegate.BecameActive();
		}
	}

	protected void OnTriggerStay2D(Collider2D other) {
		IInteractionZoneActor actor = GetActor(other);

		bool previouslyActive = active;
		active = actor.IsClosestZone(gameObject);

		if (!previouslyActive && active) {
			myDelegate.BecameActive();
		} else if (previouslyActive && !active) {
			myDelegate.LostActive();
		}
	}

	protected void OnTriggerExit2D(Collider2D other) {
		IInteractionZoneActor actor = GetActor(other);
		actor.ExitedZone(gameObject);

		if (active) {
			active = false;
			myDelegate.LostActive();
		}
	}
}
