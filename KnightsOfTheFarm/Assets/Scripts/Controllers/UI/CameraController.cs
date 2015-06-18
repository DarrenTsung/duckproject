using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	protected const float speed = 1.5f; 

	protected GameObject _target;
	protected tk2dCamera _camera;

	protected Rect _currentRoomBounds;
	protected Vector3 _currentRoomPosition;

	protected void Awake () {
		EventManager.Instance.OnPlayerRegister.AddListener((GameObject playerObject) => { SetTarget(playerObject); });
		_camera = GetComponent<tk2dCamera>();
	}

	protected void Start() {
		RoomController roomController = GameObject.FindGameObjectWithTag("Finish").GetComponent<RoomController>();
		_currentRoomBounds = roomController.Bounds();
		_currentRoomPosition = roomController.Position();
	}

	public void SetTarget(GameObject target) {
		_target = target;
	}

	protected void Update () {
		Vector3 targetPosition = _target.transform.position;
		targetPosition.z = transform.position.z; // don't move in the z axis

		transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

		RestrictCameraInBounds(_currentRoomBounds, _currentRoomPosition);
	}

	protected bool RestrictCameraInBounds(Rect otherBounds, Vector3 otherPosition) {
		Rect cameraExtents = _camera.ScreenExtents;

		float yOver = (transform.position.y + cameraExtents.yMax) - (otherPosition.y + otherBounds.yMax);
		if (yOver > 0) {
			transform.position = transform.position - new Vector3(0.0f, yOver);
		}

		float yBelow = (transform.position.y + cameraExtents.yMin) - (otherPosition.y + otherBounds.yMin);
		if (yBelow < 0) {
			transform.position = transform.position - new Vector3(0.0f, yBelow);
		}

		float xOver = (transform.position.x + cameraExtents.xMax) - (otherPosition.x + otherBounds.xMax);
		if (xOver > 0) {
			transform.position = transform.position - new Vector3(xOver, 0.0f);
		}

		float xBelow = (transform.position.x + cameraExtents.xMin) - (otherPosition.x + otherBounds.xMin);
		if (xBelow < 0) {
			transform.position = transform.position - new Vector3(xBelow, 0.0f);
		}

		return true;
	}
}
