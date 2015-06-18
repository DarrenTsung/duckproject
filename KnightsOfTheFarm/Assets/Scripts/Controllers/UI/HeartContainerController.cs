using UnityEngine;
using System.Collections;

public class HeartContainerController : MonoBehaviour {
	protected tk2dClippedSprite fillSprite;

	protected void Awake() {
		fillSprite = GetComponentInChildren<tk2dClippedSprite>();
	}

	// 0.0 - 1.0
	public void SetPercentFill(float fillPercentage) {
		if (fillPercentage < 0.0f || fillPercentage > 1.0f) {
			Debug.LogError("SetPercentFill - fillPercentage out-of-bounds: " + fillPercentage);
			return;
		}

		fillSprite.ClipRect = new Rect(0, 0, 1.0f, fillPercentage);
	}
}
