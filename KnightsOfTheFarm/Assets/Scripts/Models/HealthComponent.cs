using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour {
	[SerializeField]
	protected uint baseHealth;
	protected uint health;

	protected virtual void Start () {
		health = baseHealth;
	}

	public uint Health() {
		return health;
	}

	public virtual void TakeDamage(int damage) {
		if (damage > health) {
			health = 0;
		} else {
			health -= (uint)damage;
		}
	}

	public bool IsDead() {
		return health <= 0;
	}
}
