using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class MaterialFaceChanger : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] Vector3 multiplier;

    GameObject copy;

    // Start is called before the first frame update
    void Start()
    {
        copy = Instantiate(gameObject);

        Destroy(copy.GetComponent<MaterialFaceChanger>());
        copy.transform.SetParent(gameObject.transform);
        copy.transform.localPosition = Vector3.zero;
        Vector3 size = copy.transform.localScale;
        copy.transform.localScale = multiplier;
        copy.transform.localEulerAngles = Vector3.zero;
        copy.GetComponent<MeshRenderer>().material = material;
        copy.GetComponent<Collider>().enabled = false;
    }
}
