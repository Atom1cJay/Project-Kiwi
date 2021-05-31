using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{

	public float spawnRange;
	public float density;
	public float flowerPercentage = 0.5f;
	public float grassDrawDist = 50f;

	[Space]

	public LayerMask groundLayer;
	public GameObject grassPrefab;
	public GameObject flowerPrefab;

	private Transform[] allGrass;
	private Transform cam;
	private bool savedShown = false;

	private void Start()
	{
		cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
		SpawnGrass();
	}

	private void SpawnGrass()
	{
		int spawnCount = Mathf.RoundToInt(spawnRange * 2f * spawnRange * 2f * density);
		List<Transform> grassList = new List<Transform>();
		for (int i = 0; i < spawnCount; i++)
		{
			Vector3 spawnPos = transform.position + new Vector3(Random.Range(-spawnRange, spawnRange), 20f, Random.Range(-spawnRange, spawnRange));
			Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 35f, groundLayer);
			if (hit.collider != null)
			{
				GameObject prefab = Random.value > 1f - flowerPercentage * 0.01f ? flowerPrefab : grassPrefab;
				if (prefab == flowerPrefab)
				{
					grassList.Add(Instantiate(prefab, hit.point, Quaternion.Euler(0f, Random.value * 360f, 0f)).transform);
				}
				else
				{
					Instantiate(prefab, hit.point, Quaternion.Euler(0f, Random.value * 360f, 0f));
				}
			}
		}
		allGrass = grassList.ToArray();
	}

	private void LateUpdate()
	{
		bool showGrass = (cam.position - transform.position).magnitude < grassDrawDist;
		if (savedShown != showGrass)
		{
			savedShown = showGrass;
			foreach (Transform grass in allGrass)
			{
				grass.gameObject.SetActive(showGrass);
			}
		}
		if (showGrass)
		{
			foreach (Transform grass in allGrass)
			{
				grass.LookAt(cam, Vector3.up);
			}
		}
	}
	
}
