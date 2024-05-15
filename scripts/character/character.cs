using Godot;
using System;

public partial class character : CharacterBody2D
{
	// Speed of the player.
	// Speed of the player.
	[Export]
    public float Speed = 200.0f;

    

	// Double jump velocity of the player.
	
    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").As<float>();
    public bool hasDoubleJumped = true;
    public bool was_in_air = false;
	public bool animation_locked = false;
	public Vector2 direction = Vector2.Zero;
	private Sprite2D Sprite2D;
	private AnimationTree animationTree;
	
	private StateMachine StateMachine;

	private Area2D cliffCollisionShape;


	public override void _Ready()
	{
		Sprite2D = GetNode<Sprite2D>("Sprite2D");
		animationTree = GetNode<AnimationTree>("AnimationTree");
		StateMachine = GetNode<StateMachine>("StateMachine");
		cliffCollisionShape = GetNode<Area2D>("CliffClollision2D");
		animationTree.Active = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor() && this.StateMachine.check_if_it_can_move())
        {
            velocity.Y += gravity * (float) delta;
            was_in_air = true;
        } else if (!this.StateMachine.check_if_it_can_move()) {
			// No velocity on the Y axis if the player is on the floor.
			velocity.Y = 0;
		}
        

        
        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        direction = Input.GetVector("left", "right", "up", "down");

        if (direction.X != 0 && this.StateMachine.check_if_it_can_move())
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = 0;
		}

        // Set the new velocity.
		Velocity = velocity;
        MoveAndSlide();
        UpdateAnimation();
		UpdateFacingAnimation();

	}

	public void UpdateFacingAnimation() {
		if (direction.X < 0) {
			Sprite2D.FlipH = true;
			cliffCollisionShape.Position = new Vector2(-20, 0);
		} else if (direction.X > 0) {
			Sprite2D.FlipH = false;
			cliffCollisionShape.Position = new Vector2(0, 0);
		}
	
	}

	public void UpdateAnimation()
    {
		animationTree.Set("parameters/Movement/blend_position", direction.X);
		animationTree.Set("parameters/Movement_sword/blend_position", direction.X);

    }



	
}
