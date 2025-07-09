using Modeles;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Modeles.LabyrintheLogique;
using Newtonsoft.Json;

HttpClient client = new ();

async Task<Labyrinthe?> GetLabyrinthe(int taille)
{
    HttpResponseMessage response = await client.GetAsync($"/api/labyrinthe/{taille}");
    var laby = await response.Content.ReadFromJsonAsync<Labyrinthe>();
    return laby;
}

async Task<Labyrinthe?> RunAsync(int tailleLaby)
{
    client.BaseAddress = new Uri("http://localhost:5019");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

    try
    {
        return await GetLabyrinthe(tailleLaby);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}
/*
var guerrier = new Guerrier();
var sorcier = new Sorcier();
var elfe = new Elfe();

var zombie = new Zombie();
var zombie2 = new Zombie();

List<Personnage> equipe = [guerrier, elfe, sorcier];
List<IEnnemie> ennemies = [zombie, zombie2];


foreach (var per in equipe)
{
    var attaque = per.Capacites()[0];
    if (!attaque.CibleAllie())
    {
        if (attaque.Zone()) {
            attaque.Utiliser(per, ennemies);
            if (attaque is ISpecial)
                (attaque as ISpecial)!.SpecialAction(per, ennemies);
        } else {
            attaque.Utiliser(per, ennemies[0]);
            if (attaque is ISpecial)
                (attaque as ISpecial)!.SpecialAction(per, ennemies[0]);
        }
    } else {
        if (attaque.Zone()) {
            attaque.Utiliser(per, equipe);
            if (attaque is ISpecial)
                (attaque as ISpecial)!.SpecialAction(per, equipe);
        } else {
            attaque.Utiliser(per, equipe[0]);
            if (attaque is ISpecial)
                (attaque as ISpecial)!.SpecialAction(per, equipe[0]);
        }
    }
}

equipe.ForEach(e => e.Display());
ennemies.ForEach(e => e.Display());
*/

GameManager.Instance.Laby = RunAsync(10).GetAwaiter().GetResult()!;
GameManager.Instance.Play();
