using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class CommandConsoleTextBox : RichTextLabel
{
    private static CommandConsoleTextBox instance;
    private static int consoleHistoryLength = 64;
    private static bool needsUpdating;

    private static Queue<string> consoleStrings = new Queue<string>();

    VScrollBar vScrollBar;
    public override void _EnterTree()
    {
        base._EnterTree();
        this.FitContent = true;
        this.BbcodeEnabled = true;
        this.ScrollFollowing = true;
        vScrollBar = this.GetVScrollBar();
        instance = this;
    }
    public static void PrintText(string lineToPush)
    {
        instance.AppendText(lineToPush);
        //consoleStrings.Enqueue(lineToPush);
        //needsUpdating = true;
    }
    //private void UpdateConsoleText()
    //{
    //    while (consoleStrings.Count > consoleHistoryLength)
    //    {
    //        consoleStrings.Dequeue();
    //    }

    //    //TODO: Write text to console
    //    var stringsArray = consoleStrings.ToArray();
    //    string output = "";
    //    for (int i = 0;i < stringsArray.Length; i++)
    //    {
    //        output += stringsArray[i];
    //        output += "\n";
    //    }

    //    textLabel.Text = output;
    //}


    //public override void _Process(double delta)
    //{
    //    base._Process(delta);
    //    if (needsUpdating)
    //    {
    //        needsUpdating = false;
    //        UpdateConsoleText();
    //        vScrollBar.Value = (int)vScrollBar.MaxValue;
    //    }
    //}
}
