using UnityEngine;

public class CameraShake2D : MonoBehaviour {
		
	public static CameraShake2D instance;

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

			Vector2 shake = Random.insideUnitCircle * shakeAmount;

			camTransform.localPosition = originalPos + new Vector3 (shake.x, shake.y, 0.0f);


			shakeDuration -= Time.deltaTime * decreaseFactor;

		} else {
				
			transform.position = originalPos;
			shakeDuration = 0f;

		}

	}

	public void ShakeCamera (float duration, float amplitude, float decay) {

		shakeDuration = duration;
		shakeAmount = amplitude;
		decreaseFactor = decay;

	}

}
