using Godot;
using System;

public partial class signal_bus : Node
{
    [Signal]
    public delegate void OnHealthChangedEventHandler(Node node, int newHealth);

    public override void _Ready()
    {
    }

    public void EmitHealthChanged(Node node, int newHealth)
    {
        EmitSignal(nameof(OnHealthChangedEventHandler), node, newHealth);
    }
}
