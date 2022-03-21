using System.IO.Compression;

namespace Mmodrow.Minecraft.RecipeParser.Parser;

public class JarReader
{
    private readonly ZipArchive jarFile;


    internal JarReader(string jarPath)
    {
        if (!File.Exists(jarPath) || !jarPath.EndsWith(".jar"))
        {
            var message = string.Format($"{0} needs to point to a valid jar file. The given path was '{1}'" , nameof(jarPath), jarPath);
            throw new ArgumentException(message);
        }
        this.jarFile = ZipFile.Open(jarPath, ZipArchiveMode.Read);
    }
    
    internal Dictionary<string,string> GetJsonStringsFromJarDirectory(string directory, string fileNamePrefix = "")
    {
        if (!directory.EndsWith("/"))
        {
            directory += "/";
        }
        var recipePathPrefix = directory + fileNamePrefix;
        var recipeFilePaths = jarFile.Entries.Where(entry => entry.FullName.StartsWith(recipePathPrefix)).ToArray();
        var recipeFileContents = recipeFilePaths
            .Select(entry => new KeyValuePair<string,string>(
                entry.FullName,
                new StreamReader(entry.Open()).ReadToEnd()))
            .ToDictionary(x => x.Key, x => x.Value);

        return recipeFileContents;
    }
}