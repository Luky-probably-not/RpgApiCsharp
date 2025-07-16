using Modeles.Character;

namespace Modeles.Objets;

public class PotionSoin() : Objet("Potion de Soin", 0.7f, "Rend {ValeurPourcent} des points de vie max d'une cible")
{
    public override void Utiliser(Entite cible)
    {
        cible.Soigner(Valeur);
    }

    public override void Utiliser(List<Entite> cibles) { }
}