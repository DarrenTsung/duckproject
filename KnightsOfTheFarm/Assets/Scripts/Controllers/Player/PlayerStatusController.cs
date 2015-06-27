using UnityEngine;
using System.Collections;

public class PlayerStatusController : MonoBehaviour {
	protected Rigidbody2D pRigidbody;
	protected Animator pAnimator;

	protected GameObject pSpriteObjects;
	protected ParticleSystemController pRunParticleSystem, pWallSlideParticleSystem;
	protected GameObject pColliders;

	protected Transform pGroundCheck, pLeftCheck, pRightCheck, pRightBottomCheck, pLeftBottomCheck;
	protected bool previouslyGrounded, previouslyLeftTouching, previouslyRightTouching;
	protected bool grounded, leftTouching, rightTouching;

	protected HealthComponent pHealthComponent;

	protected void Awake () {
		pHealthComponent = GetComponent<HealthComponent>();
	}

	protected void Start () {
		PlayerComponentManager pComponentManager = GetComponent<PlayerComponentManager>();
		pGroundCheck = pComponentManager.pGroundCheck;
		pLeftCheck = pComponentManager.pLeftCheck;
		pRightCheck = pComponentManager.pRightCheck;
		pRightBottomCheck = pComponentManager.pRightBottomCheck;
		pLeftBottomCheck = pComponentManager.pLeftBottomCheck;
		pRigidbody = pComponentManager.pRigidbody;
		pAnimator = pComponentManager.pAnimator;
		pSpriteObjects = pComponentManager.pSpriteObjects;
		pColliders = pComponentManager.pColliders;
		pRunParticleSystem = pComponentManager.pRunParticleSystem;
		pWallSlideParticleSystem = pComponentManager.pWallSlideParticleSystem;
	}

	public bool IsGrounded() {
		return grounded;
	}

	public bool IsLeftTouching() {
		return leftTouching;
	}

	public bool IsRightTouching() {
		return rightTouching;
	}

	public bool WasPreviouslyGrounded() {
		return previouslyGrounded;
	}

	public bool WasPreviouslyLeftTouching() {
		return previouslyLeftTouching;
	}

	public bool WasPreviouslyRightTouching() {
		return previouslyRightTouching;
	}

	public bool MovingLeft() {
		return pRigidbody.velocity.x < 0;
	}

	public bool MovingRight() {
		return pRigidbody.velocity.x > 0;
	}

	public bool CurrentlySlashing() {
		return pAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Slash");
	}
	
	public bool CurrentlyChargingSlash() {
		return pAnimator.GetCurrentAnimatorStateInfo(0).IsTag("ChargingSlash");
	}

	// -- UPDATE --
	protected void Update () {
		UpdateChecks();
		CheckHealth();
		TurnPlayerToCorrectDirection();
	}

	protected void CheckHealth() {
		if (pHealthComponent.IsDead()) {
			// player died
		}
	}

	protected void UpdateChecks() {
		previouslyGrounded = grounded;
		previouslyLeftTouching = leftTouching;
		previouslyRightTouching = rightTouching;

		Collider2D[] emptyArray = new Collider2D[1];
		int groundedHit = Physics2D.OverlapCircleNonAlloc(pGroundCheck.position, 0.1f, emptyArray, GameLayers.ALL_TILE_LAYERS);
		grounded = groundedHit != 0 ? true : false;

		int leftTouchingHit = Physics2D.OverlapCircleNonAlloc(pLeftCheck.position, 0.1f, emptyArray, GameLayers.TILE_COLLIDER_LAYER);
		int leftBottomTouchingHit = Physics2D.OverlapCircleNonAlloc(pLeftBottomCheck.position, 0.05f, emptyArray, GameLayers.TILE_COLLIDER_LAYER);
		leftTouching = ((leftBottomTouchingHit + leftTouchingHit) != 0) ? true : false;

		int rightTouchingHit = Physics2D.OverlapCircleNonAlloc(pRightCheck.position, 0.1f, emptyArray, GameLayers.TILE_COLLIDER_LAYER);
		int rightBottomTouchingHit = Physics2D.OverlapCircleNonAlloc(pRightBottomCheck.position, 0.05f, emptyArray, GameLayers.TILE_COLLIDER_LAYER);
		rightTouching = ((rightBottomTouchingHit + rightTouchingHit) != 0) ? true : false;
		
		if (leftTouching && !previouslyLeftTouching) {
			pWallSlideParticleSystem.SetHorizontalDirection(HorizontalDirection.Left);
		}
		if (rightTouching && !previouslyRightTouching) {
			pWallSlideParticleSystem.SetHorizontalDirection(HorizontalDirection.Right);
		}

		pAnimator.SetBool("Grounded", grounded);
		pAnimator.SetBool("TouchingWall", rightTouching || leftTouching);
	}

	protected void TurnPlayerToCorrectDirection() {
		if (CurrentlySlashing()) {
			return;
		}

		if ((MovingLeft() || leftTouching) && !rightTouching) {
			pSpriteObjects.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
			pColliders.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
			pRunParticleSystem.SetHorizontalDirection(HorizontalDirection.Left);
		} else if ((MovingRight() || rightTouching) && !leftTouching) {
			pSpriteObjects.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			pColliders.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			pRunParticleSystem.SetHorizontalDirection(HorizontalDirection.Right);
		} else {
			// right touching and left touching?
		}
		
	}
}
