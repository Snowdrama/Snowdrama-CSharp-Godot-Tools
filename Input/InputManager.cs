using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// the idea is that during player select, this class can distribute an
/// IInputDevice Resource that has a list of all the InptuMap values 
/// for a single player. 
///
/// These can be used to pass the strings to functions like:
/// IInputDevice device;
/// Input.GetVector(device.Get("MoveLeft"), device.Get("MoveRight"), device.Get("MoveUp"), device.Get("MoveDown");
/// 
/// Which for different devices might be the equivalent of:
/// Input.GetVector("MoveLeft_Device_0", "MoveRight_Device_0", "MoveUp_Device_0", "MoveDown_Device_0";
/// Input.GetVector("MoveLeft_Device_1", "MoveRight_Device_1", "MoveUp_Device_1", "MoveDown_Device_1";
/// 
/// Each corresponding to a different player
/// 
/// </summary>

public partial class InputManager : Node
{
    public static InputManager instance;
    public override void _EnterTree()
    {
        base._EnterTree();
        instance = this;
    }
    public override void _ExitTree()
    {
        base._ExitTree();
        instance = null;
    }
    public override void _Ready()
    {
    }
}
