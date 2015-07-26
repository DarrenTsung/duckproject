using UnityEngine;
using System.Collections;

public class PlayerComponentManager : MonoBehaviour {

	[HideInInspector]
	public CircleCollider2D pCircleCollider;
	[HideInInspector]
	public Rigidbody2D pRigidbody;
	[HideInInspector]
	public Transform pGroundCheck;
	[HideInInspector]
	public Transform pLeftCheck, pLeftTopCheck, pLeftBottomCheck;
	[HideInInspector]
	public Transform pRightCheck, pRightTopCheck, pRightBottomCheck;  
	[HideInInspector]
	public GameObject pSpriteObjects;
	[HideInInspector]
	public GameObject pColliders;
	[HideInInspector]
	public SpriteRenderer pSpriteRenderer;
	[HideInInspector]
	public Animator pAnimator;
	[HideInInspector]
	public ParticleSystem pJumpEmitter;
	[HideInInspector]
	public ParticleSystemController[] pParticleSystemControllers;

	protected void Awake() {
		pRigidbody = GetComponent<Rigidbody2D>();
		pAnimator = GetComponent<Animator>();
		pCircleCollider = transform.Find ("Colliders/Geometry").GetComponent<CircleCollider2D>();
		pSpriteRenderer = transform.Find ("Sprite").GetComponent<SpriteRenderer>();
		pSpriteObjects = transform.Find ("Sprite").gameObject;
		pColliders = transform.Find ("Colliders").gameObject;
		pGroundCheck = transform.Find ("Checks/GroundCheck");
		pLeftCheck = transform.Find ("Checks/LeftCheck");
		pLeftTopCheck = transform.Find ("Checks/LeftTopCheck");
		pLeftBottomCheck = transform.Find ("Checks/LeftBottomCheck");
		pRightCheck = transform.Find ("Checks/RightCheck");
		pRightTopCheck = transform.Find ("Checks/RightTopCheck");
		pRightBottomCheck = transform.Find ("Checks/RightBottomCheck");
		pJumpEmitter = transform.Find ("Sprite/ParticleEmitters/JumpEmitter").GetComponent<ParticleSystem>();
		pParticleSystemControllers = GetComponentsInChildren<ParticleSystemController>();
	}

	protected void Start() {
		// register myself as the player
		EventManager.Instance.OnPlayerRegister.Invoke(gameObject);
	}
}
