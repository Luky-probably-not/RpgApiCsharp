using Modeles.Character;
using Modeles.FonctionsJeu;

namespace Modeles.Capacites;

public class VolPv() : Capacite("Vol de vie", 2, true, true, 6, 0, "Inflige {Valeur} de dégats à tous les ennemis. Restaure {Valeur10} pv a l'équipe", true)
{
    public override void Utiliser(Entite utilisateur, List<Entite> equipe, List<Entite> ennemies)
    {
        ennemies.ForEach(cible => cible.Blesser((int)Valeur, utilisateur,false));
        
        GameManager.Instance.MajEcran();
        GameManager.Instance.Ecran.Afficher();
        Thread.Sleep(700);

        equipe.ForEach(cible => cible.Soigner((int)Valeur*10));

        utilisateur.PointAction -= Cout;
    }



    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        throw new NotImplementedException();
    }
    public override void Utiliser(Entite utilisateur, List<Entite> cibles)
    {
        throw new NotImplementedException();
    }
}