namespace Modeles.FonctionsJeu.MiniGames;

public abstract class MiniJeu
{
    public abstract void Jouer();

    public abstract void Jouer(out Dictionary<string, int> result);

    public abstract void Jouer(out string result);
}