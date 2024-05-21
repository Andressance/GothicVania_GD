using Godot;
using System;

public partial class health_changed_manager : Control
{
    [Export]
    public PackedScene health_changed_label;

    public override void _Ready()
    {
        var signalBus = GetNodeOrNull("/root/SignalBus");
        if (signalBus != null)
        {
            GD.Print("SignalBus encontrado." + signalBus);
            signalBus.Connect("OnHealthChangedEventHandler", new Callable(this, "on_signal_health_changed"));
        }
        else
        {
            GD.PushWarning("SignalBus no encontrado.");
        }
    }

    public override void _Process(double delta)
    {
    }

    public void on_signal_health_changed(Node node, int amount)
    {
        Label label_instance = (Label)health_changed_label.Instantiate();
        node.AddChild(label_instance);
        label_instance.Text = amount.ToString();
        GD.Print("on_signal_health_changed: " + amount);
    }
}
