using System.Text.Json.Serialization;

namespace ValorantShop.Models;

public class RiotOffersResponse
{
    [JsonInclude]
    public RiotOffer[] Offers;
}