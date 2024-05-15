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

    private Timer tempAnim;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.canMove = false; // Desactivar el movimiento mientras está colgado
        // Mantener al personaje estático mientras está colgado
        tempAnim = new Timer();
        AddChild(tempAnim);
        tempAnim.WaitTime = 0.6f;
        tempAnim.OneShot = true;
        tempAnim.Connect("timeout", new Callable(this, "AnimationTimeout"));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        

    }

    public override void state_input(InputEvent @event)
    {

        // Responder a los inputs solo si se puede mover
        if (@event.IsActionPressed("up"))
        {
            GD.Print("up");
            playback.Travel("corner_climb");
            tempAnim.Start();
            

        }

        // Permitir al personaje soltarse del acantilado al presionar una tecla hacia abajo
        if (@event.IsActionPressed("down"))
        {
            next_state = air_state;
        }

        
    }

    public override void on_enter() {
        // Detener la animación de movimiento al colgarse del acantilado
        playback.Travel("corner_grab");
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
        playback.Travel("corner_climb");
        // Permitir al personaje subir al acantilado al presionar una tecla hacia arriba, si esta hacia la derecha
            if (isfacingRight) {
                character.Position = character.Position += new Vector2(-10, -30);
            } else {
                character.Position = character.Position += new Vector2(10, -30);
            }
            next_state = ground_state;
    }

    
}
