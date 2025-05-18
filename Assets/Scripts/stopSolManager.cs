using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Linq;

public class GroundLockManager : MonoBehaviour
{
    private ARPlaneManager planeManager;
    private ARPlane lowestPlane;
    private bool cleanupActive = false;
    private float cleanupInterval = 1f;

    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    private ARPlane FindLowestHorizontalPlane()
    {
        ARPlane lowest = null;
        float lowestY = float.MaxValue;

        foreach (var plane in planeManager.trackables)
        {
            if (plane.alignment == PlaneAlignment.HorizontalUp)
            {
                float y = plane.center.y;
                if (y < lowestY)
                {
                    lowestY = y;
                    lowest = plane;
                }
            }
        }

        return lowest;
    }

    public void LockToLowestGroundPlane()
    {
        lowestPlane = FindLowestHorizontalPlane();

        if (lowestPlane == null)
        {
            Debug.Log("Aucun plan horizontal détecté.");
            return;
        }

        foreach (var plane in planeManager.trackables)
        {
            if (plane == lowestPlane)
            {
                plane.GetComponent<MeshRenderer>().enabled = true;
                plane.enabled = true;
            }
            else
            {
                plane.GetComponent<MeshRenderer>().enabled = false;
                plane.enabled = false;
            }
        }

        lowestPlane.gameObject.SetActive(true);

        // NE PAS désactiver planeManager.requestedDetectionMode ici

        if (!cleanupActive)
        {
            cleanupActive = true;
            //StartCoroutine(CleanupRoutine());
        }
    }

    /*private IEnumerator CleanupRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(cleanupInterval);

            List<ARPlane> toDestroy = new List<ARPlane>();

            foreach (var plane in planeManager.trackables)
            {
                if (plane != lowestPlane)
                {
                    plane.GetComponent<MeshRenderer>().enabled = false;
                    plane.enabled = false;

                    toDestroy.Add(plane);
                }
            }

            foreach (var plane in toDestroy)
            {
                Destroy(plane.gameObject);
            }
        }
    }*/

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (!cleanupActive)
            return;

        foreach (var addedPlane in args.added)
        {
            if (addedPlane != lowestPlane)
            {
                addedPlane.GetComponent<MeshRenderer>().enabled = false;
                addedPlane.enabled = false;
            }
        }

        foreach (var changedPlane in args.updated)
        {
            if (changedPlane != lowestPlane)
            {
                changedPlane.GetComponent<MeshRenderer>().enabled = false;
                changedPlane.enabled = false;
            }
        }
    }

    void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }


}



/*
public class GroundLockManager : MonoBehaviour
{
    private ARPlaneManager planeManager;
    private ARPlane lowestPlane;


    private ARPlane FindLowestHorizontalPlane()
    {
        ARPlane lowest = null;
        float lowestY = float.MaxValue;

        foreach (var plane in planeManager.trackables)
        {
            if (plane.alignment == PlaneAlignment.HorizontalUp)
            {
                float y = plane.center.y;
                if (y < lowestY)
                {
                    lowestY = y;
                    lowest = plane;
                }
            }
        }

        return lowest;
    }

    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    // Appelle cette fonction via un bouton UI ou autre
    public void LockToLowestGroundPlane()
    {

        lowestPlane = FindLowestHorizontalPlane();

        if (lowestPlane == null)
        {
            Debug.Log("Aucun plan horizontal détecté.");
            return;
        }

        foreach (var plane in planeManager.trackables)
        {
            if (plane == lowestPlane)
            {
                // Garder visible et actif
                plane.GetComponent<MeshRenderer>().enabled = true;
                plane.enabled = true;
            }
            else
            {
                // Cache visuelle
                plane.GetComponent<MeshRenderer>().enabled = false;

                // Désactive le composant ARPlane pour ne plus le mettre à jour
                plane.enabled = false;

                Destroy(plane.gameObject);
            }
        }

        lowestPlane.gameObject.SetActive(true);

        // Arrêter la détection de nouveaux plans
        planeManager.requestedDetectionMode = PlaneDetectionMode.None;
    }
}
*/


