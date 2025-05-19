using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;


public class selectionnerPlan : MonoBehaviour
{
    [SerializeField] private GroundLockManager groundLockManager;
    [SerializeField] private string targetSceneName = "SampleScene";

    public void OnFinalizeButtonClicked()
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

        // Changement de scène
        SceneManager.LoadScene(targetSceneName);
    }
}