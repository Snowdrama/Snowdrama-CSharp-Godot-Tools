using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Collections.Specialized.BitVector32;

/// <summary>
/// An implementation of a command console found in many games. 
/// Tangentially designed similar to the console found in 
/// Elder Scrolls games, particularly Morrowind
/// 
/// Commands can be both global and targeted.
/// 
/// A global command contains no target for example it may call something
/// in a singleton like the entity spawner like:
/// 
/// command param1 param2 param3 param4
/// 
/// For example: 
/// 
/// spawn_entity npc_1
/// 
/// 
/// targeted commands specifiy a target using an arrow(TODO: or dot?)
/// this would be a reference to a specific entity:
/// 
/// target->command param1 param2 param3 param4
/// 
/// for example adding an item to the inventory of a crate: 
/// 
/// crate_4->add_item gold 100
/// 
/// these are just references to functions 
/// </summary>
public partial class CommandConsole : Node
{
    [Export] bool pauseOnOpen;
    [Export] CanvasLayer consoleCanvas;

    public override void _Ready()
    {
        base._Ready();
        consoleCanvas.Hide();
        consoleCanvas.Layer = 128;
        this.ProcessMode = ProcessModeEnum.Always;
        consoleCanvas.ProcessMode = ProcessModeEnum.Always;

        CommandConsole_RichTextLabel.PrintText("=================");
        CommandConsole_RichTextLabel.PrintText("'help' for commands");
        CommandConsole_RichTextLabel.PrintText("=================");


        //if (!InputMap.HasAction("OpenConsole"))
        //{
        //    InputMap.AddAction("OpenConsole");
        //    InputMap.ActionAddEvent("OpenConsole", new InputEventKey()
        //    {
        //        Keycode = Key.Quoteleft,
        //        CtrlPressed = true,
        //    });
        //}
        //if (!InputMap.HasAction("ConsoleTryAutocomplete"))
        //{
        //    InputMap.AddAction("ConsoleTryAutocomplete");
        //    InputMap.ActionAddEvent("ConsoleTryAutocomplete", new InputEventKey()
        //    {
        //        Keycode = Key.Tab
        //    });
        //}
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (Input.IsActionJustPressed("OpenConsole"))
        {
            if(consoleCanvas.Visible)
            {
                consoleCanvas.Hide();
                GetTree().Paused = false;
            }
            else
            {
                consoleCanvas.Show();
                GetTree().Paused = true;
            }
        }
    }

    //a dictionary of command names that point to a dictionary of targets and their targets.
    static Dictionary<string, Dictionary<string, GameCommand>> targetedCommands = new Dictionary<string, Dictionary<string, GameCommand>>();
	static Dictionary<string, GameCommand> globalCommands = new Dictionary<string, GameCommand>();

    //targeted commands can have targets such as "player->addGold" or "npc1->addGold"
    public static void AddTargetedAction(string targetName, string commandName, GameCommandAction action, string tooltip = "")
    {
        //create command dictionary if it isn't initialized
        if (targetedCommands == null)
        {
            targetedCommands = new Dictionary<string, Dictionary<string, GameCommand>>();
        }


        //make a command if it dosen't exist
        if (!targetedCommands.ContainsKey(commandName))
        {
            targetedCommands.Add(commandName, new Dictionary<string, GameCommand>());
        }

        //add the target if it doesn't exist.
        if (!targetedCommands[commandName].ContainsKey(targetName))
        {
            targetedCommands[commandName].Add(targetName, new GameCommand());
        }

        targetedCommands[commandName][targetName].RegisterCommand(action);
        if(!string.IsNullOrEmpty(tooltip))
        {
            targetedCommands[commandName][targetName].Tooltip = tooltip;
        }
    }

    //   public static void RemoveTargetedAction(string targetName, string actionName, GameCommandAction action)
    //   {
    //       targetedCommands[actionName][targetName] -= action;
    //   }
    /// <summary>
    /// Adds a gloabal command for use
    /// 
    /// Examples of global command:
    /// KillAll
    /// ApplyEffectAll
    /// </summary>
    /// <param name="actionName"></param>
    /// <param name="action"></param>
    public static void AddGlobalCommand(string commandName, GameCommandAction action, string tooltip = "")
    {
        if (!globalCommands.ContainsKey(commandName))
        {
            globalCommands[commandName] = new GameCommand();
        }

        globalCommands[commandName].RegisterCommand(action);

        if (!string.IsNullOrEmpty(tooltip))
        {
            globalCommands[commandName].Tooltip = tooltip;
        }
    }

    public static void RemoveGlobalCommand(string commandName, GameCommandAction action)
    {
        if (!globalCommands.ContainsKey(commandName))
        {
            return;
        }

        globalCommands[commandName].UnrgisterCommand(action);
    }
    public static void RemoveTarget(string targetName, string commandName)
    {
        if (!targetedCommands.ContainsKey(commandName) && !targetedCommands[commandName].ContainsKey(targetName))
        {
            return;
        }

        targetedCommands[commandName].Remove(targetName);
    }

    //public static (bool, string) AutoFill(string attemptText)
    //{
    //    //check if it's
    //    //we failed to find something better, so just return the original text
    //    return (false, attemptText);
    //}

    public static bool RunCommand(string input)
    {
        //TODO: parse command string
        //check if it's targeted
        GD.Print($"Running Command: {input}");

        //player->add_item gold_001 10000
        if (input.Contains("->"))
        {
            GD.Print("Command is Targeted!");
            string[] segments = input.Split(new string[]{"->", " "}, StringSplitOptions.TrimEntries);
            string target = segments[0];
            string command = segments[1];

            string[] args = segments[2..segments.Length];

            GD.Print($"Command has {args.Length} segments");
            for (int i = 0; i < args.Length; i++)
            {
                GD.Print($"{i}: {args[i]}");
            }

            if (targetedCommands.ContainsKey(command))
            {
                if (targetedCommands[command].ContainsKey(target))
                {
                    targetedCommands[command][target].Invoke(args);
                    return true;
                }
                else
                {
                    CommandConsole_RichTextLabel.PrintText($"Command {command} has no target {target} in it's command list.");
                    return false;
                }
            }
            else
            {
                CommandConsole_RichTextLabel.PrintText($"[color=#999]Command [color=#F00]{command}[/color]not found in command list." +
                    $"check to make sure that an entity with the command is loaded.[/color]");
                return false;
            }
        }
        else
        {
            //global command
            string[] segments = input.Split(new string[] { "->", " " }, StringSplitOptions.TrimEntries);
            string command = segments[0];

            string[] args = segments[1..segments.Length];

            switch (command)
            {
                case "help":
                    CommandConsole_RichTextLabel.PrintText("Check what commands are available with:");
                    CommandConsole_RichTextLabel.PrintText("list_commands");
                    CommandConsole_RichTextLabel.PrintText("list_command_targets");
                    return true;
                case "list_commands":
                    CommandConsole_RichTextLabel.PrintText("=================");
                    CommandConsole_RichTextLabel.PrintText("Listing Commands:");
                    CommandConsole_RichTextLabel.PrintText("=================");


                    CommandConsole_RichTextLabel.PrintText("[color=#00F]================= Global Commands =================[/color]");
                    foreach (var item in globalCommands)
                    {
                        var tt = (!string.IsNullOrEmpty(item.Value.Tooltip)) ? $"- {item.Value.Tooltip}" : "";
                        CommandConsole_RichTextLabel.PrintText($"{item.Key} [color=#33A]{tt}[/color]");
                    }
                    CommandConsole_RichTextLabel.PrintText("[color=#00F]================= Targeted Commands =================[/color]");
                    foreach (var item in targetedCommands)
                    {
                        var tt = (!string.IsNullOrEmpty(item.Value.First().Value.Tooltip)) ? $"- {item.Value.First().Value.Tooltip}" : "";
                        CommandConsole_RichTextLabel.PrintText($"{item.Key} [color=#33A]{tt}[/color]");
                    }
                    CommandConsole_RichTextLabel.PrintText("");
                    return true;
                case "list_command_targets":

                    if(args.Length > 0)
                    {
                        CommandConsole_RichTextLabel.PrintText("=======================================");
                        CommandConsole_RichTextLabel.PrintText($"Listing Allowed Targets for {args[0]}:");
                        CommandConsole_RichTextLabel.PrintText("=======================================");
                        foreach (var item in targetedCommands[args[0]])
                        {
                            var tt = (!string.IsNullOrEmpty(item.Value.Tooltip)) ? $"- {item.Value.Tooltip}" : "";
                            CommandConsole_RichTextLabel.PrintText($"{args[0]} -> {item.Key} [color=#33A]{tt}[/color]");
                        }
                        CommandConsole_RichTextLabel.PrintText("");
                    }
                    return true;
                default:

                    if(globalCommands.ContainsKey(command))
                    {

                    }
                    else
                    {
                        CommandConsole_RichTextLabel.PrintText($"[color=#A66][color=#F00]Error:[/color] Command [color=#F00]{command}[/color] not found in list of global commands[/color]");
                    }
                    return false;
            }
        }
    }

    private void ListCommands()
    {

    }
}

public delegate void GameCommandAction(params string[] args);
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
