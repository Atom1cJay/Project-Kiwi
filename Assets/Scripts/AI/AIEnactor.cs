using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Executes AI behavior, according to the AI behavior tree that is given to it.
/// </summary>
public class AIEnactor : MonoBehaviour
{
    [SerializeField] AITree initTree; // The default state to be in (shouldTransition() does not have to be true for it)
    [SerializeField] MeshUVConverter[] converters;
    [SerializeField] NavMeshAgent navMeshAgent;
    AITree currentTree;

    private void Awake()
    {
        currentTree = initTree;
        currentTree.Node.RegisterAsState();
    }

    private void Update()
    {
        // Consider moving to another node in the tree
        if (currentTree.Node.ShouldFinishState())
        {
            bool selectedNewState = false;
            // Inspect all of the next states (in order) to see if they are appropriate to change to
            foreach (AITree potNext in currentTree.Nexts)
            {
                if (potNext.Node.ShouldBeginState())
                {
                    // Change to the state contained by potNext
                    print("MOVE CHANGED TO " + potNext.Node);

                    //update mesh(s)
                    foreach (MeshUVConverter converter in converters)
                    {
                        converter.UpdateMesh(potNext.Node.GetAnimationID());
                    }

                    selectedNewState = true;
                    currentTree = potNext;
                    potNext.Node.RegisterAsState();
                    break;
                }
            }
            if (!selectedNewState)
            {
                Debug.LogError("AI state ended, but can't find any new AI states to transition to.");
            }
        }
        // Enact simulation for the current tree
        currentTree.Node.AdvanceTime();
        navMeshAgent.SetDestination(currentTree.Node.GetGoalPos());
        navMeshAgent.speed = currentTree.Node.GetSpeed();
        Quaternion rotGoal = Quaternion.Euler(0, currentTree.Node.GetRotation().x, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotGoal, currentTree.Node.GetRotation().y * Time.deltaTime);
    }

    public void Kill()
    {
        Debug.Log("Death");
        Destroy(gameObject);
    }
}
