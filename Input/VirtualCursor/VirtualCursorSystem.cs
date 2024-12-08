using Godot;
using System;

public class ChangeInputTypeMessage : AMessage<InputSchemeType> { }
public partial class VirtualCursorSystem : Node
{
    public static InputSchemeType currentInputType { get; private set; }
    ChangeInputTypeMessage changeInput;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

    public override void _EnterTree()
    {
        changeInput = Messages.Get<ChangeInputTypeMessage>();
        changeInput.AddListener(InputChanged);
    }

    public override void _ExitTree()
    {
        changeInput.RemoveListener(InputChanged);
        Messages.Return<ChangeInputTypeMessage>();
    }

    public void InputChanged(InputSchemeType newInputType)
    {
        currentInputType = newInputType;
    }
}
