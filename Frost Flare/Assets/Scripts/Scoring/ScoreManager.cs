using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	//the text objects that will display the score
	public Text scoreDisplayP1;
	public Text scoreDisplayP2;

	//ints to keep track of each players score
	[HideInInspector]
	public int scoreP1 = 0;
	[HideInInspector]
	public int scoreP2 = 0;

	//functions for increasing the score of a single player
	//and updating the display
	public void IncreaseP1Score () {
		scoreP1++;

		UpdateScoreP1Display ();
	}

	public void IncreaseP2Score () {
		scoreP2++;

		UpdateScoreP2Display ();
	}

	//functions for updating the score display
	public void UpdateScoreP1Display () {
		scoreDisplayP1.text = scoreP1.ToString ();
	}

	public void UpdateScoreP2Display () {
		scoreDisplayP2.text = scoreP2.ToString ();
	}

	//functions for reseting the game
	public void ResetScores () {
		
		scoreP1 = 0;
		UpdateScoreP1Display ();

		scoreP2 = 0;
		UpdateScoreP2Display ();

	}

}
