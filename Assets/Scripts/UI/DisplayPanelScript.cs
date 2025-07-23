using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisplayPanelScript : MonoBehaviour
{

    [Serializable]
    public struct PanelDelay
    {
        public Sprite image;
    }

    [SerializeField] float startLoadTime, fadeInTime, readingTime, fadeOutTime, pauseBetweenPanels, endTime;
    [SerializeField] string nextSceneName;
    Image imageLoader;

    [SerializeField] List<PanelDelay> panels;
    // Start is called before the first frame update
    void Start()
    {
        imageLoader = GetComponent<Image>();

        imageLoader.color = new Color(1f, 1f, 1f, 0f);
        StartCoroutine(ReadPanels());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ReadPanels()
    {

        yield return new WaitForSeconds(startLoadTime);

        foreach (PanelDelay pd in panels)
        {
            imageLoader.sprite = pd.image;
            imageLoader.color = new Color(1f, 1f, 1f, 0f);

            while (imageLoader.color.a < 1f)
            {
                float a = imageLoader.color.a;
                imageLoader.color = new Color(1f, 1f, 1f, a + Time.deltaTime / fadeInTime);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            yield return new WaitForSeconds(readingTime);

            while (imageLoader.color.a > 0f)
            {
                float a = imageLoader.color.a;
                imageLoader.color = new Color(1f, 1f, 1f, a - Time.deltaTime / fadeOutTime);
                yield return new WaitForSeconds(Time.deltaTime);
            }

            if (!pd.Equals(panels[0]))
                yield return new WaitForSeconds(pauseBetweenPanels);
            
        }

        yield return new WaitForSeconds(endTime);
        SceneManager.LoadScene(nextSceneName);
    }
}
