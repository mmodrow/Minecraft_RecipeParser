using Mmodrow.Minecraft.RecipeParser.Models.Minecraft;
using Mmodrow.Minecraft.RecipeParser.Parser;

Console.WriteLine("Hello, World!");
var jarReader = new JarReader(@"C:\Users\marcm\AppData\Roaming\.minecraft\versions\1.18.2\1.18.2.jar");

var itemTagParser = new TagParser(jarReader, new NamingMapper(), TagType.Items);
var tags = itemTagParser.GetTags();

var recipeParser = new RecipeParser(jarReader, tags);
var recipeJsonStrings = recipeParser.GetRecipes();


Console.WriteLine("end");