using System.Net.Http.Headers;
using System.Net.Http.Json;
using Modeles.Character;
using Modeles.FonctionsJeu.MiniGames;
using Modeles.LabyrintheLogique;
using static Modeles.Json.Options;
namespace Modeles.FonctionsJeu;

public class AppelsApi
{
    private static readonly HttpClient Client = SetupClient("http://localhost:5019");
    
    public static async Task<Labyrinthe?> GetLabyrinthe(int taille)
    {
        var response = await Client.GetAsync($"/api/labyrinthe/{taille}");
        var laby = await response.Content.ReadFromJsonAsync<Labyrinthe>();
        return laby;
    }

    public static async Task<Expedition?> GetExpedition()
    {
        var response = await Client.GetAsync($"/api/equipe");
        var expe = await response.Content.ReadFromJsonAsync<Expedition>(OptionsJson);
        return expe;
    }

    public static async Task<List<Entite>?> GetEnnemies(int niveau)
    {
        var response = await Client.GetAsync($"api/ennemie/{niveau}");
        var ennemies = await response.Content.ReadFromJsonAsync<List<Entite>>(OptionsJson);
        ennemies!.ForEach(e => e.ReinitialiserValeurAction());
        ennemies!.ForEach(e => e.MettreANiveau());
        return ennemies;
    }

    public static async Task<Dictionary<string, int>> GetRecompenses(int sommeNiveau)
    {
        var response = await Client.GetAsync($"api/loot/{sommeNiveau}");
        return (await response.Content.ReadFromJsonAsync<Dictionary<string, int>>(OptionsJson))!;
    }

    public static async Task<Dictionary<string, int>> GetRecompenseTiming(int sommeNiveau, string couleur)
    {
        var response = await Client.GetAsync($"api/loot/{sommeNiveau},{couleur}");
        return (await response.Content.ReadFromJsonAsync<Dictionary<string, int>>(OptionsJson))!;
    }

    public static async Task<Magasin> GetMagasin(int sommeNiveau)
    {
        var response = await Client.GetAsync($"api/magasin/{sommeNiveau}");
        return (await response.Content.ReadFromJsonAsync<Magasin>(OptionsJson))!;
    }

    public static async Task<MiniJeu> GetMiniJeu(int sommeNiveau)
    {
        var response = await Client.GetAsync($"api/minijeu/{sommeNiveau}");
        return (await response.Content.ReadFromJsonAsync<MiniJeu>(OptionsJson))!;
    }

    private static HttpClient SetupClient(string adresseApi)
    {
        HttpClient client = new()
        {
            BaseAddress = new Uri(adresseApi)
        };

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        return client;
    }
}