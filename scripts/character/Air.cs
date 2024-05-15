using Godot;
using System;

public partial class Air : State
{
	[Export]
	public State ground_state;

	[Export]
	public State Cliff_hanging;

	[Export]
    public float DoubleJumpVelocity = -100.0f;

	[Export]
	private string double_jump_animation;


	private bool doubleJumped = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.canMove = true;
		this.cliffCollisionShape.Connect("area_entered", new Callable(this, "on_body_entered"));

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (character.IsOnFloor())
		{
			next_state = ground_state;
		}

		
	}

	public override void state_input(InputEvent @event)
	{
		if (@event.IsActionPressed("up"))
		{
			doubleJump();
		}
	}

	public override void on_exit() {
		if (next_state == ground_state) {
			doubleJumped = false;
			playback.Travel("Movement");
		}
	}

	public void doubleJump() {
		if (!character.IsOnFloor() && !doubleJumped)
		{
			var velocity = character.Velocity;
			velocity.Y = DoubleJumpVelocity;
			character.Velocity = velocity;
			doubleJumped = true;
			playback.Travel(double_jump_animation);
		}
	}

	public void on_body_entered(Area2D area)
	{
		GD.Print("Entered cliff");
		
	}

	
}
