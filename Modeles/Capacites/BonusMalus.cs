namespace Modeles.Capacites;

public class BonusMalus(string nom, float valeur)
{
    public string Nom { get; private set; }= nom;
    public float Valeur { get; private set; } = valeur;
    public int Duree { get; private set; }= 3;
    public bool Actif { get; private set; } = true;

    public void BaisserDuree()
    {
        Duree--;
        if (Duree == 0)
            Actif = false;
    }
}