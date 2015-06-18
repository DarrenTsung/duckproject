using UnityEngine;
using System.Collections;

public interface IInteractionZoneDelegate {
	void HandleInteractionPressed(GameObject actor);
	void BecameActive();
	void LostActive();
}
