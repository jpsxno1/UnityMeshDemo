using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    /// <summary>
    /// 发生变形的mesh
    /// </summary>
    Mesh deformingMesh;

    /// <summary>
    /// 原始顶点位置数组
    /// </summary>
    Vector3[] originalVertices;

    /// <summary>
    ///发生位移的顶点位置数组
    /// </summary>
    Vector3[] displacedVertices;

    /// <summary>
    /// 弹力
    /// </summary>
    public float springForce = 20f;

    /// <summary>
    /// 阻尼系数
    /// </summary>
    public float damping = 5f;

    /// <summary>
    /// 每个顶点的速度数组
    /// </summary>
    Vector3[] vertexVelocities;

    /// <summary>
    /// 统一尺度
    /// </summary>
    float uniformScale = 1f;

    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }
        vertexVelocities = new Vector3[originalVertices.Length];
    }

    /// <summary>
    /// 增加变形力
    /// </summary>
    /// <param name="point">接触点坐标</param>
    /// <param name="force">力</param>
    public void AddDeformingForce(Vector3 point,float force) 
    {
        point = transform.InverseTransformPoint(point);
        for (int i=0;i<displacedVertices.Length;i++) 
        {
            AddForceToVertex(i,point,force);
        }
        //Debug.DrawLine(Camera.main.transform.position,point);
    }

    /// <summary>
    /// 将力转换为速度
    /// </summary>
    /// <param name="i"></param>
    /// <param name="point"></param>
    /// <param name="force"></param>
    void AddForceToVertex(int i,Vector3 point,float force) 
    {
        Vector3 pointToVertex = displacedVertices[i] - point;
        float attenuatedForce = force / (1f+pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }

    private void Update()
    {
        uniformScale = transform.localScale.x;
        for (int i=0;i<displacedVertices.Length;i++) 
        {
            UpdateVertex(i);
        }
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }

    /// <summary>
    /// 更新顶点数据
    /// </summary>
    /// <param name="i"></param>
    void UpdateVertex(int i) 
    {
        Vector3 velocity = vertexVelocities[i];
        //设置弹力
        Vector3 displacement = displacedVertices[i] - originalVertices[i];
        displacement *= uniformScale;
        velocity -= displacement * springForce * Time.deltaTime;

        //设置阻尼系数
        velocity *= 1f - damping * Time.deltaTime;

        vertexVelocities[i] = velocity;

        displacedVertices[i] += velocity * (Time.deltaTime/uniformScale);
    }
}
