using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldPosFollower : MonoBehaviour
{
    [SerializeField] Transform toFollow;
    Camera cam;
    RectTransform myTransform;

    private void Awake()
    {
        cam = Camera.main;
        myTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        myTransform.position = cam.WorldToScreenPoint(toFollow.position);
    }
}
