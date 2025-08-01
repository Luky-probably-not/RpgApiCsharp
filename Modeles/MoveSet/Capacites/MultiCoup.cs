﻿using Modeles.Character;
using Modeles.FonctionsJeu.Helper;

namespace Modeles.MoveSet.Capacites;

public class MultiCoup() : Capacite("MultiCoup", 3, false, false, 3, 0, "Inflige {Valeur} dégats 3 fois à un ennemie. Ignore la defense ennemie")

{
    public override void Utiliser(Entite utilisateur, Entite cible)
    {
        for (var i = 0; i < 3; i++)
        {
            cible.Blesser((int)Valeur,utilisateur,true);
            CombatHelper.MajEcran();
            CombatHelper.Ecran.Afficher();
            Thread.Sleep(700);
        }
        utilisateur.PointAction -= Cout;
    }

    public override void Utiliser(Entite utilisateur, List<Entite> cibles) { }
    public override void Utiliser(Entite utilisateur, List<Entite> equipe, List<Entite> ennemies) { }
}