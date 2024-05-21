using Godot;
using System;

public partial class Damageable : Node
{
    private int _health = 20;

    [Export]
    public int Health
    {
        get { return _health; }
        set
        {
            var signalBus = (signal_bus)GetNode("/root/SignalBus");
            if (signalBus != null)
            {
                signalBus.EmitHealthChanged(this, value - _health);
            }
            else
            {
                GD.PrintErr("SignalBus no encontrado.");
            }

            int oldHealth = _health;
            _health = value;
        }
    }

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            GetParent().QueueFree();
        }
    }
}
