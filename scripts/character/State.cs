using Godot;
using System;

public abstract partial class State : Node
{

	[Export]	
	public bool canMove = true;

	[Export]
	public bool canTurn;

	public CharacterBody2D character;

	public State next_state;

	public AnimationNodeStateMachinePlayback playback;

	public AnimationPlayer animationPlayer;

	public Area2D cliffCollisionShape;

	public bool isClimbing;

	public bool isSliding;

	public bool isAttacking;

	public bool isArmed;

	public bool facingRight;

	/*******************************SIGNALS**********************************************/

	[Signal]
	public delegate void InterruptStateEventHandler(State state);
	

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
