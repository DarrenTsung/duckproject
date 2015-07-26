using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {
	protected const float PLAYER_SPEED = 7.0f;
	protected const float PLAYER_JUMP_HEIGHT = 4.0f;
	protected const float PLAYER_ACCELERATION_MAX = 1.0f;
	protected const float PLAYER_WALL_FORCE_MAX = 4.0f;
	protected const float PLAYER_TELEPORTION_DELAY = 0.2f;
	protected const float PLAYER_TELEPORTION_DISTANCE = 4.0f;
	protected const float PLAYER_TELEPORTION_RECHARGE_TIME = 2.0f;
	protected const float GUESS_THRESHOLD = 0.03f;

	protected float pHitboxRadius;

	protected bool pressingUp, pressingDown;
	protected Vector2 currentAxis;

	protected Rigidbody2D pRigidbody;
	protected PlayerStatusController pStatus;
	protected Animator pAnimator;

	protected int jumpCharges, baseJumpCharges = 2;
	protected int teleportCharges, maxTeleportCharges = 1;
	protected Timer teleportChargeTimer;
	
	protected ParticleSystem pJumpEmitter;

	public void Start() {
		PlayerComponentManager pComponentManager = GetComponent<PlayerComponentManager>();
		pRigidbody = pComponentManager.pRigidbody;
		pAnimator = pComponentManager.pAnimator;
		pHitboxRadius = pComponentManager.pCircleCollider.radius;
		pJumpEmitter = pComponentManager.pJumpEmitter;

		pStatus = GetComponent<PlayerStatusController>();

		teleportChargeTimer = TimerManager.Instance.MakeTimer();
	}

	public void HandleAxisVector(Vector2 axisVector) {
		currentAxis = axisVector;
	}

	public void HandleJumpPressed() {
		if (pStatus.IsGrounded()) {
			jumpCharges = baseJumpCharges;
		} else if (pStatus.IsLeftTouching() || pStatus.IsRightTouching()) {
			// only set jump charges to 1 if we have no jump charges
			jumpCharges = Mathf.Max(1, jumpCharges);
		}

		if (pStatus.IsLeftTouching()) {
			pRigidbody.velocity = new Vector2(PLAYER_JUMP_HEIGHT * 2.0f / 3.0f * pRigidbody.gravityScale, pRigidbody.velocity.y);
		} else if (pStatus.IsRightTouching()) {
			pRigidbody.velocity = new Vector2(-PLAYER_JUMP_HEIGHT * 2.0f / 3.0f * pRigidbody.gravityScale, pRigidbody.velocity.y);
		}

		if (jumpCharges > 0) {
			jumpCharges--;

			pRigidbody.velocity = new Vector2(pRigidbody.velocity.x, PLAYER_JUMP_HEIGHT * pRigidbody.gravityScale);
			
			if (pStatus.IsGrounded()) {
				pJumpEmitter.Emit(1);
			}
		}
	}

	// returns Vec2(int, int) where int is in range of -1, 0, 1
	public Vector2 GuessPlayerDirection() {
		int xGuess, yGuess;
		if (currentAxis.x > GUESS_THRESHOLD) {
			xGuess = 1;
		} else if (currentAxis.x < -GUESS_THRESHOLD) {
			xGuess = -1;
		} else {
			xGuess = 0;
		}

		if (currentAxis.y > GUESS_THRESHOLD) {
			yGuess = 1;
		} else if (currentAxis.y < -GUESS_THRESHOLD) {
			yGuess = -1;
		} else {
			yGuess = 0;
		}

		return new Vector2(xGuess, yGuess);
	}

	public void HandleTeleportPressed() {
		if (teleportCharges > 0) {
			teleportCharges--;
			teleportChargeTimer.SetTime(PLAYER_TELEPORTION_RECHARGE_TIME);

			Vector2 pDirection = GuessPlayerDirection();

			// no direction specified from player input, teleport to same position
			if (pDirection.x == 0 && pDirection.y == 0) {
				StartCoroutine(TeleportToPosition(transform.position));
				return;
			}

			// raycast to find valid teleportation spot
			Vector2 rayDirection = pDirection;

			LayerMask raycastLayers;
			// if we're raycasting downwards, include platforms in layermask
			if (rayDirection.y < 0) {
				raycastLayers = GameLayers.ALL_TILE_LAYERS;
			} else {
				raycastLayers = GameLayers.TILE_COLLIDER_LAYER;
			}
			RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, rayDirection, PLAYER_TELEPORTION_DISTANCE + (2.0f * pHitboxRadius), raycastLayers);

			if (raycastHit.collider != null) {
				// hit something
				StartCoroutine(TeleportToPosition(raycastHit.point - (rayDirection * pHitboxRadius)));
			} else {
				// didn't hit anything
				StartCoroutine(TeleportToPosition(transform.position + (Vector3)(rayDirection * PLAYER_TELEPORTION_DISTANCE)));
			}
		}
	}

	protected IEnumerator TeleportToPosition(Vector3 position) {
		pRigidbody.isKinematic = true;
		currentAxis = new Vector2(0.0f, 0.0f);
		PlayerInputManager.Instance.DisablePlayerInput();
		pRigidbody.velocity = new Vector2(0.0f, 0.0f);

		yield return new WaitForSeconds(PLAYER_TELEPORTION_DELAY);

		transform.position = position;
		PlayerInputManager.Instance.EnablePlayerInput();
		pRigidbody.isKinematic = false;
	}

	public bool IsPressingUp() {
		return currentAxis.y > 0.0f;
	}

	public bool IsPressingDown() {
		return currentAxis.y < 0.0f;
	}

	public void FixedUpdate() {
		// we don't let the player move when he is charging up for a slash or charge slashing
		if (pStatus.CurrentlyChargingSlash() || pStatus.CurrentlyChargeSlashing()) {
			currentAxis = new Vector2(0.0f, currentAxis.y);
		}
		
		float desiredXSpeed = currentAxis.x * PLAYER_SPEED;

		// if the player is trying to go right and we're already > the max input speed, don't slow him down
		if ((pRigidbody.velocity.x > PLAYER_SPEED && currentAxis.x > 0) || 
		    (pRigidbody.velocity.x < -PLAYER_SPEED && currentAxis.x < 0)) {
			desiredXSpeed = pRigidbody.velocity.x;
		}

		// if we're touching a wall, limit the force the player can put against the wall
		if (pStatus.IsLeftTouching()) {
			desiredXSpeed = Mathf.Max(-PLAYER_WALL_FORCE_MAX, desiredXSpeed);
		} else if (pStatus.IsRightTouching()) {
			desiredXSpeed = Mathf.Min(PLAYER_WALL_FORCE_MAX, desiredXSpeed);
		}

		if (!pStatus.IsGrounded()) {
			float delta = Mathf.Clamp(desiredXSpeed - pRigidbody.velocity.x, -PLAYER_ACCELERATION_MAX, PLAYER_ACCELERATION_MAX);
			pRigidbody.velocity = pRigidbody.velocity + new Vector2(delta, 0.0f);
		} else {
			pRigidbody.velocity = new Vector2(desiredXSpeed, pRigidbody.velocity.y);
		}

		pAnimator.SetFloat("AbsHorizontalSpeed", Mathf.Abs(pRigidbody.velocity.x));
		pAnimator.SetFloat("VerticalSpeed", pRigidbody.velocity.y);
	}

	public void Update() {
		if (pStatus.IsGrounded()) {
			jumpCharges = baseJumpCharges;
		}

		if (teleportChargeTimer.IsFinished() && teleportCharges < maxTeleportCharges) {
			teleportCharges++;
			teleportChargeTimer.SetTime(PLAYER_TELEPORTION_RECHARGE_TIME);
		}
	}

	// returns a relative [-1, 1] vec2 of the player's velocity 
	public Vector2 RelativeVelocity() {
		float velocityY = pRigidbody.velocity.y;

		float relativeY = (velocityY > 0) ? velocityY / (PLAYER_JUMP_HEIGHT * pRigidbody.gravityScale) : velocityY / 100.0f;
		float relativeX = pRigidbody.velocity.x / PLAYER_SPEED;

		relativeX = Mathf.Clamp(relativeX, -1.0f, 1.0f);
		relativeY = Mathf.Clamp(relativeY, -1.0f, 1.0f);

		return new Vector2(relativeX, relativeY);
	}
}
