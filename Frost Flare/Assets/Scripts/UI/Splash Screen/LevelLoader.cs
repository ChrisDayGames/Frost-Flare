using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//TUTORIAL: https://www.youtube.com/watch?v=bXobcRa6Iq8

public class LevelLoader : MonoBehaviour {

	public Image loadingBar;
	public Text percentage;

	public void LoadLevel () {

		loadingBar.fillAmount = 0f;
		StartCoroutine (LevelCoroutine ());

	}

	IEnumerator LevelCoroutine() {

		AsyncOperation async = SceneManager.LoadSceneAsync ("Main");

		while (!async.isDone) {

			if (loadingBar.fillAmount + (async.progress / 0.9f)/2 < 0.5f) {

				loadingBar.fillAmount += 0.05f;

			} else {

				loadingBar.fillAmount = 0.5f + (async.progress / 0.9f)/2;

			}
		
			percentage.text = (int) ((loadingBar.fillAmount) * 100) + "%";
			yield return null;

		}

	}

}
