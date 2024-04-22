using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemisolidCollider : MonoBehaviour
{
    [SerializeField] Collider semisolidCollider;
    [SerializeField] float additionalHeightNeeded;
    [SerializeField] float stayActivatedBuffer; // If active, won't deactivate unless player Y distance is at least this
    CharacterController player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

    void Update()
    {
        bool playerAbove = player.bounds.min.y - additionalHeightNeeded > semisolidCollider.bounds.max.y;
        bool playerInStayActiveRange = player.bounds.min.y + stayActivatedBuffer > semisolidCollider.bounds.max.y;
        if (semisolidCollider.enabled)
        {
            semisolidCollider.enabled = playerInStayActiveRange;
        }
        else
        {
            semisolidCollider.enabled = playerAbove;
        }
    }
}