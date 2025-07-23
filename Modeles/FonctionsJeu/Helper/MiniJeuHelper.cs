using Modeles.FonctionsJeu.FonctionsJeu;
using Modeles.FonctionsJeu.MiniGames;
using Modeles.Items;

namespace Modeles.FonctionsJeu.Helper;

public static class MiniJeuHelper
{
    private static Expedition? _expedition;

    public static async Task MiniJeu(int niveau)
    {
        _expedition = GameManager.Instance.Expedition;
        Dictionary<string, int> recompense = [];
        var jeu = await AppelsApi.GetMiniJeu(niveau)!;
        switch (jeu.GetType())
        {
            case var t when t == typeof(Memory):
                jeu.Jouer(out recompense);
                break;
            case var t when t == typeof(TimingMiniGame):
                jeu.Jouer(out string res);
                recompense = AppelsApi.GetRecompenseTiming(niveau, res).GetAwaiter().GetResult();
                break;
            case var t when t == typeof(Esquive):
                jeu.Jouer(out int score);
                recompense = AppelsApi.GetRecompenseEsquive(niveau, score).GetAwaiter().GetResult();
                break;
        }
        foreach (var kvp in recompense.Where(kvp => kvp.Value != 0))
        {
            Console.WriteLine("Butin : {0} x{1}", Objet.ObjetParNom(kvp.Key).Nom, kvp.Value);
        }
        _expedition.Recompense(recompense);
        Console.WriteLine("Press any key to continue");
        Console.ReadKey();

        GameManager.Instance.Expedition = _expedition;
    }
}