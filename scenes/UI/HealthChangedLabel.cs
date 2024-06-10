using Godot;
using System;

public partial class HealthChangedLabel : Label
{
	[Export]
	public Vector2 float_speed = new Vector2(0, -50);

	private Timer timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetChild<Timer>(0);	

		timer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += float_speed * (float)delta;
	}

	public void OnTimerTimeout()
	{
		QueueFree();
	}
}
