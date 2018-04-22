using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGun : MonoBehaviour {

	public string letter;
	public TextMesh letterText;
	public GameObject firePrefab;

	public Transform fireSource;
	public float maxDistanceFireGoes = 600f;
	public float fireSpeed = 300f;

	public AudioSource fireAudioSource;
	public SpriteRenderer sprite;
	public LineRenderer stripe;

	public GameManager.LetterInfo letterInfo;

	void Start() {

		letterInfo = GameManager.Instance.GetLetterInfo(letter);
		letterText.text = letterInfo.Letter;
		letterText.color = letterInfo.LetterColor;

		// Debug.Log("LetterGun Color for " + letter + " is " + letterInfo.LetterColor);

		fireAudioSource.clip = GameManager.Instance.fireShotClip;
		fireAudioSource.pitch = letterInfo.AudioPitch;

		sprite.color = letterInfo.Color;

		var stripColor = new Color(letterInfo.Color.r, letterInfo.Color.g, letterInfo.Color.b, 0.25f);
		stripe.startColor = stripColor;
		stripe.endColor = stripColor;
	}

	void Update () {
		if(GameManager.Instance.IsGameOver()) return;

		if(Input.GetKeyDown(letter.ToLower())) {
			// Debug.Log("Fire: " + letter);
			StartCoroutine(Fire());
			GameManager.Instance.shotsFired += 1;
		}
	}

	IEnumerator Fire() {
		// _fireAudioSource.pitch = _firePitch;
		fireAudioSource.Play();
		var go = GameObject.Instantiate(firePrefab, fireSource.position, fireSource.rotation);
		go.GetComponent<Bullet>().letter = letter;

		while(maxDistanceFireGoes - go.transform.position.y > 10f) {
			go.transform.position = Vector2.MoveTowards(go.transform.position, transform.position + (Vector3.up * maxDistanceFireGoes), Time.deltaTime * fireSpeed);
			yield return null;
		}

		Destroy(go);
		// Debug.Log("Destroying GO");
		yield break;
	}
}
