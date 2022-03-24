using System.Collections;
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
        public Dictionary<string, object> DeserializedKey
        {
            get => this.Key;
            set
            {
                foreach (var valueKey in value.Keys)
                {
                    this.Key[valueKey] = this.ParseRecipeComponentListFromDifferentTypes(value[valueKey]);
                }
            }
        }

        [JsonIgnore]
        public Dictionary<string, object> Key { get; set; } = new();
        
        [JsonPropertyName("ingredient")]
        public object DeserializedIngredient
        {
            get => this.Ingredients.ToList<object>();
            set
            {
                this.Ingredients = this.ParseRecipeComponentListFromDifferentTypes(value);
            }
        }

        /// <summary>
        /// RecipeComponent or IList[RecipeComponent]
        /// </summary>
        [JsonPropertyName("ingredients")]
        public List<object> DeserializedIngredients
        {
            get => this.Ingredients.ToList<object>();
            set
            {
                this.Ingredients = this.ParseRecipeComponentListFromDifferentTypes(value);
            }
        }

        [JsonIgnore] public IList<RecipeComponent> Ingredients { get; set; } = new List<RecipeComponent>();

        [JsonPropertyName("result")]
        public object DeserializedResult { 
            set
            {
                var recipeComponent = this.ParseRecipeComponentFromDifferentTypes(value);

                if (recipeComponent.IsEmpty)
                {
                    return;
                }

                this.Result = recipeComponent;
            }
        }

        private IList<RecipeComponent> ParseRecipeComponentListFromDifferentTypes(object deserializedRecipeComponent)
        {
            if (deserializedRecipeComponent is not JsonElement {ValueKind: JsonValueKind.Array} valueElement)
            {
                return new List<RecipeComponent> {this.ParseRecipeComponentFromDifferentTypes(deserializedRecipeComponent)};
            }

            var recipeComponents = new List<RecipeComponent>();
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var individualComponent in valueElement.EnumerateArray())
            {
                var deserializedIndividualComponent = individualComponent.Deserialize<RecipeComponent>();
                if (deserializedIndividualComponent is not null && !deserializedIndividualComponent.IsEmpty)
                {
                    recipeComponents.Add(deserializedIndividualComponent);
                }
            }

            return recipeComponents;

        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        // ReSharper disable once MemberCanBeMadeStatic.Local
        private RecipeComponent ParseRecipeComponentFromDifferentTypes(object deserializedRecipeComponent)
        {
            RecipeComponent recipeComponent = new();

            switch (deserializedRecipeComponent)
            {
                case JsonObject deserializedObject:
                    recipeComponent = JsonSerializer.Deserialize<RecipeComponent>(deserializedObject.ToJsonString()) ??
                                      recipeComponent;
                    break;
                case JsonElement deserializedElement:
                    switch (deserializedElement.ValueKind)
                    {
                        case JsonValueKind.String:
                            recipeComponent.Item = deserializedElement.GetRawText().Trim('"');
                            break;
                        case JsonValueKind.Object:
                            recipeComponent = deserializedElement.Deserialize<RecipeComponent>() ?? recipeComponent;
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
                case IList deserializedList:
                    if (deserializedList.Count == 1)
                    {
                        recipeComponent = JsonSerializer.Deserialize<RecipeComponent>(deserializedList[0]?.ToString() ?? string.Empty) ??
                                          recipeComponent;
                    }
                    else
                    {
                        //TODO: here we have alternative items, that are not defined by a common tag
                        recipeComponent = recipeComponent;
                    }
                    break;
                default:
                    recipeComponent.Item = deserializedRecipeComponent.ToString() ?? string.Empty;
                    break;
            }

            return recipeComponent;
        }

        [JsonIgnore] public RecipeComponent Result { get; set; } = new();
        
        [JsonPropertyName("count")] public int Count
        {
            get => this.Result.Count;
            set => this.Result.Count = value;
        }

        [JsonPropertyName("experience")] internal double? Experience { get; set; } = null;

        /// <summary>
        /// Cooking/Smelting/Smoking/Blasting time in game ticks (20gt = 1s)
        /// </summary>
        [JsonPropertyName("cookingtime")]
        public decimal CookingTime { get; set; } = 0;

        [JsonIgnore] public RecipeType ParsedType { get; set; } = RecipeType.NotSet;

        [JsonIgnore]
        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(this.Type + this.Group )
            && !this.Pattern.Any()
            && !this.DeserializedKey.Any()
            && !this.DeserializedIngredients.Any()
            && !this.Result.IsEmpty
            && this.Experience is null
            && this.CookingTime == 0;

        public override string ToString()
        {
            return $"{this.Result.Name} => {this.ParsedType}";
        }
    }
}
