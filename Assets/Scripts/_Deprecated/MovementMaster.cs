using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MovementInfo))]
public class MovementMaster : MonoBehaviour
{
    MovementInfo mi;

    void Start()
    {
        mi = GetComponent<MovementInfo>();
    }

    private void FixedUpdate()
    {
        if (!mi.TouchingGround())
        {
            transform.SetParent(null, true);
        }
        else
        {
            GameObject ground = mi.GetGroundDetector().CollidingWith();
            Transform parent =
                ground.CompareTag("Moving Platform (Has Wrapper)") ?
                ground.transform.parent : null;
            transform.SetParent(parent);
        }
    }
}
