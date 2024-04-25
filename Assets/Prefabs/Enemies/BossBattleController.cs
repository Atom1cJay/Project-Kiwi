using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleController : MonoBehaviour
{
    [SerializeField] GameObject cloudObject;
    [SerializeField] GameObject[] getUpTornados;
    [SerializeField] GameObject[] gameObjectsToDestroy;
    [SerializeField] Color colorToMakeClouds;
    [SerializeField] float durationToLerpCloudAlphas;
    [SerializeField] float durationToFadeOutTornados;
    [SerializeField] string normalSong, bossSong;
    [SerializeField] NewMusicControlSystem musicSystem;

    bool inTrigger = false;
    bool bossBeaten = false;

    private void Start()
    {
        musicSystem.startSong(normalSong);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && cloudObject != null)
        {
            cloudObject.SetActive(true);
        }

        if (other.CompareTag("Player"))
        {
            if (!inTrigger)
                musicSystem.fadeToNextSong(normalSong, bossSong);
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && cloudObject != null)
        {
            cloudObject.SetActive(true);
        }

        if (other.CompareTag("Player") && !bossBeaten)
        {
            if (inTrigger)
                musicSystem.fadeToNextSong(bossSong, normalSong);
            inTrigger = false;
        }
    }

    public void endBossBattle()
    {
        bossBeaten = true;
        musicSystem.fadeToNextSong(bossSong, normalSong);

        foreach (GameObject g in getUpTornados)
        {
            StartCoroutine(fadeOutTornado(g.GetComponent<MeshRenderer>()));
        }

        foreach (GameObject g in gameObjectsToDestroy)
        {
            Destroy(g);
        }

    }

    IEnumerator fadeOutTornado(MeshRenderer mr)
    {
        float t = Time.time;
        float currentDisolve = mr.material.GetFloat("_Dissolve");

        while (Time.time - t < durationToFadeOutTornados)
        {
            mr.material.SetFloat("_Dissolve", Mathf.Lerp(currentDisolve, 1, (Time.time - t) / durationToFadeOutTornados));
            yield return null;
        }

        Destroy(mr.gameObject);
    }

    public void lerpCloudAlpha(GameObject cloudToFadeOut, GameObject cloudToFadeIn)
    {
        StartCoroutine(cloudAlphaLerps(cloudToFadeOut, cloudToFadeIn));
    }

    IEnumerator cloudAlphaLerps(GameObject cloudToFadeOut, GameObject cloudToFadeIn)
    {

        Debug.Log("Starting up");
        float t = Time.time;

        MeshRenderer fadeInMesh = null;
        MeshRenderer fadeOutMesh = cloudToFadeOut.GetComponent<MeshRenderer>();

        if (cloudToFadeIn != null)
        {
            fadeInMesh = cloudToFadeIn.GetComponent<MeshRenderer>();
            cloudToFadeIn.SetActive(true);
            fadeInMesh.material.SetFloat("_Alpha", 0f);
        }


        while (Time.time - t < durationToLerpCloudAlphas)
        {
            float pct = (Time.time - t) / durationToLerpCloudAlphas;

            if (cloudToFadeIn != null)
                fadeInMesh.material.SetFloat("_Alpha", pct);


            fadeOutMesh.material.SetFloat("_Alpha", 1f - pct);

            //Debug.Log("Fading out " + (Time.time - t));
            yield return null;
        }

        if (cloudToFadeIn != null)
        {
            fadeInMesh.material.SetFloat("_Alpha", 1f);
        }

        cloudToFadeOut.SetActive(false);

    }
}
