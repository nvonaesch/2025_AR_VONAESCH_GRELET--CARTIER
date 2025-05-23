using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScore : MonoBehaviour
{

    public Partie partie;
    public void ajoutScore()
    {
        int nombreAleatoire = Random.Range(0, 10);
        //partie.MiseAJourJeu(nombreAleatoire);
    }
}
