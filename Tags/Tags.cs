using Godot;
using System;

/// <summary>
/// Makes a node have a list of tags that it can be found by using TagManager
/// 
/// Note that this adds the parent node of the one that this is on, so this should always be an immediate
/// child of the node you want to able to access
/// </summary>
public partial class Tags : Node
{
    [Export] string[] tags = new string[0];
    public override void _EnterTree()
    {
        for (int i = 0; i < tags.Length; i++)
        {
            TagManager.TagObject(tags[i], this.GetParent());
        }
    }

    public override void _ExitTree()
    {
        for (int i = 0; i < tags.Length; i++)
        {
            TagManager.UntagObject(tags[i], this.GetParent());
        }
    }
}
