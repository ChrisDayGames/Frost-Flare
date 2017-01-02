using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour {

	//Call these variables exactly as the gameobjects
	//are named in the resources folder
	public const string FIRE = "Fire";
	public const string ICE = "Ice";
	public const string LIGHTNING = "Lightning";
	public const string EARTH = "Earth";
	public const string GOLD = "Gold";


	public static string[] playerChoices = {
		FIRE,
		ICE
	};

	public static string ConvertIntToPlayerType (int selection) {

		switch (selection) {

		case 0:
			return FIRE;
		case 1:
			return ICE;
		case 2:
			return LIGHTNING;
		case 3:
			return EARTH;
		case 4:
			return GOLD;

			default:
			return FIRE;

		}

	}

	public void LoadPlayerChoices () {

		//set a new type for the all the ball spawners
		GameObject[] spawners = GameObject.FindGameObjectsWithTag ("Spawner");

		for (int i = 0; i < spawners.Length; i++) {
			
			//get a reference to the spawner
			BallSpawner spawner = spawners[i].GetComponent<BallSpawner> ();

			//change its type
			spawner.choice = playerChoices[spawner.playerNumber];
			spawner.LoadNewBall ();

		}

	}

}
