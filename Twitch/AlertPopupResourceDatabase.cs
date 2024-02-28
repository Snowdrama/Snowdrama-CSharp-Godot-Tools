using Godot;

[GlobalClass]
public partial class AlertPopupResourceDatabase : Resource
{
    [ExportCategory("Alert Popup Data")]
    [Export] public Godot.Collections.Array<AlertPopupResource> followPopupResources = new Godot.Collections.Array<AlertPopupResource>();
    [Export] public Godot.Collections.Array<AlertPopupResource> subPopupResources = new Godot.Collections.Array<AlertPopupResource>();
    [Export] public Godot.Collections.Array<AlertPopupResource> bitsPopupResources = new Godot.Collections.Array<AlertPopupResource>();
    [Export] public Godot.Collections.Array<AlertPopupResource> pointsPopupResources = new Godot.Collections.Array<AlertPopupResource>();
}