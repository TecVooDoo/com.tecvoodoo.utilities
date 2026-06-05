# TecVooDoo Utilities - Reference

**Package:** `com.tecvoodoo.utilities` v1.2.0
**Namespace:** `TecVooDoo.Utilities`
**Source:** `E:\Unity\DefaultUnityPackages\com.tecvoodoo.utilities\`
**Last Updated:** April 9, 2026

---

## Quick Reference

| Module | What It Gives You |
|--------|-------------------|
| `Extensions/` | Extension methods on Unity types (Vector3, Transform, GameObject, List, Color, string, etc.) |
| `Timers/` | PlayerLoop-based timers that tick without MonoBehaviour Update overhead |
| `Patterns/` | Three singleton variants for different lifecycle needs |
| `Gameplay/` | LookAtCamera billboard (SimpleBoids flocking migrated to TVDGames in Session 1) |
| `Logging/` | Category-based debug logging, stripped from release builds |
| `WaitFor` | Cached coroutine yield objects |

---

## Extensions

All extension methods live in `TecVooDoo.Utilities`. Add `using TecVooDoo.Utilities;`.

### VectorExtensions

```csharp
// Non-destructive component override (returns new Vector3)
Vector3 v = transform.position.With(y: 0f);          // flatten to ground
Vector3 flat = transform.position.Flat();              // zero out Y
Vector3 dir = origin.DirectionTo(target);              // normalized direction

// Vector2 variant
Vector2 v2 = input.With(y: 0f);

// Vector3 to Vector2 projections
Vector2 xy = position.ToVector2XY();                   // X,Y components
Vector2 xz = position.ToVector2XZ();                   // X,Z components
```

### TransformExtensions

```csharp
Rigidbody rb = transform.GetOrAdd<Rigidbody>();        // gets or adds component
IEnumerable<Transform> kids = parent.Children();       // iterate direct children

transform.ResetPosition();                              // localPosition = zero
transform.ResetRotation();                              // localRotation = identity
transform.ResetScale();                                 // localScale = one
transform.DestroyChildren();                            // destroy all children
string path = transform.HierarchyPath();                // "Root/Parent/Child"
```

### GameObjectExtensions

```csharp
T safe = component.OrNull<T>();                        // null-safe Unity Object check
T comp = gameObject.GetOrAdd<T>();                     // gets or adds component
bool has = gameObject.HasComponent<Collider>();
gameObject.SetLayerRecursively(LayerMask.NameToLayer("Ignore Raycast"));
```

### CollectionExtensions

```csharp
list.Shuffle();                                        // Fisher-Yates in-place
T item = list.RandomElement();                         // random pick
bool empty = enumerable.IsNullOrEmpty();
enumerable.ForEach(item => DoSomething(item));
```

### ListExtensions

```csharp
bool empty = list.IsNullOrEmpty();
list.Swap(indexA, indexB);
list.RefreshWith(source);                              // clear + add all from source
```

### NumberExtensions

```csharp
float remapped = value.Remap(0f, 1f, -10f, 10f);     // remap to new range
bool close = a.Approximately(b);                       // Mathf.Approximately wrapper
bool odd  = n.IsOdd();
bool even = n.IsEven();

bool inRange = value.InRangeOf(0f, 100f);              // inclusive range check
float snapped = value.Quantize(0.25f);                 // snap to grid
int rounded = value.RoundToInt();                      // Mathf.RoundToInt
```

### ColorExtensions

```csharp
Color faded   = color.WithAlpha(0.5f);
Color sum     = color.Add(other);
Color diff    = color.Subtract(other);
Color blended = color.Blend(target, 0.5f);            // lerp by ratio
Color inv     = color.Invert();
string hex    = color.ToHex();                        // "#RRGGBBAA"
string hexRgb = color.ToHexRGB();                     // "#RRGGBB" (no alpha)
Color parsed  = ColorExtensions.FromHex("#FF0000FF");
bool ok = ColorExtensions.TryFromHex("#FF0000", out Color c); // safe parse
```

### StringExtensions

```csharp
bool blank  = str.IsBlank();                          // null, empty, or whitespace
string safe = str.OrEmpty();                          // null -> ""
string cut  = str.Truncate(20);
string part = str.Slice(2, 5);                        // Python-style slice

string alphanum = str.ToAlphanumeric();               // strip non-alphanumeric
string alphaDot = str.ToAlphanumeric(allowPeriods: true);

// TextMeshPro / Unity rich text
string rich = "text".RichColor("#FF0000");
string big  = "text".RichSize(24);
string bold = "text".RichBold();
string ital = "text".RichItalic();
```

### LayerMaskExtensions

```csharp
bool hit = layerMask.Contains(gameObject.layer);
```

### RigidbodyExtensions

```csharp
rb.ChangeDirection(newDirection);                     // preserve speed, change direction
rb.Stop();                                            // zero velocity and angularVelocity
```

---

## WaitFor

Cached yield objects -- avoid allocating new WaitForSeconds every coroutine.

```csharp
yield return WaitFor.FixedUpdate;
yield return WaitFor.EndOfFrame;
yield return WaitFor.Seconds(0.5f);                   // NOTE: not cached -- use sparingly or cache manually
```

---

## Timers

PlayerLoop-based timers. They tick via `TimerManager` which is registered into Unity's PlayerLoop -- no MonoBehaviour Update required.

### Basic Usage

```csharp
using TecVooDoo.Utilities;

CountdownTimer timer = new CountdownTimer(5f);
timer.OnTimerStart += () => Debug.Log("Started");
timer.OnTimerStop  += () => Debug.Log("Done");
timer.Start();

// In Update (or wherever you want to check):
if (timer.IsFinished) { /* do thing */ }

// Progress 0..1
float pct = timer.Progress;
```

### Timer Types

| Type | Use Case |
|------|----------|
| `CountdownTimer(float duration)` | "Do X after N seconds" |
| `StopwatchTimer()` | Measure elapsed time |
| `FrequencyTimer(int ticksPerSecond)` | Callback at fixed rate -- `OnTick` event |
| `IntervalTimer(float total, float interval)` | Callback every interval -- `OnInterval` event |

### Timer API (all types)

```csharp
timer.Start();
timer.Stop();
timer.Pause();
timer.Resume();
timer.Reset();
timer.Reset(newDuration);

float t   = timer.CurrentTime;
float pct = timer.Progress;      // 0..1
bool  run = timer.IsRunning;
bool  fin = timer.IsFinished;

timer.Dispose();                 // deregisters from TimerManager
```

### FrequencyTimer

```csharp
FrequencyTimer freq = new FrequencyTimer(10);          // 10 ticks/sec
freq.OnTick += () => Debug.Log("tick");
freq.Start();

// Change rate
freq.Reset(30);                                        // now 30 ticks/sec
```

### IntervalTimer

```csharp
IntervalTimer interval = new IntervalTimer(10f, 1f);   // 10s total, fire every 1s
interval.OnInterval += () => Debug.Log("interval");
interval.Start();
```

### TimerManager

Timers self-register when started via `TimerManager.RegisterTimer()`. Manual use:

```csharp
TimerManager.UpdateTimers();    // if not using TimerBootstrapper (manual tick)
TimerManager.Clear();           // deregister all timers (scene unload cleanup)
```

---

## Patterns

### Singleton\<T\>

Basic scene singleton. One instance per scene. Does NOT survive scene loads.

```csharp
public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
        // your init
    }
}

// Usage
GameManager.Instance.DoThing();
if (GameManager.HasInstance) { ... }
GameManager mgr = GameManager.TryGetInstance();        // returns null if not present
```

### PersistentSingleton\<T\>

Survives scene loads via `DontDestroyOnLoad`. Duplicate instances destroy themselves.

```csharp
public class AudioManager : PersistentSingleton<AudioManager>
{
    public bool AutoUnparentOnAwake = true;    // unparents before DDOL (default true)
}
```

### RegulatorSingleton\<T\>

Newest-instance-wins. When a duplicate is created, it destroys the older instance. Useful when you want scene-loaded instances to override stale ones.

```csharp
public class LevelManager : RegulatorSingleton<LevelManager>
{
    // InitializationTime is set automatically on Awake
}
```

---

## Gameplay

### LookAtCamera

Billboard component. Attach to any GameObject to face the camera.

```csharp
// Inspector: set Mode in the enum dropdown
// Runtime:
GetComponent<LookAtCamera>().SetMode(LookAtCamera.BillboardMode.LookAt);
```

**Modes:**

| Mode | Behavior |
|------|----------|
| `LookAt` | Face toward camera |
| `LookAtInverted` | Face away from camera |
| `CameraForward` | Match camera forward direction |
| `CameraForwardInverted` | Match camera backward direction |

---

## Logging

### CategoryLogger

Category-tagged console logging. All Log/LogWarning calls are compiled out in release builds (non-editor, non-development). LogError always compiles in.

```csharp
CategoryLogger.Log("Combat", "Hit landed", "#00FF00");
CategoryLogger.LogWarning("AI", "Target lost", "#FFFF00");
CategoryLogger.LogError("Audio", "Clip missing");       // survives release builds

// With context (highlights object in Console)
CategoryLogger.Log("Physics", "Collision", "#00FFFF", this.gameObject);
```

Output format: `[Category] message` with the color applied via rich text prefix.

---

**End of Reference**
