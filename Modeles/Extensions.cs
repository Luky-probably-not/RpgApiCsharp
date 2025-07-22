using Modeles.Items;
using Pastel;
using System.Drawing;
using Modeles.MoveSet;

namespace Modeles;

public static class Extensions
{
    public class StringColorise(string str,Color? color = null)
    {
        public int Length { get; set; } = str.Length;
        public string Str { get; set; } = color == null ? str : str.Pastel((Color)color);
        public string BaseStr { get; set; } = str;

        public Color? Couleur = color;
    }

    public static List<Dictionary<int, string>> SplitEveryNth(this Capacite cap, int n)
    {
        var temp = cap.Description.Split(" ").ToList();
        var index = temp.FindIndex(x => x is "{ValeurPourcent}" or "{Valeur}" or "{Valeur10}");
        temp[index] = cap.RemplacerValeurDescription(temp[index]);
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

    public static List<Dictionary<int, string>> SplitEveryNth(this Objet obj, int n)
    {
        var temp = obj.Description.Split(" ").ToList();
        var index = temp.FindIndex(x => x.Contains("{ValeurPourcent}") || x.Contains("{Valeur}"));
        temp[index] = obj.RemplacerValeurDescription(temp[index]);
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

    public static string FormatterLigne(this List<StringColorise> liste)
    {
        var result = string.Join("", liste.Select(f => f.Str));
        var length = 0;
        liste.ForEach(e => length += e.Length);
        var lastElement = result[^1].ToString();
        var check = lastElement is "┐" or "┘" or "│" or "┤";
        for (var i = 0; i < 154 - length; i++)
        {
            result += " ";
        }

        return check ? result : result[..^1] + "│";
    }

    public static string MettreAuMilieu(string str, int count)
    {
        var reste = (count - str.Length) % 2 == 1 ? " " : "";
        var vide = new string(' ', (int)Math.Ceiling((count - str.Length -reste.Length) / 2f));
        return vide + reste + str + vide;
    }


    public static List<T> Swap<T>(this List<T> list, int id1, int id2)
    {
        (list[id1], list[id2]) = (list[id2], list[id1]);
        return list;
    }
}