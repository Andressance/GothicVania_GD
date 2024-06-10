using Godot;
using System;

public partial class snail : CharacterBody2D {

    [Export]
    public AnimationTree animationTree;

    [Export]
    public Vector2 starting_move_direction = new Vector2(-1, 0); // Izquierda

    [Export]
    public float speed = 20;

    [Export]
    private StateMachine stateMachine;

    [Export]
    private State hit_state;

    [Export]
    private Sprite2D sprite;

    [Export]
    public Area2D cliffCollisionShape;
    private signal_bus signal_bus;

    private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").As<float>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        
        signal_bus = GetNode<signal_bus>("/root/signal_bus");
        animationTree = GetNode<AnimationTree>("AnimationTree");
        stateMachine = GetNode<StateMachine>("StateMachine");
        animationTree.Active = true;
    }

    public override void _PhysicsProcess(double delta)
    {   
        var velocity = Velocity;

        if (this.stateMachine.currentState.canMove && this.stateMachine.currentState != hit_state)
        {
            velocity.X = starting_move_direction.X * speed;
        }
        else
        {
            velocity.X = 0;
        }

        if (IsOnFloor())
        {
            velocity.Y = 0;
        }
        else
        {
            velocity.Y += gravity * (float) delta;
        }

        Velocity = velocity;
        UpdateFacingAnimation();
        MoveAndSlide();
    
    }

    public void UpdateFacingAnimation() {
        if (starting_move_direction.X == 1)
        {
            // Va hacia la derecha
            sprite.FlipH = true;
            cliffCollisionShape.Position = new Vector2(40   , 0);
        }
        else
        {
            sprite.FlipH = false;
            cliffCollisionShape.Position = new Vector2(0, 0);
        }
    }



}