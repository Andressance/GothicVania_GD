using Godot;
using System;

public partial class health_changed_manager : Control
{
    [Export]
    public PackedScene health_changed_label;

    [Export]
    public Color damage_color = new Color(1, 0, 0, 1);

    [Export]
    public Color heal_color = new Color(0, 1, 0, 1);

    private signal_bus signal_bus;

    public override void _Ready()
    {
        signal_bus = GetNode<signal_bus>("/root/signal_bus");

        if (signal_bus != null)
        {
            // Conecta la señal al método local
            signal_bus.Connect("OnHealthChanged", new Callable(this, "on_signal_health_changed"));
        }
        else
        {
            GD.PrintErr("No se encontró el nodo SignalBus");
        }
    }

    public void on_signal_health_changed(Node node, int amount)
    {        
        /*
        var label_instance = health_changed_label.Instantiate();
        node.AddChild(label_instance);
        // Get the label node and set the text
        for (int i = 0; i < node.GetChildCount(); i++)
        {
            if (node.GetChild(i) is Label)
            {
                (label_instance as Label).Text = amount.ToString();
                if (amount > 0)
                {
                    (label_instance as Label).Modulate = damage_color;
                }
                else
                {
                    (label_instance as Label).Modulate = heal_color;
                }
            }
        }
        */

        
    }
}
