using UnityEngine;
using System.Collections;

public enum GameTag {
	NULL_TAG = 0,

	// player tags
	FORWARD_SLASH_HITBOX_TAG = 1000,
	DOWNWARD_SLASH_HITBOX_TAG = 1001,

	// other tags
	ABSORPTION_HITBOX_TAG = 2000,
};

public class GameTags {
	public const string ABSORPTION_HITBOX_TAG     = "AbsorptionHitbox";
	public const string FORWARD_SLASH_HITBOX_TAG  = "ForwardSlashHitbox";
	public const string DOWNWARD_SLASH_HITBOX_TAG = "DownwardSlashHitbox";

	public static GameTag TagForGameObject(GameObject g) {
		if (g.CompareTag(ABSORPTION_HITBOX_TAG)) {
			return GameTag.ABSORPTION_HITBOX_TAG;
		} else if (g.CompareTag(FORWARD_SLASH_HITBOX_TAG)) {
			return GameTag.FORWARD_SLASH_HITBOX_TAG;
		} else if (g.CompareTag(DOWNWARD_SLASH_HITBOX_TAG)) {
			return GameTag.DOWNWARD_SLASH_HITBOX_TAG;
		} else {
			Debug.LogError("GameTags - unknown tag for gameObject: " + g.name + ", you must register the tag in GameTags.cs!");
		}

		return GameTag.NULL_TAG;
	}
}
