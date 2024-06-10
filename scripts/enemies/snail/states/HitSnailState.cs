using Godot;
using System;

public partial class HitSnailState : State
{
	[Export]
	private Damageable damageable;

	[Export]
	public StateMachine state_machine;

	[Export]
	public State walk_state;

	[Export]
	public State dead_state;

	private Vector2 knockback_velocity = new Vector2(200, 0);

	[Export]
	private Timer timer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		damageable.Connect("OnHit", new Callable(this, "OnHealthChanged"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void on_enter()
	{
	}

	public void OnHealthChanged(Node node, int health)
	{
		if (damageable.Health > 0) {
			EmitSignal("InterruptState", this);
			timer.Start();
		} else {
			EmitSignal("InterruptState", dead_state);
		}
	}

	public override void on_exit()
	{
		character.Velocity = Vector2.Zero;
		
	}

	public void _on_timer_timeout()
	{
		GD.Print("HitSnailState::on_timer_timeout");
		EmitSignal("InterruptState", walk_state);
	}
}
