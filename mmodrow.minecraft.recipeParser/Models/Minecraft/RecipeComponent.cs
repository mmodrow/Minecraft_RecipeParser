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

        public override string ToString()
        {
            var name = Item;
            if (string.IsNullOrWhiteSpace(name))
            {
                name = Tag;
            }

            return $"{name} ({Count})";
        }
    }
}
