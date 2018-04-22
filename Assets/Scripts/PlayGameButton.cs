using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGameButton : MonoBehaviour {

	// I couldn't get the button click UI to bind to the GameManager since it's "Don't Destroy On Load"
	// ... and TIME TIME TIME!
	public void PlayGame() {
		if(GameManager.Instance == null) {
			SceneManager.LoadScene("Game");
			return;
		}

		if(SceneManager.GetActiveScene().name == "GameOver") {
			SceneManager.LoadScene("MainMenu");
			return;
		}

		GameManager.Instance.RestartGame();
	}
}
