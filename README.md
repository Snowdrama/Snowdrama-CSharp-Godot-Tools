# SnowdramaGodotTools
A package of tools I use in Godot! 

I'm curently using Godot 4.4 Beta 1 but These should all work in Godot 4.2+

If there's any issues with older versions open an issue and I can look into it!

# Tools
Here's some of the things my tools have to help you out!
* Scene transitions with a flexible way to make your own transitions!
* Simple apring animation tools animating things like UI!
* Simple tools for creating Options like video, audio, and input options!
* Audio tools, like sound pools as well as managing music tracks.
* 'Tables' a few tools for making it easy to set up and manage things like Loot Tables for randomly generated loot from a set of items.
* UI Routing for managing UI windows

# Examples

This is something I want to work on soon but currently there's no examples included

# Installation
The intended way to install this currently is to add it as a Git Submodule in your project. This isn't set up like a Godot Addon, though I might change that at some point if there's people asking!

I usually install it to the Tools directory like this: 
```
git submodule add https://github.com/Snowdrama/Snowdrama-CSharp-Godot-Tools.git Tools
```

# Dependencies
* I use JSON.NET for some things so you should install that using Nuget latest should work if not open an issue please!
* I also believe they need C# 7+ but could be wrong

# Known Things
Not all of my tools are actively working, this set of tools is constantly being worked on.
### Not Currently Working
* Delaunay Triangulation
* Dungeon Generators are not all working like 3D dungeons
* Flood pathfinding isn't working yet, it's very experimental anyway
* Input Rebinding, it's something I want to work on soon
