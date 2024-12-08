using Godot;
using System;


/// <summary>
/// Based on a video I cannot find anymore so if anyone can remember a GDC talk or something like it that sounds like this
/// Please hit me up on Twitter @_Snowdrama
/// 
/// The basic idea is that when pathfinding for large swarms, you don't want every entity to do something like A* 
/// that'd be too expensive. Instead we create a large grid of nodes and each node points to another node
/// eventually leading to the target. entities can then just "look at their feet" and find an arrow that points them
/// in the rigt direction. This way you only do the expensive pathfinding once
/// 
/// This grid of nodes is calculated only "once" by the entity things can target, for example:
/// The player would calculate this and then swarms of 1000 Zombies could flood the player.
/// 
/// In theory this can be used for any thing not just pathfinding, but instead anything where you want the 
/// distanceTraveled to something to be important, this can also be layered together linearly/averaged.
/// 
/// for example if you have 4 players, you can do this 4 times, then layer the fields together
/// the directions and distances will change if they're weighted by the distanceTraveled, and so the layered field
/// would generally tell you the distanceTraveled to the closest player.
/// 
/// This could also map different conditions like biomes, or work as a way to figure out regions generating
/// political maps in games with countries in real time.
/// 
/// 
/// All that said, this is a basic pathfinding version lol
/// 
/// </summary>

public struct FloodGridCell
{
	Vector2 directionToGoal;
	int distanceInSteps;
	bool passible;
}
public partial class FloodPathfinding : Node
{
	[Export] Vector2I gridSize;

    FloodGridCell[,] gridPoints;

	public override void _Ready()
	{
	}



	public void FloodFill(Vector2I origin)
	{
		
	}

	public void UpdateTile()
	{

	} 

}
