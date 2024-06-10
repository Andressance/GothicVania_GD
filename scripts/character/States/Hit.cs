using Godot;
using System;

public partial class Hit : State
{
	[Export]
	private Damageable damageable;

	[Export]
	public StateMachine state_machine;

	[Export]
	public State walk_state;

	[Export]
	public State dead_state;

	private Vector2 knockback_velocity = new Vector2(0, 0);

	[Export]
	private Timer timer;

	private bool isHit = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		damageable.Connect("OnHit", new Callable(this, "OnHealthChanged"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{				
		

		if (isHit) {
			if (facingRight) {
				knockback_velocity.X = -30;
			} else {
				knockback_velocity.X = 30;
			}

			// Knockback hacia la direccion opuesta
			Vector2 velocity = character.Velocity;
			velocity.X = knockback_velocity.X;
			character.Velocity = velocity;

			character.MoveAndSlide();
		}
		
		
	}

	public override void on_enter()
	{
		playback.Travel("take_damage");
		isHit = true;
		canMove = false;
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
		isHit = false;
		character.Velocity = Vector2.Zero;
        character.MoveAndSlide();
		canMove = true;
		
	}

	public void _on_timer_timeout()
	{
		EmitSignal("InterruptState", walk_state);
	}
}
