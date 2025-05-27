[System.Serializable]
public class Jeu
{
    public Lancer lancer1 ;
    public Lancer lancer2 ;

    public int pointsSupplementaires = 0;

    public int numero;

    public bool strike => lancer1 != null && lancer1.score == 10;
    public bool spare =>
        !strike &&
        lancer1 != null && lancer2 != null &&
        (lancer1.score + lancer2.score == 10);

}
