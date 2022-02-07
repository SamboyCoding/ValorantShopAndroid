using System.Text.Json.Serialization;

namespace ValorantShop.Models;

public class ValApiSkin
{
    [JsonInclude]
    public string uuid;
    [JsonInclude]
    public string displayName;
    [JsonInclude]
    public string displayIcon;
    [JsonInclude]
    public ValApiSkinLevel[] levels;
    
    public class ValApiSkinLevel
    {
        [JsonInclude]
        public string uuid;
        [JsonInclude]
        public string displayName;
        [JsonInclude]
        public string displayIcon;
    }
}