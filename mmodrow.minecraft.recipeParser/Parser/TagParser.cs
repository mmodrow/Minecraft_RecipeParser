using System.Text.Json;
using Mmodrow.Minecraft.RecipeParser.Models.Minecraft;

namespace Mmodrow.Minecraft.RecipeParser.Parser;

public class TagParser : BaseJarParser
{
    private readonly TagType tagType;
    private readonly NamingMapper namingMapper;
    internal string TagDirectoryName => this.namingMapper.EnumNameToMinecraftName(this.tagType.ToString(), false);

    internal TagParser(JarReader jarReader, NamingMapper namingMapper, TagType tagType) : base(jarReader, @"data/minecraft/tags/")
    {
        this.tagType = tagType;
        this.namingMapper = namingMapper;
    }

    internal Dictionary<string, string> GetJsonStrings(string fileNamePrefix = "")
    {
        return base.GetJsonStrings(fileNamePrefix, this.TagDirectoryName);
    }

    internal Dictionary<string,Tag> GetTags(bool collapseTags)
    {
        var tagJsonStrings = this.GetJsonStrings();

        var tags = tagJsonStrings.Select(kvp =>
        {
            var (key, value) = kvp;
            var deserialized = JsonSerializer.Deserialize<Tag>(value);
            if (deserialized is null)
            {
                return null;
            }

            deserialized.Name = key.Split("/").Last().Split(".").First();
            return deserialized;
        }).Where(e => e is not null).Select(e => e!).ToArray();

        foreach (var tag in tags)
        {
            this.FlattenValues(tag, tags);
            if (collapseTags)
            {
                CollapseTag(tag);
            }
        }

        return tags.ToDictionary(t => t.Name, t => t);
    }

    private static void CollapseTag(Tag tag)
    {
        var oakItem = tag.FlattenedValues.FirstOrDefault(value => value.StartsWith("oak"));
        if (oakItem is not null)
        {
            tag.FlattenedValues = new List<string> {oakItem};
        }
        
        tag.FlattenedValues = tag.FlattenedValues.Take(1).ToList();
    }

    internal void FlattenValues(Tag tag, ICollection<Tag> tags)
    {
        if (tag.FlattenedValues.Any())
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
                this.FlattenValues(referencedTag, tags);
                tag.FlattenedValues.AddRange(referencedTag.FlattenedValues);
            }
            else
            {
                tag.FlattenedValues.Add(name);
            }
        }
    }
}