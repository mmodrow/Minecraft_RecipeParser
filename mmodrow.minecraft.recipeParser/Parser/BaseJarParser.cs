namespace Mmodrow.Minecraft.RecipeParser.Parser;

public class BaseJarParser
{
    private readonly JarReader jarReader;
    private readonly string dataDirectoryPrefix;

    internal BaseJarParser(JarReader jarReader, string dataDirectoryPrefix)
    {
        this.jarReader = jarReader;
        this.dataDirectoryPrefix = dataDirectoryPrefix;
    }

    protected Dictionary<string, string> GetJsonStrings(string fileNamePrefix = "", string dataDirectorySuffix = "")
    {
        var dataDirectory = this.dataDirectoryPrefix;
        if (string.IsNullOrWhiteSpace(dataDirectorySuffix))
        {
            return this.jarReader.GetJsonStringsFromJarDirectory(dataDirectory, fileNamePrefix);
        }

        if (!dataDirectory.EndsWith("/"))
        {
            dataDirectory += "/";
        }

        dataDirectory += dataDirectorySuffix;

        return this.jarReader.GetJsonStringsFromJarDirectory(dataDirectory, fileNamePrefix);
    }
}