using Modeles.Character;

namespace Modeles.Capacites;

public class Sacrifice() : Capacite(2, false, false, 4, 0)
{
    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        var degats = utilisateur.PointDeVie * 0.4;
        cible.Blesser((int)(Valeur + degats),utilisateur.Attaque,false);
        utilisateur.PointDeVie -= (int)degats;
        utilisateur.PointAction -= Cout;
    }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles) { }
}