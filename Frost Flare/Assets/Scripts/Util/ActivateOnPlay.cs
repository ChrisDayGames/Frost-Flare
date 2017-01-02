using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnPlay : MonoBehaviour {

	private bool hasReset = false;
	
	// Update is called once per frame
	void Update () {

		if (!hasReset && GameController.state == GameController.PLAYING) {

			gameObject.SetActive (false);
			gameObject.SetActive (true);
			hasReset = true;

		}

		if (GameController.state != GameController.PLAYING) {
			hasReset = false;
		}
		
	}

}
