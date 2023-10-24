using System.Linq;
using Godot;

[Tool]
public partial class FitterTest : Container
{
    [Export(PropertyHint.Range, "0.0, 0.5")]
    float margin;
    private float oldMargin;
    private Vector2 oldSize;

#if TOOLS //removes below in release builds
    public override void _Process(double delta)
    {
        if (Engine.IsEditorHint())
        {
            //if the state changed 
            if(oldMargin != margin)
            {            
                UpdateContainerContents();
                oldMargin = margin;
            }
        }
    }
#endif
    

    public override void _Notification(int what)
    {
        if (what == NotificationSortChildren)
        {
            UpdateContainerContents();
        }
    }

    public void SetSomeSetting()
    {
        QueueSort();
    }
    private void UpdateContainerContents()
    {
        // Must re-sort the children
        var anchorPosition = new Vector2(margin, margin);
        var anchorEnd = new Vector2(1.0f, 1.0f) - new Vector2(margin, margin);
        foreach (Control c in GetChildren())
        {
            c.SetAnchorAndOffset(Side.Left, anchorPosition.X, 0);
            c.SetAnchorAndOffset(Side.Right, anchorEnd.X, 0);
            c.SetAnchorAndOffset(Side.Top, anchorPosition.Y, 0);
            c.SetAnchorAndOffset(Side.Bottom, anchorEnd.Y, 0);
        }
    }
}