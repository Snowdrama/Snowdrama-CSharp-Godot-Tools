public class GameCommand
{
    GameCommandAction command;
    public string Tooltip;
    public int argumentCount;
    
    public void Invoke(params string[] args)
    {
        command.Invoke(args);
    }

    public void RegisterCommand(GameCommandAction addCommand)
    {
        command += addCommand;
    }
    public void UnrgisterCommand(GameCommandAction removeCommand)
    {
        command -= removeCommand;
    }
}
