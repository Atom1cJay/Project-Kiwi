using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScroller : MonoBehaviour
{

	public Vector3 scrollSpeed = Vector3.forward * 4f;
	public float cloudSkyRange = 300f;
	public Vector2 cloudHeight = new Vector2(20f, 35f);
	public float cloudDensity = 0.01f;
	public Vector2 cloudScale = new Vector2(2f, 6f);

	[Space]

	public GameObject[] cloudPrefabs;

	private Transform[] clouds;

	private void Start()
	{
		SpawnClouds();
	}

	/// Spawn initial clouds
	private void SpawnClouds()
	{
		int cloudAmount = Mathf.RoundToInt(cloudSkyRange * cloudSkyRange * 0.0004f * cloudDensity);
		clouds = new Transform[cloudAmount];
		for (int i = 0; i < cloudAmount; i++)
		{
			Vector3 spawnPos = new Vector3(
				Random.Range(-cloudSkyRange, cloudSkyRange), 
				Random.Range(cloudHeight.x, cloudHeight.y), 
				Random.Range(-cloudSkyRange, cloudSkyRange));
			clouds[i] = Instantiate(
				cloudPrefabs[Random.Range(0, cloudPrefabs.Length)], spawnPos, Quaternion.identity, transform).transform;
			clouds[i].localScale *= Random.Range(cloudScale.x, cloudScale.y);
		}
	}

	private void Update()
	{
		ScrollClouds();
	}

	/// Move them across the sky and loop back to beginning
	private void ScrollClouds()
	{
		foreach(Transform cloud in clouds)
		{
			cloud.position += scrollSpeed * Time.deltaTime;
			
			if (Mathf.Abs(cloud.position.x) > cloudSkyRange)
			{
				if (cloud.position.x > 0f)
				{
					cloud.position = new Vector3(-cloudSkyRange * 0.95f, cloud.position.y, cloud.position.z);
				}
				else
				{
					cloud.position = new Vector3(cloudSkyRange * 0.95f, cloud.position.y, cloud.position.z);
				}
			}
			if (Mathf.Abs(cloud.position.z) > cloudSkyRange)
			{
				if (cloud.position.z > 0f)
				{
					cloud.position = new Vector3(cloud.position.x, cloud.position.y, -cloudSkyRange * 0.95f);
				}
				else
				{
					cloud.position = new Vector3(cloud.position.x, cloud.position.y, cloudSkyRange * 0.95f);
				}
			}
		}
	}

}
