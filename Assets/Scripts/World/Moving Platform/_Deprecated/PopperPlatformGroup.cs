using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopperPlatformGroup : MonoBehaviour
{
    [SerializeField] float timeBtwnStates;
    List<PopperPlatform> popperPlatforms = new List<PopperPlatform>();

    /// <summary>
    /// Registers the given popper platforms as part of this group. Assumes
    /// no repeats.
    /// </summary>
    public void RegisterPlatform(PopperPlatform pp)
    {
        popperPlatforms.Add(pp);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        StartCoroutine(Go());
    }

    IEnumerator Go()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBtwnStates);
            foreach (PopperPlatform pp in popperPlatforms)
            {
                pp.SwitchState();
            }
        }
    }
}
