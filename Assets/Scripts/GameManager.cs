﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

#region Singleton
	private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }

	void Awake() {
		if(Instance != null) {
			Destroy(this);
			return;
		}

		_instance = this;
		DontDestroyOnLoad(this);
	}
#endregion Singleton


	public int shotsFired;
	public int enemiesKilled;
	public int enemiesEscaped;
	public int life;
	public Text scoreText;
	public Text killsText;
	public Text escapesText;
	public Text shotsText;

	public float spawnInitialWait = 5f;
	public float spawnCooldown = 5f;

	public GameObject[] enemies;

	public Transform enemySpawnXPosition;
	public Transform enemyEscapeXPosition;

	public int Score { get { return (enemiesKilled * 5) - shotsFired - (enemiesEscaped * 10); } }

	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnEnemies", spawnInitialWait, spawnCooldown);
		InvokeRepeating("UpdateScores", 0f, 0.25f);
	}

	void UpdateScores() {
		scoreText.text = Score.ToString();
		killsText.text = enemiesKilled.ToString();
		escapesText.text = enemiesEscaped.ToString();
		shotsText.text = shotsFired.ToString();
	}
	
	void SpawnEnemies() {
		var rng = Random.Range(0, enemies.Length);

		GameObject.Instantiate(enemies[rng], enemySpawnXPosition.position, enemySpawnXPosition.rotation);
	}
}
