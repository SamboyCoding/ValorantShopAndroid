using Android.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Android.Widget;
using ValNet;
using WebSocketSharp.Net;

namespace ValorantShop;

public class RiotUserManager
{
    public static RiotUser CurrentUser = new();

    private static string _credsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "creds.json");
    
    private static readonly Uri RiotUri = new("https://auth.riotgames.com");

    public static bool HasAuthData => File.Exists(_credsPath);
    
    public static async Task LoginFromCookies(string cookieString)
    {
        var cookieStrings = cookieString.Split(';');
        foreach (var cookie in cookieStrings)
        {
            CurrentUser.UserClient.CookieContainer.SetCookies(RiotUri, cookie);
        }

        await CurrentUser.Authentication.AuthenticateWithCookies();
        SaveCreds();
    }

    public static async Task<bool> LoginFromSavedCredentials()
    {
        PopulateCreds();
        try
        {
            await CurrentUser.Authentication.AuthenticateWithCookies();
            return true;
        }
        catch (Exception e)
        {
            Toast.MakeText(MainActivity.Instance, "Failed to authenticate with saved credentials. You may have been logged out due to not selecting \"remember me\".", ToastLength.Long).Show();
            Console.WriteLine(e);
            CurrentUser = new();
            return false;
        } 
    }

    private static void SaveCreds()
    {
        List<string> cookieStrings = new();
        foreach (string cookie in CurrentUser.UserClient.CookieContainer.GetCookieHeader(RiotUri).Split(';'))
        {
            cookieStrings.Add(cookie);
        }
        
        File.WriteAllText(_credsPath, JsonSerializer.Serialize(cookieStrings));
    }

    private static void PopulateCreds()
    {
        var creds = File.ReadAllText(_credsPath);
        var cookieStrings = JsonSerializer.Deserialize<List<string>>(creds);
        foreach (var cookieString in cookieStrings)
        {
            CurrentUser.UserClient.CookieContainer.SetCookies(RiotUri, cookieString);
        }
    }

}