using Modeles.FonctionsJeu.FonctionsJeu;

namespace Modeles.FonctionsJeu.Helper;

public static class MagasinHelper
{
    private static Expedition? _expedition;


    public static async Task Magasin(int niveau)
    {
        _expedition = GameManager.Instance.Expedition;
        var magasin = await AppelsApi.GetMagasin(niveau)!;
        var choix = 0;
        while (true)
        {
            magasin.Afficher(choix);
            choix = ChoixMagasin(choix, out var touche);
            if (touche == ConsoleKey.Escape)
                break;
            if (touche != ConsoleKey.Spacebar)
                continue;

            var objet = magasin.Objets[choix];
            var achatValide = magasin.Offres.TryGetValue(objet, out var cout);
            if (!achatValide)
                continue;
            if (cout > _expedition.Pieces)
                continue;

            achatValide = magasin.Stock[objet] > 0;
            if (!achatValide)
                continue;
            _expedition.Pieces -= magasin.Offres[objet];
            _expedition.Sac[objet]++;
            magasin.Stock[objet] -= 1;
        }

        GameManager.Instance.Expedition = _expedition;
    }

    private static int ChoixMagasin(int choix, out ConsoleKey touche)
    {
        touche = Console.ReadKey().Key;
        List<ConsoleKey> toucheValide =
        [
            ConsoleKey.LeftArrow,
            ConsoleKey.RightArrow,
            ConsoleKey.UpArrow,
            ConsoleKey.DownArrow,
            ConsoleKey.Spacebar,
            ConsoleKey.Escape
        ];
        while (!toucheValide.Contains(touche))
            touche = Console.ReadKey().Key;
        choix += touche switch
        {
            ConsoleKey.RightArrow => 1,
            ConsoleKey.UpArrow => -2,
            ConsoleKey.DownArrow => 2,
            ConsoleKey.LeftArrow => -1,
            _ => 0
        };
        if (choix < 0) choix += 6;
        if (choix > 5) choix -= 6;
        return choix;
    }
}