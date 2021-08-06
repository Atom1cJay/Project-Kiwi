using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] float timeToDisappear, timeToRespawn;
    Collider col;
    MeshRenderer mr;
    Color color;
    bool playerOnPlatform, disappearing;
    float respawnTime;

    private void Start()
    {
        col = GetComponent<Collider>();
        mr = GetComponent<MeshRenderer>();
        color = mr.material.color;
        disappearing = false;
        col.enabled = true;
        respawnTime = 0f;
    }

    void Update()
    {
        playerOnPlatform = transform.parent.Find("Player") != null;
        if (playerOnPlatform && !disappearing)
        {
            disappearing = true;
            StartCoroutine("StartDisappear");
        }

    }

    IEnumerator StartDisappear()
    {
        Color c;

        for (int i = 100; i > 0; i--)
        {
            c = new Color(color.r, color.g, color.b, i / 100f);
            mr.material.color = c;
            yield return new WaitForSeconds(timeToDisappear / 100f);
        }

        c = new Color(color.r, color.g, color.b, 0f);
        mr.material.color = c;

        col.enabled = false;

        yield return new WaitForSeconds(timeToRespawn);

        col.enabled = true;
        mr.material.color = color;
        disappearing = false;
    }

}
