using Godot;
using System;

public partial class Damageable : Node
{
    [Signal]
    public delegate void OnHitEventHandler(Node node, int amage_taken);
    private signal_bus signal_bus;

    private int _health = 100;

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
                if (signal_bus != null)
                {
                    signal_bus.EmitSignal("OnHealthChanged", GetParent(), _health);
                }
                else
                {
                    GD.PrintErr("signal_bus is null in Health setter");
                }
            }
        }
    }

    public override void _Ready()
    {
        signal_bus = GetNode<signal_bus>("/root/signal_bus");
        if (signal_bus == null)
        {
            GD.PrintErr("Failed to get signal_bus node");
        }
        Health = _health;  // Aseguramos que se llame al setter para emitir la señal inicial
    }

    public override void _Process(double delta)
    {
    }

    public void TakeDamage(int damage)
    {
        this.EmitSignal("OnHit", GetParent(), damage);
        Health -= damage;
        
    }

    public void _on_animation_tree_animation_finished(String anim_name)
    {
        if (anim_name == "die")
        {
            GetParent().QueueFree();
        }
    }
    
}
