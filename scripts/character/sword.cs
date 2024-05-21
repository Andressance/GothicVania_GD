using Godot;
using System;

public partial class sword : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Monitoring = false;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_body_entered(Node body)
	{	
		// Esta monitorizando
		foreach (Node ChildEnteredTree in body.GetChildren())
		{
			if (ChildEnteredTree is Damageable)
			{
				(ChildEnteredTree as Damageable).TakeDamage(10);
			}
		}
	}
}
