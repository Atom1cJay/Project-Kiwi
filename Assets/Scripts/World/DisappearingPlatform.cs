using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] float timeToDisappear;
    [Header("Irrelevant if disappears on Enable")] [SerializeField] float timeToRespawn;
    [SerializeField] bool disappearsOnEnable;
    Collider col;
    MeshRenderer mr;
    Color originalColor;
    bool disappearing;

    private void OnEnable()
    {
        print("enabled");
        col = GetComponent<Collider>();
        mr = GetComponent<MeshRenderer>();
        originalColor = mr.material.color;
        disappearing = false;
        col.enabled = true;
        if (disappearsOnEnable)
        {
            StartDisappear();
        }
    }

    /// Can be called by a Treadable, Poundable, etc.
    public void StartDisappear()
    {
        if (!disappearing)
        {
            disappearing = true;
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        float t = timeToDisappear;

        while (t > 0)
        {
            mr.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, t / timeToDisappear);
            t -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        mr.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        col.enabled = false;

        if (disappearsOnEnable)
        {
            EndDisappear();
            gameObject.SetActive(false); // Wait for reactivation
        }

        yield return new WaitForSeconds(timeToRespawn);

        EndDisappear();
    }

    /// <summary>
    /// Resets the object to its pre-disappearance state.
    /// </summary>
    void EndDisappear()
    {
        // General Reset
        col.enabled = true;
        mr.material.color = originalColor;
        disappearing = false;
    }
}
