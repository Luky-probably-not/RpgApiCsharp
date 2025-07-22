using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Modeles.Capacites;
using Modeles.Character;
using Modeles.Character.Ennemie;
using Modeles.Character.Personnage;
using Modeles.FonctionsJeu.MiniGames;
using Modeles.Objets;

namespace Modeles.Json;

public static class Options
{
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
                                        new JsonDerivedType(typeof(Buff),        nameof(Buff)),
                                        new JsonDerivedType(typeof(Energie),     nameof(Energie)),
                                        new JsonDerivedType(typeof(Frappe),      nameof(Frappe)),
                                        new JsonDerivedType(typeof(MultiCoup),   nameof(MultiCoup)),
                                        new JsonDerivedType(typeof(Sacrifice),   nameof(Sacrifice)),
                                        new JsonDerivedType(typeof(Soin),        nameof(Soin)),
                                        new JsonDerivedType(typeof(VolPv),       nameof(VolPv))
                                    }
                                };
                                break;
                            case var t when t == typeof(Objet):
                                ti.PolymorphismOptions = new JsonPolymorphismOptions
                                {
                                    TypeDiscriminatorPropertyName = "$type",
                                    IgnoreUnrecognizedTypeDiscriminators = true,
                                    DerivedTypes =
                                    {
                                        new JsonDerivedType(typeof(AttaqueBoost), nameof(AttaqueBoost)),
                                        new JsonDerivedType(typeof(DefenseBoost), nameof(DefenseBoost)),
                                        new JsonDerivedType(typeof(PotionDoubleDegats), nameof(PotionDoubleDegats)),
                                        new JsonDerivedType(typeof(PotionEnergie), nameof(PotionEnergie)),
                                        new JsonDerivedType(typeof(PotionReductionDegats), nameof(PotionReductionDegats)),
                                        new JsonDerivedType(typeof(PotionSoin), nameof(PotionSoin))
                                    }
                                };
                                break;
                                
                            case var t when t == typeof(MiniJeu):
                                ti.PolymorphismOptions = new JsonPolymorphismOptions
                                {
                                    TypeDiscriminatorPropertyName = "$type",
                                    IgnoreUnrecognizedTypeDiscriminators =true,
                                    DerivedTypes =
                                    {
                                        new JsonDerivedType(typeof(Memory), nameof(Memory)),
                                        new JsonDerivedType(typeof(TimingMiniGame), nameof(TimingMiniGame))
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