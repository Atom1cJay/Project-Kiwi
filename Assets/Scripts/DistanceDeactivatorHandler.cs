using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Master script for handling the state of each DistanceDeactivator
/// </summary>
public class DistanceDeactivatorHandler : MonoBehaviour
{
    public static DistanceDeactivatorHandler instance;
    [SerializeField] int objsToCheckPerFrame;
    List<DistanceDeactivator> deactivators = new List<DistanceDeactivator>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Only one DistanceDeactivatorHandler should exist.");
        }
        instance = this;
    }

    public void Register(DistanceDeactivator deactivator)
    {
        deactivators.Add(deactivator);
    }

    void Start()
    {
        StartCoroutine(ManageDeactivation());
    }

    IEnumerator ManageDeactivation()
    {
        yield return new WaitForSeconds(1f); // To allow registration time
        print(deactivators.Count);
        int objsCheckedThisFrame = 0;

        while (true)
        {
            foreach(DistanceDeactivator dd in deactivators)
            {
                dd.HandleDistance(transform.position);
                objsCheckedThisFrame++;
                if (objsCheckedThisFrame >= objsToCheckPerFrame)
                {
                    yield return new WaitForEndOfFrame();
                    objsCheckedThisFrame = 0;
                }
            }
            yield return null;
        }
    }
}
