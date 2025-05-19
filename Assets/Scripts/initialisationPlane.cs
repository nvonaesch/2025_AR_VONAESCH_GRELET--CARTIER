using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initialisationPlane : MonoBehaviour
{

    public Material planeMaterial; // Assigne un matériau pour voir le plane
    void Start()
    {
        GameObject newPlane = new GameObject("ReconstructedPlane");
        newPlane.transform.position = PlaneData.PlaneCenter;
        newPlane.transform.rotation = PlaneData.PlaneRotation;

        Mesh mesh = new Mesh();
        mesh.vertices = PlaneData.Vertices;
        mesh.triangles = PlaneData.Triangles;
        mesh.RecalculateNormals();

        var meshFilter = newPlane.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        var meshRenderer = newPlane.AddComponent<MeshRenderer>();
        meshRenderer.material = planeMaterial;
    }
}
