#if TOOLS
using Godot;
using System;

[Tool]
public partial class UICurveEditorTool : EditorPlugin
{
    public override void _EnterTree()
    {
        RegisterCustomNode(
            "UICurveNode",
            "Control",
            "res://addons/SnowdramaTools/UICurve/UICurveNode.cs",
            "res://addons/SnowdramaTools/UICurve/UICurveIcon.svg");

        RegisterCustomNode(
            "UICurve",
            "Resource",
            "res://addons/SnowdramaTools/UICurve/UICurve.cs",
            "res://addons/SnowdramaTools/UICurve/UICurveIcon.svg");
    }

    private void RegisterCustomNode(
        string name,
        string baseNode,
        string scriptPath, 
        string texturePath)
    {
        Script script = GD.Load<Script>(scriptPath);
        Texture2D icon = GD.Load<Texture2D>(texturePath);
        AddCustomType(name, baseNode, script, icon);
    }

    public override void _ExitTree()
    {
        RemoveCustomType("UICurveNode");
        RemoveCustomType("UICurve");
    }
}
#endif
