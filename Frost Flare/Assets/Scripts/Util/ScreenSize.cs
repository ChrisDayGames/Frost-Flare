using UnityEngine;
using System.Collections;

public class ScreenSize : MonoBehaviour {

	public static float width = 0.0f;
	public static float height = 0.0f;

	Vector3 bottomCorner;

	// Use this for initialization
	void Awake () {

		Vector2 edgeVector = Camera.main.ViewportToWorldPoint(new Vector2 (0,0));
		bottomCorner = new Vector2 (edgeVector.x, edgeVector.y);

		width = -bottomCorner.x * 2;
		height = -bottomCorner.y * 2;
	
	}

}
