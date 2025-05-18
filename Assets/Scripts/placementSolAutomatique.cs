using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AutoPlaceOnGround : MonoBehaviour
{
    public GameObject objectToPlace; // prefab à instancier
    public float groundYThreshold = 0.3f; // hauteur max pour considérer que c'est le sol

    private ARPlaneManager planeManager;
    private bool objectPlaced = false;

    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (objectPlaced) return;

        foreach (var plane in args.added)
        {
            // Vérifie si le plan est horizontal et assez bas
            if (plane.alignment == PlaneAlignment.HorizontalUp && plane.center.y < groundYThreshold)
            {
                // Place l'objet et désactive le reste
                Vector3 spawnPosition = plane.center;
                Instantiate(objectToPlace, spawnPosition, Quaternion.identity);
                objectPlaced = true;

                DisableOtherPlanes();
                break;
            }
        }
    }

    void DisableOtherPlanes()
    {
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
        planeManager.enabled = false;
    }
}
