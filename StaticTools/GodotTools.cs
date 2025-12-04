using Godot;

namespace Snowdrama.Core;
public static class GodotTools
{
    public static bool IsNodeSelectedEditor(Node possiblySelectedNode)
    {
#if TOOLS
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
#endif
        return false;
    }
}
