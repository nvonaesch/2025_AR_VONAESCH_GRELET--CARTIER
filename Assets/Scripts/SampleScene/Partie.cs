using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Partie : MonoBehaviour
{
    public List<Jeu> jeux1 = new List<Jeu>();
    public List<Jeu> jeux2 = new List<Jeu>();

    public bool indiceLancer = false;
    public int indiceJeu = 0;
    public bool finie = false;

    public bool joueur1 = true;


    public List<TextMeshProUGUI> textsLancers = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> textsScoresCumules = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> textsScoresCumulesJ2 = new List<TextMeshProUGUI>();

    private List<bool> etatQuilles = new List<bool>();

    private GameObject quilleManager;
    public GameObject video;
    public GameObject autreJoueur;

    private int indiceJeuCumuleJ1 = 0;
    private int indiceJeuCumuleJ2 = 0;
    public TextMeshProUGUI scoreFinalJ1;
    public TextMeshProUGUI scoreFinalJ2;

    void Start()
    {


        for (int i = 0; i < 20; i++)
        {
            Jeu jeu = new Jeu
            {
                lancer1 = new Lancer(),
                lancer2 = new Lancer(),
            };

            jeux1.Add(jeu);

            Jeu jeu2 = new Jeu
            {
                lancer1 = new Lancer(),
                lancer2 = new Lancer(),
            };

            jeux2.Add(jeu2);
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

        for (int i = startIndex; i < jeux1.Count && scores.Count < nombre; i++)
        {
            var j = jeux1[i];
            if (j.lancer1 != null)
                scores.Add(j.lancer1.score);
            if (j.lancer2 != null && scores.Count < nombre)
                scores.Add(j.lancer2.score);
            if (j.strike == true)
                scores.Add(jeux1[i + 1].lancer1.score);
        }

        return scores.Take(nombre).Sum();
    }


    public void MiseAJourJeu()
    {
        UpdateEtatToutesQuilles();
        int nbQuilles = etatQuilles.Count(q => q == Constantes.TOMBEE);
        int score = nbQuilles;

        if (finie != Constantes.FINIE)
        {
            Jeu jeuActuel;
            bool tempBool = joueur1;
            if (joueur1)
            {
                jeuActuel = jeux1[indiceJeu];
            }
            else
            {
                jeuActuel = jeux2[indiceJeu];
            }

            switch (indiceLancer)
            {
                case Constantes.PREMIER_LANCER:
                    jeuActuel.lancer1.score = score;
                    UpdateTextLancer(indiceJeu * 2, score);
                    if (score == 10)
                    {
                        indiceLancer = !indiceLancer;
                        indiceJeu += 1;
                        joueur1 = !joueur1;
                    }
                    break;
                case Constantes.SECOND_LANCER:
                    score = nbQuilles - jeuActuel.lancer1.score;
                    UpdateTextLancer(indiceJeu * 2 + 1, score);
                    jeuActuel.lancer2.score = score;
                    indiceJeu += 1;
                    joueur1 = !joueur1;

                    break;
            }
            indiceLancer = !indiceLancer;
            if (indiceJeu == 40)
            {
                finie = Constantes.FINIE;
                StartCoroutine(RetourAuMenu());
            }

            if (tempBool)
            {
                textsScoresCumules[indiceJeuCumuleJ1].text = CalculerScoreJeu(jeuActuel, indiceJeu + 1).ToString();
                if (indiceLancer == Constantes.PREMIER_LANCER)
                    indiceJeuCumuleJ1 += 1;
            }
            else
            {
                textsScoresCumulesJ2[indiceJeuCumuleJ2].text = CalculerScoreJeu(jeuActuel, indiceJeu + 1).ToString();
                if (indiceLancer == Constantes.PREMIER_LANCER)
                    indiceJeuCumuleJ2 += 1;
            }




            if (nbQuilles == 9)
            {

                VideoPlanePlayer video910 = video.GetComponent<VideoPlanePlayer>();
                video910.PlayVideo();
            }


            ReplacerQuilles();
            scoreFinalJ1.text = CalculerScorePartieJ1().ToString();
            scoreFinalJ2.text = CalculerScorePartieJ2().ToString();

        }
        else
        {
            scoreFinalJ1.text = CalculerScorePartieJ1().ToString();
            scoreFinalJ2.text = CalculerScorePartieJ2().ToString();
            
        }
    }

    int CalculerScorePartieJ1()
    {
        int score = 0;
        foreach (Jeu j in jeux1)
        {
            score += CalculerScoreJeu(j, j.numero);
        }
        return score;
    }

    int CalculerScorePartieJ2()
    {
        int score = 0;
        foreach (Jeu j in jeux2)
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
    private IEnumerator RetourAuMenu()
    {
        yield return new WaitForSeconds(5f); 
        SceneManager.LoadScene("Menu");
    }
}
