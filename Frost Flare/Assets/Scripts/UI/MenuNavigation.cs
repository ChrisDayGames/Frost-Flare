﻿using UnityEngine;
using System.Collections;

public class MenuNavigation : MonoBehaviour {

	public GameObject menuContainer;
	public GameObject[] menuScreens;
	public GameObject[] popUpScreens;

	//boolean array for managing the starting of games
	public bool[] playersAreReady;

	public void CloseAll (GameObject[] arrayToClose) {

		for (int i = 0; i < arrayToClose.Length; i++) {

			if (arrayToClose [i].activeSelf) {

				arrayToClose [i].SetActive(false);

			}

		}

	}

	public void GoToScreen (string screenName) {

		PlayersAreNotReady ();
		OpenMenu ();

		GameController.state = GameController.MENU;

		CloseAll (popUpScreens);
		CloseAll (menuScreens);

		for (int i = 0; i < menuScreens.Length; i++) {

			if (menuScreens[i].name == screenName) {

				menuScreens [i].SetActive (true);
				return;

			}

		}
			
	}

	public void ShowPopUp (string popUpName) {

		CloseAll (popUpScreens);

		for (int i = 0; i < menuScreens.Length; i++) {

			if (popUpScreens[i].name == popUpName) {

				popUpScreens [i].SetActive (true);
				return;

			}

		}

	}

	public void OpenMenu () {

		if (!menuContainer.activeSelf)
			menuContainer.SetActive (true);

	}

	public void CloseMenu () {

		menuContainer.SetActive (false);

	}
		
	public void QuitGame () {

		Application.Quit ();

	}


	public void PlayersAreNotReady () {

		for (int i = 0; i < playersAreReady.Length; i++) {
			playersAreReady[i] = false;
		}

	}

}
