using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Attack : ScriptableObject
{
    [SerializeField] string debugName;
    [SerializeField] GameObject hitbox;
    [SerializeField] float duration; // -1 for no limit
    GameObject activeHitbox;

    /// <summary>
    /// Spawns the appropriate hitbox for the appropriate amount of time under
    /// the given transform.
    /// </summary>
    public void EnableAttack(Transform parent)
    {
        activeHitbox = Instantiate(hitbox, parent);
        if (duration != -1)
        {
            MonobehaviourUtils.Instance.StartCoroutine("ExecuteCoroutine", WaitForDurationEnd());
        }
    }

    /// <summary>
    /// Waits for the span of the attack to conclude, and then ends the attack.
    /// </summary>
    IEnumerator WaitForDurationEnd()
    {
        yield return new WaitForSeconds(duration);
        DisableAttack();
    }

    /// <summary>
    /// Destroys the hitbox, ending the attack.
    /// </summary>
    public void DisableAttack()
    {
        Destroy(activeHitbox.gameObject);
    }
}
