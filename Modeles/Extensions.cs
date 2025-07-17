using Pastel;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Modeles.Capacites;
using Modeles.Character;
using Modeles.Character.Ennemie;
using Modeles.Character.Personnage;

namespace Modeles;

public static class Extensions
{
    public class StringColorise(string str,Color? color = null)
    {
        public int Length { get; set; } = str.Length;
        public string Str { get; set; } = color == null ? str : str.Pastel((Color)color);
        public string BaseStr { get; set; } = str;

        public List<StringColorise> SplitEveryNth(int n)
        {
            if (BaseStr == null || n <= 0) return [];
            List<StringColorise> liste = [];
            for (var i = 0; i < Length; i += n)
            {
                liste.Add(new (Str.Substring(i, Math.Min(n, Length))));
            }
            return liste;
        }
    }

    public static void MoveToBack<T>(this List<T> liste)
    {
        var item = liste[0];
        liste.RemoveAt(0);
        liste.Add(item);
    }

}