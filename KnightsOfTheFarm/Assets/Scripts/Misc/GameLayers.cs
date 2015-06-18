using UnityEngine;
using System.Collections;

public class GameLayers {

	public static LayerMask ALL_TILE_LAYERS = LayerMask.GetMask("TileColliders", "PlatformColliders");
	public static LayerMask TILE_COLLIDER_LAYER = LayerMask.GetMask("TileColliders");
}
