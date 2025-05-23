using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;


public class selectionnerPlan : MonoBehaviour
{
    [SerializeField] private GroundLockManager groundLockManager;
    [SerializeField] private string targetSceneName = "SampleScene";

    public void SavePlaneAndLoadScene()
    {
        var plane = groundLockManager.GetLockedPlane();

        if (plane == null)
        {
            Debug.LogWarning("Aucun plan verrouillé trouvé !");
            return;
        }

        // Sauvegarde des infos essentielles
        PlaneData.PlaneCenter = plane.center;
        PlaneData.PlaneRotation = plane.transform.rotation;

        var meshFilter = plane.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            var mesh = meshFilter.mesh;
            PlaneData.Vertices = mesh.vertices;
            PlaneData.Triangles = mesh.triangles;
        }

        // Changement de scène
        SceneManager.LoadScene(targetSceneName);
    }
}