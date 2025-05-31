using System.Collections.Generic;
using UnityEngine;

public class setupQuille : MonoBehaviour
{
    public GameObject quillePrefab;
    public GameObject scorePrefab;
    public float spacing = 0.4f;

    //bidouille
    private Quaternion quilleRotation = Quaternion.Euler(-90f, 0f, 0f);
    private Quaternion scoreRotation = Quaternion.Euler(0f, -90f, 0f);
    private int totalRows = 4;
    private Vector3 centerPosition;
    private float totalDepth;
    private float startZ;
    private GameObject reconstructedPlane;

    void Start()
    {
        reconstructedPlane = GameObject.Find("ReconstructedPlane");
        centerPosition = reconstructedPlane.transform.position;

        totalDepth = (totalRows - 1) * spacing;
        startZ = -totalDepth / 2f;
        float lastRowZ = startZ + (totalRows - 1) * spacing;

        float scoreZ = lastRowZ + 2.0f;
        Vector3 scorePosition = centerPosition + new Vector3(0f, 1f, scoreZ);

        scorePrefab.transform.SetParent(reconstructedPlane.transform);
        scorePrefab.transform.position = scorePosition;
        scorePrefab.transform.rotation = scoreRotation;
        scorePrefab.name = "ScoreDisplay";
        
        int numQuille = 0;

        totalDepth = (totalRows - 1) * spacing;
        startZ = -totalDepth / 2f;

        for (int row = 0; row < totalRows; row++)
        {
            int quillesInRow = row + 1;
            float z = startZ + row * spacing;

            float startX = -(spacing * row) / 2f;

            for (int i = 0; i < quillesInRow; i++)
            {
                numQuille = placerQuille(startX, i, z, reconstructedPlane, numQuille);
            }
        }
    }

    int placerQuille(float startX, int i, float z, GameObject reconstructedPlane, int num)
    {
        float x = startX + i * spacing;
        Vector3 quillePosition = centerPosition + new Vector3(x, 0, z);

        GameObject quille = Instantiate(quillePrefab, quillePosition, quilleRotation, reconstructedPlane.transform);
        quille.tag = "Quille";
        Quille quilleScript = quille.AddComponent<Quille>();
        quilleScript.numero = num;
        quilleScript.Initialiser();

        return num + 1;
    }
    
    public void ChangerActivationToutesQuilles(List<bool> quillesTombees, GameObject[] quilles)
    {
        foreach (GameObject q in quilles)
        {
            Quille quille = q.GetComponent<Quille>();
            if (quillesTombees[quille.numero] == Constantes.DEBOUT)
            {
                quille.ActiverQuille();
            }
            else
            {
                quille.DesactiverQuille();
            }
            quillesTombees[quille.numero] = Constantes.DEBOUT;
        }
    }
}

