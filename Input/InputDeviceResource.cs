using Godot;
using System;


/// <summary>
/// This is an example resource used for setting a player's input device.
/// 
/// This is used due to Godot's InputMap.
/// 
/// This basically has a list of strings that should be found as actions
/// in the input map.
/// 
/// If your game doesn't need these interactions you should make a new 
/// resource and use the IInputDevice interface. 
/// </summary>



public interface IInputDevice
{
    public bool GetInputString(string keyName, out string outputValue);
}

[GlobalClass]
public partial class InputDeviceResource : Resource, IInputDevice
{
    [Export] public string InputKey_MoveUp { get; private set; }
    [Export] public string InputKey_MoveDown { get; private set; }
    [Export] public string InputKey_MoveLeft { get; private set; }
    [Export] public string InputKey_MoveRight { get; private set; }


    [Export] public string InputKey_LookUp { get; private set; }
    [Export] public string InputKey_LookDown { get; private set; }
    [Export] public string InputKey_LookLeft { get; private set; }
    [Export] public string InputKey_LookRight { get; private set; }

    [Export] public string InputKey_Jump { get; private set; }

    [Export] public string InputKey_Shoot { get; private set; }


    //We'd prefer to use the device directly if we know the values above,
    //but this allows us to generalize in the device management code.
    //as the manager just takes a list of IDeviceInterface Resources
    public bool GetInputString(string keyName, out string outputValue)
    {
        switch (keyName)
        {
            case "MoveUp":
                outputValue = InputKey_MoveUp;
                return true;
            case "MoveDown":
                outputValue = InputKey_MoveDown;
                return true;
            case "MoveLeft":
                outputValue = InputKey_MoveLeft;
                return true;
            case "MoveRight":
                outputValue = InputKey_MoveRight;
                return true;
            case "LookUp":
                outputValue = InputKey_LookUp;
                return true;
            case "LookDown":
                outputValue = InputKey_LookDown;
                return true;
            case "LookLeft":
                outputValue = InputKey_LookLeft;
                return true;
            case "LookRight":
                outputValue = InputKey_LookRight;
                return true;
            case "Jump":
                outputValue = InputKey_Jump;
                return true;
            case "Shoot":
                outputValue = InputKey_Shoot;
                return true;
        }
        outputValue = null;
        return false;
    }
}
