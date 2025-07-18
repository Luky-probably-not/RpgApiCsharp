using Modeles.Character;
using System.Drawing;
using static Modeles.Extensions;

namespace Modeles.Capacites;

public abstract class Capacite(string nom, float val, bool aoe, bool ally, int cout, int gain, string description)
{
    public string Nom { get; set; } = nom;
    public float Valeur { get; set; } = val;
    public bool Zone { get; set; } = aoe;
    public bool Allie { get; set; } = ally;
    public int Cout { get; set; } = cout;
    public int Gain { get; set; } = gain;
    public string Description { get; set; } = description;

    public abstract void Utiliser(Entite utilisateur, Entite cible);
    public abstract void Utiliser(Entite utilisateur, List<Entite> cibles);

    
    public string RemplacerValeurDescription(string str)
    {
        return str.Replace(
                "{ValeurPourcent}",
                Math.Round(Valeur * 100) + "%")
            .Replace(
                "{Valeur}",
                Valeur.ToString());

    }
}