using UnityEngine;
using System.Collections;

public class PlayerStatusController : MonoBehaviour {
	protected Rigidbody2D pRigidbody;
	protected Animator pAnimator;

	protected GameObject pSpriteObjects;
	protected ParticleSystemController[] pParticleSystemControllers;
	protected GameObject pColliders;

	protected Transform pGroundCheck;
	protected Transform pRightCheck, pRightTopCheck, pRightBottomCheck;
	protected Transform pLeftCheck, pLeftTopCheck, pLeftBottomCheck;
	
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
		pLeftTopCheck = pComponentManager.pLeftTopCheck;
		pLeftBottomCheck = pComponentManager.pLeftBottomCheck;
		pRightCheck = pComponentManager.pRightCheck;
		pRightTopCheck = pComponentManager.pRightTopCheck;
		pRightBottomCheck = pComponentManager.pRightBottomCheck;
		pRigidbody = pComponentManager.pRigidbody;
		pAnimator = pComponentManager.pAnimator;
		pSpriteObjects = pComponentManager.pSpriteObjects;
		pColliders = pComponentManager.pColliders;
		pParticleSystemControllers = pComponentManager.pParticleSystemControllers;
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
	
	public bool CurrentlyChargeSlashing() {
		return pAnimator.GetCurrentAnimatorStateInfo(0).IsTag("ChargeSlash");
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
		int leftTopTouchingHit = Physics2D.OverlapCircleNonAlloc(pLeftTopCheck.position, 0.1f, emptyArray, GameLayers.TILE_COLLIDER_LAYER);
		int leftBottomTouchingHit = Physics2D.OverlapCircleNonAlloc(pLeftBottomCheck.position, 0.05f, emptyArray, GameLayers.TILE_COLLIDER_LAYER);
		leftTouching = ((leftBottomTouchingHit + leftTouchingHit) != 0) ? true : false;

		int rightTouchingHit = Physics2D.OverlapCircleNonAlloc(pRightCheck.position, 0.1f, emptyArray, GameLayers.TILE_COLLIDER_LAYER);
		int rightTopTouchingHit = Physics2D.OverlapCircleNonAlloc(pRightTopCheck.position, 0.1f, emptyArray, GameLayers.TILE_COLLIDER_LAYER);
		int rightBottomTouchingHit = Physics2D.OverlapCircleNonAlloc(pRightBottomCheck.position, 0.05f, emptyArray, GameLayers.TILE_COLLIDER_LAYER);
		rightTouching = ((rightBottomTouchingHit + rightTouchingHit) != 0) ? true : false;
		
		bool rightAllTouching = ((rightTouchingHit != 0) && (rightTopTouchingHit != 0) && (rightBottomTouchingHit != 0));
		bool leftAllTouching = ((leftTouchingHit != 0) && (leftTopTouchingHit != 0) && (leftBottomTouchingHit != 0));

		pAnimator.SetBool("Grounded", grounded);
		pAnimator.SetBool("TouchingWall", rightAllTouching || leftAllTouching);
	}

	protected void TurnPlayerToCorrectDirection() {
		if (CurrentlySlashing()) {
			return;
		}

		HorizontalDirection d = HorizontalDirection.Left;
		if ((MovingLeft() || leftTouching) && !rightTouching) {
			pSpriteObjects.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
			pColliders.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
			d = HorizontalDirection.Left;
		} else if ((MovingRight() || rightTouching) && !leftTouching) {
			pSpriteObjects.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			pColliders.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			d = HorizontalDirection.Right;
		} else {
			// right touching and left touching?
		}
		
		foreach (ParticleSystemController ps in pParticleSystemControllers) {
			ps.SetHorizontalDirection(d);
		}
		
	}
}
