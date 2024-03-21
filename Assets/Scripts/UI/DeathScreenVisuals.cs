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
        while (leaf.sizeDelta.x > 0)
        {
            float shrinkAmtThisFrame = leafSizeDecreaseSpeed * Time.deltaTime;
            //print(leafSizeDecreaseSpeed);
            leaf.sizeDelta -= new Vector2(shrinkAmtThisFrame, shrinkAmtThisFrame);
            //print(leaf.sizeDelta);
            yield return null;
        }
        SceneManager.LoadScene(scene);
    }
}
