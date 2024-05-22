using Godot;
using System;

public partial class Damageable : Node
{
    private signal_bus signal_bus;

    private int _health = 20;

    [Export]
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            // Validamos que la vida no sea negativa
            if (value < 0) value = 0;

            // Solo emitimos la señal si hay un cambio en el valor de Health
            if (_health != value)
            {
                _health = value;
                signal_bus.EmitSignal("OnHealthChanged", GetParent(), _health);
				
            }
        }
    }

    public override void _Ready()
    {
        signal_bus = GetNode<signal_bus>("/root/signal_bus");
        Health = _health;  // Aseguramos que se llame al setter para emitir la señal inicial
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
