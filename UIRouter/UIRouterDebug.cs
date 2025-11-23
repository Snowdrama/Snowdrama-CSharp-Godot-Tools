using Godot;

public partial class UIRouterDebug : Node
{
    [Export] UIRouter router;
    [Export] string commandName = "Pause";
    public override void _EnterTree()
    {
        base._EnterTree();
        CommandConsole.AddGlobalCommand($"PrintRoutes{commandName}", PrintRoutes, "Prints all the UI Routes");
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    private void PrintRoutes(params string[] args)
    {
        Debug.LogWarning("Printing Routes!");
        var routes = router.GetAllRoutes();
        for (int i = 0; i < routes.Count; i++)
        {
            CommandConsole_RichTextLabel.PrintText($"{routes[i]}");
        }
    }
}
