using Modeles.Character;

namespace Modeles.Capacites;

public class Sacrifice() : Capacite("Sacrifice", 0.4f, false, false, 9, 0, "Perd {ValeurPourcent} des pv actuels. Augmente les dégats en fonction des pv perdus")
{
    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        var degats = utilisateur.PointDeVie * Valeur;
        cible.Blesser((int)(3 + degats),utilisateur,false);
        utilisateur.PointDeVie -= (int)degats;
        utilisateur.PointAction -= Cout;
    }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles) { }
    public override void Utiliser(Entite utilisateur, List<Entite> equipe, List<Entite> ennemies) { }
}