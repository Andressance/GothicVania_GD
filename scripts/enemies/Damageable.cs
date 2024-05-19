using Godot;
using System;

public partial class Damageable : Node
{
	[Export]
	public int health = 20;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			GetParent().QueueFree();
		}

	}
}
