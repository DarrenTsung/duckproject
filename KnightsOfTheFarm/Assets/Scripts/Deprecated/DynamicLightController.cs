using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
public class DynamicLightController : MonoBehaviour {

	[SerializeField]
	protected LayerMask raycastLayers;

	protected Dictionary<float, GameObject> lightShafts;
	protected Light pointLight;
	protected float lightShaftCount;

	protected void Awake () {
		pointLight = GetComponent<Light>();
		lightShafts = new Dictionary<float, GameObject>();
		lightShaftCount = 300.0f;
	}

	protected void Start () {
		// load up light shafts in 360 degrees
		for (float i=0; i < 2.0f * Mathf.PI; i += 2.0f * Mathf.PI / lightShaftCount) {
			GameObject lightShaft = Instantiate(PrefabManager.Instance.LightShaft) as GameObject;
			lightShaft.transform.parent = transform;
			lightShaft.transform.localPosition = Vector3.zero;

			LineRenderer lineRenderer = lightShaft.GetComponent<LineRenderer>();
			lineRenderer.SetPosition(0, Vector3.zero);
			lightShafts.Add(i, lightShaft);
		}
		RecomputeLightShafts();
	}

	protected void Update () {
		RecomputeLightShafts();
	}

	private void RecomputeLightShafts() {
		foreach (KeyValuePair<float, GameObject> entry in lightShafts) {
			GameObject lightShaft = entry.Value;
			float radianAngle = entry.Key;

			Vector2 rayDirection = new Vector2(Mathf.Cos (radianAngle), Mathf.Sin (radianAngle));

			float shaftDistance = pointLight.range;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, pointLight.range, raycastLayers);
			if (hit.collider != null) {
				shaftDistance = Vector2.Distance(hit.point, transform.position); 
			}

			float flippedPercent = 1.0f - shaftDistance / pointLight.range;
			float endWidth = 2.0f * Mathf.PI * shaftDistance / lightShaftCount;

			LineRenderer lineRenderer = lightShaft.GetComponent<LineRenderer>();
			lineRenderer.SetPosition(1, shaftDistance * rayDirection);
			lineRenderer.SetColors(pointLight.color, new Color(flippedPercent, flippedPercent, flippedPercent));
			lineRenderer.SetWidth(0.0f, endWidth);
		}
	}
}
*/
