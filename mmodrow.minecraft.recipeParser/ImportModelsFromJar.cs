using Mmodrow.Minecraft.RecipeParser.Models.Minecraft;
using Mmodrow.Minecraft.RecipeParser.Parser;

namespace mmodrow.minecraft.recipeParser
{
    internal class ImportModelsFromJar
    {
        internal void Import(RecipeParserOptions args)
        {
            var jarReader = new JarReader(args.JarPath!);

            var namingMapper = new NamingMapper();
            var itemTagParser = new TagParser(jarReader, namingMapper, TagType.Items);
            var tags = itemTagParser.GetTags(args.CollapseTags);

            var recipeParser = new RecipeParser(jarReader, namingMapper, tags);
            var recipeJsonStrings = recipeParser.GetRecipes();
        }
    }
}
