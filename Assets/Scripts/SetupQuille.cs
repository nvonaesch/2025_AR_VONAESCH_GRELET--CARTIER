using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setupQuille : MonoBehaviour
{
    public GameObject quillePrefab;
    public GameObject scorePrefab;
    public float spacing = 0.4f; 

    void Start()
    {
        GameObject reconstructedPlane = GameObject.Find("ReconstructedPlane");
        Vector3 centerPosition = reconstructedPlane.transform.position;

        Vector3 scorePosition = centerPosition + new Vector3(0f, 1f, 0.4f);
        GameObject scoreDisplay = Instantiate(scorePrefab, scorePosition, Quaternion.identity, reconstructedPlane.transform);

        Vector3 lookDirection = Camera.main.transform.position - scoreDisplay.transform.position;
        lookDirection.y = 0; 
        scoreDisplay.transform.rotation = Quaternion.LookRotation(lookDirection);

        int totalRows = 4;
        int quilleCount = 0;

        Quaternion quilleRotation = Quaternion.Euler(-90f, 0f, 0f);

        float totalDepth = (totalRows - 1) * spacing;
        float startZ = -totalDepth / 2f;

        for (int row = 0; row < totalRows; row++)
        {
            int quillesInRow = row + 1;
            float z = startZ + row * spacing;

            float startX = -(spacing * row) / 2f;

            for (int i = 0; i < quillesInRow; i++)
            {
                float x = startX + i * spacing;
                Vector3 quillePosition = centerPosition + new Vector3(x, 0, z);

                Instantiate(quillePrefab, quillePosition, quilleRotation, reconstructedPlane.transform);
                quilleCount++;
            }
        }

    }
}
