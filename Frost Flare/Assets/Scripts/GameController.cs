using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public const string ONE_PLAYER = "1p";
	public const string TWO_PLAYER = "2p";

	public const string MENU = "menu";
	public const string PLAYING = "playing";
	public const string OVER = "over";

	public static string state = MENU;
	public static string mode = TWO_PLAYER;
	public static GameController instance;

	public MenuNavigation menuNav;
	public ScoreManager score;
	public CharacterSelection characterSelection;

	//boolean array for managing the starting of games
	public bool[] playersAreReady;

	void Awake () {

		if (instance == null)
			instance = this;

	}

	public void StartGame () {

		//Do not start the game if the players are not ready
		for (int i = 0; i < playersAreReady.Length; i++) {
			if (!playersAreReady[i])
				return;
		}

		characterSelection.LoadPlayerChoices();
		RestartGame ();

	}

	public void EndGame () {

		if (mode == TWO_PLAYER) {
			menuNav.ShowPopUp ("Over");

		}

		state = OVER;

	}

	public void RestartGame () {

		ClearObjects ();

		menuNav.CloseMenu ();
		menuNav.CloseAll (menuNav.popUpScreens);
		state = PLAYING;
		
	}

	public void ClearObjects () {

		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Ball");

		for (int i = 0; i < balls.Length; i++) {
			Destroy (balls[i]);
		}

	}
		
}
