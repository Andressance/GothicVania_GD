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
	public override void on_enter() {
		
	}
	public override void on_exit() {

		if (next_state == ground_state) {
			playback.Travel("Movement");
		}
		if (next_state == Cliff_hanging) {
			playback.Travel("corner_grab");
		}


		doubleJumped = false;
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

	public void _on_cliff_clollision_2d_area_entered(Area2D area)
	{
		if (area.IsInGroup("cliff")) {
            next_state = Cliff_hanging;
        }
		
	}



}
