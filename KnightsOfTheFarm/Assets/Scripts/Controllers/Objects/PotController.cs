using UnityEngine;
using System.Collections;
using DT.LootSystem;

public class PotController : MonoBehaviour, IHitController {

	void IHitController.HandleHit(GameObject source) {
		LootController lootController = GetComponent<LootController>();
		if (lootController) {
			lootController.SpawnLoot();
		}

		Destroy (gameObject);
	}

}
