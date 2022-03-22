using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Mmodrow.Minecraft.RecipeParser.Models.Minecraft
{
    internal class Recipe
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "";

        [JsonPropertyName("group")]
        public string Group { get; set; } = "";

        [JsonPropertyName("pattern")]
        public string[] Pattern { get; set; }  = Array.Empty<string>();

        /// <summary>
        /// string, RecipeComponent or IList[RecipeComponent]
        /// </summary>
        [JsonPropertyName("key")]
        public Dictionary<string, object> Key { get; set; } = new();

        /// <summary>
        /// RecipeComponent or IList[RecipeComponent]
        /// </summary>
        [JsonPropertyName("ingredients")]
        public List<object> Ingredients { get; set; } = new();

        [JsonPropertyName("result")]
        public object Result { 
            set
            {
                RecipeComponent recipeComponent = new();
                switch (value)
                {
                    case JsonObject valueObject:
                        recipeComponent = JsonSerializer.Deserialize <RecipeComponent>(valueObject.ToJsonString()) ?? recipeComponent;

                        this.IsResultTag = recipeComponent.IsTag;

                        this.ResultName = !string.IsNullOrWhiteSpace(recipeComponent.Item) ? recipeComponent.Item :
                            !string.IsNullOrWhiteSpace(recipeComponent.Tag) ? recipeComponent.Tag : string.Empty;

                        this.ResultCount = Math.Max(recipeComponent.Count, 1);
                        break;
                    case JsonElement valueElement:
                        switch (valueElement.ValueKind)
                        {
                            case JsonValueKind.String:
                                this.ResultName = valueElement.GetRawText().Trim('"');
                                break;
                            case JsonValueKind.Object:
                                recipeComponent = valueElement.Deserialize<RecipeComponent>() ?? recipeComponent;
                                break;
                            case JsonValueKind.Undefined:
                            case JsonValueKind.Array:
                            case JsonValueKind.Number:
                            case JsonValueKind.True:
                            case JsonValueKind.False:
                            case JsonValueKind.Null:
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;
                    default:
                        this.ResultName = value.ToString() ?? string.Empty;
                        break;
                }

                if (recipeComponent.IsEmpty)
                {
                    return;
                }

                ResultName = recipeComponent.Name;
                IsResultTag = recipeComponent.IsTag;
                ResultCount = recipeComponent.Count;
            }
        }

        [JsonIgnore] public string ResultName { get; set; } = "";

        [JsonIgnore]
        public bool IsResultTag { get; set; }

        [JsonPropertyName("count")] public int Count
        {
            get => ResultCount;
            set => ResultCount = value;
        }

        [JsonIgnore] public int ResultCount { get; set; } = 1;

        [JsonPropertyName("experience")] internal double? Experience { get; set; } = null;

        /// <summary>
        /// Cooking/Smelting/Smoking/Blasting time in game ticks (20gt = 1s)
        /// </summary>
        [JsonPropertyName("cookingtime")]
        public decimal CookingTime { get; set; } = 0;

        [JsonIgnore] public RecipeType ParsedType { get; set; } = RecipeType.NotSet;

        [JsonIgnore]
        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(Type + Group + ResultName)
            && !Pattern.Any()
            && !Key.Any()
            && !Ingredients.Any()
            && !IsResultTag
            && ResultCount == 1
            && Experience is null
            && CookingTime == 0;

        public override string ToString()
        {
            return $"{ResultName} => {ParsedType}";
        }
    }
}
