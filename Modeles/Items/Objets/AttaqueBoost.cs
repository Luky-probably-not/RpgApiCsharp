using Modeles.Character;

namespace Modeles.Items.Objets;

public class AttaqueBoost() : Objet("Potion d'Attaque", 0.3f, "Augmente l'attaque de l'équipe de {ValeurPourcent}", true)
{
    public override void Utiliser(Entite cible) { }

    public override void Utiliser(List<Entite> cibles)
    {
        cibles.ForEach(e => e.Bonus.Add(new ("Attaque", 1+Valeur)));
    }
}