using Godot;

public class Debug
{
    public static bool Assert(bool assert,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        if (assert)
        {
            //we're valid!
            return true;
        }
        var path = sourceFilePath.Split("\\");
        GD.PrintErr($"{path[path.Length - 1]}[{sourceLineNumber}]:Assertion failed!");
        return false;
    }
    public static void Log(string message,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        var path = sourceFilePath.Split("\\");
        GD.Print($"[{path[path.Length - 1]}:{sourceLineNumber}]:[{message}]");
    }
    public static void LogRich(string message,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        var path = sourceFilePath.Split("\\");
        GD.PrintRich($"[{path[path.Length - 1]}:{sourceLineNumber}]:[{message}]");
    }
    public static void LogWarn(string message,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        var path = sourceFilePath.Split("\\");
        GD.PrintRich($"[color=#FF0][{path[path.Length - 1]}:{sourceLineNumber}]:[{message}][/color]");
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