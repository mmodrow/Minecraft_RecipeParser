using System.Text.Json.Serialization;

namespace Mmodrow.Minecraft.RecipeParser.Models.Minecraft
{
    internal class Recipe
    {
        [JsonPropertyName("type")]
        internal string Type { get; set; } = "";
        [JsonPropertyName("pattern")]
        internal string[] Pattern = Array.Empty<string>();
        [JsonPropertyName("key")]
        internal IDictionary<string, RecipeComponent> Key { get; set; } = new Dictionary<string, RecipeComponent>();

        [JsonPropertyName("ingredients")]
        internal ICollection<RecipeComponent> Ingredients { get; set; } = new List<RecipeComponent>();

        [JsonPropertyName("result")]
        internal RecipeComponent Result { get; set; } = new();

        [JsonPropertyName("experience")]
        internal double? Experience { get; set; }

        /// <summary>
        /// Cooking/Smelting/Smoking/Blasting time in game ticks (20gt = 1s)
        /// </summary>
        [JsonPropertyName("cookingtime")]
        internal decimal? CookingTime { get; set; }

        [JsonIgnore]
        internal RecipeType ParsedType { get; set; }

        public override string ToString()
        {
            return $"{Result} => {ParsedType}";
        }
    }
}
