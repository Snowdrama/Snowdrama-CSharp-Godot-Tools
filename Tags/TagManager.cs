using Godot;
using System;
using System.Collections.Generic;

public partial class TagManager : Node
{
	public static Dictionary<string, List<Node>> taggedObjects = new Dictionary<string, List<Node>>();

    public static void TagObject(string tag, Node node)
    {
        ValidateList(tag);
        taggedObjects[tag].Add(node);
    }

    public static void UntagObject(string tag, Node node)
    {
        ValidateList(tag);
        taggedObjects[tag].Remove(node);
    }

    private static void ValidateList(string tag)
    {
        if(!taggedObjects.ContainsKey(tag))
        {
            taggedObjects.Add(tag, new List<Node>());
        }
    }
}
