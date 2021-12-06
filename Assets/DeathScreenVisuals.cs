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

    private void Awake()
    {
        leaf.sizeDelta = new Vector2(leafStartingDimension, leafStartingDimension);
        StartCoroutine("LeafShrink");
    }

    IEnumerator LeafShrink()
    {
        while (leaf.sizeDelta.x > 0)
        {
            float shrinkAmtThisFrame = leafSizeDecreaseSpeed * Time.deltaTime;
            leaf.sizeDelta -= new Vector2(shrinkAmtThisFrame, shrinkAmtThisFrame);
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
