using Modeles.Character;

namespace Modeles.MoveSet;

public abstract class Capacite(string nom, float val, bool aoe, bool ally, int cout, int gain, string description, bool tous = false)
{
    public string Nom { get; set; } = nom;
    public float Valeur { get; set; } = val;
    public bool Zone { get; set; } = aoe;
    public bool Allie { get; set; } = ally;
    public int Cout { get; set; } = cout;
    public int Gain { get; set; } = gain;
    public string Description { get; set; } = description;
    public bool ToutLeMonde { get; set; } = tous;

    public abstract void Utiliser(Entite utilisateur, Entite cible);
    public abstract void Utiliser(Entite utilisateur, List<Entite> cibles);
    public abstract void Utiliser(Entite utilisateur, List<Entite> equipe, List<Entite> ennemies);
    
    public string RemplacerValeurDescription(string str)
    {
        return str.Replace(
                "{ValeurPourcent}",
                Math.Round(Valeur * 100) + "%")
            .Replace(
                "{Valeur}",
                Valeur.ToString())
            .Replace(
                "{Valeur10}",
                (Valeur * 10).ToString());

    }
}