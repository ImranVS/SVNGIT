using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ComboBoxListItem
    {
        [JsonProperty("text")]
        public string DisplayText { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
