using CommandLine;

namespace mmodrow.minecraft.recipeParser;

internal class RecipeParserOptions
{
    [Option('p', "JarPath", Required = true, HelpText = "Input file to be processed.")]
    public string? JarPath { get; set; }


    [Option('c', "CollapseTags", Required = true, HelpText = "If set to true, tags will be collapsed on their first entry to form recipes.")]
    public bool CollapseTags { get; set; }
}