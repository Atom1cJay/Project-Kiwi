using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectibleGroup : MonoBehaviour
{
    [SerializeField] CollectibleReader[] collectables;
    [SerializeField] UnityEvent<int, int> onCollected; // arg1 = num collected so far, arg2 = total in group
    [SerializeField] UnityEvent onGroupCollected;
    int remaining;

    void Start()
    {
        remaining = collectables.Length;
        foreach (CollectibleReader reader in collectables)
        {
            reader.onCollect.AddListener(OnCollected);
        }
    }

    void OnCollected()
    {
        remaining -= 1;
        onCollected.Invoke(collectables.Length - remaining, collectables.Length);
        if (remaining == 0)
        {
            onGroupCollected.Invoke();
            print("Whole group is dead");
        }
    }
}
