using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public Vector3 rotationAxis;
	public float rotationSpeed;

	// Update is called once per frame
	void Update () {
		
		transform.Rotate (rotationAxis * (rotationSpeed) * Time.deltaTime);

	}

}
