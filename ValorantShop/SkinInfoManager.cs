using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ValorantShop.Models;

namespace ValorantShop;

public static class SkinInfoManager
{
    private const string SkinsInfoUrl = "https://valorant-api.com/v1/weapons/skins";
    
    private static string _pricesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "prices.json");
    private static string _skinsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "skins.json");

    private static List<RiotOffer>? _priceInfo;
    private static List<ValApiSkin>? _skinInfo;

    private static void LoadPriceInfoFromCache()
    {
        if (!File.Exists("prices.json"))
        {
            _priceInfo = new();
            return;
        }

        _priceInfo = JsonSerializer.Deserialize<List<RiotOffer>>(File.ReadAllText(_pricesPath)) ?? throw new("Failed to deserialize prices.json");
    }

    private static void LoadSkinInfoFromCache()
    {
        if (!File.Exists("skins.json"))
        {
            _skinInfo = new();
            return;
        }

        _skinInfo = JsonSerializer.Deserialize<List<ValApiSkin>>(File.ReadAllText(_skinsPath)) ?? throw new("Failed to deserialize skins.json");
    }

    private static async Task LoadPriceInfoFromRiot()
    {
        _priceInfo = JsonSerializer.Deserialize<RiotOffersResponse>((await RiotUserManager.CurrentUser.Store.GetStoreOffers()).ToString())!.Offers.ToList();
        await File.WriteAllTextAsync(_pricesPath, JsonSerializer.Serialize(_priceInfo));
    }

    private static async Task LoadSkinInfoFromValApi()
    {
        var client = new HttpClient {DefaultRequestHeaders = {{"User-Agent", "Valorant Shop Viewer Proof-Of-Concept (Samboy#0063 on discord)"}}};
        var response = await client.GetFromJsonAsync<ValorantApiResponse<ValApiSkin[]>>(SkinsInfoUrl) ?? throw new("Failed to load skins from Valorant-API");
        _skinInfo = response.data.ToList();
        await File.WriteAllTextAsync(_skinsPath, JsonSerializer.Serialize(_skinInfo));
    }

    public static async Task<int> GetPriceForSkin(string uuid)
    {
        if (_priceInfo == null)
            LoadPriceInfoFromCache();
        
        if(_priceInfo!.All(s => s.OfferID != uuid))
            //Skin not found, try to load it from the API
            await LoadPriceInfoFromRiot();
        
        return _priceInfo!.FirstOrDefault(s => s.OfferID == uuid)?.Cost.InVp ?? 9999;
    }

    public static async Task<(string name, string? imageUrl)> GetNameAndImageForSkin(string uuid)
    {
        if(_skinInfo == null)
            LoadSkinInfoFromCache();
        
        if(_skinInfo!.All(s => s.levels[0].uuid != uuid))
            //Skin not found, try to load it from the API
            await LoadSkinInfoFromValApi();
        
        var info = _skinInfo!.FirstOrDefault(s => s.levels[0].uuid == uuid);

        return (info?.displayName ?? $"Unknown Skin {uuid}", info?.levels[0].displayIcon);
    }
}