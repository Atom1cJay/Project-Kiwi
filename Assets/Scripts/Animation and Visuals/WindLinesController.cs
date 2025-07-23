using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the wind lines spawner to always be at the position of the player but not be influenced by its rotation.
/// </summary>
public class WindLinesController : MonoBehaviour
{

	private Transform anchor;

	private void Start()
	{
		if (transform.parent == null) { Destroy(gameObject); return; }
		anchor = transform.parent;
		transform.SetParent(null);
	}

	private void Update()
	{
		if (anchor == null) { return; }
		transform.position = anchor.position;
	}

}
