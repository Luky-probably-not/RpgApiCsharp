using Modeles.Character;

namespace Modeles.Items.Objets;

public class DefenseBoost() : Objet("Potion de Défense", 0.5f, "Augmente la défense de l'équipe de {ValeurPourcent}", true)
{
    public override void Utiliser(Entite cible) { }

    public override void Utiliser(List<Entite> cibles)
    {
        cibles.ForEach(e => e.Bonus.Add(new("Defense", 1+Valeur)));
    }
}