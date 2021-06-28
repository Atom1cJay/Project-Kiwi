using UnityEngine;
using System.Collections;

public class MonobehaviourUtils : MonoBehaviour
{
    private static MonobehaviourUtils _instance;

    public static MonobehaviourUtils Instance
    {
        get
        {
            if (!_instance)
                Debug.LogError("No instance exists for MonobehaviorUtils.");
            return _instance;
        }
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
    }

    public IEnumerator ExecuteCoroutine(IEnumerator cor)
    {
        while (cor.MoveNext())
            yield return cor.Current;
    }
}
