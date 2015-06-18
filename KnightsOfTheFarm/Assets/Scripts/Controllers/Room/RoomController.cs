using UnityEngine;
using System.Collections;

public class RoomController : MonoBehaviour {
	protected tk2dTileMap _tilemap;

	protected Rect _bounds;

	protected void Awake() {
		_tilemap = GetComponentInChildren<tk2dTileMap>();
		_bounds = new Rect(0.0f, 0.0f, _tilemap.width, _tilemap.height);
	}

	public Rect Bounds() {
		return _bounds;
	}

	public Vector3 Position() {
		// subtract half a single tile (the natural offset to the tk2d tilemap)
		return transform.position - new Vector3(0.5f, 0.5f);
	}
}
