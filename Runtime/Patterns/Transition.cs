// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.
// Based on Transition by Adam Myhre (adammyhre)

using System;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Non-generic transition base. Evaluated each tick by CharacterState.GetTransition().
    /// </summary>
    public abstract class Transition
    {
        public abstract bool Evaluate();
        public abstract void Apply(CharacterStateMachine machine);
    }

    /// <summary>
    /// Typed transition. When the condition returns true, applies the target state
    /// to the state machine via a type-safe ChangeState call.
    /// </summary>
    public class Transition<TState> : Transition where TState : CharacterState<TState>
    {
        readonly Func<bool> condition;
        readonly TState target;

        public Transition(TState target, Func<bool> condition)
        {
            this.target = target;
            this.condition = condition;
        }

        public override bool Evaluate() => condition();

        public override void Apply(CharacterStateMachine machine)
        {
            machine.ChangeState(target);
        }
    }
}
