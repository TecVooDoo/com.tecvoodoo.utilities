// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

// Needs the Input System package; the define comes from the asmdef's versionDefines, so projects
// without com.unity.inputsystem still compile TVU (this type is simply absent there).
#if TVU_INPUT_SYSTEM
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Tracks which kind of device the player used LAST -- a gamepad, or a pointer (mouse / keyboard / touch).
    /// </summary>
    /// <remarks>
    /// Decided from Input System EVENTS that actually CHANGE a control, never from device presence or
    /// <c>lastUpdateTime</c>: some pads (PlayStation-style HID) stream reports while idle, so "the pad reported
    /// most recently" is true for a mouse player who merely has one plugged in. A pad only counts when a control
    /// moves past <see cref="PadThreshold"/>, which stick jitter does not reach. Mouse, keyboard and touch emit no
    /// idle noise, so any change from them counts.
    ///
    /// Promoted from BloodMiner (BM S306, <c>BM.Game.LastInputDevice</c>) in TVU 1.4.0. A virtual pointer driven by
    /// a stick (a gamepad cursor's virtual Mouse) is gamepad input: register it with
    /// <see cref="RegisterVirtualGamepadDevice"/>.
    /// </remarks>
    public static class LastInputDevice
    {
        /// <summary>Default for <see cref="PadThreshold"/>; above measured stick jitter (BM S306).</summary>
        public const float DefaultPadThreshold = 0.5f;

        static readonly List<InputDevice> s_virtualGamepadDevices = new List<InputDevice>(2);
        static bool s_subscribed;

        /// <summary>True when the most recent real input came from a gamepad.</summary>
        public static bool IsGamepad { get; private set; }

        /// <summary>How far a gamepad control must move to count as gamepad use (0..1).</summary>
        public static float PadThreshold { get; set; } = DefaultPadThreshold;

        /// <summary>Raised when <see cref="IsGamepad"/> flips. The new value is already set.</summary>
        public static event Action Changed;

        /// <summary>Starts listening to Input System events. Called automatically before the first scene loads; idempotent.</summary>
        public static void Enable()
        {
            InputSystem.onEvent -= OnEvent;
            InputSystem.onEvent += OnEvent;
            s_subscribed = true;
        }

        /// <summary>Stops listening. <see cref="IsGamepad"/> keeps its last value.</summary>
        public static void Disable()
        {
            InputSystem.onEvent -= OnEvent;
            s_subscribed = false;
        }

        /// <summary>Treats a (usually virtual) pointer device as gamepad input, e.g. a stick-driven cursor's Mouse.</summary>
        /// <param name="device">The device to treat as a gamepad. Unregister it before removing the device.</param>
        public static void RegisterVirtualGamepadDevice(InputDevice device)
        {
            if (device != null && !s_virtualGamepadDevices.Contains(device))
                s_virtualGamepadDevices.Add(device);
        }

        /// <summary>Reverses <see cref="RegisterVirtualGamepadDevice"/>.</summary>
        /// <param name="device">The device to stop treating as a gamepad.</param>
        public static void UnregisterVirtualGamepadDevice(InputDevice device)
        {
            s_virtualGamepadDevices.Remove(device);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            // Domain reload can be disabled (Enter Play Mode Options), so statics must be cleared by hand.
            Disable();
            IsGamepad = false;
            PadThreshold = DefaultPadThreshold;
            Changed = null;
            s_virtualGamepadDevices.Clear();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void AutoEnable()
        {
            Enable();
        }

        static void OnEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
                return;

            // A stick-driven virtual pointer IS using the pad.
            bool pad = device is Gamepad || s_virtualGamepadDevices.Contains(device);
            if (!pad && !(device is Pointer) && !(device is Keyboard))
                return; // Touchscreen is a Pointer
            if (pad == IsGamepad)
                return; // already the active kind -- skip the control enumeration

            if (!AnyControlChanged(eventPtr, device, pad ? PadThreshold : 0f))
                return;

            IsGamepad = pad;
            Changed?.Invoke();
        }

        // A foreach over the struct collection instead of LINQ Any(): this runs on input events.
        static bool AnyControlChanged(InputEventPtr eventPtr, InputDevice device, float threshold)
        {
            foreach (InputControl _ in eventPtr.EnumerateChangedControls(device, threshold))
                return true;
            return false;
        }

        /// <summary>True while subscribed to Input System events.</summary>
        public static bool IsEnabled => s_subscribed;
    }
}
#endif
