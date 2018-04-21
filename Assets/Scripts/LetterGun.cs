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

	private AudioSource _fireAudioSource;
	private float _firePitch;

	void Start() {
		_fireAudioSource = GetComponent<AudioSource>();
		_fireAudioSource.clip = GameManager.Instance.fireShotClip;

		// get ascii value
		var ascii = (int)letter.ToCharArray()[0] - 65;
		// adjust pitch based on this: 
		// https://answers.unity.com/questions/127562/pitch-in-unity.html
		_firePitch = Mathf.Pow(1.05946f, ascii);

		letterText.text = letter;
	}

	void Update () {
		if(Input.GetKeyDown(letter.ToLower())) {
			// Debug.Log("Fire: " + letter);
			StartCoroutine(Fire());
			GameManager.Instance.shotsFired += 1;
		}
	}

	IEnumerator Fire() {
		_fireAudioSource.pitch = _firePitch;
		_fireAudioSource.Play();
		var go = GameObject.Instantiate(firePrefab, fireSource.position, fireSource.rotation);
		
		while(maxDistanceFireGoes - go.transform.position.y > 10f) {
			go.transform.position = Vector2.MoveTowards(go.transform.position, transform.position + (Vector3.up * maxDistanceFireGoes), Time.deltaTime * fireSpeed);
			yield return null;
		}

		Destroy(go);
		// Debug.Log("Destroying GO");
		yield break;
	}
}
