using Godot;
using System;

public partial class DamageArea : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_body_entered(Node body)
	{
		foreach (Node ChildEnteredTree in body.GetChildren())
		{
			if (ChildEnteredTree is Damageable)
			{
				GD.Print("Body " + body.Name + " entered DamageArea");
				(ChildEnteredTree as Damageable).TakeDamage(10);				
			}
		}
	}
}
