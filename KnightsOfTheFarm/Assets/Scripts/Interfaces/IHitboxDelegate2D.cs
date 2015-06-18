using UnityEngine;
using System.Collections;

public interface IHitboxDelegate2D {
	void OnHitboxEnter2D(GameTag hitboxTag, Collider2D other);
	void OnHitboxStay2D(GameTag hitboxTag, Collider2D other);
	void OnHitboxExit2D(GameTag hitboxTag, Collider2D other);
}
