using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    CheckpointLoader cL;
    [SerializeField] GameObject[] rings;
    [SerializeField] ParticleSystem[] PS;
    [SerializeField] Rotator[] rotators;

    void Awake()
    {
        SetInactive();

    }

    private void Start()
    {

        cL = CheckpointLoader.Instance;
    }

    //Set checkpoint to inactive
    public void SetInactive()
    {
        StartCoroutine(SetInactiveScaling());

        foreach (ParticleSystem ps in PS)
        {
            ps.Stop();
        }

        foreach (Rotator rot in rotators)
        {
            rot.enabled = false;
        }
    
    }

    //Set checkpoint to active
    public void SetActive()
    {
        StartCoroutine(SetActiveScaling());

        foreach (ParticleSystem ps in PS)
        {
            ps.Play();
        }

        foreach (Rotator rot in rotators)
        {
            rot.enabled = true;
        }
    }

    IEnumerator SetActiveScaling()
    {
        Debug.Log("Set Active!");
        GameObject ring = rings[0];

        while (ring.transform.localScale.magnitude < 1.732f)
        {

            foreach (GameObject g in rings)
            {
                g.transform.localScale = Vector3.Lerp(g.transform.localScale, Vector3.one, Time.deltaTime);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator SetInactiveScaling()
    {
        Debug.Log("Setawd Active!");
        GameObject ring = rings[0];

        while (ring.transform.localScale.magnitude > 1.311f)
        {

            foreach (GameObject g in rings)
            {
                g.transform.localScale = Vector3.Lerp(g.transform.localScale, new Vector3(0.6f, 1f, 0.6f), Time.deltaTime);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    //get position of checkpoint
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 9)
        {
            cL.SetCheckpoint(this);
        }
    }
}
