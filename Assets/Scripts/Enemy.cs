﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public string letter;
	public TextMesh letterText;
	public float baseSpeed = 10f;
	public ParticleSystem explosionPFXPrefab;
	public ParticleSystem bonusExplosionPFXPrefab;
	public SpriteRenderer sprite;
	
	private Vector2 _destination;
	private AudioSource _moveClip;
	private float _movePitch;
	private GameManager.LetterInfo letterInfo;

	void Start() {
		letterInfo = GameManager.Instance.GetLetterInfo(letter);
		letter = letterInfo.Letter;
		letterText.text = letterInfo.Letter;
		letterText.color = letterInfo.LetterColor;

		_moveClip = GetComponent<AudioSource>();
		_moveClip.clip = GameManager.Instance.enemyMoveClip;
		_moveClip.pitch = letterInfo.AudioPitch;

		sprite.color = letterInfo.Color;

		baseSpeed = baseSpeed + letterInfo.ASCIIOffset;
		_destination = new Vector2(GameManager.Instance.enemyEscapeXPosition.position.x, transform.position.y);
	}
	
	void Update () {
		// if game is over don't do anything
		if(CheckIfGameOver()) return;

		if(Vector2.Distance(transform.position, _destination) < 10f) {
			Escape();
		}

		transform.position = Vector2.MoveTowards(transform.position, _destination, Time.deltaTime * baseSpeed);

		if(!_moveClip.isPlaying) {
			_moveClip.Play();
		}
	}

	bool CheckIfGameOver() {
		if(!GameManager.Instance.IsGameOver()) return false;

		Die(false, false);
		return true;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if(GameManager.Instance.IsGameOver()) return;

		if(collider.gameObject.tag != "Bullet") return;

		var bonus = collider.gameObject.GetComponent<Bullet>().letter == letter;
		Die(!bonus, bonus);
	}

	void Escape() {
		// Debug.Log("Escape");
		Die(false, false);
		GameManager.Instance.SetGameOver();
	}

	void Die(bool awardNormalPoints, bool awardBonusPoints) {

		if(awardNormalPoints) {
			GameManager.Instance.enemiesHit += 1;
		}
		if(awardBonusPoints) {
			GameManager.Instance.enemiesBonus += 1;
		}
		
		_moveClip.Stop();

		ParticleSystem pfx;
		if(awardBonusPoints)
			pfx = GameObject.Instantiate(bonusExplosionPFXPrefab, transform.position, Quaternion.identity);
		else 
			pfx = GameObject.Instantiate(explosionPFXPrefab, transform.position, Quaternion.identity);

		var audio = pfx.GetComponent<AudioSource>();
		audio.clip = GameManager.Instance.enemyExplosionClip;
		audio.pitch = awardBonusPoints ? letterInfo.AudioPitch/2 : letterInfo.AudioPitch;
		audio.Play();

		pfx.Play();
		Destroy(pfx.gameObject, pfx.main.duration);

		Destroy(this.gameObject);
	}
}
