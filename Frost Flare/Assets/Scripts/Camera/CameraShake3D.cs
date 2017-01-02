
using UnityEngine;
using System.Collections;

public class CameraShake3D : MonoBehaviour {
		
	public static CameraShake3D instance;

		// Transform of the camera to shake. Grabs the gameObject's transform
		// if null.
	public Transform camTransform;
	public Camera cam;

		// How long the object should shake for.
	public float shakeDuration = 0f;

		// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

	void Awake() {
			
		if (camTransform == null) {
				
			camTransform = GetComponent(typeof(Transform)) as Transform;
			cam = camTransform.GetComponent<Camera> ();
		}

		if (instance == null) {

			instance = this;

		}

	}

	void OnEnable() {
			
		originalPos = camTransform.localPosition;

	}

	void Update() {
			
		if (shakeDuration > 0) {

			Vector3 shake = Random.insideUnitSphere * shakeAmount;

			camTransform.localPosition = originalPos + shake;


			shakeDuration -= Time.deltaTime * decreaseFactor;

		} else {
				
			shakeDuration = 0f;

		}

	}

	public void ShakeCamera (float duration, float amplitude, float decay) {

		shakeDuration = duration;
		shakeAmount = amplitude;
		decreaseFactor = decay;

	}

}
