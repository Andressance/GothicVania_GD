using Godot;
using System;

public abstract partial class State : Node
{

	[Export]
	public bool canMove = true;

	public CharacterBody2D character;

	public State next_state;

	public AnimationNodeStateMachinePlayback playback;

	public AnimationPlayer animationPlayer;

	public Timer timer;

	public Area2D cliffCollisionShape;

	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public virtual void on_exit()
	{
	}

	public virtual void on_enter()
	{
	}

	public virtual void state_input(InputEvent @event)
	{
	}

	public void SetCliffCollisionShape(Area2D cliffShape)
	{
		cliffCollisionShape = cliffShape;
	}

}
