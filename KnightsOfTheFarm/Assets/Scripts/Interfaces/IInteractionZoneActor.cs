using UnityEngine;
using System.Collections;

public interface IInteractionZoneActor {
	void EnteredZone(GameObject zone);
	void ExitedZone(GameObject zone);
	bool IsClosestZone(GameObject zone);
}
