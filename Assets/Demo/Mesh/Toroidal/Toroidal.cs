using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Toroidal : MonoBehaviour {

    public int radius;
    public int offset;
    public int height;
    public int lack;
    private Mesh mesh;
    Vector3[] vertices;
    Vector3[] normals;
    Color32[] colors;
    private void OnEnable()
    {
        mesh = new Mesh();
        CreateVerticels();
        CreateTriagnle();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    private void CreateVerticels()
    {
        if (radius * 8 - lack <= 0) return;

        int count = (8 * radius - lack) * 4;
        vertices = new Vector3[count];
        normals = new Vector3[count];
        colors = new Color32[count];
        int v = 0;
        int size = radius;
        int i = 0;

        if (lack <= radius * 6)
        {
            for (int x = 0; x < radius * 2; ++x)
            {
                Mapping(v++, x, 0, 0, radius, size, 0);
                ++i;
            }
            if(lack <= radius * 4)
            {
                for (int z = 0; z < radius * 2; ++z)
                {
                    Mapping(v++, radius * 2, 0, z, radius, size, 0);
                    ++i;
                }
                if(lack <= radius * 2)
                {
                    for (int x = radius * 2; x >  0; --x)
                    {
                        Mapping(v++, x, 0, radius * 2, radius, size, 0);
                        ++i;
                    }
                    for (int z = radius * 2; z > lack; --z)
                    {
                        Mapping(v++, 0, 0, z, radius, size, 0);
                        ++i;
                    }
                }
                else
                {
                    for (int x = radius * 2; x > lack -  radius * 2; --x)
                    {
                        Mapping(v++, x, 0, radius * 2, radius, size, 0);
                        ++i;
                    }
                }
            }
            else
            {
                for (int z = 0; z < radius * 6 - lack; ++z)
                {
                    Mapping(v++, radius * 2, 0, z, radius, size, 0);
                    ++i;
                }
            }
        }
        else
        {
            for (int x = 0; x < radius * 8 - lack; ++x)
            {
                Mapping(v++, x, 0, 0, radius, size, 0);
                ++i;
            }
        }
 
        for (int x = 0; x < i; ++x)
        {
            normals[v] = (normals[x] + Vector3.up * height).normalized;
            vertices[v] = vertices[x] + Vector3.up * height;
            colors[v] = new Color32(colors[x].r, (byte)height, colors[x].b, 1);
            ++v;
        }

        size = radius - offset;
        int start = v;
        i = v;
        if (lack <= radius * 6)
        {
            for (int x = 0; x < radius * 2; ++x)
            {
                Mapping(v++, x, 0, 0, radius, size, 0);
                ++i;
            }
            if (lack <= radius * 4)
            {
                for (int z = 0; z < radius * 2; ++z)
       
