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

    public List<Dictionary<int, string>> SplitEveryNth(int n)
    {
        var temp = Description.Split(" ").ToList();
        var index = temp.FindIndex(x => x is "{ValeurPourcent}" or "{Valeur}");
        temp[index] = RemplacerValeurDescription(temp[index]);
        var valeur = temp[index];
        List<Dictionary<int, string>> result = [new Dictionary<int, string> { { 0, "" } }, new Dictionary<int, string> { { 0, "" } }, new Dictionary<int, string> { { 0, "" } }];
        var str = "";
        var emplacement = 0;
        var i = 0;
        Dictionary<int, string> dic = [];
        var id = 0;
        foreach (var item in temp)
        {
            if (str.Length > n)
            {
                dic.Add(str.Length - 1, str[..^1]);
                result[id] = dic;
                id++;
                dic = [];
                str = "";
                i++;
            }
            if (item == valeur)
                emplacement = i;
            str += $"{item} ";
        }
        if (!string.IsNullOrEmpty(str))
            result[id] = new Dictionary<int, string> { { str.Length - 1, str[..^1] } };

        var t = result[emplacement];
        foreach (var kvp in t.Where(kvp => kvp.Value.Contains(valeur)))
        {
            t[kvp.Key] = kvp.Value.Replace(valeur, new StringColorise(valeur, Color.Yellow).Str);
        }
        result[emplacement] = t;
        
        return result;
    }
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