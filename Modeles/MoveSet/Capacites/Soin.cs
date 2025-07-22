using Modeles.Character;

namespace Modeles.MoveSet.Capacites;

public class Soin() : Capacite("Soin", 0.3f, false, true, 3, 0, "Soigne un allié de {ValeurPourcent} de ses points de vie max")
{
    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        cible.Soigner(Valeur);
        utilisateur.PointAction -= Cout;
    }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles){ }
    public override void Utiliser(Entite utilisateur, List<Entite> equipe, List<Entite> ennemies) { }
}