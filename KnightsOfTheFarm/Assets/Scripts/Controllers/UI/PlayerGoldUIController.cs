using UnityEngine;
using System.Collections;

public class PlayerGoldUIController : MonoBehaviour {
	protected PlayerInventoryController inventory;
	protected tk2dTextMesh textMesh;

	protected void Awake() {
		textMesh = GetComponentInChildren<tk2dTextMesh>();
		EventManager.Instance.OnPlayerRegister.AddListener((GameObject playerObject) => { SetupWithPlayer(playerObject); });
	}
	
	protected void SetupWithPlayer(GameObject playerObject) {
		inventory = playerObject.GetComponent<PlayerInventoryController>();

		textMesh.text = inventory.Gold().ToString();
	}

	protected void Update() {
		textMesh.text = inventory.Gold().ToString();
	}
}
