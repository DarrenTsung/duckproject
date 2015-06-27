using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputManager : Singleton<PlayerInputManager> {

	protected PlayerInputManager () {}

	[SerializeField]
	private GameObject pObject;

	protected PlayerMovementController pMovementController;
	protected PlayerActionController pActionController;

	protected KeyCode interactKey = KeyCode.E;
	protected KeyCode slashKey = KeyCode.J;
	protected KeyCode jumpKey = KeyCode.K;
	protected KeyCode teleportKey = KeyCode.L;

	[HideInInspector]
	public bool interactionEnabled, movementInputEnabled, slashingEnabled, jumpingEnabled, teleportingEnabled;

	protected void Awake() {
		EventManager.Instance.OnPlayerRegister.AddListener((GameObject playerObject) => { SetPlayerController(playerObject); });
	}

	public void SetPlayerController (GameObject playerObject) {
		pObject = playerObject;
		pMovementController = pObject.GetComponent<PlayerMovementController>();
		pActionController = pObject.GetComponent<PlayerActionController>();

		EnablePlayerInput();
	}

	public void DisablePlayerInput() {
		interactionEnabled = false;
		movementInputEnabled = false;
		slashingEnabled = false;
		jumpingEnabled = false;
		teleportingEnabled = false;
	}

	public void EnablePlayerInput() {
		interactionEnabled = true;
		movementInputEnabled = true;
		slashingEnabled = true;
		jumpingEnabled = true;
		teleportingEnabled = true;
	}

	protected void Update() {
		if (pObject) {
			if (slashingEnabled) {
				if (Input.GetKeyDown(slashKey)) {
					pActionController.HandleSlashPressedDown();
				} else if (Input.GetKeyUp(slashKey)) {
					pActionController.HandleSlashReleased();
				}
			}

			if (interactionEnabled) {
				if (Input.GetKeyDown(interactKey)) {
					pActionController.HandleInteractPressed();
				}
			}

			if (jumpingEnabled) {
				if (Input.GetKeyDown(jumpKey)) {
					pMovementController.HandleJumpPressed();
				}
			}

			if (movementInputEnabled) {
				Vector2 axisVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
				pMovementController.HandleAxisVector(axisVector);
			}

			if (teleportingEnabled) {
				if (Input.GetKeyDown(teleportKey)) {
					pMovementController.HandleTeleportPressed();
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.F)) {
			Debug.Break();
		}
		if (Input.GetKeyDown(KeyCode.P)) {
			Application.CaptureScreenshot("Screenshot.png");
		}
		if (Input.GetKeyDown(KeyCode.M)) {
			pObject.GetComponent<HealthComponent>().TakeDamage(10);
		}
	}
}
