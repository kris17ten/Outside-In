using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;

public class PortalManager : MonoBehaviour
{
    public GameObject MainCamera;

    public GameObject Lab;

    private Material[] labMaterials;

    // Start is called before the first frame update
    void Start()
    {
        labMaterials = Lab.GetComponent<Renderer>().sharedMaterials;
        for (int i = 0; i < labMaterials.Length; ++i)
        {
            labMaterials[i].SetInt("_StencilTest", (int)CompareFunction.Equal);
        }
    }

    // Update is called once per frame
    void OnTriggerStay(Collider collider)
    {
        Vector3 camPositionInPortal = transform.InverseTransformPoint(MainCamera.transform.position);
        Debug.Log("Y Pos: " + camPositionInPortal.y);

        if(camPositionInPortal.y < 0.5f)
        {
            for(int i=0; i<labMaterials.Length; ++i)
            {
                labMaterials[i].SetInt("_StencilTest", (int)CompareFunction.NotEqual);
            }
        }
        else
        {
            for (int i = 0; i < labMaterials.Length; ++i)
            {
                labMaterials[i].SetInt("_StencilTest", (int)CompareFunction.Equal);
            }
        }
    }
}
