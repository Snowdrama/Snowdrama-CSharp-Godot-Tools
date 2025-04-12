using Godot;
using System;

namespace Snowdrama.Core;
public static class GodotTools
{
    public static bool IsNodeSelectedEditor(Node possiblySelectedNode)
    {
        if (Engine.IsEditorHint())
        {
            var selectedNodes = EditorInterface.Singleton.GetSelection().GetSelectedNodes();
            if (selectedNodes != null && 
                selectedNodes.Count > 0 && 
                selectedNodes[0] == possiblySelectedNode)
            {
                return true;
            }
        }
        return false;
    }
}
