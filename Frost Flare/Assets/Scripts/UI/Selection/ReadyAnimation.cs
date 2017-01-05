using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyAnimation : MonoBehaviour {

	public Animator anim;

	// Use this for initialization
	void OnEnable () {
		anim.enabled = true;
	}

	public void ActivateAnimator () {
		anim.enabled = true;
	}

	public void DeactivateAnimator () {
		anim.enabled = false;
	}

}
