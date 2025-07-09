using Modeles.Character;

namespace Modeles.Capacites;

public abstract class Capacite(float val, bool aoe, bool ally, int cout, int gain)
{
    public float Valeur { get; protected set; } = val;
    public bool Zone { get; protected set; } = aoe;
    public bool Allie { get; protected set; } = ally;
    public int Cout { get; protected set; } = cout;
    public int Gain { get; protected set; } = gain;

    public abstract void Utiliser(Entite utilisateur, Entite cible);
    public abstract void Utiliser(Entite utilisateur, List<Entite> cibles);
}