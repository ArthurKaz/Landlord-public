using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCamera : MonoBehaviour
{
    [SerializeField]
    private Material source;
    [SerializeField]
    private Camera View;
    void Start()
    {
        if(source!=null) {
            Material material = new Material(source);
            GetComponent<MeshRenderer>().material = source;
        }
        View.targetTexture = new RenderTexture(256, 256, 24);
        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = View.targetTexture;
    }

    void Update()
    {
        GetComponent<MeshRenderer>().material.mainTexture = View.targetTexture;
    }
}
