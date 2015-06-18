using UnityEngine;
using System.Collections;

// controls the gold, items, and effects the player can pick up
public class PlayerInventoryController : MonoBehaviour {
	protected int goldCount;

	public int Gold() {
		return goldCount;
	}

	protected void Awake() {
		EventManager.Instance.OnGoldCollected.AddListener((int goldCollected) => { AddGold(goldCollected); });

		goldCount = 0;
	}

	protected void AddGold(int goldToAdd) {
		goldCount += goldToAdd;
	}
}
