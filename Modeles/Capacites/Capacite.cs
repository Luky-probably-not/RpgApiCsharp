using System.Drawing;
using System.Globalization;
using Modeles.Character;
using static Modeles.Extensions;
namespace Modeles.Capacites;

public abstract class Capacite(string nom, float val, bool aoe, bool ally, int cout, int gain, string description)
{
    public string Nom { get; protected set; } = nom;
    public float Valeur { get; protected set; } = val;
    public bool Zone { get; protected set; } = aoe;
    public bool Allie { get; protected set; } = ally;
    public int Cout { get; protected set; } = cout;
    public int Gain { get; protected set; } = gain;
    public string Description { get; protected set; } = description;

    public abstract void Utiliser(Entite utilisateur, Entite cible);
    public abstract void Utiliser(Entite utilisateur, List<Entite> cibles);

    private static StringColorise DescriptionColorise(string str, float val)
    {
        var valeur = str.Contains("{ValeurPourcent")
            ? Math.Round(val * 100)+"%"
            : str.Contains("{Valeur}")
                ? val.ToString()
                : null;
        var desc = str
            .Replace("{ValeurPourcent}", valeur)
            .Replace("{Valeur}", valeur);

        var taille = desc.Length;
        var baseDesc = desc;

        if (string.IsNullOrEmpty(valeur)) return new StringColorise(desc) { Length = taille };
        var color = new StringColorise(valeur, Color.Yellow);
        desc = desc.Replace(valeur, color.Str);

        return new StringColorise(desc) { Length = taille, BaseStr = baseDesc};
    }

    public string RemplacerValeurDescription()
    {
        return Description.Contains("{ValeurPourcent")
            ? Math.Round(val * 100) + "%"
            : Description.Contains("{Valeur}")
                ? val.ToString()
                : "";
    }
}