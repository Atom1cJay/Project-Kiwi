using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectibleGroup : MonoBehaviour
{
    static Dictionary<string, int> liveCollectables = new Dictionary<string, int>();
    [SerializeField] string id;
    [SerializeField] UnityEvent onGroupCollected;
    bool isCollected; // Just in case--make sure double collect shouldn't happen (it shouldn't happen in the first place)

    [RuntimeInitializeOnLoadMethod]
    void InitStatics()
    {
        liveCollectables = new Dictionary<string, int>();
    }

    void Start()
    {
        CollectibleReader reader = GetComponent<CollectibleReader>();
        reader.onCollect.AddListener(OnCollected);
        if (!liveCollectables.ContainsKey(id))
        {
            liveCollectables[id] = 1;
        }
        else
        {
            liveCollectables[id] += 1;
        }
    }

    void OnCollected()
    {
        if (!isCollected)
        {
            isCollected = true;
            liveCollectables[id] -= 1;
        }
        if (liveCollectables[id] == 0)
        {
            onGroupCollected.Invoke();
            print("Whole group is dead");
        }
    }
}
