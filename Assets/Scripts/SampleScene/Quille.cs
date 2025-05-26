using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quille : MonoBehaviour
{
    public int numero;
    GameObject reconstructedPlane = GameObject.Find("ReconstructedPlane");

    private Vector3 positionInitiale;
    private Quaternion rotationInitiale;

    public bool isTombee()
    {

        float angleSeuil = 45f;
        float hauteurSeuil = 0.5f;

        float angle = Quaternion.Angle(rotationInitiale, transform.rotation);
        float deltaY = positionInitiale.y - transform.position.y;
        
        if (angle > angleSeuil || deltaY > hauteurSeuil)
        {
            return true;
        }

        return false;
    }

    public void DesactiverQuille()
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null)
            mesh.enabled = false;

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
        ResetPosition();
    }

    public void ActiverQuille()
    {
        ResetPosition();
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null)
            mesh.enabled = true;

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
    }

    public void Initialiser()
    {
        positionInitiale = transform.position;
        rotationInitiale = transform.rotation;
    }

    public void ResetPosition()
    {
        transform.position = positionInitiale;
        transform.rotation = rotationInitiale;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }
    }

}
