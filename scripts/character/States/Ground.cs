using Godot;
using System;

public partial class Ground : State
{
    // Jump velocity of the player.
    [Export]
    public float JumpVelocity = -150.0f;

    // Speed of the player.
    private Vector2 velocity;

    [Export]
    private State air_state;

    [Export]
    private State cliff_hanging;

    [Export]
    private string jump_animation;

    [Export]
    private string jump_loop;
    private bool jumping = false;


    private int attack_count = 0;

    private Timer attackTimer;
    private Timer between_attacks;
    private float idleTimeout = 1.0f; // Tiempo en segundos antes de regresar a idle
    private int attackCount = 0;
    private bool canAttack = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.canMove = true;
        isSliding = false;

        // Configuración del temporizador de ataque
        attackTimer = new Timer();
        AddChild(attackTimer);
        attackTimer.WaitTime = idleTimeout;
        attackTimer.OneShot = true;
        attackTimer.Connect("timeout", new Callable(this, "AttackTimerTimeout"));

        // Iniciar el temporizador de ataque
        between_attacks = new Timer();
        AddChild(between_attacks);
        between_attacks.WaitTime = 0.4f;
        between_attacks.OneShot = true;
        between_attacks.Connect("timeout", new Callable(this, "CanAttack"));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (!character.IsOnFloor() && !jumping)
        {
            next_state = air_state;
            playback.Travel(jump_loop);
            
        }

        if (character.IsOnFloor())
        {
            jumping = false;
        }
    }

    public override void state_input(InputEvent @event)
    {
        if (@event.IsActionPressed("up"))
        {
            jump();
            isArmed = false;
        }
        if (@event.IsActionPressed("attack"))
        {
            attack();
        }

        if (@event.IsActionPressed("fold") && isArmed)
        {
            isArmed = false;
            playback.Travel("fold_sword");
        }

        if (@event.IsActionPressed("shift") && character.IsOnFloor() && character.Velocity.X != 0)
        {
            playback.Travel("slide");
            isArmed = false;
            slide();
        }
        
        // Nueva lógica para volver al estado "Movement" si se pulsa la dirección contraria mientras se está deslizando
        if (@event.IsActionPressed("left") && isSliding)
        {
            playback.Travel("Movement");
            isArmed = false;
            isSliding = false;
            character.Velocity = 200.0f * Vector2.Left;
        }
        else if (@event.IsActionPressed("right") && character.IsOnFloor() && isSliding)
        {
            playback.Travel("Movement");
            isArmed = false;
            isSliding = false;
            character.Velocity = 200.0f * Vector2.Right;
        }

        // If its holded
        if (@event.IsActionPressed("down") && character.IsOnFloor())
        {
            playback.Travel("crouch");
            canMove = false;
        }
        else if (@event.IsActionReleased("down"))
        {
            if (isArmed)
            {
                playback.Travel("Movement_sword");
            }
            else
            {
                playback.Travel("Movement");
            }
            canMove = true;
        }
    }

    public void jump()
    {
        if (character.IsOnFloor())
        {
            velocity = character.Velocity;
            velocity.Y = JumpVelocity;
            character.Velocity = velocity;
            next_state = air_state;
            playback.Travel(jump_animation);
            jumping = true;
        }
    }

    public void attack()
    {
        

        if (!isArmed)
        {
            isArmed = true;
            playback.Travel("draw_sword");
        }
        else if (canAttack)
        {
            // No te puedes mover mientras atacas
            canMove = false;

            // Realizar lógica de ataque
            // Reiniciar el temporizador de ataque
            attackTimer.Stop();
            attackTimer.Start();

            // Elegir la animación de ataque basada en el contador de ataques
            switch (attackCount)
            {
                case 0:
                    playback.Travel("attack_1");
                    break;
                case 1:
                    playback.Travel("attack_2");
                    break;
                case 2:
                    playback.Travel("attack_3");
                    break;
            }

            // Incrementar el contador de ataques
            attackCount = (attackCount + 1) % 3; // Reinicia el contador si llega a 3

            // Reiniciar el temporizador entre ataques
            between_attacks.Stop();
            between_attacks.Start();
            
            canAttack = false; // Mover el estado canAttack después de iniciar el temporizador
        }
    }

    public void slide()
    {
        isSliding = true;
    }

    public void AttackTimerTimeout()
    {
        canMove = true;
        playback.Travel("Movement_sword");
        canAttack = true;
    }

    public void CanAttack()
    {
        canAttack = true;
    }

    public void _on_animation_tree_animation_finished(string anim_name)
    {
        if (anim_name == "slide")
        {
            isSliding = false;
            character.Velocity = Vector2.Zero;
            
            if (isArmed)
            {
                playback.Travel("Movement_sword");
            }
            else
            {
                playback.Travel("Movement");
            }
        }
    }

    public void _on_cliff_clollision_2d_area_entered(Area2D area)
    {
        if (area.IsInGroup("cliff")) {
            next_state = cliff_hanging;
        }
    }

    public void _on_cliff_clollision_2d_area_exited(Area2D area)
    {
        if (area.IsInGroup("cliff") && !character.IsOnFloor()) {
            next_state = air_state;
        }
    }
    public override void on_exit()
    {
        isSliding = false;
    }

}
