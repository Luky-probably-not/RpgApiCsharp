using Modeles.FonctionsJeu;

GameManager.Instance.Debut(5).GetAwaiter().GetResult();
foreach (var kvp in GameManager.Instance.expedition.Sac)
{
    Console.WriteLine("cle : {0}; value : {1}",kvp.Key, kvp.Value);
}