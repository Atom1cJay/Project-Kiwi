using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassController : MonoBehaviour
{

	public Sprite[] sprites;
	public SpriteRenderer sr;

	//private Transform cam;
	private float swaySpeed;
	private float swayStrength;

	private void Start()
	{
		swaySpeed = Random.Range(1f, 5f);
		swayStrength = Random.Range(10f, 20f);
		//cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
		sr.sprite = sprites[Random.Range(0, sprites.Length)];
		transform.localScale = transform.localScale * Random.Range(1f, 2f);
	}

	
	private void LateUpdate()
	{
		//if (Random.value > 0.3f) { return; }
		float angle = Mathf.Sin(Time.time * swaySpeed) * swayStrength;
		transform.localRotation = Quaternion.Euler(0f, 0f, angle);

		/*
		Vector3 lookVector = new Vector3(cam.forward.x, transform.forward.y, cam.forward.z);
		if (!lookVector.Equals(Vector3.zero)) { transform.forward = lookVector; }
		

		if (Random.value > 0.3f) { return; }
		transform.LookAt(cam, Vector3.up);
		*/
	}
	
}
