using Godot;
using System;

public partial class snail : CharacterBody2D {

    private signal_bus signal_bus;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        
        signal_bus = GetNode<signal_bus>("/root/signal_bus");
    }

}