using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

		PreStart();
	}
#endregion Singleton

	[System.Serializable]
	public struct LetterInfo {
		public string Letter;
		public int ASCIIOffset;
		public float AudioPitch;
		public Color Color;
	}

	public int shotsFired;
	public int enemiesHit;
	public int enemiesBonus;
	public int life;

	public int enemyEscapeGameOverCount = 1;
	public float minEnemySpeed = 50f;
	public float maxEnemySpeed = 200f;
	public float spawnInitialWait = 5f;
	public float spawnCooldown = 5f;

	public GameObject[] enemies;

	public Transform enemySpawnXPosition;
	public Transform enemySpawnMinYPosition;
	public Transform enemySpawnMaxYPosition;
	public Transform enemyEscapeXPosition;

	public AudioClip musicClip;
	public AudioClip fireShotClip;
	public AudioClip enemyMoveClip;

	public int Score { get { return (enemiesHit * 5) + (enemiesBonus * 10) - shotsFired; } }

	private bool _isGameOver = false;
	private AudioSource _musicAudioSource;
	private Dictionary<string, LetterInfo> _letterInfoCache = new Dictionary<string, LetterInfo>();
	private Dictionary<string, Color> _lettersToColors = new Dictionary<string, Color>();

	void PreStart() { 

		// from: http://vladowiki.fmf.uni-lj.si/doku.php?id=notes:rcolor
		_lettersToColors.Add("A", new Color(240f/255f, 163f/255f, 255f/255f));
		_lettersToColors.Add("B", new Color(0f/255f, 117f/255f, 220f/255f));
		_lettersToColors.Add("C", new Color(153f/255f, 63f/255f, 0f/255f));
		_lettersToColors.Add("D", new Color(76f/255f, 0f/255f, 92f/255f));
		_lettersToColors.Add("E", new Color(25f/255f, 25f/255f, 25f/255f));
		_lettersToColors.Add("F", new Color(0f/255f, 92f/255f, 49f/255f));
		_lettersToColors.Add("G", new Color(43f/255f, 206f/255f, 72f/255f));
		_lettersToColors.Add("H", new Color(255f/255f, 204f/255f, 153f/255f));
		_lettersToColors.Add("I", new Color(128f/255f, 128f/255f, 128f/255f));
		_lettersToColors.Add("J", new Color(148f/255f, 255f/255f, 141f/255f));
		_lettersToColors.Add("K", new Color(143f/255f, 124f/255f, 0f/255f));
		_lettersToColors.Add("L", new Color(157f/255f, 204f/255f, 0f/255f));
		_lettersToColors.Add("M", new Color(194f/255f, 0f/255f, 136f/255f));
		_lettersToColors.Add("N", new Color(0f/255f, 51f/255f, 128f/255f));
		_lettersToColors.Add("O", new Color(255f/255f, 164f/255f, 5f/255f));
		_lettersToColors.Add("P", new Color(255f/255f, 168f/255f, 187f/255f));
		_lettersToColors.Add("Q", new Color(66f/255f, 102f/255f, 0f/255f));
		_lettersToColors.Add("R", new Color(255f/255f, 0f/255f, 16f/255f));
		_lettersToColors.Add("S", new Color(94f/255f, 241f/255f, 242f/255f));
		_lettersToColors.Add("T", new Color(0f/255f, 153f/255f, 143f/255f));
		_lettersToColors.Add("U", new Color(224f/255f, 255f/255f, 102f/255f));
		_lettersToColors.Add("V", new Color(116f/255f, 10f/255f, 255f/255f));
		_lettersToColors.Add("W", new Color(153f/255f, 0f/255f, 0f/255f));
		_lettersToColors.Add("X", new Color(255f/255f, 255f/255f, 128f/255f));
		_lettersToColors.Add("Y", new Color(225f/255f, 225f/255f, 0f/255f));
		_lettersToColors.Add("Z", new Color(255f/255f, 80f/255f, 5f/255f));
	}

	void Start () {

		_musicAudioSource = GetComponent<AudioSource>();
		_musicAudioSource.clip = musicClip;
		_musicAudioSource.Play();
		
		InvokeRepeating("SpawnEnemies", spawnInitialWait, spawnCooldown);
	}

	void Update() {
		// TEST HACKS
		if(Input.GetKeyDown(KeyCode.Escape)) {
			SetGameOver();
		}
	}

	void SpawnEnemies() {
		// Debug.Log("Spawn");
		if(IsGameOver()) return;
		// Debug.Log("Spawning!");

		var rndLetter = ((char)Random.Range(65, 91)).ToString();
		var rndEnemy = Random.Range(0, enemies.Length);
		var rndY = Random.Range(enemySpawnMinYPosition.position.y, enemySpawnMaxYPosition.position.y);

		var position = new Vector2(enemySpawnXPosition.position.x, rndY);
		var go = GameObject.Instantiate(enemies[rndEnemy], position, enemySpawnXPosition.rotation);
		var enemyGO = go.GetComponent<Enemy>();
		enemyGO.letter = rndLetter;
	}

	public void StartGame() {
		// Debug.Log("StartGame");
		enemiesHit = 0;
		enemiesBonus = 0;
		shotsFired = 0;

		_isGameOver = false;
		SceneManager.LoadScene("Game");
	}

	public void SetGameOver() {
		// Debug.Log("SetGameOver");
		_isGameOver = true;

		SceneManager.LoadScene("GameOver");
	}

	public bool IsGameOver() {
		return _isGameOver;
	}

	public LetterInfo GetLetterInfo(string letter) {
		
		// get ascii letter, adjusted to A=0
		var ascii = (int)letter.ToCharArray()[0] - 65;

		if(!_letterInfoCache.ContainsKey(letter)) {
			float c = (ascii+1f)*10f/255f;
			_letterInfoCache.Add(letter, 
				new LetterInfo {
					Letter = letter,
					ASCIIOffset = ascii,
					// https://answers.unity.com/questions/127562/pitch-in-unity.html
					AudioPitch = Mathf.Pow(1.05946f, ascii) - 0.1f,
					Color = _lettersToColors[letter]
				});
		}

		return _letterInfoCache[letter];
	}

}
