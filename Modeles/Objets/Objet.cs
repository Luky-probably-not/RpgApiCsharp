using System.Globalization;
using Modeles.Character;

namespace Modeles.Objets;

public abstract class Objet(string nom, float valeur, string description, bool zone)
{
    public string Nom { get; set; } = nom;
    public float Valeur { get; set; } = valeur;
    public string Description { get; set; } = description;
    public bool Zone { get; set; } = zone;

    public abstract void Utiliser(Entite cible);
    public abstract void Utiliser(List<Entite> cibles);

    public string RemplacerValeurDescription(string str)
    {
        return str.Replace(
                "{ValeurPourcent}",
                Math.Round(Valeur * 100) + "%")
            .Replace(
                "{Valeur}",
                Valeur.ToString(CultureInfo.InvariantCulture));

    }

    public static int RandomAmount(int sommeNiveau)
    {
        var rand = new Random();
        var total = 1;
        float k = 100;
        while ((float)rand.NextDouble() < sommeNiveau / (sommeNiveau + k))
        {
            total++;
            k *= 3;
        }
        return total;
    }

    public static Objet ObjetParNom(string nom)
    {
        return nom switch
        {
            nameof(PotionSoin) => new PotionSoin(),
            nameof(PotionEnergie) => new PotionEnergie(),
            nameof(AttaqueBoost) => new AttaqueBoost(),
            nameof(DefenseBoost) => new DefenseBoost(),
            nameof(PotionDoubleDegats) => new PotionDoubleDegats(),
            nameof(PotionReductionDegats) => new PotionReductionDegats(),
            _ => new PotionSoin()
        };
    }
}