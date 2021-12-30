using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a (node in a) behavior tree composed of AI states.
/// </summary>
public class AITree : MonoBehaviour
{
    // Serialized Fields
    [SerializeField] AAIState node;
    [SerializeField] AITree[] nexts;

    // Accessible Fields
    public AAIState Node { get { return node; } }
    public AITree[] Nexts { get { return nexts; } }
}
