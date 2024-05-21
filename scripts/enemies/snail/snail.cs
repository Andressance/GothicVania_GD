using Godot;
using Godot.Collections;
using System;

public partial class snail : CharacterBody2D
{
	[Export]
	public State initialState;

	[Export]
	public StateMachineSnail stateMachine;

	[Export]
	public const float Speed = -20.0f;



	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready()
    {
		stateMachine = GetNode<StateMachineSnail>("StateMachine");	
		stateMachine.currentState = initialState;

    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Apply gravity if not on the floor
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}

		if (this.stateMachine.currentState is Walk) {
			velocity.X = Speed;
		}

		GD.Print("Current state: ", stateMachine.currentState);
		GD.Print("Velocity: ", velocity);
		GD.Print("IsOnFloor: ", IsOnFloor());


		

		
	
		

		Velocity = velocity;
		MoveAndSlide();
	}
}
