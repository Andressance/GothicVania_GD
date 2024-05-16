using Godot;
using System;

public partial class Cliff_hanging : State
{
    [Export]
    public State air_state;

    [Export]
    public State ground_state;

    [Export]
    public string corner_climb;

    private bool isHanging = false; // Variable para saber si el personaje está colgado del acantilado

    // Variable para almacenar la velocidad original del personaje antes de colgarse del acantilado
    private Vector2 originalVelocity;

    private float x_move;

    private Timer tempAnim;

    private bool isFacingRight;

    private Vector2 direction;

    private bool isClimbing = false;

    private int animFrameCount = 0; // Contador para el número de ejecuciones del temporizador

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.canMove = false; // Desactivar el movimiento mientras está colgado
        // Mantener al personaje estático mientras está colgado
        tempAnim = new Timer();
        AddChild(tempAnim);
        tempAnim.WaitTime = 0.1f;
        tempAnim.OneShot = true; // Hacer que se detenga después de una ejecución
        tempAnim.Connect("timeout", new Callable(this, "AnimationTimeout"));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // Establecer la dirección del personaje independientemente de si está tratando de moverse horizontalmente
        direction = Input.GetVector("left", "right", "up", "down");
        // Establecer la dirección del personaje solo si no está en el proceso de trepar
        if (!isClimbing)
        {
            if (direction.X > 0)
            {
                x_move = 15;
                isFacingRight = true;
            }
            else if (direction.X < 0)
            {
                x_move = -15;
                isFacingRight = false;
            }
        }
    }


    public override void state_input(InputEvent @event)
    {
        // Responder a los inputs solo si se puede mover
        if (@event.IsActionPressed("up") && !isClimbing && isHanging)
        {

            playback.Travel(corner_climb);
            animFrameCount = 0; // Reiniciar el contador de frames
            tempAnim.Start(); // Iniciar el temporizador
            isClimbing = true;

        }

        // Permitir al personaje soltarse del acantilado al presionar una tecla hacia abajo
        if (@event.IsActionPressed("down") && !isClimbing && isHanging)
        {
            next_state = ground_state;
        }
        if (@event.IsActionPressed("left") && isFacingRight && !isClimbing && isHanging)
        {
            next_state = air_state;
        }
        if (@event.IsActionPressed("right") && !isFacingRight && !isClimbing && isHanging)
        {
            next_state = air_state;
        }
    }

    public override void on_enter()
    {
        // Detener la animación de movimiento al colgarse del acantilado
        if (!isHanging)
        {
            playback.Travel("corner_grab");
            isHanging = true;
        }
    }


    public override void on_exit()
    {
        if (next_state == ground_state)
        {
            playback.Travel("Movement");
        }

        if (next_state == air_state)
        {
            playback.Travel("jump_loop");
        }
        isHanging = false;
        isClimbing = false;
    }

    public void AnimationTimeout()
    {
        character.Position = new Vector2(character.Position.X, character.Position.Y - 7);

        // Incrementar el contador de frames
        animFrameCount++;

        // Si el contador de frames es menor que 4, reiniciar el temporizador
        if (animFrameCount < 4)
        {
            tempAnim.Start();
        }
        else
        {
            // Si el contador de frames es igual a 4, detener el temporizador y reiniciar el contador
            tempAnim.Stop();
            animFrameCount = 0;
            next_state = ground_state;
            character.Position = new Vector2(character.Position.X + x_move, character.Position.Y);

        }
    }
}
