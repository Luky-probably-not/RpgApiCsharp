using Modeles.Character;

namespace Modeles.Objets;

public abstract class Objet(string nom, float valeur, string description)
{
    public string Nom { get; set; } = nom;
    public float Valeur { get; set; } = valeur;

    public string Description { get; set; } = description;

    public abstract void Utiliser(Entite cible);
    public abstract void Utiliser(List<Entite> cibles);

    public string RemplacerValeurDescription()
    {
        return Description.Contains("{ValeurPourcent")
            ? Math.Round(Valeur * 100) + "%"
            : Description.Contains("{Valeur}")
                ? Valeur.ToString()
                : "";
    }
}