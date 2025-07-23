using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControllerSystem : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float transitionTime;

    // Start is called before the first frame update
    List<MusicControlPoint> controlPoints;
    
    bool transitioning = false;
    int currentController = 0;
    void Start()
    {
        controlPoints = new List<MusicControlPoint>();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("MusicControlPoint");

        foreach (GameObject g in gameObjects)
        {
            controlPoints.Add(g.GetComponent<MusicControlPoint>());
        }

        float dist = Mathf.Infinity;
        foreach (MusicControlPoint p in controlPoints)
        {
            if (Vector3.Distance(p.position, player.transform.position) < dist) {
                dist = Vector3.Distance(p.position, player.transform.position);
                currentController = controlPoints.IndexOf(p);
            }
        }

        if (controlPoints.Count > 0)
            AudioMasterController.instance.FadeInSound(controlPoints[currentController].song, transitionTime, gameObject);
    }

    private void Update()
    {
        if (!transitioning && Time.time > transitionTime + 0.01f)
        {
            float dist = Mathf.Infinity;
            int temp = 0;
            foreach (MusicControlPoint p in controlPoints)
            {
                if (Vector3.Distance(p.position, player.transform.position) < dist)
                {
                    dist = Vector3.Distance(p.position, player.transform.position);
                    temp = controlPoints.IndexOf(p);
                }
            }

            if (temp != currentController)
            {
                StartCoroutine(TransitionToNext(controlPoints[currentController].song, controlPoints[temp].song));
                currentController = temp;
            }
        }
        
    }
    
    IEnumerator TransitionToNext(string from, string to)
    {
        transitioning = true;
        AudioMasterController.instance.FadeInSound(to, transitionTime, gameObject);
        AudioMasterController.instance.FadeOutSound(from, transitionTime);
        yield return new WaitForSeconds(transitionTime + .01f);
        transitioning = false;
    }
}
