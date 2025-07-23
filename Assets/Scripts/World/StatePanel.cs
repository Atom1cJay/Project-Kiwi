using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a single State Panel, part of a StatePanelGroup. State Panels
/// are either walkable or damaging, and they alternate periodically at a
/// pace determined by their group.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Damager))]
public class StatePanel : MonoBehaviour
{
    [SerializeField] StatePanelGroup group;
    [SerializeField] bool walkable;
    MeshRenderer mr;
    Damager damager;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        damager = GetComponent<Damager>();
        group.RegisterPanel(this);
    }

    public void AlternateState()
    {
        walkable = !walkable;
        UpdateToAlternatedState();
    }

    void UpdateToAlternatedState()
    {
        Color c = walkable ? Color.blue : Color.yellow;
        damager.SetActivated(!walkable);
        mr.material.color = c;
    }
}
