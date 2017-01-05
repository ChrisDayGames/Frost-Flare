using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour {

	//Call these variables exactly as the gameobjects
	//are named in the resources folder
	public const string FIRE = "Fire";
	public const string ICE = "Ice";
	public const string EARTH = "Earth";
	public const string LIGHTNING = "Lightning";
	public const string GOLD = "Gold";


	public static string[] playerChoices = {
		FIRE,
		ICE
	};

	public Color[] ballColors;

	public SpriteRenderer[] coloredImagesPlayer1;
	public SpriteRenderer[] coloredImagesPlayer2;

	public static string ConvertIntToPlayerType (int selection) {

		switch (selection) {

		case 0:
			return FIRE;
		case 1:
			return ICE;
		case 2:
			return EARTH;
		case 3:
			return LIGHTNING;
		case 4:
			return GOLD;

			default:
			return FIRE;

		}

	}

	public Color ConvertStringToPlayerColors (string selection) {
		switch (selection) {

		case FIRE:
			return ballColors [0];
		case ICE:
			return ballColors [1];
		case EARTH:
			return ballColors [2];
		case LIGHTNING:
			return ballColors [3];
		case GOLD:
			return ballColors [4];

			default:
			return ballColors [0];

		}
	}

	public void LoadPlayerChoices () {

		GameController.instance.ShowControls ();

		//set a new type for the all the ball spawners
		GameObject[] spawners = GameObject.FindGameObjectsWithTag ("Spawner");

		Color player1Color = new Color ();
		Color player2Color = new Color ();

		for (int i = 0; i < spawners.Length; i++) {
			
			//get a reference to the spawner
			BallSpawner spawner = spawners[i].GetComponent<BallSpawner> ();

			//change its type
			spawner.choice = playerChoices[spawner.playerNumber];
			spawner.LoadNewBall ();

			if (i == 0) {

				player1Color = ConvertStringToPlayerColors (spawner.choice);
				
			}

			if (i == spawners.Length - 1) {

				player2Color = ConvertStringToPlayerColors (spawner.choice);

			}

		}

		for (int j = 0; j < coloredImagesPlayer1.Length; j++) {

			coloredImagesPlayer1[j].color = player1Color;
		}

		for (int j = 0; j < coloredImagesPlayer2.Length; j++) {

			coloredImagesPlayer2[j].color = player2Color;
		}

		GameController.instance.HideControls ();

	}

}
