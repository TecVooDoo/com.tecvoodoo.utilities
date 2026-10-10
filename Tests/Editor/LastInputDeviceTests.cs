// TecVooDoo Utilities - Tests
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

#if TVU_INPUT_SYSTEM
using System.Reflection;
using NUnit.Framework;
using TecVooDoo.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TecVooDoo.Utilities.Tests
{
    /// <summary>
    /// Drives real Input System events through InputTestFixture (an isolated input manager per test).
    /// </summary>
    [TestFixture]
    public class LastInputDeviceTests : InputTestFixture
    {
        int _changedCount;

        public override void Setup()
        {
            base.Setup();
            // Clear statics left by an earlier test, then subscribe to THIS fixture's input manager.
            typeof(LastInputDevice).GetMethod("ResetStatics", BindingFlags.NonPublic | BindingFlags.Static)!.Invoke(null, null);
            LastInputDevice.Enable();
            _changedCount = 0;
            LastInputDevice.Changed += OnChanged;
        }

        public override void TearDown()
        {
            LastInputDevice.Changed -= OnChanged;
            LastInputDevice.Disable();
            base.TearDown();
        }

        void OnChanged()
        {
            _changedCount++;
        }

        [Test]
        public void StartsAsPointer()
        {
            Assert.IsFalse(LastInputDevice.IsGamepad);
            Assert.IsTrue(LastInputDevice.IsEnabled);
        }

        [Test]
        public void StickPastThreshold_SwitchesToGamepad_AndRaisesChangedOnce()
        {
            Gamepad pad = InputSystem.AddDevice<Gamepad>();

            Set(pad.leftStick, new Vector2(0.9f, 0f));

            Assert.IsTrue(LastInputDevice.IsGamepad);
            Assert.AreEqual(1, _changedCount);
        }

        [Test]
        public void StickJitterBelowThreshold_StaysPointer()
        {
            // The BM S306 case: an idle pad streaming stick noise must not steal the mouse player's prompts.
            // 0.3 is deliberately ABOVE the Input System's default stick deadzone (0.125), so it is
            // PadThreshold (0.5) that rejects it -- a value inside the deadzone would pass for the wrong reason.
            Gamepad pad = InputSystem.AddDevice<Gamepad>();

            Set(pad.leftStick, new Vector2(0.3f, 0f));

            Assert.IsFalse(LastInputDevice.IsGamepad);
            Assert.AreEqual(0, _changedCount);
        }

        [Test]
        public void ButtonPress_SwitchesToGamepad()
        {
            Gamepad pad = InputSystem.AddDevice<Gamepad>();

            Press(pad.buttonSouth);

            Assert.IsTrue(LastInputDevice.IsGamepad);
        }

        [Test]
        public void MouseMoveAfterPad_SwitchesBackToPointer()
        {
            Gamepad pad = InputSystem.AddDevice<Gamepad>();
            Mouse mouse = InputSystem.AddDevice<Mouse>();
            Press(pad.buttonSouth);

            Set(mouse.position, new Vector2(120f, 80f));

            Assert.IsFalse(LastInputDevice.IsGamepad);
            Assert.AreEqual(2, _changedCount);
        }

        [Test]
        public void KeyPressAfterPad_SwitchesBackToPointer()
        {
            Gamepad pad = InputSystem.AddDevice<Gamepad>();
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            Press(pad.buttonSouth);

            Press(keyboard.spaceKey);

            Assert.IsFalse(LastInputDevice.IsGamepad);
        }

        [Test]
        public void SameKindAgain_DoesNotRaiseChanged()
        {
            Gamepad pad = InputSystem.AddDevice<Gamepad>();
            Press(pad.buttonSouth);
            Release(pad.buttonSouth);
            Press(pad.buttonEast);

            Assert.AreEqual(1, _changedCount);
        }

        [Test]
        public void RegisteredVirtualMouse_CountsAsGamepad()
        {
            Mouse virtualMouse = InputSystem.AddDevice<Mouse>("VirtualCursor");
            LastInputDevice.RegisterVirtualGamepadDevice(virtualMouse);

            Set(virtualMouse.position, new Vector2(50f, 50f));

            Assert.IsTrue(LastInputDevice.IsGamepad);
        }

        [Test]
        public void UnregisteredVirtualMouse_CountsAsPointerAgain()
        {
            Gamepad pad = InputSystem.AddDevice<Gamepad>();
            Mouse virtualMouse = InputSystem.AddDevice<Mouse>("VirtualCursor");
            LastInputDevice.RegisterVirtualGamepadDevice(virtualMouse);
            LastInputDevice.UnregisterVirtualGamepadDevice(virtualMouse);
            Press(pad.buttonSouth);

            Set(virtualMouse.position, new Vector2(50f, 50f));

            Assert.IsFalse(LastInputDevice.IsGamepad);
        }

        [Test]
        public void PadThreshold_IsConfigurable()
        {
            Gamepad pad = InputSystem.AddDevice<Gamepad>();
            LastInputDevice.PadThreshold = 0.1f;

            // Same 0.3 that the default threshold rejects (above the 0.125 stick deadzone).
            Set(pad.leftStick, new Vector2(0.3f, 0f));

            Assert.IsTrue(LastInputDevice.IsGamepad);
        }

        [Test]
        public void Disable_StopsTracking()
        {
            Gamepad pad = InputSystem.AddDevice<Gamepad>();
            LastInputDevice.Disable();

            Press(pad.buttonSouth);

            Assert.IsFalse(LastInputDevice.IsGamepad);
            Assert.IsFalse(LastInputDevice.IsEnabled);
        }
    }
}
#endif
