using UnityEngine;
using System.Collections;

public class ParticleSystemController : MonoBehaviour {
	[SerializeField]
	protected Material rightMaterial, leftMaterial;
	
	protected Renderer _particleSystemRenderer;
	
	public void SetHorizontalDirection(HorizontalDirection d) {
		Material chosen = null, backup = null;
		switch (d) {
			case HorizontalDirection.Left:
				chosen = leftMaterial;
				backup = rightMaterial;
				break;
			case HorizontalDirection.Right:
				chosen = rightMaterial;
				backup = leftMaterial;
				break;
		}
		
		if (chosen != null) {
			_particleSystemRenderer.material = chosen;
		} else {
			_particleSystemRenderer.material = backup;
		}
	}
	
	protected void Awake() {
		_particleSystemRenderer = GetComponent<ParticleSystem>().GetComponent<Renderer>();
	}
}
