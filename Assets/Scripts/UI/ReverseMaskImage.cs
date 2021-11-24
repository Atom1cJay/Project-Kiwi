using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

// To be placed IN PLACE OF the image component on a UI gameObject in order to
// reverse the effect of its mask component.
public class ReverseMaskImage : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material mat = new Material(base.materialForRendering);
            mat.SetFloat("_StencilComp", (float)CompareFunction.NotEqual);
            return mat;
        }
    }
}
