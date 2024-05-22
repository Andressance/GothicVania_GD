using Godot;
using System;

public partial class signal_bus : Node
{
	[Signal]
    public delegate void OnHealthChangedEventHandler(Node node, int amount);

    public override void _Ready()
    {
        GD.Print("SignalBus está listo y cargado como autoload");
		GD.Print("Ruta del nodo: " + GetPath());
    }


}
