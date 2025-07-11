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
    }

    public static void MoveToBack<T>(this List<T> liste)
    {
        var item = liste[0];
        liste.RemoveAt(0);
        liste.Add(item);
    }

    public static JsonSerializerOptions OptionsJson { get; } = CreerOptions();

    public static void ApplyTo(JsonSerializerOptions target)
    {
        target.PropertyNamingPolicy = OptionsJson.PropertyNamingPolicy;
        target.WriteIndented = OptionsJson.WriteIndented;
        target.TypeInfoResolver = OptionsJson.TypeInfoResolver;
    }

    private static JsonSerializerOptions CreerOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    ti =>
                    {
                        switch (ti.Type)
                        {
                            case var t when t == typeof(Entite):
                                ti.PolymorphismOptions = new JsonPolymorphismOptions
                                {
                                    TypeDiscriminatorPropertyName = "$type",
                                    IgnoreUnrecognizedTypeDiscriminators = true,
                                    DerivedTypes =
                                    {
                                        new JsonDerivedType(typeof(Sorcier), nameof(Sorcier)),
                                        new JsonDerivedType(typeof(Chevalier), nameof(Chevalier)),
                                        new JsonDerivedType(typeof(Barbare), nameof(Barbare)),

                                        new JsonDerivedType(typeof(Zombie), nameof(Zombie))
                                    }
                                };
                                break;
                            case var t when t == typeof(Capacite):
                                ti.PolymorphismOptions = new JsonPolymorphismOptions
                                {
                                    TypeDiscriminatorPropertyName = "$type",
                                    IgnoreUnrecognizedTypeDiscriminators = true,
                                    DerivedTypes =
                                    {
                                        new JsonDerivedType(typeof(AttaqueZone), nameof(AttaqueZone)),
                                        new JsonDerivedType(typeof(Buff), nameof(Buff)),
                                        new JsonDerivedType(typeof(Energie), nameof(Energie)),
                                        new JsonDerivedType(typeof(Frappe), nameof(Frappe)),
                                        new JsonDerivedType(typeof(MultiCoup), nameof(MultiCoup)),
                                        new JsonDerivedType(typeof(Sacrifice), nameof(Sacrifice)),
                                        new JsonDerivedType(typeof(Soin), nameof(Soin))
                                    }
                                };
                                break;
                        }
                    }
                }
            }
        };

        return options;
    }
}