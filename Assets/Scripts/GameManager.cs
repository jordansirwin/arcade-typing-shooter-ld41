using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public int life;

	public float spawnInitialWait = 5f;
	public float spawnCooldown = 5f;

	public GameObject[] enemies;

	public Transform enemySpawnXPosition;
	public Transform enemyEscapeXPosition;

	public int Score { get { return enemiesKilled - shotsFired; } }

	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnEnemies", spawnInitialWait, spawnCooldown);
	}
	
	void SpawnEnemies() {
		var rng = Random.Range(0, enemies.Length);

		GameObject.Instantiate(enemies[rng], enemySpawnXPosition.position, enemySpawnXPosition.rotation);
	}
}
