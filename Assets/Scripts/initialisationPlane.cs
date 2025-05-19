using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initialisationPlane : MonoBehaviour
{
    void Start()
    {
        // Placer ce GameObject à la position/rotation du plan détecté dans la scène précédente
        transform.position = PlaneData.PlaneCenter;
        transform.rotation = PlaneData.PlaneRotation;
    }
}