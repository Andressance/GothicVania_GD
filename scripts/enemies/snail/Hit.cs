using Godot;
using System;

public partial class Hit : State
{
	[Export]
	public Damageable damageable;

		// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (damageable == null)
		{
			GD.PushWarning("Damageable is not set");
			return;
		}

		damageable.Connect("TakeDamage", new Callable(this, "OnTakeDamage"));

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnTakeDamage(int damage)
	{
		GD.Print("Take Damage");
	}
}
