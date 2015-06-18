using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealthUIController : MonoBehaviour {
	protected const string HEART_CONTAINER_PREFAB_NAME = "HeartContainer";
	protected const float HEART_CONTAINER_OFFSET = 0.85f;

	protected List<HeartContainerController> heartContainers;
	protected PlayerHealthComponent model;

	protected void Awake() {
		heartContainers = new List<HeartContainerController>();

		EventManager.Instance.OnPlayerRegister.AddListener((GameObject playerObject) => { SetupWithPlayer(playerObject); });
	}

	protected void SetupWithPlayer(GameObject playerObject) {
		model = playerObject.GetComponent<PlayerHealthComponent>();

		for (int i = 0; i < model.NumberOfContainers(); i++) {
			GameObject heartContainerObject = PrefabManager.Instance.SpawnPrefab(HEART_CONTAINER_PREFAB_NAME, Vector3.zero);
			heartContainerObject.transform.parent = transform;
			heartContainerObject.transform.localPosition = new Vector3(HEART_CONTAINER_OFFSET * i, 0.0f, 0.0f);

			HeartContainerController controller = heartContainerObject.GetComponent<HeartContainerController>();
			if (!controller) {
				Debug.LogError("SetupWithPlayer - Something went wrong, no health container controller");
			}
			heartContainers.Add(controller);
		}
	}

	protected void Update() {
		for (int i = 0; i < model.NumberOfContainers(); i++) {
			HeartContainerController controller = heartContainers[i];

			float percentFilled = model.PercentFilledForContainer(i);
			controller.SetPercentFill(percentFilled);
		}
	}
}
