using Godot;

public class Debug{
    public static void Log(string message,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        var path = sourceFilePath.Split("\\");
        GD.Print($"[{path[path.Length-1]}:{sourceLineNumber}]:[{message}]");
    }
    public static void LogError(string message, 
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        var path = sourceFilePath.Split("\\");
        GD.PrintErr($"{path[path.Length - 1]}[{sourceLineNumber}]:[{message}]");
    }
    public static void LogVerbose(string message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        GD.Print($"{message}:  [{sourceFilePath}]:[{sourceLineNumber}]:[{memberName}]");
    }
    public static void LogErrorVerbose(string message,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        GD.PrintErr($"{message}:  [{sourceFilePath}]:[{sourceLineNumber}]:[{memberName}]");
    }
}