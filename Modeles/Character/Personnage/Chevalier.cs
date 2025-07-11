using Modeles.Capacites;

namespace Modeles.Character.Personnage;
public class Chevalier : Entite 
{
    public Chevalier() : base("Chevalier", 10, 10, 60, 100, [
        "♘ ◓  ",
        "\\Ր⛏  ",
        " ⟫  ⟫",
        "⟪  ⟪ "
    ])
    {
        AjouterCapacite(new Buff());
        AjouterCapacite(new AttaqueZone());
        AjouterCapacite(new Frappe());
        ReinitialiserValeurAction();
    }
}
