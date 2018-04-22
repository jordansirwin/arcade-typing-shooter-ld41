using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour {

	public Text hitsText;
	public Text bonusText;
	public Text shotsText;
	public Text scoreText;

	void Start () {
		InvokeRepeating("UpdateScores", 0f, 0.2f);

		var scoreGO = GameObject.FindGameObjectWithTag("FinalScoreText");
		if(scoreGO != null)
			scoreText = scoreGO.GetComponent<Text>();
	}
	
	void UpdateScores() {
		hitsText.text = GameManager.Instance.enemiesHit.ToString();
		bonusText.text = GameManager.Instance.enemiesBonus.ToString();
		shotsText.text = GameManager.Instance.shotsFired.ToString();

		if(scoreText != null)
			scoreText.text = GameManager.Instance.Score.ToString();
	}
}
