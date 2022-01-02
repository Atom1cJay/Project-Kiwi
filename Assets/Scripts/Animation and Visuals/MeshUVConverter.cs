using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUVConverter : MonoBehaviour
{
    [System.Serializable]
    public class StateAndMat
    {
        public String name;
        public Material mat;
    }

    public StateAndMat[] stateAndMats;
    [SerializeField] SkinnedMeshRenderer[] meshes;
    [SerializeField] Material standardMat;

    private void Awake()
    {
        if (standardMat == null)
        {
            standardMat = new Material(Shader.Find("Specular"));
        }

        foreach (SkinnedMeshRenderer mesh in meshes)
        {
            mesh.material = standardMat;
        }
    }

    public void UpdateMesh(String s)
    {
        bool foundIt = false;
        Material mat = null;
        foreach(StateAndMat sam in stateAndMats)
        {
            if (sam.name.Equals(s))
            {
                mat = sam.mat;
                foundIt = true;
                break;
            }
        }

        if (!foundIt)
        {
            mat = standardMat;
        }

        foreach (SkinnedMeshRenderer mesh in  meshes)
        {
            mesh.material = mat;
        }
    }
}
