using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Executes AI behavior, according to the AI behavior tree that is given to it.
/// </summary>
public class AIEnactor : MonoBehaviour
{
    [SerializeField] AITree initTree;
    AITree currentTree;

    private void Awake()
    {
        currentTree = initTree;
    }

    private void Update()
    {
        // Consider moving to another node in the tree
        foreach (AITree potNext in currentTree.Nexts)
        {
            if (potNext.Node.ShouldTransition())
            {
                currentTree = potNext;
                break;
            }
        }
        // Enact simulation for the current tree
        currentTree.Node.AdvanceTime();
        if (currentTree.Node.ShouldDestroy())
        {
            print("Death");
        }
        print("Vel: currentTree.Node.GetVelocity()");
    }
}
