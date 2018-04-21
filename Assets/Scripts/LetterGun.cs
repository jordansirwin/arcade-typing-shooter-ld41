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

	void Start() {
		letterText.text = letter;
	}

	void Update () {
		if(Input.GetKeyDown(letter.ToLower())) {
			// Debug.Log("Fire: " + letter);
			StartCoroutine(Fire());
		}
	}

	IEnumerator Fire() {
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
