using UnityEngine;
using System.Collections;

public class HitboxController2D : MonoBehaviour {

	public IHitboxDelegate2D Delegate() {
		IHitboxDelegate2D myDelegate = GetComponentInParent<IHitboxDelegate2D>();

		if (myDelegate == null) {
			Debug.LogError("HitboxController2D - no delegate found for gameobject: " + gameObject.name);
		}

		return myDelegate;
	}

	public void OnTriggerEnter2D(Collider2D other) {
		Delegate().OnHitboxEnter2D(GameTags.TagForGameObject(gameObject), other);
	}

	public void OnTriggerStay2D(Collider2D other) {
		Delegate().OnHitboxStay2D(GameTags.TagForGameObject(gameObject), other);
	}

	public void OnTriggerExit2D(Collider2D other) {
		Delegate().OnHitboxExit2D(GameTags.TagForGameObject(gameObject), other);
	}
}
