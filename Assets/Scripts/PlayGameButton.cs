using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameButton : MonoBehaviour {

	// I couldn't get the button click UI to bind to the GameManager since it's "Don't Destroy On Load"
	// ... and TIME TIME TIME!
	public void PlayGame() {
		GameManager.Instance.StartGame();
	}
}
