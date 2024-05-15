using Godot;

public abstract partial class Transition : Node
{

    string _transitionName;
    [Export] public string TransitionName
    {
        private set
        {
            _transitionName = value.Trim().ToLower(); 
        }
        get
        { 
            return _transitionName;
        }
    }
    public abstract void SetTransitionValue(float transitionValue);
}