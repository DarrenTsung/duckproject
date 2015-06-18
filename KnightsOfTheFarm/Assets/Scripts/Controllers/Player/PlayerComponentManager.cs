using UnityEngine;
using System.Collections;

public class PlayerComponentManager : MonoBehaviour {

	[HideInInspector]
	public CircleCollider2D pCircleCollider;
	[HideInInspector]
	public Rigidbody2D pRigidbody;
	[HideInInspector]
	public Transform pGroundCheck, pLeftCheck, pRightCheck, pLeftBottomCheck, pRightBottomCheck;
	[HideInInspector]
	public GameObject pSpriteObjects;
	[HideInInspector]
	public GameObject pColliders;
	[HideInInspector]
	public SpriteRenderer pSpriteRenderer;
	[HideInInspector]
	public Animator pAnimator;

	protected void Awake() {
		pRigidbody = GetComponent<Rigidbody2D>();
		pAnimator = GetComponent<Animator>();
		pCircleCollider = transform.Find ("Colliders/Geometry").GetComponent<CircleCollider2D>();
		pSpriteRenderer = transform.Find ("Sprite").GetComponent<SpriteRenderer>();
		pSpriteObjects = transform.Find ("Sprite").gameObject;
		pColliders = transform.Find ("Colliders").gameObject;
		pGroundCheck = transform.Find ("Checks/GroundCheck");
		pLeftCheck = transform.Find ("Checks/LeftCheck");
		pRightCheck = transform.Find ("Checks/RightCheck");
		pRightBottomCheck = transform.Find ("Checks/RightBottomCheck");
		pLeftBottomCheck = transform.Find ("Checks/LeftBottomCheck");
	}

	protected void Start() {
		// register myself as the player
		EventManager.Instance.OnPlayerRegister.Invoke(gameObject);
	}
}
