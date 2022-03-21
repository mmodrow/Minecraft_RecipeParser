using System.Text.RegularExpressions;

namespace Mmodrow.Minecraft.RecipeParser.Parser;

public class NamingMapper
{
    private readonly Regex pascalCaseWordBreak = new("((?!^).)([A-Z])");
    private readonly Regex snakeCaseWordBreak = new("_([a-z0-9])");
    private readonly Regex namespacePrefix = new("^([^:]+:)");
    private readonly Regex tagFlagPrefix = new("^(#)");

    internal string MinecraftNameToEnumName(string minecraftName)
    {
        return SnakeCaseToPascalCase(minecraftName);
    }

    internal string EnumNameToMinecraftName(string minecraftName, bool addMinecraftNamespace = true)
    {
        return PascalCaseToSnakeCase(minecraftName, addMinecraftNamespace ? "minecraft:" : string.Empty);
    }

    internal string PascalCaseToSnakeCase(string pascalCasedName, string prefix)
    {
        return prefix + pascalCaseWordBreak.Replace(pascalCasedName, @"$1_\L$2").ToLower();
    }

    internal string SnakeCaseToPascalCase(string snakeCasedName, bool dropNamespacePrefix = true, bool dropTagFlag = true)
    {
        if (dropNamespacePrefix)
        {
            snakeCasedName = namespacePrefix.Replace(snakeCasedName, "");
        }

        if (dropTagFlag)
        {
            snakeCasedName = tagFlagPrefix.Replace(snakeCasedName, "");
        }

        return snakeCaseWordBreak.Replace(snakeCasedName, @"\U$1");
    }

    
}