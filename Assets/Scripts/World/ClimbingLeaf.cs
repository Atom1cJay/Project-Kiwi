using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingLeaf : MonoBehaviour
{
    [SerializeField] Animator leafAnimator;
    [SerializeField] Collider collider;
    [SerializeField] ClimbingLeaf[] leafsToChain;
    [SerializeField] float timeToStayUp;
    [SerializeField] float timeToChain;
    [SerializeField] float timeToWaitAtStart;
    [SerializeField] float animationLag;

    bool leafUp = false;

    public void startLeaf()
    {
        StopAllCoroutines();
        StartCoroutine(leafProcess());
    }

    IEnumerator leafProcess()
    {
        if (!leafUp)
            yield return new WaitForSeconds(timeToWaitAtStart);

        if (leafsToChain.Length > 0)
            Invoke("chainLeaves", timeToChain);

        if (!leafUp)
        {
            leafUp = true;
            leafAnimator.SetTrigger("RISE");
        }

        collider.enabled = true;

        yield return new WaitForSeconds(timeToStayUp);

        leafAnimator.SetTrigger("FALL");

        yield return new WaitForSeconds(animationLag);

        collider.enabled = false;
        leafUp = false;
    }

    void chainLeaves()
    {
        foreach (ClimbingLeaf leaf in leafsToChain)
            leaf.startLeaf();
    }

}
