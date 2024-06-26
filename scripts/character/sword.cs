using Godot;
using System;

public partial class sword : Area2D
{
	// Called when the node enters the scene tree for the first time.

	[Export]
	public int damage = 10;

	public override void _Ready()
	{
		Monitoring = false;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_body_entered(CharacterBody2D body)
	{	
		// Esta monitorizando
		foreach (Node ChildEnteredTree in body.GetChildren())
		{
			if (ChildEnteredTree is Damageable)
			{
				(ChildEnteredTree as Damageable).TakeDamage(damage);				
			}
		}
	}
}
