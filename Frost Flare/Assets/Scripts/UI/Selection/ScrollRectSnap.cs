using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour {

	//The panel that shows that the player is ready
	public RectTransform readyPanel;

	//an int for accessing the character selection array
	public int playerNumber;

	//public vars
	public RectTransform panel; 		//To hold the scroll panel
	public RectTransform[] containedButtons;	//The buttons that are in the scrolling list
	public RectTransform center; 		// center to compare the distance for each button

	//private vars
	private float[] distances; 			//all buttons distance to the center
	private bool dragging = false; 		//will be true while we drag the panel
	private int buttonDistance;			//Will hold the distance between the buttons
	private int minButtonIndex;			//Index of the button with the smallest distance

	void Start () {
		
		//initialize the distances array
		int numberOfButtons = containedButtons.Length;
		distances = new float[numberOfButtons];

		//get distance between any 2 buttons buttons
		//make sure the buttons in the buttons array are evenly spaced out
		buttonDistance = (int) Mathf.Abs (containedButtons[1].anchoredPosition.x - containedButtons[0].anchoredPosition.x);

	}

	void Update () {

		//start the min distance at the highest possible value
		//this is so that we can ensure all distances
		//in the actual button array will be smaller
		float minDistance = int.MaxValue;

		for (int i = 0; i < containedButtons.Length; i++) {
			//record the current distance of each button to the center
			distances[i] = Mathf.Abs (center.transform.position.x - containedButtons[i].transform.position.x);

			//check if the current distance is lower than the current lowest
			if (distances [i] < minDistance) {

				//if it is this is the minDistance
				minDistance = distances[i];

				//record which button is associated with the min distance
				minButtonIndex = i;

			}

		}

		//make the currently selected button bigger
		for (int i = 0; i < containedButtons.Length; i++) {

			if (i == minButtonIndex) {
				containedButtons[i].sizeDelta = new Vector2 (150, 150);
			} else {
				containedButtons[i].sizeDelta = new Vector2 (100, 100);
			}

		}


		//If we are not currently dragging, then move to the position of the current button that is closest to the center
		if (!dragging) {
			LerpToButton (minButtonIndex * -buttonDistance);
		}

	}

	//simple, humble LERP
	void LerpToButton (int position) {

		//find the new x value of the scroll window
		float newX = Mathf.Lerp (panel.anchoredPosition.x, position, Time.deltaTime * 10f);
		//create a new vector to feed the new x position to the scroll window's position
		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);

		//give the window a the new position
		panel.anchoredPosition = newPosition;

	}

	//These functions will be used in the inspector on the actual scroll window script
	public void StartDrag () {
		dragging = true;
	}

	public void EndDrag () {
		dragging = false;
	}

	public void ConfirmSelection () {
		CharacterSelection.playerChoices[playerNumber] = CharacterSelection.ConvertIntToPlayerType (minButtonIndex);
	}

	public void ShowReadyPanel () {

		readyPanel.gameObject.SetActive (true);
		GameController.instance.playersAreReady [playerNumber] = true;

	}

	public void HideReadyPanel () {
		
		readyPanel.gameObject.SetActive (false);
		GameController.instance.playersAreReady [playerNumber] = false;

	}

}
