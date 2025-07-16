using Modeles.Character;

namespace Modeles.Objets;

public class AttaqueBoost() : Objet("Potion d'Attaque", 0.3f, "Augmente l'attaque de l'équipe de {ValeurPourcent}")
{
    public override void Utiliser(Entite cible) { }

    public override void Utiliser(List<Entite> cibles)
    {
        cibles.ForEach(e => e.Bonus.Add(new ("Attaque", Valeur)));
    }
}