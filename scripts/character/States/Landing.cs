using Godot;
using System;

// Create a class_name 

public partial class Landing : State
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.canMove = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
