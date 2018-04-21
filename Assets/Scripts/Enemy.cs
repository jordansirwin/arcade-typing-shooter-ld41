﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public float speed = 100f;

	private Vector2 _destination;

	void Start() {
		_destination = new Vector2(GameManager.Instance.enemyEscapeXPosition.position.x, transform.position.y);
	}
	
	void Update () {
		if(Vector2.Distance(transform.position, _destination) < 10f) {
			Escape();
		}

		transform.position = Vector2.MoveTowards(transform.position, _destination, Time.deltaTime * speed);
	}

	void Escape() {
		GameManager.Instance.enemiesKilled += 1;
		Destroy(this.gameObject);
	}
}