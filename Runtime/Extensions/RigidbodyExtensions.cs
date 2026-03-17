// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Extension methods for Rigidbody.
    /// </summary>
    public static class RigidbodyExtensions
    {
        /// <summary>
        /// Changes the direction of the Rigidbody's velocity while preserving its speed.
        /// If the direction is zero, the Rigidbody is unchanged.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody to modify.</param>
        /// <param name="direction">The new movement direction (will be normalized).</param>
        /// <returns>The Rigidbody for method chaining.</returns>
        public static Rigidbody ChangeDirection(this Rigidbody rigidbody, Vector3 direction)
        {
            if (direction.sqrMagnitude == 0f) return rigidbody;
            direction.Normalize();

#if UNITY_6000_0_OR_NEWER
            rigidbody.linearVelocity = direction * rigidbody.linearVelocity.magnitude;
#else
            rigidbody.velocity = direction * rigidbody.velocity.magnitude;
#endif
            return rigidbody;
        }

        /// <summary>
        /// Stops the Rigidbody by setting both linear and angular velocities to zero.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody to stop.</param>
        /// <returns>The Rigidbody for method chaining.</returns>
        public static Rigidbody Stop(this Rigidbody rigidbody)
        {
#if UNITY_6000_0_OR_NEWER
            rigidbody.linearVelocity = Vector3.zero;
#else
            rigidbody.velocity = Vector3.zero;
#endif
            rigidbody.angularVelocity = Vector3.zero;
            return rigidbody;
        }
    }
}
