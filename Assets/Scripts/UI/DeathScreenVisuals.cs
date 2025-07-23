using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScreenVisuals : MonoBehaviour
{
    [SerializeField] RectTransform leaf;
    [SerializeField] float leafStartingDimension; // For X and Y
    [SerializeField] float leafSizeDecreaseSpeed;
    [SerializeField] bool isDeath = true;

    public void gotoScene(int index)
    {
        leaf.gameObject.SetActive(true);
        leaf.sizeDelta = new Vector2(leafStartingDimension, leafStartingDimension);
        StartCoroutine("LeafShrink", index);
    }

    private void Awake()
    {
        if (isDeath)
        {
            leaf.sizeDelta = new Vector2(leafStartingDimension, leafStartingDimension);
            StartCoroutine("LeafShrink", SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator LeafShrink(int scene)
    {
        float t = Time.realtimeSinceStartup;
        while (leaf.sizeDelta.x > 0)
        {
            float timeThisFrame = Time.realtimeSinceStartup - t;
            float shrinkAmtThisFrame = leafSizeDecreaseSpeed * timeThisFrame;
            t = Time.realtimeSinceStartup;
            //print(leafSizeDecreaseSpeed);
            leaf.sizeDelta -= new Vector2(shrinkAmtThisFrame, shrinkAmtThisFrame);
            //print(leaf.sizeDelta);
            yield return null;
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);
    }
}
