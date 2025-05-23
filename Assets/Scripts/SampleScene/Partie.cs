using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using System.Xml.Schema;

public class Partie : MonoBehaviour
{
    public List<Jeu> jeux = new List<Jeu>();

    public bool indiceLancer = false;
    public int indiceJeu = 0;
    public bool finie = false;

    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textIndiceJeu;
    public TextMeshProUGUI textIndiceLancer;
    public TextMeshProUGUI textPointsLancer;

    public List<TextMeshProUGUI> textsLancers = new List<TextMeshProUGUI>();
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            Jeu jeu = new Jeu
            {
                numero = i,
                lancer1 = new Lancer(1),
                lancer2 = new Lancer(2),

            };

            jeux.Add(jeu);
        }
    }

    int CalculerScoreJeu(Jeu jeu, int index)
    {
        int scoreDeBase = (jeu.lancer1?.score ?? 0) + (jeu.lancer2?.score ?? 0);

        if (jeu.strike)
        {
            jeu.pointsSupplementaires = GetProchainsLancers(index + 1, 2);
        }
        else if (jeu.spare)
        {
            jeu.pointsSupplementaires = GetProchainsLancers(index + 1, 1);
        }

        return scoreDeBase + jeu.pointsSupplementaires;
    }

    int GetProchainsLancers(int startIndex, int nombre)
    {
        List<int> scores = new List<int>();

        for (int i = startIndex; i < jeux.Count && scores.Count < nombre; i++)
        {
            var j = jeux[i];
            if (j.lancer1 != null)
                scores.Add(j.lancer1.score);
            if (j.lancer2 != null && scores.Count < nombre)
                scores.Add(j.lancer2.score);
            if (j.strike == true)
                scores.Add(jeux[i + 1].lancer1.score);
        }

        return scores.Take(nombre).Sum();
    }

    public void MiseAJourJeu(int nbBoules)
    {
        if (finie == false)
        {
            Jeu jeuActuel = jeux[indiceJeu];
            switch (indiceLancer)
            {
                case false:
                    jeuActuel.lancer1.score = nbBoules;
                    UpdateTextLancer(indiceJeu * 2, nbBoules);
                    if (nbBoules == 10)
                    {
                        indiceLancer = !indiceLancer;
                        indiceJeu += 1;
                    }
                    break;
                case true:
                    UpdateTextLancer(indiceJeu * 2 + 1, nbBoules);
                    jeuActuel.lancer2.score = nbBoules;
                    indiceJeu += 1;
                    break;
            }
            indiceLancer = !indiceLancer;
            if (indiceJeu == 10)
            {
                finie = true;
            }

            UpdateText(nbBoules);
        }
    }

    int CalculerScorePartie()
    {
        int score = 0;
        foreach (Jeu j in jeux)
        {
            score += CalculerScoreJeu(j, j.numero);
        }
        return score;
    }

    void UpdateText(int points)
    {
        if (textScore != null)
        {
            textScore.text = "Score : " + CalculerScorePartie();
            textIndiceJeu.text = "Jeu numéro " + indiceJeu;
            textIndiceLancer.text = "Lancer numéro " + (indiceLancer ? "2" : "1");
            textPointsLancer.text = "Points sur le lancer précédent : " + points;
        }
        else
        {
            Debug.LogWarning("TextMesh non assigné");
        }
    }

    void UpdateTextLancer(int indice, int points)
    {
        textsLancers[indice].text = points.ToString() ; 
    }
}
