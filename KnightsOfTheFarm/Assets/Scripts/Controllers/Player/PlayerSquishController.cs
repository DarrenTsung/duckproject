using UnityEngine;
using System.Collections;

public class PlayerSquishController : MonoBehaviour {
	protected const float HORIZONTAL_SQUISH_MAX = 0.95f;
	protected const float VERTICAL_SQUISH_MAX = 0.95f;

	protected const float MAX_SQUISH_DELTA = 0.05f;

	protected PlayerMovementController playerMovementController;
	protected CircleCollider2D pCircleCollider;

	protected float baseRadius;

	public void Start() {
		playerMovementController = GetComponent<PlayerMovementController>();
		PlayerComponentManager pComponentManager = GetComponent<PlayerComponentManager>();
		pCircleCollider = pComponentManager.pCircleCollider;
		baseRadius = pCircleCollider.radius;
	}
	
	void LateUpdate () {
		Vector3 previousScale = transform.localScale;

		Vector2 relativeVelocity = playerMovementController.RelativeVelocity();

		float squishAmountX = 1.0f - (Mathf.Pow(Mathf.Abs(relativeVelocity.y), 3.0f) * (1.0f - HORIZONTAL_SQUISH_MAX));
		float squishAmountY = 1.0f - (Mathf.Pow(Mathf.Abs(relativeVelocity.x), 3.0f) * (1.0f - VERTICAL_SQUISH_MAX));

		// max change between new squish amount and old squish amount is 0.1f
		float newSquishX = previousScale.x + Mathf.Clamp(squishAmountX - previousScale.x, -MAX_SQUISH_DELTA, MAX_SQUISH_DELTA);
		float newSquishY = previousScale.y + Mathf.Clamp(squishAmountY - previousScale.y, -MAX_SQUISH_DELTA, MAX_SQUISH_DELTA);

		transform.localScale = new Vector3(newSquishX, newSquishY, 1.0f);
		pCircleCollider.radius = baseRadius * newSquishY;
	}
}
