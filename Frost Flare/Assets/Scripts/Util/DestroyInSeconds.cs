using UnityEngine;
using System.Collections;

public class DestroyInSeconds : MonoBehaviour {

	public float destroyWait;

	private float timer;

	// Use this for initialization
	void Start () {

		SetTimer (destroyWait);
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time > timer) {

			Destroy (gameObject);

		}
	
	}

	public void SetTimer (float waitTime) {

		timer = Time.time + destroyWait;

	}

}
