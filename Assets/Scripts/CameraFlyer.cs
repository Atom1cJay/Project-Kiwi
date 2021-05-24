using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlyer : MonoBehaviour
{

	public float sensitivity = 75f;
	public float moveSpeed = 10f;
	public float drag = 0.995f;

	private Vector2 lookRot = Vector2.zero;
	private Vector3 velocity = Vector3.zero;

	void Start()
	{

	}

	private void Update()
	{
		MouseLook();
		WASDMove();
	}

	private void MouseLook()
	{
		Cursor.lockState = CursorLockMode.Locked;

		Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * sensitivity * 0.01f;

		lookRot.x -= mouseInput.y;
		lookRot.x = Mathf.Clamp(lookRot.x, -89f, 89f);
		lookRot.y += mouseInput.x;

		transform.rotation = Quaternion.Euler(lookRot.x, lookRot.y, 0f);
	}

	private void WASDMove()
	{
		Quaternion facingDir = Quaternion.Euler(0f, lookRot.y, 0f);
		Vector3 moveDir = Vector3.zero;
		if (Input.GetKey(KeyCode.W))
		{
			moveDir += Vector3.forward;
		}
		if (Input.GetKey(KeyCode.A))
		{
			moveDir -= Vector3.right;
		}
		if (Input.GetKey(KeyCode.S))
		{
			moveDir -= Vector3.forward;
		}
		if (Input.GetKey(KeyCode.D))
		{
			moveDir += Vector3.right;
		}
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.E))
		{
			moveDir += Vector3.up;
		}
		if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftShift))
		{
			moveDir -= Vector3.up;
		}

		if (Input.GetMouseButton(0))
		{
			moveDir *= 4f;
		}

		if (moveDir != Vector3.zero)
		{
			moveDir = facingDir * moveDir * moveSpeed * Time.deltaTime * 0.1f;
			velocity += moveDir;

		}
		velocity *= (1.0f - drag * Time.deltaTime);
		transform.position += velocity;
	}

}
