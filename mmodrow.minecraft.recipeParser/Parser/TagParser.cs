using Newtonsoft.Json;
using System.Linq;
using Mmodrow.Minecraft.RecipeParser.Models.Minecraft;

namespace Mmodrow.Minecraft.RecipeParser.Parser;

public class TagParser : BaseJarParser
{
    private readonly TagType tagType;
    private readonly NamingMapper namingMapper;
    internal string TagDirectoryName => namingMapper.EnumNameToMinecraftName(tagType.ToString(), false);

    internal TagParser(JarReader jarReader, NamingMapper namingMapper, TagType tagType) : base(jarReader, @"data/minecraft/tags/")
    {
        this.tagType = tagType;
        this.namingMapper = namingMapper;
    }

    internal Dictionary<string, string> GetJsonStrings(string fileNamePrefix = "")
    {
        return base.GetJsonStrings(fileNamePrefix, TagDirectoryName);
    }

    internal Dictionary<string,Tag> GetTags()
    {
        var tagJsonStrings = GetJsonStrings();

        var tags = tagJsonStrings.Select(kvp =>
        {
            var (key, value) = kvp;
            var deserialized = JsonConvert.DeserializeObject<Tag>(value);
            if (deserialized is null)
            {
                return null;
            }

            deserialized.Name = key.Split("/").Last().Split(".").First();
            return deserialized;
        }).Where(e => e is not null).Select(e => e!).ToArray();

        foreach (var tag in tags)
        {
            FlattenValues(tag, tags);
        }

        return tags.ToDictionary(t => t.Name, t => t);
    }

    internal void FlattenValues(Tag tag, ICollection<Tag> tags)
    {
        if (tag.FlattenedValues.Count >= tag.Values.Length)
        {
            return;
        }

        foreach (var value in tag.Values)
        {
            var name = value.Replace("minecraft:", string.Empty);
            if (name.StartsWith("#"))
            {
                name = name.Replace("#", string.Empty);
                var referencedTag = tags.First(t => t.Name == name);
                FlattenValues(referencedTag, tags);
                tag.FlattenedValues.AddRange(referencedTag.FlattenedValues);
            }
            else
            {
                tag.FlattenedValues.Add(name);
            }
        }
    }
}