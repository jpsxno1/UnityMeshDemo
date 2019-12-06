using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshDeformerInput:MonoBehaviour
{
    public float force = 10f;

    /// <summary>
    /// 力的偏移
    /// </summary>
    public float forceOffset = 0.1f;

    private void Update()
    {
        if (Input.GetMouseButton(0)) 
        {
            HandleInput();
        }
    }

    void HandleInput() 
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay,out hit)) 
        {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer) 
            {
                Vector3 point = hit.point;
                point += hit.normal * forceOffset;
                deformer.AddDeformingForce(point,force);
            }
        }
    }
}

