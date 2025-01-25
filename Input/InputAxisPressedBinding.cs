using Godot;

public partial class InputAxisPressedBinding : Resource
{
    [Export] public string ActionName = "MoveUp";

    public JoyAxis Axis = JoyAxis.TriggerRight;

    public float AxisValue = 1.0f;
    public void SetAxisBinding(JoyAxis newAxis)
    {
        SwapJoyAxis(ActionName, Axis, newAxis);
        Axis = newAxis;
    }

    public void UpdateInputMap()
    {
        if (!InputMap.HasAction(ActionName)) { InputMap.AddAction(ActionName); }
        InputMap.ActionAddEvent(ActionName, new InputEventJoypadMotion()
        {
            Axis = Axis,
            AxisValue = 1.0f,
        });
    }
    private void SwapJoyAxis(string positiveAction, JoyAxis oldAxis, JoyAxis newAxis)
    {
        var positiveOld = new InputEventJoypadMotion()
        {
            Axis = oldAxis,
            AxisValue = 1.0f,
        };
        var positiveNew = new InputEventJoypadMotion()
        {
            Axis = newAxis,
            AxisValue = 1.0f,
        };
        if (!InputMap.HasAction(positiveAction))
        {
            InputMap.AddAction(positiveAction);
        }
        //we need to remove the old action
        if (InputMap.ActionHasEvent(positiveAction, positiveOld))
        {
            InputMap.ActionEraseEvent(positiveAction, positiveOld);
        }
        //now add the new key
        InputMap.ActionAddEvent(positiveAction, positiveNew);
    }
    public bool GetAxisPressed()
    {
        return Input.IsActionPressed(ActionName);
    }
}