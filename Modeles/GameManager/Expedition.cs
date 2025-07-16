using Modeles.Character;
using Modeles.Objets;
namespace Modeles.GameManager;

public class Expedition
{
    public required List<Entite> Equipe;
    public int Pieces = 0;
    public Dictionary<Objet, int> Sac = InitialiserSac();

    private static Dictionary<Objet, int> InitialiserSac()
    {
        return new Dictionary<Objet, int>()
        {
            { new PotionSoin(), 1 },
            { new PotionEnergie(), 1 },
            { new AttaqueBoost(), 1 },
            { new DefenseBoost(), 1 }
        };
    }
}