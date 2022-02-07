using System;
using System.Text.Json.Serialization;

namespace ValorantShop.Models;

public class RiotOffer
{
    [JsonInclude]
    public string OfferID;
    [JsonInclude]
    public bool IsDirectPurchase;
    [JsonInclude]
    public DateTime StartDate;
    [JsonInclude]
    public RiotOfferCost Cost;
    [JsonInclude]
    public RiotOfferReward[] Rewards;

    public class RiotOfferCost
    {
        [JsonPropertyName("85ad13f7-3d1b-5128-9eb2-7cd8ee0b5741")]
        [JsonInclude]
        public int InVp;
    }

    public class RiotOfferReward
    {
        [JsonInclude]
        public string ItemTypeID;
        [JsonInclude]
        public string ItemID;
        [JsonInclude]
        public int Quantity;
    }
}