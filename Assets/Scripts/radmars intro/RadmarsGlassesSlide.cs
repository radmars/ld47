﻿using UnityEngine;

public class RadmarsGlassesSlide : MonoBehaviour
{

	private SpriteRenderer spriteRenderer;
	public Sprite[] glasses;
	int counter = 0;
	float initialy;

	void Start()
	{
		initialy = transform.position.y;
		spriteRenderer = GetComponent<SpriteRenderer>();
		SetActive(0);
	}

	void Update()
	{
		transform.position = new Vector3(
			transform.position.x,
			Mathf.Clamp((1 - Time.fixedTime / 1.6f) * initialy, 0, initialy),
			transform.position.z
		);
	}

	private void FixedUpdate()
	{
		if (this.counter < 100) SetActive(0);
		else if (this.counter < 105) SetActive(1);
		else if (this.counter < 110) SetActive(2);
		else if (this.counter < 115) SetActive(3);
		else SetActive(0);
		counter++;
	}

	void SetActive(int i)
	{
		var x = glasses[i];
		spriteRenderer.sprite = x;
	}
}
