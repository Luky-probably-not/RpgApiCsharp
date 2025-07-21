using System.Drawing;
using Modeles.Character;
using Modeles.Objets;
using static Modeles.Extensions;
namespace Modeles.FonctionsJeu;

public class Expedition
{
    public required List<Entite> Equipe { get; set; }
    public int Pieces { get; set; }= 0;
    public Dictionary<string, int> Sac { get; set; } = InitialiserSac();

    public static readonly List<string> IndexObjet =
    [
        nameof(PotionSoin), nameof(PotionEnergie), nameof(AttaqueBoost), nameof(DefenseBoost),
        nameof(PotionDoubleDegats), nameof(PotionReductionDegats)
    ];

    private static Dictionary<string, int> InitialiserSac()
    {
        return new Dictionary<string, int>()
        {
            { nameof(PotionSoin), 0 },
            { nameof(PotionEnergie), 0 },
            { nameof(AttaqueBoost), 0 },
            { nameof(DefenseBoost), 0 },
            { nameof(PotionDoubleDegats), 0},
            { nameof(PotionReductionDegats), 0 },
        };
    }

    public void Recompense(Dictionary<string, int> loot)
    {
        foreach (var kvp in loot)
        {
            if (kvp.Key == "Pieces")
            {
                Pieces += kvp.Value;
                continue;
            }
            Sac[kvp.Key] += kvp.Value;
        }
    }

    public StringColorise QuantitePieces()
    {
        return new(MettreAuMilieu($"Pièces : x{Pieces}$",50),Color.Yellow);
    }
}