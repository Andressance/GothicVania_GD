using Godot;
using System;

public partial class character : CharacterBody2D
{
    [Export]
    public float Speed = 200.0f;

    [Export]
    public State HitState;
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").As<float>();
    public bool hasDoubleJumped = true;
    public bool was_in_air = false;
    public bool animation_locked = false;
    public Vector2 direction = Vector2.Zero;
    private Sprite2D Sprite2D;
    private AnimationTree animationTree;
    private StateMachine StateMachine;
    private Area2D cliffCollisionShape;
    private Area2D attackCollisionShape;
    private float slideSpeed = 400.0f;

    public bool facingRight = true;

    public override void _Ready()
    {
        Sprite2D = GetNode<Sprite2D>("Sprite2D");
        animationTree = GetNode<AnimationTree>("AnimationTree");
        StateMachine = GetNode<StateMachine>("StateMachine");
        cliffCollisionShape = GetNode<Area2D>("CliffClollision2D");
        attackCollisionShape = GetNode<Area2D>("AttackCollision2D");
        animationTree.Active = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        this.StateMachine.currentState.facingRight = facingRight;

        Vector2 velocity = Velocity;

        // Aplicar gravedad si no está en el suelo
        if (!IsOnFloor() && StateMachine.check_if_it_can_move())
        {
            velocity.Y += gravity * (float)delta;
            was_in_air = true;
        }
        else if (!StateMachine.check_if_it_can_move())
        {
            velocity.Y = 0;
        }

        // Obtener la dirección de entrada
        direction = Input.GetVector("left", "right", "up", "down");

        // Manejar el movimiento horizontal
        if (StateMachine.currentState.canMove && !StateMachine.currentState.isAttacking && StateMachine.currentState != HitState)
        {
            if (direction.X != 0 && !StateMachine.currentState.isSliding)
            {
                // Movimiento normal
                velocity.X = direction.X * Speed;
            }
            else if (StateMachine.currentState.isSliding)
            {
                velocity.X = slideSpeed;
            }
            else
            {
                velocity.X = 0;
            }
        }
        else
        {
            // Detener el movimiento horizontal durante el ataque
            velocity.X = 0;
        }

        // Detener deslizamiento cuando está cayendo
        if (!IsOnFloor() && StateMachine.currentState.isSliding)
        {
            StateMachine.currentState.isSliding = false;
            velocity.X = direction.X * Speed;
        }

        // Restablecer el estado de deslizamiento al aterrizar
        if (IsOnFloor() && was_in_air)
        {
            was_in_air = false;
            StateMachine.currentState.isSliding = false;
        }

        // Establecer la nueva velocidad
        Velocity = velocity;
        MoveAndSlide();

        // Actualizar animaciones
        UpdateAnimation();
        UpdateFacingAnimation();
    }


    public void UpdateFacingAnimation()
    {
        if (direction.X < 0 && StateMachine.check_if_it_can_turn())
        {
            Sprite2D.FlipH = true;
            cliffCollisionShape.Position = new Vector2(-20, 0);
            attackCollisionShape.Position = new Vector2(-30, 0);
            slideSpeed = -400.0f;
            facingRight = false;
        }
        else if (direction.X > 0 && StateMachine.check_if_it_can_turn())
        {
            Sprite2D.FlipH = false;
            cliffCollisionShape.Position = new Vector2(0, 0);
            attackCollisionShape.Position = new Vector2(0, 0);
            slideSpeed = 400.0f;
            facingRight = true;
        }
    }

    public void UpdateAnimation()
    {
        animationTree.Set("parameters/Movement/blend_position", direction.X);
        animationTree.Set("parameters/Movement_sword/blend_position", direction.X);
    }

}
