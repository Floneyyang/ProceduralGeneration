using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public int xSize = 20;
    public int zSize = 20;

    public float xPerlin = 0.3f;
    public float zPerlin = 0.3f;
    public float Height = 2f;

    public Gradient gradient;
    public float maxIntensity;
    public float minIntensity;

    public int BandforX = 3;
    public int BandforZ = 2;
    public int BandforHeight = 1;
    public float maxHeight;
    public float minHeight;


    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Color[] colors;

    float minMeshHeight;
    float maxMeshHeight;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        InitShape();
        UpdateMesh();
    }

    private void Update()
    {
        InitShape();
        UpdateMesh();
    }

    void InitShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        for(int i = 0, z = 0; z <= zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * Audio.audioBandBuffer[BandforX]*(maxIntensity-minIntensity) + minIntensity, z * Audio.audioBandBuffer[BandforZ] * (maxIntensity - minIntensity) + minIntensity) * Height * Audio.audioBandBuffer[BandforHeight]*(maxHeight - minHeight) + minHeight;
                vertices[i] = new Vector3(x, y, z);

                if (y > maxMeshHeight) maxMeshHeight = y;
                if (y < minMeshHeight) minMeshHeight = y;

                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for(int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {

                triangles[tris] = vert;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        colors = new Color[vertices.Length];
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minMeshHeight,maxMeshHeight,vertices[i].y);//color varies based on height
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
//Reference: https://www.youtube.com/watch?v=eJEpeUH1EMg
