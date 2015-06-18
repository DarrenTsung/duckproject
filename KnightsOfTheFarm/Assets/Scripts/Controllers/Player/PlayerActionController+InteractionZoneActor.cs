using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class PlayerActionController {
	// created in Awake()
	protected HashSet<GameObject> interactionZonesTouching;
	protected GameObject closestInteractionZone;

	public void EnteredZone(GameObject zone) {
		interactionZonesTouching.Add(zone);
	}

	public void ExitedZone(GameObject zone) {
		interactionZonesTouching.Remove(zone);
	}

	public bool IsClosestZone(GameObject zone) {
		return closestInteractionZone == zone;
	}

	// called in Update()
	protected void UpdateClosestInteractionZone() {
		float minimumDistance = float.MaxValue;
		GameObject closestGameObject = null;
		
		foreach (GameObject otherGameObject in interactionZonesTouching) {
			float distanceToOther = Vector2.Distance(gameObject.transform.position, otherGameObject.transform.position);
			if (distanceToOther < minimumDistance) {
				minimumDistance = distanceToOther;
				closestGameObject = otherGameObject;
			}
		}

		closestInteractionZone = closestGameObject;
	}

	public void HandleInteractPressed() {
		if (closestInteractionZone != null) {
			closestInteractionZone.GetComponentInParent<IInteractionZoneDelegate>().HandleInteractionPressed(gameObject);
		}
	}
}
