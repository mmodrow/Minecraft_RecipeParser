using Mmodrow.Minecraft.RecipeParser.Models.Minecraft;
using Newtonsoft.Json;

namespace Mmodrow.Minecraft.RecipeParser.Parser
{
    internal class RecipeParser : BaseJarParser
    {
        private readonly IDictionary<string, Tag> tags;

        internal RecipeParser(JarReader jarReader, IDictionary<string,Tag> tags) : base(jarReader, @"data/minecraft/recipes/")
        {
            this.tags = tags ?? throw new ArgumentNullException(nameof(tags));
        }

        internal Dictionary<string, string> GetJsonStrings(string recipePrefix = "")
        {
            return base.GetJsonStrings(recipePrefix);
        }

        public IDictionary<string,ICollection<Recipe>> GetRecipes(string recipePrefix = "")
        {
            var recipes = new Dictionary<string, ICollection<Recipe>>();

            var recipeJsonStrings = GetJsonStrings(recipePrefix).Values;

            foreach (var recipeJsonString in recipeJsonStrings.ToArray())
            {
                var deserialized = JsonConvert.DeserializeObject<Recipe>(recipeJsonString);
                Enum.TryParse<RecipeType>(deserialized.Type, out var parsedRecipeType);

                deserialized.ParsedType = parsedRecipeType;
            }
            return recipes;
        }
    }
}
