using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupQuille : MonoBehaviour
{
    public GameObject quillePrefab;
    public GameObject solManager;
    public float spacing = 0.5f; 

void Start()
    {
        Vector3 centerPosition = solManager.transform.position;
        int totalRows = 4;
        int quilleCount = 0;

        Quaternion quilleRotation = Quaternion.Euler(-90f, 0f, 0f); // 👈 Rotation de -90° sur l'axe X

        for (int row = 0; row < totalRows; row++)
        {
            int quillesInRow = row + 1;
            float rowZ = row * spacing;
            float startX = -(spacing * row) / 2f;

            for (int i = 0; i < quillesInRow; i++)
            {
                float x = startX + i * spacing;
                Vector3 quillePosition = centerPosition + new Vector3(x, 0, rowZ);

                Instantiate(quillePrefab, quillePosition, quilleRotation, solManager.transform);
                quilleCount++;
            }
        }

        Debug.Log("Nombre de quilles instanciées : " + quilleCount);
    }
}
