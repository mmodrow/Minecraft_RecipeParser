using System.Text.Json.Serialization;

namespace Mmodrow.Minecraft.RecipeParser.Models.Minecraft
{
    internal class Tag
    {
        [JsonIgnore]
        internal string Name { get; set; } = "";

        [JsonPropertyName("replace")]
        internal bool Replace { get; set; }

        [JsonPropertyName("values")] public string[] Values { get; set; } = Array.Empty<string>();

        [JsonIgnore] internal List<string> FlattenedValues { get; set; } = new();

        public override string ToString()
        {
            return Name;
        }
    }
}
