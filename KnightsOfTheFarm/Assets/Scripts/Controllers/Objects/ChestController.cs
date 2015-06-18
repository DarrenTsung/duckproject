using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour, IInteractionZoneDelegate {
	protected Animator _animator;
	protected Transform _spawnPoint;

	protected void Awake() {
		_animator = GetComponent<Animator>();
		_spawnPoint = transform.FindChild("SpawnPoint");
	}

	public void HandleInteractionPressed(GameObject actor) {
		_animator.SetTrigger("Open");

		LootController lootController = GetComponent<LootController>();
		if (lootController) {
			lootController.SpawnLoot(_spawnPoint.position);
		}
	}

	public void BecameActive() {
		_animator.SetBool("Active", true);
	}

	public void LostActive() {
		_animator.SetBool("Active", false);
	}
}
