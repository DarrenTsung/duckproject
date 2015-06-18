using UnityEngine;
using System.Collections;

public class PotController : MonoBehaviour, IHitController {

	void IHitController.HandleHit(GameObject source) {
		LootController lootController = GetComponent<LootController>();
		if (lootController) {
			lootController.SpawnLoot();
		}

		Destroy (gameObject);
	}

}
