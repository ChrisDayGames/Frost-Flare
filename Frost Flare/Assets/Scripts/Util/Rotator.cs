using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public Vector3 rotationAxis;
	public float rotationSpeed;
	public bool randomRotation;

	void Start () {

		if (randomRotation) {

			int xSign = Random.Range(0, 2) == 0 ? -1 : 1;
			int ySign = Random.Range(0, 2) == 0 ? -1 : 1;
			int zSign = Random.Range(0, 2) == 0 ? -1 : 1;

			rotationAxis = new Vector3 (
				xSign * Random.RandomRange (20, 45),
				ySign * Random.RandomRange (20, 45),
				zSign * Random.RandomRange (20, 45)
			);

			rotationSpeed = Random.Range (0.5f, 1f);

		}

	}


	// Update is called once per frame
	void Update () {
		
		transform.Rotate (rotationAxis * (rotationSpeed) * Time.deltaTime);

	}

}
