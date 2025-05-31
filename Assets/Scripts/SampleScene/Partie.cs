using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Partie : MonoBehaviour
{
    public List<Jeu> jeux = new List<Jeu>();

    public bool indiceLancer = false;
    public int indiceJeu = 0;
    public bool finie = false;


    public List<TextMeshProUGUI> textsLancers = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> textsScoresCumules = new List<TextMeshProUGUI>();
    private List<bool> etatQuilles = new List<bool>();

    private GameObject quilleManager;
    public GameObject video;


    void Start()
    {


        for (int i = 0; i < 10; i++)
        {
            Jeu jeu = new Jeu
            {
                lancer1 = new Lancer(),
                lancer2 = new Lancer(),
            };

            jeux.Add(jeu);
        }
        etatQuilles = Enumerable.Repeat(false, 10).ToList();

        int size = etatQuilles.Count();
        Debug.LogWarning($"Taille start : {size}");

        quilleManager = GameObject.Find("QuilleManager");
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

    void UpdateScoreCumule()
    {
        int scoreTotal = 0;
        for (int i = 0; i < jeux.Count; i++)
        {
            scoreTotal += CalculerScoreJeu(jeux[i], i);
            if (i < textsScoresCumules.Count && textsScoresCumules[i] != null && string.IsNullOrEmpty(textsScoresCumules[i].text))
            {
                textsScoresCumules[i].text = scoreTotal.ToString();
            }
        }
    }
    
    public void MiseAJourJeu()
    {
        UpdateEtatToutesQuilles();
        int nbQuilles = etatQuilles.Count(q => q == Constantes.TOMBEE);
        int score = nbQuilles;

        if (finie != Constantes.FINIE)
        {
            Jeu jeuActuel = jeux[indiceJeu];
            switch (indiceLancer)
            {
                case Constantes.PREMIER_LANCER:
                    jeuActuel.lancer1.score = score;
                    UpdateTextLancer(indiceJeu * 2, score);
                    if (score == 10)
                    {
                        indiceLancer = !indiceLancer;
                        indiceJeu += 1;
                    }
                    break;
                case Constantes.SECOND_LANCER:
                    score = nbQuilles - jeuActuel.lancer1.score;
                    UpdateTextLancer(indiceJeu * 2 + 1, score);
                    jeuActuel.lancer2.score = score;
                    indiceJeu += 1;
                    break;
            }
            indiceLancer = !indiceLancer;
            if (indiceJeu == 20)
            {
                finie = Constantes.FINIE;
            }

            UpdateScoreCumule();

            if (nbQuilles == 9)
            {

                VideoPlanePlayer video910 = video.GetComponent<VideoPlanePlayer>();
                video910.PlayVideo();
            }
            ReplacerQuilles();

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

    void UpdateTextLancer(int indice, int points)
    {
        textsLancers[indice].text = points.ToString();
    }

    private void UpdateEtatQuille(int index, bool etat)
    {
        Debug.LogWarning($"UpdateEtatQuille {index}, {etat}");
        int size = etatQuilles.Count();
        Debug.LogWarning($"Taille : {size}");
        etatQuilles[index] = etat;
        Debug.Log($"Quille {index} tombée : {etatQuilles[index]}");

    }

    private void UpdateEtatToutesQuilles()
    {
        GameObject[] quilles = GameObject.FindGameObjectsWithTag("Quille");

        foreach (GameObject q in quilles)
        {
            Quille quille = q.GetComponent<Quille>();
            UpdateEtatQuille(quille.numero, quille.isTombee());
        }
    }

    void ReplacerQuilles()
    {
        GameObject[] quilles = GameObject.FindGameObjectsWithTag("Quille");
        setupQuille setup = quilleManager.GetComponent<setupQuille>();

        if (indiceLancer == false)
        {
            for (int i = 0; i < 10; i++) { etatQuilles[i] = false; }
        }

        setup.ChangerActivationToutesQuilles(etatQuilles, quilles);
    }
}
