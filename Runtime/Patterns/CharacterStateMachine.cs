// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.
// Based on CharacterStateMachine by Adam Myhre (adammyhre)

using System.Collections.Generic;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Lightweight state machine for character states. Evaluates transitions each tick
    /// and automatically switches state when a condition is met.
    /// </summary>
    public class CharacterStateMachine
    {
        CharacterState current;

        public void ChangeState<TState>(TState newState) where TState : CharacterState<TState>
        {
            current?.Exit();
            current = newState;
            current.Enter();
        }

        public void Tick(float dt)
        {
            if (current == null) return;

            Transition t = current.GetTransition();
            if (t != null)
            {
                t.Apply(this);
                return;
            }

            current.Tick(dt);
        }
    }

    /// <summary>
    /// CRTP base for typed states. Override OnEnter, OnExit, OnTick.
    /// Transitions added via SetTransition are evaluated before each tick.
    /// </summary>
    public abstract class CharacterState<TState> : CharacterState where TState : CharacterState<TState>
    {
        public override void Enter()  => ((TState)this).OnEnter();
        public override void Exit()   => ((TState)this).OnExit();
        public override void Tick(float dt) => ((TState)this).OnTick(dt);

        protected abstract void OnEnter();
        protected abstract void OnExit();
        protected abstract void OnTick(float dt);
    }

    /// <summary>
    /// Non-generic base. Holds the transition list and abstract lifecycle methods.
    /// Prefer CharacterState&lt;TState&gt; for concrete states.
    /// </summary>
    public abstract class CharacterState
    {
        readonly List<Transition> transitions = new List<Transition>();

        public Transition GetTransition()
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if (transitions[i].Evaluate()) return transitions[i];
            }
            return null;
        }

        public void SetTransition<TState>(Transition<TState> t) where TState : CharacterState<TState>
        {
            transitions.Add(t);
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Tick(float dt);
    }
}
