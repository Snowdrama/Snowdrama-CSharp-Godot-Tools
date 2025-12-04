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
    public static void LogWarning(string message,
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

    public static void Blue(string message,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        LogColor(message, "#00F", sourceFilePath, sourceLineNumber);
    }

    public static void Red(string message,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        LogColor(message, "#F00", sourceFilePath, sourceLineNumber);
    }

    public static void Green(string message,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        LogColor(message, "#0F0", sourceFilePath, sourceLineNumber);
    }

    public static void Cyan(string message,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        LogColor(message, "#0FF", sourceFilePath, sourceLineNumber);
    }

    public static void Magenta(string message,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        LogColor(message, "#F0F", sourceFilePath, sourceLineNumber);
    }

    public static void Yellow(string message,
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        LogColor(message, "#FF0", sourceFilePath, sourceLineNumber);
    }

    public static void LogColor(string message,
        string colorHex = "#FFF",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
        )
    {
        var path = sourceFilePath.Split("\\");
        GD.PrintRich($"[color={colorHex}][{path[path.Length - 1]}:{sourceLineNumber}]:[{message}][/color]");
    }
}