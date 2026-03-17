# TecVooDoo Utilities - Code Reference

Purpose: Quick reference for existing code, APIs, and conventions. Check this before writing new code to avoid duplicating functionality or referencing non-existent classes.

Last Updated: 2026-02-07 (Session 6 - verified against 85/85 NUnit tests)

---

## Namespaces

| Namespace | Purpose | Status |
|-----------|---------|--------|
| TecVooDoo.Utilities | Root - extensions, common types | Implemented |
| TecVooDoo.Utilities.Timers | PlayerLoop-based timer system | Implemented |
| TecVooDoo.Utilities.Patterns | Singleton, PersistentSingleton, RegulatorSingleton | Implemented |
| TecVooDoo.Utilities.Gameplay | Reusable gameplay components (LookAtCamera) | Implemented |
| TecVooDoo.Utilities.Logging | Category-based debug logging | Implemented |
| TecVooDoo.Utilities.Editor | Editor-only utilities | Implemented |

---

## Scripts

### TecVooDoo.Utilities (Extensions)

#### VectorExtensions.cs

**Path:** `Runtime/Extensions/VectorExtensions.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library

**API:**

| Method | Signature | Description |
|--------|-----------|-------------|
| With | `Vector3.With(float? x, float? y, float? z)` | Replace individual components. `pos.With(y: 0)` returns (pos.x, 0, pos.z) |
| Flat | `Vector3.Flat()` | Zero Y component. Equivalent to `With(y: 0)` |
| DirectionTo | `Vector3.DirectionTo(Vector3 target)` | Normalized direction from this to target |

**Usage:**

```csharp
Vector3 position = transform.position;
Vector3 flatPos = position.Flat();                    // (x, 0, z)
Vector3 raised = position.With(y: 5f);                // (x, 5, z)
Vector3 dir = position.DirectionTo(target.position);  // normalized
```

---

#### TransformExtensions.cs

**Path:** `Runtime/Extensions/TransformExtensions.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library

**API:**

| Method | Signature | Description |
|--------|-----------|-------------|
| GetOrAdd<T> | `Transform.GetOrAdd<T>()` where T : Component | Get existing component or add new one |
| Children | `Transform.Children()` | Enumerate all direct children as IEnumerable<Transform> |

**Usage:**

```csharp
Rigidbody rb = transform.GetOrAdd<Rigidbody>();  // never null
foreach (Transform child in transform.Children()) { /* iterate children */ }
```

**Notes:**
- `OrNull()` is on GameObjectExtensions (works on any UnityEngine.Object subtype)

---

#### GameObjectExtensions.cs

**Path:** `Runtime/Extensions/GameObjectExtensions.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library

**API:**

| Method | Signature | Description |
|--------|-----------|-------------|
| OrNull<T> | `UnityEngine.Object.OrNull<T>()` | Returns C# null if destroyed (solves fake-null) |
| GetOrAdd<T> | `GameObject.GetOrAdd<T>()` | Get existing component or add new one |
| HasComponent<T> | `GameObject.HasComponent<T>()` | Returns bool via TryGetComponent (no allocation) |

**Notes:**
- `OrNull()` works on any UnityEngine.Object subtype (GameObject, Transform, Component, etc.)
- Solves the Unity fake-null problem where `== null` returns true but C# `?.` does not

---

#### CollectionExtensions.cs

**Path:** `Runtime/Extensions/CollectionExtensions.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library

**API:**

| Method | Signature | Description |
|--------|-----------|-------------|
| Shuffle<T> | `IList<T>.Shuffle()` | Fisher-Yates in-place shuffle |
| RandomElement<T> | `IList<T>.RandomElement()` | Return random element |
| IsNullOrEmpty<T> | `IEnumerable<T>.IsNullOrEmpty()` | Null-safe empty check for any enumerable |
| ForEach<T> | `IEnumerable<T>.ForEach(Action<T>)` | Apply action to each element without LINQ |

**Notes:**
- List-specific operations (Swap, RefreshWith) are in ListExtensions.cs.
- ForEach avoids needing `using System.Linq` just for iteration side-effects.

---

#### NumberExtensions.cs

**Path:** `Runtime/Extensions/NumberExtensions.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library

**API:**

| Method | Signature | Description |
|--------|-----------|-------------|
| Remap | `float.Remap(float fromMin, float fromMax, float toMin, float toMax)` | Map value from one range to another |
| Approximately | `float.Approximately(float other)` | Wrapper for Mathf.Approximately |
| IsOdd | `int.IsOdd()` | Odd check via bitwise AND |
| IsEven | `int.IsEven()` | Even check via bitwise AND |

---

#### ColorExtensions.cs

**Path:** `Runtime/Extensions/ColorExtensions.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library

**API:**

| Method | Signature | Description |
|--------|-----------|-------------|
| WithAlpha | `Color.WithAlpha(float alpha)` | Return color with different alpha |
| Add | `Color.Add(Color other)` | Add RGBA components, clamped to 0-1 |
| Subtract | `Color.Subtract(Color other)` | Subtract RGBA components, clamped to 0-1 |
| Blend | `Color.Blend(Color target, float ratio)` | Linear blend between two colors |
| Invert | `Color.Invert()` | Return inverted color (1-r, 1-g, 1-b), alpha preserved |
| ToHex | `Color.ToHex()` | Convert Color to hex string (#RRGGBBAA) |
| FromHex | `ColorExtensions.FromHex(string hex)` | Parse hex string to Color (static method) |

**Usage:**

```csharp
Color faded = Color.red.WithAlpha(0.5f);       // red at 50% opacity
string hex = someColor.ToHex();                  // "#FF0000FF"
Color opposite = someColor.Invert();             // inverted RGB
Color mixed = Color.red.Blend(Color.blue, 0.5f); // purple
```

---

#### StringExtensions.cs

**Path:** `Runtime/Extensions/StringExtensions.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library (selective)

**API:**

| Method | Signature | Description |
|--------|-----------|-------------|
| IsBlank | `string.IsBlank()` | True if null, empty, or whitespace-only |
| OrEmpty | `string.OrEmpty()` | Returns string or empty string if null |
| Truncate | `string.Truncate(int maxLength)` | Truncate to max length |
| Slice | `string.Slice(int start, int end)` | Substring with negative end index support |
| ToAlphanumeric | `string.ToAlphanumeric(bool allowPeriods)` | Strip non-alphanumeric chars (loop-based, no LINQ) |
| RichColor | `string.RichColor(string color)` | Wrap in rich text color tag |
| RichSize | `string.RichSize(int size)` | Wrap in rich text size tag |
| RichBold | `string.RichBold()` | Wrap in rich text bold tags |
| RichItalic | `string.RichItalic()` | Wrap in rich text italic tags |

**Notes:**
- ToAlphanumeric rewrites the Git Amend source using explicit loops instead of LINQ.
- Rich text helpers are useful for debug logging with CategoryLogger.

---

#### LayerMaskExtensions.cs

**Path:** `Runtime/Extensions/LayerMaskExtensions.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library

**API:**

| Method | Signature | Description |
|--------|-----------|-------------|
| Contains | `LayerMask.Contains(int layer)` | Check if layer is in mask via bitwise AND |

**Usage:**

```csharp
LayerMask groundLayers = LayerMask.GetMask("Ground", "Terrain");
bool isGround = groundLayers.Contains(hitObject.layer);
```

**Notes:**
- One-liner but used constantly. `(mask & (1 << layer)) != 0` is easy to mistype.

---

#### ListExtensions.cs

**Path:** `Runtime/Extensions/ListExtensions.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library (expanded)

**API:**

| Method | Signature | Description |
|--------|-----------|-------------|
| IsNullOrEmpty<T> | `List<T>.IsNullOrEmpty()` | Null-safe empty check |
| Swap<T> | `IList<T>.Swap(int a, int b)` | Swap two elements in place |
| RefreshWith<T> | `List<T>.RefreshWith(IEnumerable<T>)` | Clear + AddRange (sweep-list pattern) |

**Notes:**
- Shuffle and RandomElement moved here from CollectionExtensions (they operate on IList, not IEnumerable).
- RefreshWith is used by TimerManager for safe iteration during callbacks.
- Source Fisher-Yates shuffle is clean, no allocation.

---

#### RigidbodyExtensions.cs

**Path:** `Runtime/Extensions/RigidbodyExtensions.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library

**API:**

| Method | Signature | Description |
|--------|-----------|-------------|
| Stop | `Rigidbody.Stop()` | Zero out velocity and angular velocity |
| ChangeDirection | `Rigidbody.ChangeDirection(Vector3 direction)` | Redirect velocity to new direction, preserving magnitude |

**Notes:**
- Source already handles Unity 6 `linearVelocity` vs legacy `velocity` via conditional compilation.
- Stop() zeroes both linear and angular velocity in one call.

---

### TecVooDoo.Utilities.Timers

#### Timer.cs

**Path:** `Runtime/Timers/Timer.cs`
**Type:** Abstract class, IDisposable
**Status:** Implemented
**Origin:** Git Amend Improved Timers (rewrite)

**API:**

| Member | Type | Description |
|--------|------|-------------|
| CurrentTime | float (property) | Current elapsed/remaining time |
| IsRunning | bool (property) | Whether timer is active |
| Progress | float (property) | Concrete - Mathf.Clamp01(CurrentTime / initialTime) |
| IsFinished | bool (abstract property) | Whether timer has completed its cycle |
| OnTimerStart | Action | Fires when timer starts |
| OnTimerStop | Action | Fires when timer stops |
| Start | void | Reset CurrentTime, register with TimerManager, invoke OnTimerStart |
| Stop | void | Deregister from TimerManager, invoke OnTimerStop |
| Pause | void | Set IsRunning false without deregistering |
| Resume | void | Set IsRunning true |
| Reset() | void (virtual) | Reset CurrentTime to initialTime |
| Reset(float) | void (virtual) | Reset with new initialTime |
| Tick | void (abstract) | Called by TimerManager each frame |
| Dispose | void | Deregister and suppress finalizer |

**Notes:**
- Uses public Action fields (same as source) for simplicity - callers can subscribe/unsubscribe freely.
- Constructor initializes both `initialTime` and `CurrentTime` to the provided value (Session 4 bug fix - was defaulting CurrentTime to 0f).
- All timer types derive from this base. Call Dispose() when the owning object is destroyed.
- Implements proper IDisposable with destructor/finalizer pattern (GC.SuppressFinalize).
- Uses explicit types throughout (no `var`). Full XML documentation on all public members.

---

#### TimerManager.cs

**Path:** `Runtime/Timers/TimerManager.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Improved Timers (rewrite)

**API:**

| Method | Description |
|--------|-------------|
| RegisterTimer(Timer) | Add timer to update loop |
| DeregisterTimer(Timer) | Remove timer from update loop |

**Notes:**
- Uses sweep-list pattern (via ListExtensions.RefreshWith) for safe iteration during callbacks
- Uses List.Remove (O(n)) - adequate for typical timer counts (< 50). Optimize to HashSet if profiling shows need.
- Ticks all registered timers via PlayerLoop injection (Update timing)
- Clear() disposes all timers - called automatically on editor play mode exit

---

#### CountdownTimer.cs

**Path:** `Runtime/Timers/CountdownTimer.cs`
**Type:** Class : Timer
**Status:** Implemented
**Origin:** Git Amend Improved Timers (rewrite)

**API:**

| Member | Description |
|--------|-------------|
| CountdownTimer(float duration) | Constructor with countdown duration |
| IsFinished | bool - true when time reaches zero |
| Progress | float - 1 at start, 0 when finished |
| Reset(float newDuration) | Reset with optional new duration |

---

#### FrequencyTimer.cs

**Path:** `Runtime/Timers/FrequencyTimer.cs`
**Type:** Class : Timer
**Status:** Implemented
**Origin:** Git Amend Improved Timers (rewrite)

**API:**

| Member | Description |
|--------|-------------|
| FrequencyTimer(float ticksPerSecond) | Constructor with tick rate |
| OnTick | event Action - fires each tick |

---

#### IntervalTimer.cs

**Path:** `Runtime/Timers/IntervalTimer.cs`
**Type:** Class : Timer
**Status:** Implemented
**Origin:** Git Amend Improved Timers (rewrite)

**API:**

| Member | Description |
|--------|-------------|
| IntervalTimer(float duration, float interval) | Constructor with total duration and tick interval |
| OnInterval | event Action - fires at each interval |
| IsFinished | bool - true when total duration expires |

---

#### StopwatchTimer.cs

**Path:** `Runtime/Timers/StopwatchTimer.cs`
**Type:** Class : Timer
**Status:** Implemented
**Origin:** Git Amend Improved Timers (rewrite)

**API:**

| Member | Description |
|--------|-------------|
| StopwatchTimer() | Constructor (counts up from zero) |
| GetElapsedTime() | float - total elapsed time |
| Progress | float - always 0 (no target duration) |

---

#### PlayerLoopUtils.cs

**Path:** `Runtime/Timers/PlayerLoopUtils.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Improved Timers (rewrite)

**API:**

| Method | Description |
|--------|-------------|
| InsertSystem<T>(PlayerLoopSystem, int) | Insert custom system into Unity PlayerLoop |
| RemoveSystem<T>(ref PlayerLoopSystem) | Remove custom system from PlayerLoop |

**Notes:**
- No UnityEditor references (source issue fixed). PrintPlayerLoop is available at runtime for debugging.
- InsertSystem/RemoveSystem work recursively through the PlayerLoop hierarchy.

---

#### TimerBootstrapper.cs

**Path:** `Runtime/Timers/TimerBootstrapper.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Improved Timers (rewrite)

**Notes:**
- Uses `[RuntimeInitializeOnLoadMethod]` to auto-inject TimerManager into PlayerLoop
- Must clean up on editor play mode exit

---

### TecVooDoo.Utilities.Patterns

#### Singleton.cs

**Path:** `Runtime/Patterns/Singleton.cs`
**Type:** Abstract generic MonoBehaviour
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library (rewrite)

**API:**

| Member | Description |
|--------|-------------|
| Instance | static T - singleton instance |
| HasInstance | static bool - whether instance exists |

**Notes:**
- Instance property auto-creates if none found (FindAnyObjectByType, then AddComponent).
- Sets instance in Awake via InitializeSingleton(). Override Awake but always call base.Awake().
- Does NOT call DontDestroyOnLoad (see PersistentSingleton for that).
- TryGetInstance() returns null without auto-creating.

---

#### PersistentSingleton.cs

**Path:** `Runtime/Patterns/PersistentSingleton.cs`
**Type:** Generic MonoBehaviour (standalone, does NOT inherit Singleton<T>)
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library (rewrite)

**API:**

| Member | Description |
|--------|-------------|
| Instance | static T - singleton instance (auto-creates if needed) |
| HasInstance | static bool - whether instance exists |
| TryGetInstance() | static T - returns null without auto-creating |
| AutoUnparentOnAwake | bool - unparent for DontDestroyOnLoad (default true) |

**Notes:**
- Standalone class (does NOT inherit from Singleton<T>) - each has its own implementation.
- Calls DontDestroyOnLoad. If a duplicate is found, the NEWER instance destroys itself.
- AutoUnparentOnAwake ensures root object for DontDestroyOnLoad compatibility.
- Used for managers that survive scene transitions (GameManager, AudioManager, etc.)

---

#### RegulatorSingleton.cs

**Path:** `Runtime/Patterns/RegulatorSingleton.cs`
**Type:** Generic MonoBehaviour (standalone, does NOT inherit PersistentSingleton<T>)
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library (rewrite)

**API:**

| Member | Description |
|--------|-------------|
| Instance | static T - singleton instance (auto-creates with HideAndDontSave) |
| HasInstance | static bool - whether instance exists |
| InitializationTime | float - Time.time when this instance was initialized |

**Notes:**
- Standalone class (does NOT inherit from PersistentSingleton<T>).
- When a duplicate is found, the OLDER instance is destroyed (based on InitializationTime).
- Calls DontDestroyOnLoad. Auto-created instances use HideFlags.HideAndDontSave.
- Singleton destroys the newcomer. RegulatorSingleton destroys the incumbent.
- Use case: hot-reload during development, scene restart flows.

---

### TecVooDoo.Utilities.Gameplay

#### LookAtCamera.cs

**Path:** `Runtime/Gameplay/LookAtCamera.cs`
**Type:** MonoBehaviour
**Status:** Implemented
**Origin:** Code Monkey Toolkit (rewrite)

**API:**

| Member | Type | Description |
|--------|------|-------------|
| mode | [SerializeField] BillboardMode | Billboard rendering mode |
| SetMode(BillboardMode) | void | Set mode at runtime |

**BillboardMode Enum:**
```csharp
public enum BillboardMode
{
    LookAt,              // transform.LookAt(camera position)
    LookAtInverted,      // face away from camera
    CameraForward,       // match camera forward direction
    CameraForwardInverted // match negative camera forward (default)
}
```

**Usage:**

```csharp
// Attach to world-space health bar, fish name label, NPC dialogue bubble
LookAtCamera billboard = healthBarCanvas.GetOrAdd<LookAtCamera>();
billboard.SetMode(LookAtCamera.BillboardMode.CameraForward);
```

**Notes:**
- Camera.main cached in Awake (no per-frame FindGameObjectWithTag allocation).
- Runs in LateUpdate to resolve after all movement.
- Default mode is CameraForwardInverted (most common for UI billboards).
- Common HOK use: fish labels, NPC indicators, floating UI.

---

### TecVooDoo.Utilities (Helpers)

#### WaitFor.cs

**Path:** `Runtime/WaitFor.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Git Amend Unity Utility Library

**API:**

| Member | Type | Description |
|--------|------|-------------|
| EndOfFrame | WaitForEndOfFrame | Cached instance |
| FixedUpdate | WaitForFixedUpdate | Cached instance |
| Seconds(float) | WaitForSeconds | Cached by duration (dictionary lookup) |

**Usage:**

```csharp
yield return WaitFor.EndOfFrame;
yield return WaitFor.FixedUpdate;
yield return WaitFor.Seconds(0.5f);  // cached, no allocation after first call
```

**Notes:**
- Prevents repeated `new WaitForSeconds()` allocations in coroutines
- Dictionary-cached by float key for WaitForSeconds
- Still relevant even with UniTask because some third-party APIs use coroutines

---

### TecVooDoo.Utilities.Logging

#### CategoryLogger.cs

**Path:** `Runtime/Logging/CategoryLogger.cs`
**Type:** Static class
**Status:** Implemented
**Origin:** Backbone Logger concept (written from scratch)

**API:**

| Member | Description |
|--------|-------------|
| Log(string category, string message, string color, Object context) | Info log with colored category prefix |
| LogWarning(string category, string message, string color, Object context) | Warning with colored category prefix |
| LogError(string category, string message, string color, Object context) | Error with colored category prefix |

**Usage:**

```csharp
CategoryLogger.Log("Fishing", "Cast initiated at power 0.75");
CategoryLogger.Log("Ferry", "Soul boarded raft", "#00FF00");
CategoryLogger.LogWarning("AI", "Pathfinding timeout on soul");
CategoryLogger.LogError("Save", "Failed to write save file");
```

**Notes:**
- Written from scratch, inspired by Backbone Logger's category concept.
- Log and LogWarning are stripped from release builds via [Conditional("UNITY_EDITOR")] and [Conditional("DEVELOPMENT_BUILD")].
- LogError is NOT stripped - errors should always be visible in all builds.
- Default colors: cyan for Log, yellow for LogWarning, red for LogError.
- Categories are strings for flexibility across projects.
- V1 is intentionally simple. Category enable/disable and LogLevel filtering can be added later if needed (YAGNI).

---

## Coding Rules and Guidelines

### Language Rules

1. **No `var` keyword** - Use explicit types always. `List<Timer> timers = new List<Timer>();` not `var timers = new List<Timer>();`
2. **No LINQ in runtime code** - LINQ allocates. Use explicit loops. `System.Linq` using directive should not appear in Runtime/ scripts.
3. **No per-frame allocations** - No `new` in Update/FixedUpdate/LateUpdate/Tick paths. Pre-allocate or cache everything.
4. **ASCII only** - No smart quotes, em-dashes, or Unicode characters in code, comments, or string literals. Plain ASCII everywhere.
5. **Explicit access modifiers** - Always write `private`, `public`, `internal`, `protected`. Never rely on default access.
6. **Prefer async/await (UniTask) over coroutines** - This package does not depend on UniTask, but WaitFor.cs exists specifically to support projects that still use coroutines via third-party APIs.

### Self-Documenting Code

1. **Names over comments** - If code needs a comment to explain what it does, rename variables/methods first. A comment should explain *why*, not *what*.
2. **XML documentation on all public members** - Every public class, method, property, and field gets a `<summary>` tag. This is non-negotiable.
3. **Method names are verbs** - `Stop()`, `ChangeDirection()`, `ComputeHeight()`. Not `Stopper()` or `DirectionChange()`.
4. **Boolean names are questions** - `IsRunning`, `HasInstance`, `IsNullOrEmpty`. Not `Running` or `Empty`.
5. **No abbreviations** - `position` not `pos`, `direction` not `dir`, `transform` not `trans`. Exception: universally understood abbreviations (`id`, `max`, `min`).
6. **Constants explain magic numbers** - `private const float GRAVITY = 9.8f;` not bare `9.8f` in formulas.

### Naming Conventions

| Element | Convention | Example |
|---------|-----------|---------|
| Namespaces | `TecVooDoo.Utilities.<System>` | `TecVooDoo.Utilities.Timers` |
| Classes | PascalCase | `CountdownTimer`, `VectorExtensions` |
| Public methods | PascalCase | `GetOrAdd<T>()`, `DirectionTo()` |
| Public properties | PascalCase | `IsRunning`, `Progress` |
| Public fields | camelCase (SerializeField only) | `[SerializeField] float duration` |
| Private fields | camelCase, no prefix | `float currentTime`, `bool isRunning` |
| Constants | UPPER_SNAKE_CASE | `private const int MAX_LAYERS = 64;` |
| Static readonly | PascalCase | `public static readonly List<T> Instances` |
| Events | On + PastTense | `OnTimerStarted`, `OnTimerStopped` |
| Extension methods | PascalCase matching the operation | `Shuffle()`, `WithAlpha()`, `DirectionTo()` |
| Enums | PascalCase (type and values) | `LookAtMode.CameraForward` |
| Type parameters | T prefix | `Singleton<T>`, `GetOrAdd<T>()` |

### Architecture Rules

1. **Namespace matches folder path** - `Runtime/Extensions/` = `TecVooDoo.Utilities`, `Runtime/Timers/` = `TecVooDoo.Utilities.Timers`
2. **One public class per file** - File name matches class name exactly.
3. **Extension classes are static** - All extension method files contain a single static class.
4. **No MonoBehaviour in extension classes** - Extensions operate on existing types, they do not inherit MonoBehaviour.
5. **Zero external dependencies** - This package must compile in any Unity 6 project without requiring UniTask, SOAP, Odin, or any third-party package.
6. **Runtime/Editor separation** - No `using UnityEditor` in Runtime/ scripts unless wrapped in `#if UNITY_EDITOR` guards.
7. **Events, not public Actions** - Use `event Action` not `public Action`. Prevents external code from invoking events directly.
8. **Prefer composition over inheritance** - Note: the three singleton variants (Singleton, PersistentSingleton, RegulatorSingleton) are standalone classes, NOT an inheritance chain. Each has its own implementation.

### Performance Rules

1. **Cache component lookups** - `GetComponent<T>()` once in Awake/Start, store in field. Never in Update.
2. **Cache Camera.main** - `Camera.main` performs `FindGameObjectWithTag` internally. Cache the transform reference.
3. **Use struct returns where possible** - Extension methods returning new Vector3/Color values are fine (stack allocated).
4. **Prefer `List<T>` over `HashSet<T>` for small collections** - Cache-friendly iteration beats O(1) lookup when n < 50.
5. **String concatenation** - Use string interpolation `$""` or `StringBuilder` for multi-part strings. Never `+` in loops.
6. **Avoid boxing** - Do not pass value types through `object` parameters. Use generic methods instead.

### File Template

Every Runtime script follows this template:

```csharp
// TecVooDoo Utilities
// Copyright (c) 2026 TecVooDoo LLC. All rights reserved.

using System;
using UnityEngine;

namespace TecVooDoo.Utilities
{
    /// <summary>
    /// Brief description of what this class does.
    /// </summary>
    public static class ExampleExtensions
    {
        // Implementation
    }
}
```

### Assembly References

Any project using this package adds a reference to `TecVooDoo.Utilities` in their asmdef:

```json
{
  "references": [
    "TecVooDoo.Utilities"
  ]
}
```

---

## Dependencies Reference

**This package has ZERO external dependencies.**

It works alongside but does not reference:
- UniTask (async/await)
- SOAP (ScriptableObject architecture)
- Odin (inspector/serialization)
- Any other third-party package

---

## File Locations Quick Reference

| Type | Location |
|------|----------|
| Runtime scripts | Runtime/ |
| Editor scripts | Editor/ |
| Runtime tests | Tests/Runtime/ |
| Editor tests | Tests/Editor/ |
| Documentation | Documents/ |
| Package manifest | package.json |

---

**End of Code Reference**
