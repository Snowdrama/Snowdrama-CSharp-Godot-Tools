using Godot;

public partial class ModalPanel : Control
{
    public void SetActive(bool state)
    {
        this.Visible = true;
        if (this.Visible)
        {
            this.ProcessMode = ProcessModeEnum.Inherit;
        }
        else
        {
            this.ProcessMode = ProcessModeEnum.Disabled;
        }
    }
}