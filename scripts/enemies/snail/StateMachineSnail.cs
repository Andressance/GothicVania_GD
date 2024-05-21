using Godot;
using Godot.Collections;
using System;

public partial class StateMachineSnail : Node
{
	[Export]
	public State currentState;

	[Export]
	public AnimationTree animationTree;

	[Export]
	public AnimationPlayer animationPlayer;

	public bool isFacingRight;

	private AnimationNodeStateMachinePlayback playback;

	private Array<State> states;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (currentState == null)
		{
			GD.PushWarning("Current State is not set");
			return;
		}
		if (animationTree == null)
		{
			GD.PushWarning("Animation Tree is not set");
			return;
		}
		if (animationPlayer == null)
		{
			GD.PushWarning("Animation Player is not set");
			return;
		}
		
		playback = (AnimationNodeStateMachinePlayback) animationTree.Get("parameters/playback");

		states = new Array<State>();

		foreach (State state in GetChildren())
		{
			states.Add(state);
			state.playback = playback;
			state.animationPlayer = animationPlayer;
		}


	}

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
}
