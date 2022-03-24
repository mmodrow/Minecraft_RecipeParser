using System.Text.Json.Serialization;

namespace Mmodrow.Minecraft.RecipeParser.Models.Minecraft
{
    internal class RecipeComponent
    {
        [JsonPropertyName("item")]
        public string Item { get; set; } = "";

        [JsonPropertyName("tag")]
        public string Tag { get; set; } = "";

        [JsonPropertyName("count")]
        public int Count { get; set; } = 1;

        [JsonIgnore]
        public string Name
        {
            get
            {
                var name = this.Item;
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = this.Tag;
                }

                return name;
            }
        }

        [JsonIgnore]
        public bool IsTag => string.IsNullOrWhiteSpace(this.Item) && !string.IsNullOrWhiteSpace(this.Tag);

        public override string ToString()
        {
            return $"{this.Name} ({this.Count})";
        }

        [JsonIgnore]
        public bool IsEmpty => string.IsNullOrWhiteSpace(this.Item + this.Tag) && this.Count <= 1;
    }
}
