using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public string letter;
	public TextMesh letterText;
	public float baseSpeed = 10f;
	public ParticleSystem explosionPFXPrefab;
	public SpriteRenderer sprite;
	
	private Vector2 _destination;
	private AudioSource _moveClip;
	private float _movePitch;

	void Start() {
		var letterInfo = GameManager.Instance.GetLetterInfo(letter);
		letterText.text = letterInfo.Letter;

		_moveClip = GetComponent<AudioSource>();
		_moveClip.clip = GameManager.Instance.enemyMoveClip;
		_moveClip.pitch = letterInfo.AudioPitch;
		_moveClip.dopplerLevel = letterInfo.AudioPitch;

		sprite.color = letterInfo.Color;

		baseSpeed = baseSpeed + letterInfo.ASCIIOffset;
		_destination = new Vector2(GameManager.Instance.enemyEscapeXPosition.position.x, transform.position.y);
	}
	
	void Update () {
		if(Vector2.Distance(transform.position, _destination) < 10f) {
			Escape();
		}

		transform.position = Vector2.MoveTowards(transform.position, _destination, Time.deltaTime * baseSpeed);

		if(!_moveClip.isPlaying) {
			_moveClip.Play();
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(collider.gameObject.tag != "Bullet") return;

		Die();
	}

	void Escape() {
		GameManager.Instance.enemiesEscaped += 1;
		Destroy(this.gameObject);
	}

	void Die() {
		_moveClip.Stop();
		Destroy(this.gameObject);
		GameManager.Instance.enemiesKilled += 1;

		var pfx = GameObject.Instantiate(explosionPFXPrefab, transform.position, Quaternion.identity);
		pfx.Play();
		Destroy(pfx, pfx.main.duration);
	}
}
