using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class WFCSpawnedInstance3D : Node3D
{
	public WFCNode node;
    [Export] public Array<Node3D> nodeContainingChildrenToToggle;
    Array<Node3D> enabled_N = new Array<Node3D>();
    Array<Node3D> disabled_N = new Array<Node3D>();
    Array<Node3D> enabled_S = new Array<Node3D>();
    Array<Node3D> disabled_S = new Array<Node3D>();
    Array<Node3D> enabled_E = new Array<Node3D>();
    Array<Node3D> disabled_E = new Array<Node3D>();
    Array<Node3D> enabled_W = new Array<Node3D>();
    Array<Node3D> disabled_W = new Array<Node3D>();


    public void ConfigureSpawnedInstance(WFCTile tile)
    {
        foreach (var node in nodeContainingChildrenToToggle)
        {
            foreach (var item in node.GetChildren())
            {
                AddItem(item, "_N", "_DN", enabled_N, disabled_N);
                AddItem(item, "_S", "_DS", enabled_S, disabled_S);
                AddItem(item, "_E", "_DE", enabled_E, disabled_E);
                AddItem(item, "_W", "_DW", enabled_W, disabled_W);
            }
        }
        SetListProps(enabled_N, disabled_N, tile.ConnectionType_N);
        SetListProps(enabled_S, disabled_S, tile.ConnectionType_S);
        SetListProps(enabled_E, disabled_E, tile.ConnectionType_E);
        SetListProps(enabled_W, disabled_W, tile.ConnectionType_W);
    }

    public void SetListProps(Array<Node3D> enableList, Array<Node3D> disableList, int value)
    {

        if (value != 0)
        {
            foreach (var item in disableList)
            {
                if (item is NavigationLink3D)
                {
                    var navLink = (NavigationLink3D)item;
                    navLink.Enabled = false;
                }
                if (item is CollisionShape3D)
                {
                    var colShape = (CollisionShape3D)item;
                    colShape.Disabled = true;
                }
                item.Visible = false;
            }
            foreach (var item in enableList)
            {
                if (item is NavigationLink3D)
                {
                    var navLink = (NavigationLink3D)item;
                    navLink.Enabled = true;
                }
                if (item is CollisionShape3D)
                {
                    var colShape = (CollisionShape3D)item;
                    colShape.Disabled = false;
                }
                item.Visible = true;
            }
        }
        else
        {
            foreach (var item in disableList)
            {
                if (item is NavigationLink3D)
                {
                    var navLink = (NavigationLink3D)item;
                    navLink.Enabled = true;
                }
                if (item is CollisionShape3D)
                {
                    var colShape = (CollisionShape3D)item;
                    colShape.Disabled = false;
                }
                item.Visible = true;
            }
            foreach (var item in enableList)
            {
                if (item is NavigationLink3D)
                {
                    var navLink = (NavigationLink3D)item;
                    navLink.Enabled = false;
                }
                if (item is CollisionShape3D)
                {
                    var colShape = (CollisionShape3D)item;
                    colShape.Disabled = true;
                }
                item.Visible = false;
            }
        }
    }

    public void AddItem(Node item, string enabledModifier, string disabledModifier, Array<Node3D> enableList, Array<Node3D> disableList)
    {
        if (item is Node3D)
        {
            if (item.Name.ToString().IndexOf(disabledModifier) != -1)
            {
                //GD.Print($"Adding {item.Name}");
                disableList.Add((Node3D)item);
                //recurse over children we want to disable them as well
                List<Node> nodeList = GetAllChildrenRecursive(item, new List<Node>());
                foreach (var child in nodeList)
                {
                    //GD.Print($"Adding {child.Name}");
                    disableList.Add((Node3D)child);
                }
            }
            else if (item.Name.ToString().IndexOf(enabledModifier) != -1)
            {
                //GD.Print($"Adding {item.Name}");
                enableList.Add((Node3D)item);
                //recurse over children we want to disable them as well
                List<Node> nodeList = GetAllChildrenRecursive(item, new List<Node>());
                foreach (var child in nodeList)
                {
                    //GD.Print($"Adding {child.Name}");
                    enableList.Add((Node3D)child);
                }
            }
        }
    }

    public List<Node> GetAllChildrenRecursive(Node node, List<Node> nodeList)
    {
        if(node.GetChildCount() == 0)
        {
            return nodeList;
        }

        foreach (var child in node.GetChildren())
        {
            nodeList.Add(child);
            nodeList = GetAllChildrenRecursive(child, nodeList);
        }
        return nodeList;
    }
}
