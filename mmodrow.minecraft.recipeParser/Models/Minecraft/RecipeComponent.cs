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
                var name = Item;
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = Tag;
                }

                return name;
            }
        }

        [JsonIgnore]
        public bool IsTag => string.IsNullOrWhiteSpace(Item) && !string.IsNullOrWhiteSpace(Tag);

        public override string ToString()
        {
            return $"{Name} ({Count})";
        }

        [JsonIgnore]
        public bool IsEmpty => string.IsNullOrWhiteSpace(Item + Tag) && Count == 0;
    }
}
