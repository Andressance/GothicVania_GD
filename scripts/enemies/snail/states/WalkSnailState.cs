using Godot;
using System;

public partial class WalkSnailState : State
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_cliff_area_area_entered(Area2D area)
	{
		var snail = (snail)character;
		snail.starting_move_direction = -snail.starting_move_direction;
	}
}
