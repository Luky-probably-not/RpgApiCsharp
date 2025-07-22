using Modeles.MoveSet;

namespace Modeles.Character;

public abstract class Entite(string nom, int att, int def, int pv, int vitesse, List<string> sprite)
{
    public int Niveau { get; set; } = 1;

    public int XpActuel = 0;
    public int XpBesoin = 100;

    public bool DoubleXp = false;
    public string Nom { get; set; } = nom;

    public int AttaqueDeBase {get; set;} = att;

    public int Attaque
    {
        get
        {
            var attBuff = Bonus.Find(b => b.Nom == "Attaque")?.Valeur ?? 1;
            var attNerf = Malus.Find(m => m.Nom == "Attaque")?.Valeur ?? 1;
            return (int)(AttaqueDeBase * attBuff * attNerf);
        }
    }
    public int DefenseDeBase {get; set; } = def;

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
    public int PointDeVieMax {get; set; } = pv;
    public int Vitesse {get; set;} = vitesse;
    public bool Vivant { get; set; } = true;

    private int _valeurAction;

    public int ValeurAction
    {
        get => _valeurAction;
        set => _valeurAction = Math.Max(0, value);
    }

    protected int _pointAction = 5;

    public int PointAction 
    {
        get => _pointAction;
        set => _pointAction = Math.Clamp(value,0,10);
    }
    public List<string> Sprite { get; set; } = sprite;
    public List<Capacite> Capacites { get; set; } = [];

    public List<BonusMalus> Bonus = [];
    public List<BonusMalus> Malus = [];

    public void MettreANiveau()
    {
        PointDeVie    = (int)Math.Round(PointDeVie    * (1f + Niveau / 10f));
        PointDeVieMax = (int)Math.Round(PointDeVieMax * (1f + Niveau / 10f));
        AttaqueDeBase = (int)Math.Round(AttaqueDeBase * (1f + Niveau / 10f));
        DefenseDeBase = (int)Math.Round(DefenseDeBase * (1f + Niveau / 10f));
    }

    public void Blesser(int puissanceAttaque, Entite assailant, bool ignoreDefense = false)
    {
        var degats = puissanceAttaque + assailant.Attaque;
        if (!ignoreDefense)
            degats -= (Defense + 10) / 10;
        var buffs = assailant.Bonus;
        var doubleDegats = buffs.Find(e => e.Nom == "DoubleDegats");
        if (doubleDegats != null)
        {
            degats *= 2;
            assailant.Bonus.Remove(doubleDegats);
        }

        var nerfs = Bonus.Find(e => e.Nom == "DiviseDegats");
        if (nerfs != null)
        {
            degats /= 2;
            Bonus.Remove(nerfs);
        }
        PointDeVie = Math.Max(0, PointDeVie - degats);
        if (PointDeVie == 0)
            Vivant = false;
    }

    public void Soigner(float puissanceSoin)
    {
        PointDeVie = Math.Min(PointDeVieMax, PointDeVie + (int)(PointDeVieMax * puissanceSoin));
    }

    public void Soigner(int puissanceSoin)
    {
        PointDeVie = Math.Min(PointDeVieMax, PointDeVie + puissanceSoin);
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

    public void FinTour(bool doubleXp)
    {
        DoubleXp = doubleXp ? doubleXp : DoubleXp;
        BonusMalusTour();
        ReinitialiserValeurAction();
    }

    public void FinCombat(int xp)
    {
        Bonus = [];
        Malus = [];
        PointAction = 5;
        NiveauSup(xp);
        ReinitialiserValeurAction();
    }

    public void NiveauSup(int xp)
    {
        XpActuel += DoubleXp ? xp*2 : xp;
        if (DoubleXp) DoubleXp = false;
        if (XpActuel < XpBesoin) return;
        Niveau++;
        XpActuel -= XpBesoin;
        XpBesoin = Niveau ^ 2 + 100;
        MettreANiveau();
    }
}