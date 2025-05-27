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

    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textIndiceJeu;
    public TextMeshProUGUI textIndiceLancer;
    public TextMeshProUGUI textPointsLancer;

    public List<TextMeshProUGUI> textsLancers = new List<TextMeshProUGUI>();

    private List<bool> quillesTombees = new List<bool>();

    private GameObject quilleManager;
<<<<<<< HEAD
    private GameObject scoreDisplay;
=======
    public GameObject video;
>>>>>>> 8876dbf9aa25c6e507b7fb7f92d234ca86e896ed

    void Start()
    {
        scoreDisplay = GameObject.FindWithTag("ScoreDisplay");
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
        quillesTombees = Enumerable.Repeat(false, 10).ToList();

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

    public void MiseAJourJeu()
    {
        UpdateEtatToutesQuilles();
        int nbQuilles = quillesTombees.Count(q => q == true);
        int score = nbQuilles;

        if (finie == false)
        {
            Jeu jeuActuel = jeux[indiceJeu];
            switch (indiceLancer)
            {
                case false:
                    jeuActuel.lancer1.score = score;
                    UpdateTextLancer(indiceJeu * 2, score);
                    if (score == 10)
                    {
                        indiceLancer = !indiceLancer;
                        indiceJeu += 1;
                    }
                    break;
                case true:
                    score = nbQuilles - jeuActuel.lancer1.score;
                    UpdateTextLancer(indiceJeu * 2 + 1, score);
                    jeuActuel.lancer2.score = score;
                    indiceJeu += 1;
                    break;
            }
            indiceLancer = !indiceLancer;
            if (indiceJeu == 10)
            {
                finie = true;
            }

            UpdateText(score);
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

    void ScoreAppear()
    {

    }

    void UpdateTextLancer(int indice, int points)
    {
        textsLancers[indice].text = points.ToString();
    }

    private void UpdateEtatQuille(int index, bool etat)
    {
        quillesTombees[index] = etat;
        Debug.Log($"Quille {index} tombée : {quillesTombees[index]}");

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
            for (int i = 0; i < 10; i++) { quillesTombees[i] = false; }
        }

        setup.ChangerActivationToutesQuilles(quillesTombees, quilles);
    }
}
