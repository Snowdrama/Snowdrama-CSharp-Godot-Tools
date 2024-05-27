using Godot;
using System;
using System.Collections.Generic;
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

    [Export] CanvasLayer consoleCanvas;

    public override void _Ready()
    {
        base._Ready();
        consoleCanvas.Hide();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (Input.IsActionJustPressed("OpenConsole"))
        {
            if(consoleCanvas.Visible)
            {
                consoleCanvas.Hide();
            }
            else
            {
                consoleCanvas.Show();
            }
        }
    }

    //a dictionary of command names that point to a dictionary of targets and their targets.
    static Dictionary<string, Dictionary<string, GameCommand>> targetedCommands = new Dictionary<string, Dictionary<string, GameCommand>>();
	static Dictionary<string, GameCommand> globalCommands = new Dictionary<string, GameCommand>();

    //targeted commands can have targets such as "player->addGold" or "npc1->addGold"
    public static void AddTargetedAction(string targetName, string commandName, GameCommandAction action)
    {
        if (targetedCommands == null)
        {
            targetedCommands = new Dictionary<string, Dictionary<string, GameCommand>>();
        }
        if (!targetedCommands.ContainsKey(commandName))
        {
            targetedCommands.Add(commandName, new Dictionary<string, GameCommand>());
        }
        if (!targetedCommands[commandName].ContainsKey(targetName))
        {
            targetedCommands[commandName].Add(targetName, new GameCommand());
        }

        targetedCommands[commandName][targetName].RegisterCommand(action);
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
    public static void AddGlobalAction(string actionName, GameCommandAction action)
    {
        globalCommands[actionName].RegisterCommand(action);
    }

    public static void RemoveGlobalAction(string actionName, GameCommandAction action)
    {
        globalCommands[actionName].UnrgisterCommand(action);
    }

    //public static (bool, string) AutoFill(string attemptText)
    //{
    //    //check if it's




    //    //we failed to find something better, so just return the original text
    //    return (false, attemptText);
    //}

    public static void RunCommand(string input)
    {
        //TODO: parse command string
        //check if it's targeted
        GD.Print($"Running Command: {input}");
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
                }
                else
                {
                    CommandConsoleTextBox.PrintText($"Command {command} has no target {target} in it's command list.");
                }
            }
            else
            {
                CommandConsoleTextBox.PrintText($"[color=#999]Command [color=#F00]{command}[/color]not found in command list." +
                    $"check to make sure that an entity with the command is loaded.[/color]");
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
                case "list_commands":
                    CommandConsoleTextBox.PrintText("=================");
                    CommandConsoleTextBox.PrintText("Listing Commands:");
                    CommandConsoleTextBox.PrintText("=================");
                    foreach (var item in globalCommands)
                    {
                        CommandConsoleTextBox.PrintText($"Global Command: {item.Key}");
                    }
                    foreach (var item in targetedCommands)
                    {
                        CommandConsoleTextBox.PrintText($"Targeted Command: {item.Key}");
                    }
                    CommandConsoleTextBox.PrintText("\n\n");
                    break;
                case "list_command_targets":

                    if(args.Length > 0)
                    {
                        CommandConsoleTextBox.PrintText("=======================================");
                        CommandConsoleTextBox.PrintText($"Listing Allowed Targets for {args[0]}:");
                        CommandConsoleTextBox.PrintText("=======================================");
                        foreach (var item in targetedCommands[args[0]])
                        {
                            CommandConsoleTextBox.PrintText($"Target for {args[0]} Command: {item.Key}");
                        }
                        CommandConsoleTextBox.PrintText("\n\n");
                    }
                    break;
                default:
                    break;
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

    public GameCommand()
    {

    }

    public void Invoke(params string[] args)
    {
        command.Invoke(args);
    }

    public void RegisterCommand(GameCommandAction addCommand)
    {
        command += addCommand;
    }
    public void UnrgisterCommand(GameCommandAction addCommand)
    {
        command -= addCommand;
    }
}
