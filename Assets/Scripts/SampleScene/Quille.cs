using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quille : MonoBehaviour
{
    public int numero;
    GameObject reconstructedPlane = GameObject.Find("ReconstructedPlane");

    public bool isTombee()
    {
        float angle = Vector3.Angle(reconstructedPlane.transform.up, transform.up);

        if (angle > 45f)
        {
            return true;
        }
        else return false;
    }

}
