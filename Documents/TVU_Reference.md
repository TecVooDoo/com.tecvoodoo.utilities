# TecVooDoo Utilities - Reference

**Package:** `com.tecvoodoo.utilities` v1.3.0
**Namespace:** `TecVooDoo.Utilities`
**Source:** `E:\Unity\DefaultUnityPackages\com.tecvoodoo.utilities\`
**Last Updated:** April 9, 2026

---

## Quick Reference

| Module | What It Gives You |
|--------|-------------------|
| `Extensions/` | Extension methods on Unity types (Vector3, Transform, GameObject, List, Color, string, etc.) |
| `Timers/` | PlayerLoop-based timers that tick without MonoBehaviour Update overhead |
| `Patterns/` | Three singleton variants for different lifecycle needs, plus a CRTP state machine |
| `Gameplay/` | LookAtCamera billboard (SimpleBoids flocking migrated to TVDGames in Session 1) |
| `Logging/` | Category-based debug logging, stripped from release builds |
| `WaitFor` | Cached coroutine yield objects |
| `Terrain/` | 16-bit heightmap texture -> TerrainData at real-world scale (filename-encoded `WL_` / `Height_`) |

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

### CharacterStateMachine / Transition

CRTP (Curiously Recurring Template Pattern) state machine. Based on adammyhre gist. **Sole copy since 2026-09-26** -- an identical duplicate in TVDGames (`TecVooDoo.Games`) was deleted (TVG 1.4.0) because importing both namespaces made every use `CS0104`-ambiguous.

**Key types:**

| Type | Purpose |
|------|---------|
| `CharacterStateMachine` | Drives current state, evaluates transitions, calls `Tick()` |
| `CharacterState<TState>` | Base for concrete states. Override `OnEnter()`, `OnExit()`, `OnTick(float dt)` |
| `Transition<TState>` | Condition-based transition to a target state |

**Usage:**

```csharp
// Define states
public class IdleState : CharacterState<IdleState>
{
    protected override void OnEnter() { /* ... */ }
    protected override void OnExit() { /* ... */ }
    protected override void OnTick(float dt) { /* ... */ }
}

public class MoveState : CharacterState<MoveState>
{
    protected override void OnEnter() { /* ... */ }
    protected override void OnExit() { /* ... */ }
    protected override void OnTick(float dt) { /* ... */ }
}

// Wire up
CharacterStateMachine sm = new CharacterStateMachine();
IdleState idle = new IdleState();
MoveState move = new MoveState();

idle.SetTransition(new Transition<MoveState>(move, () => isMoving));
move.SetTransition(new Transition<IdleState>(idle, () => !isMoving));

sm.ChangeState(idle);

// Each frame
sm.Tick(Time.deltaTime);
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

## Terrain

Added v1.3.0 (2026-10-10, TVD S52). Source: Sandbox ENTRY-437 (USA Real-World Heightmaps), which measured the traps this module exists to avoid.

```csharp
// One call: scale from the filename, resolution from the texture (4096 -> 4097), heights at 16-bit precision.
bool ok = HeightmapTerrain.TryApply(terrain.terrainData, heightmapTexture, assetPath);

// Or explicitly
HeightmapTerrain.Apply(terrainData, heightmapTexture, worldLength: 8192f, heightRange: 763f);

// Pieces
HeightmapFileName.TryParse("USA_24_WL_8192_Height_763.png", out float wl, out float range); // 8192, 763
int res = HeightmapTerrain.ResolutionFor(4096);                  // 4097 (2^n + 1, clamped 33..4097)
float[,] h = HeightmapSampler.Sample(heightmapTexture, 4097);     // [y, x], 0..1
```

**Import the PNG as:** format **R16**, **sRGB off**, **Non-Power of 2 = None**. Read/Write is not needed (the sampler reads through a GPU blit to an `RFloat` target).

**Traps it avoids (all measured in ENTRY-437):**
- **`Texture2D.LoadImage` drops 16-bit PNGs to 8-bit** (256 levels = ~3 m terraces on a 763 m tile). Never use it for heightmaps.
- **4097 -> 4096 import resample.** The sampler pins corner samples to the source's corner texels, so a 4096 texture fills a 4097 grid edge-to-edge.
- **Scale lives only in the filename.** `Height_N` is the range (max - min), not altitude.
- `heightmapResolution` is set before `size` (changing resolution rescales size).

**`HeightmapTerrain` compiles only with the `com.unity.modules.terrain` built-in module** (`TVU_TERRAIN_MODULE`, from the asmdef's `versionDefines`). BloodMiner and HideNReap disable that module; there the type is absent and TVU still compiles. `HeightmapFileName` and `HeightmapSampler` have no module dependency.

---

## Logging

### CategoryLogger

Category-tagged console logging. Log/LogWarning carry `[Conditional("DEBUG")]` (Unity: `DEBUG` = editor or development build), so they are compiled out of release builds at the call site. LogError always compiles in.

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
