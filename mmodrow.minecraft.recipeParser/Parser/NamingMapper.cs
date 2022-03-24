using System.Text.RegularExpressions;

namespace Mmodrow.Minecraft.RecipeParser.Parser;

public class NamingMapper
{
    private readonly Regex pascalCaseWordBreak = new("((?!^).)([A-Z])");
    private readonly Regex namespacePrefix = new("^([^:]+:)");
    private readonly Regex tagFlagPrefix = new("^(#)");

    internal string MinecraftNameToEnumName(string minecraftName)
    {
        return this.SnakeCaseToPascalCase(minecraftName);
    }

    internal string EnumNameToMinecraftName(string minecraftName, bool addMinecraftNamespace = true)
    {
        return this.PascalCaseToSnakeCase(minecraftName, addMinecraftNamespace ? "minecraft:" : string.Empty);
    }

    internal string PascalCaseToSnakeCase(string pascalCasedName, string prefix)
    {
        return prefix + this.pascalCaseWordBreak.Replace(pascalCasedName, @"$1_\L$2").ToLower();
    }

    internal string SnakeCaseToPascalCase(string snakeCasedName, bool dropNamespacePrefix = true, bool dropTagFlag = true)
    {
        if (dropNamespacePrefix)
        {
            snakeCasedName = this.namespacePrefix.Replace(snakeCasedName, "");
        }

        if (dropTagFlag)
        {
            snakeCasedName = this.tagFlagPrefix.Replace(snakeCasedName, "");
        }

        return string.Join("" ,snakeCasedName.Split('_').Select(token => token[..1].ToUpper() + token[1..]));
    }

    
}