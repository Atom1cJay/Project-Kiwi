using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Can be used to make the camera pan to a specific position on request.
/// For instance, it can be used in conjunction with a Poundable so that
/// when something is pounded, the camera pans to a certain position.
/// </summary>
public class CameraPanActivator : MonoBehaviour
{
    [SerializeField] Vector3 pos;
    [SerializeField] float enterTime;
    [SerializeField] float stayTime;
    [SerializeField] float exitTime;
    [SerializeField] CameraControl cc;

    public void Activate()
    {
        cc.MoveTo(pos, enterTime, stayTime, exitTime);
    }
}
