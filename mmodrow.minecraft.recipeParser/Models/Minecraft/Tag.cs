using System.Text.Json.Serialization;

namespace Mmodrow.Minecraft.RecipeParser.Models.Minecraft
{
    internal class Tag
    {
        [JsonIgnore]
        public string Name { get; set; } = "";

        [JsonPropertyName("replace")]
        public bool Replace { get; set; } = false;

        [JsonPropertyName("values")]
        public string[] Values { get; set; } = Array.Empty<string>();

        [JsonIgnore] internal List<string> FlattenedValues { get; set; } = new();

        public override string ToString()
        {
            return this.Name;
        }

        [JsonIgnore]
        public bool IsEmpty => this.Name == string.Empty && this.Replace == false && !this.Values.Any();
    }
}
