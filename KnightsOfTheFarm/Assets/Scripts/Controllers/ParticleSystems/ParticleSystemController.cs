using UnityEngine;
using System.Collections;

public class ParticleSystemController : MonoBehaviour {
	[SerializeField]
	protected GameObject leftParticleSystem, rightParticleSystem;
	protected ParticleSystem leftPS, rightPS;
	
	[SerializeField]
	protected float emissionRate = 0.0f;
	protected HorizontalDirection direction = HorizontalDirection.Right;
	
	public void SetHorizontalDirection(HorizontalDirection d) { 		
		direction = d;
		ApplyEmissionRateToChildren();
	}
	
	protected void OnDidApplyAnimationProperties() {
		ApplyEmissionRateToChildren();
	}
	
	protected void ApplyEmissionRateToChildren() {
		switch (direction) {
			case HorizontalDirection.Left:
				leftPS.emissionRate = emissionRate;
				rightPS.emissionRate = 0.0f;
				break;
			case HorizontalDirection.Right:
				leftPS.emissionRate = 0.0f;
				rightPS.emissionRate = emissionRate;
				break;
		}
	}
	
	protected void Start() {
		if (leftParticleSystem != null) {
			leftPS = leftParticleSystem.GetComponent<ParticleSystem>();
		}
		if (rightParticleSystem != null) {
			rightPS = rightParticleSystem.GetComponent<ParticleSystem>();
		}
	}
}
