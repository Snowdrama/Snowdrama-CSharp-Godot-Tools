using Godot;

public class Debug{
    public static void Log(string message, 
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
            GD.Print($"{message}:  [{sourceFilePath}]:[{sourceLineNumber}]:[{memberName}]");
    }
    public static void LogError(string message, 
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
            GD.PrintErr($"{message}:  [{sourceFilePath}]:[{sourceLineNumber}]:[{memberName}]");
    }
}