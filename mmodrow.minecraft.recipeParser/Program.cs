using CommandLine;
using mmodrow.minecraft.recipeParser;

var importer = new ImportModelsFromJar();
Parser.Default.ParseArguments<RecipeParserOptions>(args)
    .WithParsed(importer.Import)
    .WithNotParsed(_ => Environment.ExitCode = 3);

Console.WriteLine("end");