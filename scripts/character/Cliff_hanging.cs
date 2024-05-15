using Godot;
using System;

public partial class Cliff_hanging : State
{
    [Export]
    public State air_state;

    [Export]
    public State ground_state;

    // Variable para almacenar la velocidad original del personaje antes de colgarse del acantilado
    private Vector2 originalVelocity;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.canMove = false; // Desactivar el movimiento mientras está colgado
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // Mantener al personaje estático mientras está colgado
        // character.Velocity = Vector2.Zero;

        
    }

    public override void state_input(InputEvent @event)
    {
        // Solo responder a los inputs cuando está colgado
        if (character.IsOnFloor())
        {
            return;
        }

        // Responder a los inputs solo si se puede mover
        if (@event.IsActionPressed("up"))
        {
            next_state = ground_state;
        }

        // Permitir al personaje soltarse del acantilado al presionar una tecla hacia abajo
        if (@event.IsActionPressed("down"))
        {
            next_state = air_state;
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
}
