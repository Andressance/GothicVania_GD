using Godot;
using System;

public partial class Die : State
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		canMove = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void on_enter()
    {
        playback.Travel("die");
    }
}
