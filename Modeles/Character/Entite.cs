using Modeles.Capacites;

namespace Modeles.Character;

public abstract class Entite(string nom, int att, int def, int pv, int vitesse, List<string> sprite)
{
    public string Nom { get; protected set; } = nom;

    public int AttaqueDeBase {get; protected set;} = att;

    public int Attaque
    {
        get
        {
            var attBuff = Bonus.Find(b => b.Nom == "Attaque")?.Valeur ?? 1;
            var attNerf = Malus.Find(m => m.Nom == "Attaque")?.Valeur ?? 1;
            return (int)(AttaqueDeBase * attBuff * attNerf);
        }
        private set { }
    }
    public int DefenseDeBase {get; protected set;} = def;

    public int Defense
    {
        get
        {
            var defBuff = Bonus.Find(b => b.Nom == "Defense")?.Valeur ?? 1;
            var defNerf = Malus.Find(b => b.Nom == "Defense")?.Valeur ?? 1;
            return (int)(DefenseDeBase * defBuff * defNerf);
        }
        private set{ }
    }
    public int PointDeVie {get; set;} = pv;
    public int PointDeVieMax {get; protected set;} = pv;
    public int Vitesse {get; set;} = vitesse;
    public bool Vivant { get; protected set; } = true;

    private int _valeurAction;

    public int ValeurAction
    {
        get => _valeurAction;
        set => _valeurAction = Math.Max(0, value);
    }

    public int PointAction { get; set; } = 5;
    public List<string> Sprite { get; protected set; } = sprite;
    public List<Capacite> Capacites { get; protected set; } = [];

    public List<BonusMalus> Bonus = [];
    public List<BonusMalus> Malus = [];

    public void Blesser(int puissanceAttaque, int attaqueAssaillant, bool ignoreDefense = false)
    {
        var degats = puissanceAttaque * attaqueAssaillant;
        if (!ignoreDefense)
            degats /= (Defense + 10) / 10;
        PointDeVie = Math.Max(0, PointDeVie - degats);
        if (PointDeVie == 0)
            Vivant = false;
    }

    public void Soigner(float puissanceSoin)
    {
        PointDeVie = Math.Min(PointDeVieMax, PointDeVie + (int)(PointDeVieMax * puissanceSoin));
    }

    public void Ressuciter(float puissanceSoin)
    {
        if (!Vivant)
            Vivant = true;
        PointDeVie = Math.Min(PointDeVieMax, PointDeVie + (int)(PointDeVieMax * puissanceSoin));
    }

    public void ReinitialiserValeurAction()
    {
        _valeurAction = (int)Math.Round(10000f / Vitesse);
    }

    public void BonusMalusTour()
    {
        Bonus.ForEach(b => b.BaisserDuree());
        Malus.ForEach(m => m.BaisserDuree());
        Bonus.RemoveAll(b => !b.Actif);
        Malus.RemoveAll(m => !m.Actif);
    }

    public void AjouterCapacite(Capacite capacite)
    {
        if (Capacites.Count != 3)
            Capacites.Add(capacite);
    }
}