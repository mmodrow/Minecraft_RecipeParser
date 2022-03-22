using Mmodrow.Minecraft.RecipeParser.Models.Minecraft;
using System.Text.Json;

namespace Mmodrow.Minecraft.RecipeParser.Parser
{
    internal class RecipeParser : BaseJarParser
    {
        private readonly NamingMapper namingMapper;
        private readonly IDictionary<string, Tag> tags;

        internal RecipeParser(JarReader jarReader, NamingMapper namingMapper, IDictionary<string,Tag> tags) : base(jarReader, @"data/minecraft/recipes/")
        {
            this.namingMapper = namingMapper;
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
                var deserialized = JsonSerializer.Deserialize<Recipe>(recipeJsonString);

                if (deserialized is null)
                {
                    throw new NullReferenceException(nameof(deserialized));
                }

                if (Enum.TryParse<RecipeType>(namingMapper.MinecraftNameToEnumName(deserialized.Type), out var parsedRecipeType))
                {
                    deserialized.ParsedType = parsedRecipeType;
                }

                if (!recipes.ContainsKey(deserialized.ResultName))
                {
                    recipes[deserialized.ResultName] = new List<Recipe>();
                }

                recipes[deserialized.ResultName].Add(deserialized);
            }
            return recipes;
        }
    }
}
