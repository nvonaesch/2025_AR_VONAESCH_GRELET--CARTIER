using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Linq;
using Unity.VisualScripting;

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
                plane.gameObject.SetActive(false);
            }
        }

        lowestPlane.gameObject.SetActive(true);

        // NE PAS désactiver planeManager.requestedDetectionMode ici

        if (!cleanupActive)
        {
            cleanupActive = true;
        }
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (!cleanupActive)
            return;

        foreach (var addedPlane in args.added)
        {
            if (addedPlane != lowestPlane)
            {
                addedPlane.gameObject.SetActive(false);
            }
        }

        foreach (var changedPlane in args.updated)
        {
            if (changedPlane != lowestPlane)
            {
                changedPlane.gameObject.SetActive(false);
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

    public ARPlane GetLockedPlane()
    {
        return lowestPlane;
    }

}

