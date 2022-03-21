using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Mmodrow.Minecraft.RecipeParser.Models.Minecraft
{
    internal class Recipe
    {
        [JsonPropertyName("type")]
        internal string Type { get; set; } = "";

        [JsonPropertyName("group")]
        internal string Group { get; set; } = "";

        [JsonPropertyName("pattern")]
        internal string[] Pattern = Array.Empty<string>();

        [JsonPropertyName("key")]
        internal Dictionary<string, RecipeComponent> Key { get; set; } = new();

        [JsonPropertyName("ingredients")]
        internal List<RecipeComponent> Ingredients { get; set; } = new();

        [JsonPropertyName("result")]
        internal RecipeComponent Result { get; set; } = new();

        [JsonPropertyName("experience")] internal double? Experience { get; set; } = null;

        /// <summary>
        /// Cooking/Smelting/Smoking/Blasting time in game ticks (20gt = 1s)
        /// </summary>
        [JsonPropertyName("cookingtime")]
        internal decimal? CookingTime { get; set; } = 0;

        [JsonIgnore] internal RecipeType ParsedType { get; set; } = RecipeType.NotSet;

        public override string ToString()
        {
            return $"{Result} => {ParsedType}";
        }
    }
}
