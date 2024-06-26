using Godot;
using Godot.Collections;
using System;


public partial class StateMachine : Node
{
	[Export]
	public State currentState;
	[Export]
	public CharacterBody2D character;

	[Export]
	public AnimationTree animationTree;

	[Export]
	public AnimationPlayer animationPlayer;

	[Export]
	public Area2D cliffCollisionShape;

	[Export]
	public State CliffState;


	public bool isFacingRight;

	private AnimationNodeStateMachinePlayback playback;



	private Array<State> states;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{	
		// We check if the character is set and if the current state is set
		if (character == null)
		{
			GD.PushWarning("Character is not set");
		
		}
		if (currentState == null) {
			GD.PushWarning("Current State is not set");
			return;
		}
		if (animationTree == null) {
			GD.PushWarning("Animation Tree is not set");
			return;
		}
		if (animationPlayer == null) {
			GD.PushWarning("Animation Player is not set");
			return;
		}
		if (cliffCollisionShape == null) {
			GD.PushWarning("Cliff Collision Shape is not set");
		}

		playback = (AnimationNodeStateMachinePlayback) animationTree.Get("parameters/playback");

		
		
		
		// We set the character to the states
		states = new Array<State>();
		for (int i = 0; i < GetChildCount(); i++)
		{
			if (GetChild(i) is State) {
				
				states.Add(GetChild<State>(i));
				GetChild<State>(i).character = character;
				GetChild<State>(i).playback = playback;
				GetChild<State>(i).animationPlayer = animationPlayer;
				GetChild<State>(i).cliffCollisionShape = cliffCollisionShape;
				GetChild<State>(i).SetCliffCollisionShape(cliffCollisionShape);

				GetChild<State>(i).Connect("InterruptState", new Callable(this, "on_state_interrupted"));

			} else { 
				GD.PushWarning("Child" + GetChild(i).Name + " is not a State");
			}
		}

		// We set the first state

		
	}

	public bool check_if_it_can_move()
	{
		return currentState.canMove;
	}

	public bool check_if_it_can_turn()
	{
		return currentState.canTurn;
	}
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        if (currentState != null) {
			switch_states(currentState.next_state);
		}

		currentState._Process(delta);


    }
	
	public void switch_states(State next_state)
	{
		if (next_state != null)
		{
			currentState.on_exit();
			currentState = next_state;
			currentState.next_state = null;

		}

		currentState.on_enter();
	}

	public void _input(InputEvent @event)
	{
		currentState.state_input(@event);
	}


	public void on_state_interrupted(State state)
	{
		switch_states(state);
	}	
}
