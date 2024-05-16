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
        // Si el personaje está mirando a la derecha el movimiento será hacia la derecha
        direction = Input.GetVector("left", "right", "up", "down");

        if (direction.X > 0)
        {
            x_move = 10;
            isFacingRight = false;
        }
        else if (direction.X < 0)
        {
            x_move = -10;
            isFacingRight = true;
        }
    }

    public override void state_input(InputEvent @event)
    {
        // Responder a los inputs solo si se puede mover
        if (@event.IsActionPressed("up"))
        {
            if (isHanging)
            {
                playback.Travel(corner_climb);
                animFrameCount = 0; // Reiniciar el contador de frames
                tempAnim.Start(); // Iniciar el temporizador
            }
        }

        // Permitir al personaje soltarse del acantilado al presionar una tecla hacia abajo
        if (@event.IsActionPressed("down"))
        {
            next_state = air_state;
        }

        if (@event.IsActionPressed("left") && isFacingRight)
        {
            next_state = air_state;
        }
        if (@event.IsActionPressed("right") && !isFacingRight)
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

    // Método para permitir al personaje soltarse del acantilado
    public void ReleaseFromCliff()
    {
        canMove = true; // Activar el movimiento al soltarse del acantilado
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
    }

    public void AnimationTimeout()
    {
        character.Position = new Vector2(character.Position.X, character.Position.Y - 5);

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
            isHanging = false;
            character.Position = new Vector2(character.Position.X + x_move, character.Position.Y);

        }
    }
}
