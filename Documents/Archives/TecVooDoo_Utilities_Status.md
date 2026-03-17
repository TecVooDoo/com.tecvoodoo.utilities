# TecVooDoo Utilities - Project Status

**Project:** TecVooDoo Utilities
**Developer:** TecVooDoo LLC / Rune (Stephen Brandon)
**Type:** Local Unity Package
**Source:** `E:\Unity\DefaultUnityPackages\com.tecvoodoo.utilities`
**Target Unity:** 6000.3.0f1+ (Unity 6)
**License:** Proprietary (TecVooDoo LLC)
**Document Version:** 6
**Last Updated:** February 7, 2026

**Last Session (Feb 7, 2026 - Session 6):** Phase 8 Sandbox migration COMPLETE. Removed com.gitamend.unityutils from Sandbox manifest (improvedtimers was already removed). Verified no Sandbox code references Git Amend namespaces. Resolved stale Library/Bee cache causing CS2001 errors after removal (deleted Library/Bee, reopened Unity). Sandbox compiles clean with only com.tecvoodoo.utilities. Expanded migration scope: HOK, DLYH, FearSteez, AssetInventory still pending. Git Amend folders kept on disk until all projects migrated.

**Previous Session (Feb 7, 2026 - Session 5):** Phase 7 validated (85/85 NUnit, 53/53 scene tests). Fixed Timer constructor bug (CurrentTime not initialized). Phase 8 started: removed com.gitamend.unityutils from Sandbox manifest (improvedtimers was already removed). No Sandbox code referenced Git Amend namespaces - clean removal. HOK migration deferred.

**Previous Session (Feb 7, 2026 - Session 4):** Phase 7 (Tests) complete. Wrote 6 NUnit test files (77 unit tests) in Tests/Runtime/ and 1 scene test runner MonoBehaviour in Sandbox. Added testables entry to Sandbox manifest.json to enable test discovery. Fixed Timer constructor bug: added CurrentTime = value (was defaulting to 0f). All 85 NUnit tests and 53 scene tests passing.

**Previous Session (Feb 7, 2026 - Session 3):** All code written. Phases 1-6 complete: package structure, 11 extension files, WaitFor.cs, 8 timer system files, 3 singleton patterns, LookAtCamera, CategoryLogger. 25 files total. CodeReference updated to Implemented status.

**Previous Session (Feb 7, 2026 - Session 2):** Deep source review of all cherry-pick candidates. Read all 9 Git Amend Improved Timer files, all 42 Git Amend Unity Utils files, and 33+ Code Monkey Toolkit files. Expanded extension scope from 5 to 11 files based on actual source quality. Added RegulatorSingleton, LookAtCamera, expanded namespaces. No code written yet.

**Previous Session (Feb 7, 2026 - Session 1):** Project documentation created. Architecture planned based on Sandbox asset evaluations. Cherry-pick candidates catalogued. No code written.

---

## Purpose

A shared utility library for all TecVooDoo Unity projects. Consolidates proven, reusable code patterns discovered during asset evaluations into a single, well-structured local package that follows TecVooDoo coding standards.

**This package replaces:**
- `com.gitamend.improvedtimers` (1.0.4)
- `com.gitamend.unityutils` (1.0.21)

**This package does NOT replace:**
- UniTask (standalone - too deeply integrated with PlayerLoop, 70K LOC)
- RNGNeeds (standalone - Burst-compiled probability, black box)
- SOAP (standalone - architectural foundation)
- Odin (standalone - commercial inspector framework)
- Any editor-only tools (UDebug Panel, Fullscreen Editor, Wingman, Scriptable Sheets, etc.)

---

## Quick Context

**What is this?** A curated utility library built from the best pieces of evaluated third-party packages, rewritten to TecVooDoo standards. Think of it as the "greatest hits" of code patterns worth keeping, without the baggage.

**Why not just use the source packages?** Each source package carries dead weight, architectural incompatibilities, or coding style mismatches. Cherry-picking the valuable algorithms and rewriting them to project standards produces a cleaner, smaller, fully-owned dependency.

**Evaluation Source:** All candidates were identified during Sandbox asset evaluations (see `E:\Unity\Sandbox\Documents\Sandbox_AssetLog.md`).

---

## Development Priorities (Inherited from TecVooDoo Standards)

1. **SOLID principles** - single responsibility, open/closed, Liskov substitution, interface segregation, dependency inversion
2. **Memory efficiency** - no per-frame allocations, no per-frame LINQ, object pooling where appropriate
3. **Clean code** - readable, maintainable, consistent formatting
4. **Self-documenting code** - clear naming over comments; if code needs a comment, consider refactoring first
5. **Zero external dependencies** - this package must not depend on any third-party package (UniTask, Odin, SOAP, etc.)

---

## Coding Standards

- No `var` keyword - explicit types always
- No per-frame allocations or LINQ
- Prefer async/await (UniTask) patterns but do NOT depend on UniTask directly
- ASCII-only documentation and identifiers
- Explicit types on all fields, properties, and return values
- All public API members must have XML documentation comments
- Every script must have an assembly definition reference

---

## Architecture

### Package Structure

```
com.tecvoodoo.utilities/
  package.json
  LICENSE.md
  CHANGELOG.md
  Documents/
    TecVooDoo_Utilities_Status.md        (this file)
    TecVooDoo_Utilities_CodeReference.md  (API reference)
  Runtime/
    TecVooDoo.Utilities.asmdef
    Extensions/
      VectorExtensions.cs        (Vector3.With, Flat, DirectionTo)
      TransformExtensions.cs     (GetOrAdd<T>, OrNull, Children)
      GameObjectExtensions.cs    (OrNull, HasComponent)
      CollectionExtensions.cs    (Shuffle, RandomElement, IsNullOrEmpty, ForEach)
      ListExtensions.cs          (IsNullOrEmpty, Swap, RefreshWith)
      NumberExtensions.cs        (Remap, Approximately, IsOdd, IsEven)
      ColorExtensions.cs         (WithAlpha, ToHex, Invert)
      StringExtensions.cs        (IsNullOrEmpty, Truncate, RichText helpers)
      LayerMaskExtensions.cs     (Contains - bitwise layer check)
      RigidbodyExtensions.cs     (Stop, ChangeDirection - Unity 6 ready)
    Timers/
      Timer.cs                   (abstract base)
      TimerManager.cs            (PlayerLoop sweep-list)
      CountdownTimer.cs
      FrequencyTimer.cs
      IntervalTimer.cs
      StopwatchTimer.cs
      PlayerLoopUtils.cs         (PlayerLoop injection)
      TimerBootstrapper.cs       (auto-init)
    Patterns/
      Singleton.cs               (generic MonoBehaviour singleton)
      PersistentSingleton.cs     (DontDestroyOnLoad variant)
      RegulatorSingleton.cs      (destroys older instance on duplicate)
    Gameplay/
      LookAtCamera.cs            (4 billboard modes, cached camera)
      SimpleBoids.cs             (flocking sim, refactored from NVJOB Boids)
    WaitFor.cs                   (cached WaitForSeconds, WaitForEndOfFrame)
    Logging/
      CategoryLogger.cs          (category-based Debug.Log wrapper)
  Editor/
    TecVooDoo.Utilities.Editor.asmdef
    (editor utilities as needed)
  Tests/
    Runtime/
      TecVooDoo.Utilities.Tests.asmdef
    Editor/
      TecVooDoo.Utilities.Editor.Tests.asmdef
```

### Assembly Definitions

| Assembly | Type | Platforms | Dependencies |
|----------|------|-----------|--------------|
| TecVooDoo.Utilities | Runtime | All | None |
| TecVooDoo.Utilities.Editor | Editor | Editor only | TecVooDoo.Utilities |
| TecVooDoo.Utilities.Tests | Tests | Editor only | TecVooDoo.Utilities |
| TecVooDoo.Utilities.Editor.Tests | Tests | Editor only | TecVooDoo.Utilities, TecVooDoo.Utilities.Editor |

### Namespace Structure

| Namespace | Purpose |
|-----------|---------|
| TecVooDoo.Utilities | Root namespace (extensions, common types) |
| TecVooDoo.Utilities.Timers | PlayerLoop-based timer system |
| TecVooDoo.Utilities.Patterns | Singleton, PersistentSingleton, RegulatorSingleton |
| TecVooDoo.Utilities.Gameplay | Reusable gameplay components (LookAtCamera) |
| TecVooDoo.Utilities.Logging | Category-based logging |
| TecVooDoo.Utilities.Editor | Editor-only utilities |

---

## Cherry-Pick Candidates

### From Git Amend - Improved Timers (ENTRY-019)

**Status:** Full absorption planned
**Source:** `E:\Unity\DefaultUnityPackages\com.gitamend.improvedtimers`
**What to take:** Entire timer system (rewrite to standards)

| Component | Source File | Rewrite Notes |
|-----------|------------|---------------|
| Timer (abstract base) | Timer.cs | Remove `var`, add XML docs, make Actions into events |
| TimerManager | TimerManager.cs | Keep sweep-list pattern, fix O(n) removal |
| CountdownTimer | CountdownTimer.cs | Clean rewrite |
| FrequencyTimer | FrequencyTimer.cs | Clean rewrite |
| IntervalTimer | IntervalTimer.cs | Clean rewrite |
| StopwatchTimer | StopwatchTimer.cs | Clean rewrite |
| PlayerLoopUtils | PlayerLoopUtils.cs | Keep PlayerLoop injection, add UNITY_EDITOR guards |
| TimerBootstrapper | TimerBootstrapper.cs | Add proper editor cleanup |

**Issues to fix during rewrite:**
- `using UnityEditor` without `#if UNITY_EDITOR` guard
- Public Action fields should be events (prevent external invocation)
- O(n) List.Remove in TimerManager - consider HashSet or swap-and-pop
- `var` keyword throughout
- ListExtensions.RefreshWith is trivial - inline or drop

### From Git Amend - Unity Utility Library (ENTRY-007)

**Status:** Expanded cherry-pick (revised after full 42-file source review)
**Source:** `E:\Unity\DefaultUnityPackages\com.gitamend.unityutils`
**What to take:** Zero-alloc helpers + commonly-used extensions

| Utility | Take? | Notes |
|---------|-------|-------|
| Vector3.With(x,y,z) | Yes | Replace individual components without new Vector3() |
| Vector3.Flat() | Yes | Zero Y component (XZ plane projection) |
| Vector3.DirectionTo() | Yes | Normalized direction between vectors |
| Transform.GetOrAdd<T>() | Yes | Get component or add if missing |
| .OrNull() | Yes | Unity null-safe extension (returns C# null for destroyed objects) |
| WaitFor cached yields | Yes | Cached WaitForSeconds, WaitForEndOfFrame, WaitForFixedUpdate |
| Singleton<T> | Yes | Rewrite to TecVooDoo standards |
| PersistentSingleton<T> | Yes | DontDestroyOnLoad variant |
| RegulatorSingleton<T> | Yes | Destroys older instances by init time. Useful for hot-reload/scene restart. |
| CollectionExtensions | Yes | Shuffle, RandomElement, IsNullOrEmpty, ForEach |
| NumberExtensions | Yes | Remap, Approximately, IsOdd, IsEven |
| ColorExtensions | Yes | WithAlpha, ToHex, Invert. Used everywhere (UI, debug, materials). |
| StringExtensions (selective) | Yes | IsNullOrEmpty, Truncate, rich text helpers. Skip LINQ-based methods. |
| LayerMaskExtensions | Yes | Bitwise Contains. One-liner but prevents constant mistyping. |
| ListExtensions | Yes | IsNullOrEmpty, Swap, RefreshWith. RefreshWith used by TimerManager. |
| RigidbodyExtensions | Yes | Stop, ChangeDirection. Already handles Unity 6 linearVelocity. |

**What to skip:**
- AsyncOperationExtensions, AwaitableExtensions, TaskExtensions, UnityEventExtensions (UniTask handles async)
- ReflectionExtensions (heavy reflection, LINQ in hot paths)
- SerializedPropertyExtensions, PropertyPathExtensions (Odin handles this better)
- VisualElementExtensions (UI Toolkit specific, not needed yet)
- CameraExtensions (too specific - viewport extents with margin)
- VectorConversionExtensions (System.Numerics interop, niche)
- ResourcesUtils (URP Volume loading, too specific)
- EditorExtensions + hotkeys (personal preference, not shared utility)
- RendererExtensions (ZWrite toggling, too specific)
- SpanExtensions (block copy, niche)
- DateTimeExtensions (construction helpers, trivial)
- EnumeratorExtensions (IEnumerator to IEnumerable, niche)
- UQueryBuilderExtensions (UI Toolkit specific)
- VectorMath.cs (advanced vector ops - overlap with VectorExtensions, evaluate if needed later)
- AllocCounter, FrameRateLimiter (debug-only, not shared utility)

### From Code Monkey Toolkit (ENTRY-014)

**Status:** Revised after full 33+ file source review
**Source:** `E:\Unity\Sandbox\Assets\CodeMonkey\Toolkit\`
**What to take:** One proven gameplay component, algorithms deferred

| Component | Take? | Notes |
|-----------|-------|-------|
| LookAtCamera (4 billboard modes) | Yes | Rewrite with cached camera ref (source uses Camera.main which allocates). CameraForward/Inverted/LookAt/LookAtInverted modes. HOK needs: fish labels, NPC indicators, floating UI. |
| GridSystem<T> + GridPosition | Deferred | Well-built generic grid with world/grid conversion and events. Evaluate when HOK inventory system starts. |
| GridSystemXY<T> | Deferred | 2D variant for UI grids. Same timing as GridSystem. |
| HealthSystem | No | Clean standalone class but SOAP events handle this pattern. Not needed as a utility. |
| FunctionTimer/FunctionUpdater | No | TecVooDoo timer system is better architected (PlayerLoop vs hidden MonoBehaviour). |
| WebRequests | No | Coroutine-based HTTP. UniTask async is preferred. |
| ChatBubble/ChatBubble3D | No | Text Animator + Dialogue System already in HOK stack. |
| FirstPersonController/TopDownController | No | Not HOK's game type. |
| DrawMesh/DrawPixels | No | Not relevant to current projects. |
| KeyDoorSystem | No | Too game-specific. |
| GameAssets | No | Singleton asset container conflicts with SOAP resource management. |
| FPSCounter | No | Trivial (5 lines). Write inline if needed. |
| InputWindowUI/ErrorDetectorUI | No | UI-specific, not shared utility. |
| TextPopupUI | No | UI-specific. |
| PointerHooks | No | UI event delegation. Evaluate if needed for specific UI work. |
| MousePositionRaycast/Plane | No | Too specific. Use Physics.Raycast directly. |
| TakeScreenshot | No | Debug utility, not shared. |
| MeshUtils | No | Drawing-specific mesh generation. |

### From Backbone Logger (ENTRY-012)

**Status:** Pattern reference only
**Source:** Removed from Sandbox (no asmdef)
**What to take:** The category logging concept

| Pattern | Notes |
|---------|-------|
| Category-based log filtering | Debug.Log with category tags, enable/disable per category at runtime |
| Log level hierarchy | Verbose/Debug/Info/Warning/Error with configurable minimum level |

**Implementation:** Write from scratch using the concept. Backbone's actual code had no asmdef and mixed runtime/editor code.

### From Scripts Vault (ENTRY-016)

**Status:** Algorithm-only cherry-pick
**Source:** Removed from Sandbox (rejected - security flaws)
**What to take:** One algorithm

| Algorithm | Take? | Notes |
|-----------|-------|-------|
| CameraShake (damped sine wave) | Maybe | Compare against Code Monkey's version. Take the better one. |
| Everything else | No | Security flaws, thread safety issues, math bugs |

---

## Active TODO

### Phase 1: Project Setup - COMPLETE
- [x] Create package.json with correct metadata
- [x] Create Runtime and Editor assembly definitions
- [x] Create Test assembly definitions
- [x] Set up folder structure (Extensions, Timers, Patterns, Gameplay, Logging)
- [x] Create LICENSE.md

### Phase 2: Extensions (11 files + WaitFor) - COMPLETE
- [x] VectorExtensions.cs (With, Flat, DirectionTo)
- [x] TransformExtensions.cs (GetOrAdd, Children)
- [x] GameObjectExtensions.cs (OrNull, GetOrAdd, HasComponent)
- [x] CollectionExtensions.cs (Shuffle, RandomElement, IsNullOrEmpty, ForEach)
- [x] ListExtensions.cs (IsNullOrEmpty, Swap, RefreshWith)
- [x] NumberExtensions.cs (Remap, Approximately, IsOdd, IsEven)
- [x] ColorExtensions.cs (WithAlpha, Add, Subtract, Blend, Invert, ToHex, FromHex)
- [x] StringExtensions.cs (IsBlank, OrEmpty, Truncate, Slice, ToAlphanumeric, Rich text helpers)
- [x] LayerMaskExtensions.cs (Contains)
- [x] RigidbodyExtensions.cs (Stop, ChangeDirection - Unity 6 linearVelocity)
- [x] WaitFor.cs (cached FixedUpdate, EndOfFrame, Seconds dictionary)

### Phase 3: Timers (8 files) - COMPLETE
- [x] Timer.cs (abstract base with IDisposable)
- [x] TimerManager.cs (sweep-list pattern)
- [x] CountdownTimer.cs
- [x] FrequencyTimer.cs
- [x] IntervalTimer.cs
- [x] StopwatchTimer.cs
- [x] PlayerLoopUtils.cs (no UnityEditor refs in runtime)
- [x] TimerBootstrapper.cs (proper #if UNITY_EDITOR guards)

### Phase 4: Patterns (3 files) - COMPLETE
- [x] Singleton<T> (MonoBehaviour, no DontDestroyOnLoad)
- [x] PersistentSingleton<T> (DontDestroyOnLoad, destroys newer duplicates)
- [x] RegulatorSingleton<T> (DontDestroyOnLoad, destroys older instances)

### Phase 5: Gameplay (2 files) - COMPLETE
- [x] LookAtCamera.cs (4 billboard modes, cached camera ref, LateUpdate)
- [x] SimpleBoids.cs (flocking simulation, refactored from NVJOB Boids ENTRY-083)

### Phase 6: Logging (1 file) - COMPLETE
- [x] CategoryLogger.cs (color-coded category prefix, [Conditional] stripping)

### Phase 7: Tests - COMPLETE
- [x] NUnit: VectorExtensionsTests.cs (12 tests)
- [x] NUnit: NumberExtensionsTests.cs (10 tests)
- [x] NUnit: CollectionExtensionsTests.cs + ListExtensionsTests (12 tests)
- [x] NUnit: ColorExtensionsTests.cs (10 tests)
- [x] NUnit: StringExtensionsTests.cs (15 tests)
- [x] NUnit: TimerTests.cs - Countdown/Stopwatch/Frequency/Interval (18 tests)
- [x] Scene: TecVooDooTestRunner.cs (MonoBehaviour Play mode tests for timers, extensions, singletons, LookAtCamera, CategoryLogger)

### Phase 8: Integration - IN PROGRESS
**Sandbox - COMPLETE**
- [x] Remove com.gitamend.improvedtimers from Sandbox manifest (was already removed)
- [x] Remove com.gitamend.unityutils from Sandbox manifest
- [x] Verify no Sandbox code references Git Amend namespaces (confirmed clean)
- [x] Verify Sandbox compiles after removal (clean after Library/Bee cache deletion)

**HOK - PENDING**
- [ ] Add com.tecvoodoo.utilities to HOK manifest
- [ ] Remove com.gitamend.improvedtimers from HOK manifest
- [ ] Remove com.gitamend.unityutils from HOK manifest
- [ ] Update HOK code (using statements, namespace changes)
- [ ] Verify HOK compiles with new package
- [ ] Delete Library/Bee if stale cache errors occur

**DLYH - PENDING**
- [ ] Add com.tecvoodoo.utilities to DLYH manifest
- [ ] Remove Git Amend packages from DLYH manifest
- [ ] Update DLYH code references if any
- [ ] Verify DLYH compiles

**FearSteez - PENDING**
- [ ] Add com.tecvoodoo.utilities to FearSteez manifest
- [ ] Remove Git Amend packages from FearSteez manifest
- [ ] Update FearSteez code references if any
- [ ] Verify FearSteez compiles

**AssetInventory - PENDING**
- [ ] Add com.tecvoodoo.utilities to AssetInventory manifest
- [ ] Remove Git Amend packages from AssetInventory manifest
- [ ] Update AssetInventory code references if any
- [ ] Verify AssetInventory compiles

**Cleanup (after ALL projects migrated)**
- [ ] Delete com.gitamend.improvedtimers folder from DefaultUnityPackages
- [ ] Delete com.gitamend.unityutils folder from DefaultUnityPackages

### Phase 9: Extension Gap-Fill + Utility Patterns - PENDING
Cherry-pick candidates from IvanMurzak ecosystem deep-dives (Session 26-27). All are small implementations, zero dependencies. Rewrite from scratch to TecVooDoo standards.

**From ENTRY-060 (Unity-Extensions comparison):**
- [ ] TransformExtensions.cs: Add `Reset()`, `ResetPosition()`, `ResetRotation()`, `ResetScale()` (reset local transform to identity)
- [ ] TransformExtensions.cs: Add `DestroyChildren()`, `DestroyChildrenImmediate()` (destroy all child objects)
- [ ] TransformExtensions.cs: Add `HierarchyPath()` (returns "Scene/Parent/Child" string for debug logging)
- [ ] VectorExtensions.cs: Add `ToVector2XY()`, `ToVector2XZ()` (extract 2D projection from Vector3)
- [ ] VectorExtensions.cs: Add `RoundToInt()`, `CeilToInt()`, `ToVector3Int()` (Vector3 to Vector3Int conversions)
- [ ] GameObjectExtensions.cs: Add `SetLayerRecursively(int layer)` (recursive layer assignment for hierarchies)
- [ ] ColorExtensions.cs: Add `ToHexRGB()` (RGB-only hex without alpha) and `TryFromHex(string, out Color)` (safe parsing variant)

**From IvanMurzak full GitHub sweep (Session 27):**
- [ ] UIExtensions.cs (new file): Add `NonDrawingGraphic` class -- invisible UI raycast target with zero draw calls (~20 lines, from Unity-NonDrawingGraphic). Override `UpdateGeometry()` to skip mesh generation. Universal UI pattern for invisible touch zones.
- [ ] TimerExtensions or FileUtility: Add `SaveDelay()` debounce pattern -- coalesce rapid save calls into single write using existing timer system (~30 lines, pattern from Unity-Saver). Prevents redundant disk I/O on frequent state changes.
- [ ] FileUtility.cs (new file): Add async file I/O wrapper -- background thread read/write with UniTask (~50 lines, pattern from Unity-Saver). Thread-safe file operations complementing existing async patterns.

**From Session 35 retroactive scan (all 135 Sandbox assets):**

Tier 1 -- Extension method additions (~15 lines):
- [ ] VectorExtensions.cs: Add `InRangeOf(Vector3, float)` -- sqrMagnitude distance check, no sqrt (ENTRY-007)
- [ ] VectorExtensions.cs: Add `Quantize(float)` -- grid snapping utility (ENTRY-007)

Tier 2 -- Small standalone utilities (~55 lines, write from scratch):
- [ ] Gameplay/SlowMotion.cs (new file): `Time.timeScale` + `Time.fixedDeltaTime` correction in one call. Prevents common bug of forgetting fixedDeltaTime. (~15 lines, concept from ENTRY-016)
- [ ] Gameplay/CameraShake.cs (new file): Perlin noise-based screen shake. (~40 lines, concept from ENTRY-016, rewrite from scratch)
- [ ] Logging/CategoryLogger.cs: Audit -- verify `[Conditional("ENABLE_LOGGING")]` is applied for build stripping (pattern from ENTRY-012)

Tier 3 -- Larger utilities, write from scratch (~440 lines):
- [ ] Grids/GridSystem.cs + GridPosition.cs (new files): Generic `GridSystem<T>`, `GridHexXZ<T>`, `GridPosition` struct with IEquatable. Non-trivial hex math. (~300 lines, algorithm concept from ENTRY-014)
- [ ] Patterns/Observable.cs (new file): `Observable<T>` reactive value wrapper with `OnValueChanged` callbacks. (~60 lines, concept from ENTRY-129 GDS)
- [ ] Patterns/EventBus.cs (new file): Lightweight type-safe `EventBus<TEvent>` pub/sub with Subscribe/Unsubscribe/Publish. (~80 lines, concept from ENTRY-129 GDS)

**Finalization:**
- [ ] Update tests for all new methods
- [ ] Update CodeReference with new API entries

---

## Packages This Replaces

### com.gitamend.improvedtimers (1.0.4)

| Aspect | Current | TecVooDoo Version |
|--------|---------|-------------------|
| Timer base class | Abstract with IDisposable | Same + proper events |
| TimerManager | Sweep-list, O(n) remove | Sweep-list, O(1) remove |
| Timer types | Countdown, Frequency, Interval, Stopwatch | Same |
| PlayerLoop injection | Works but no editor guards | With UNITY_EDITOR guards |
| Code style | Uses `var`, public Action fields | Explicit types, events |
| Assembly | Single runtime asmdef | Runtime + Editor + Tests |

### com.gitamend.unityutils (1.0.21)

| Aspect | Current | TecVooDoo Version |
|--------|---------|-------------------|
| Extensions | 28 files, mixed quality | Cherry-picked best 16 utilities (11 extension files + WaitFor + 3 singletons + LookAtCamera) |
| Dependencies | None declared but references old Input | Zero dependencies, no Input refs |
| allowUnsafeCode | true (but no unsafe code) | false |
| Dead weight | AsyncUtils, old Input, heavy reflection | None - only proven useful code |
| Code style | Uses `var`, some LINQ in hot paths | Explicit types, zero-alloc |

---

## What Works (Completed)

### Package Infrastructure (Phase 1)
- package.json, LICENSE.md, 4 assembly definitions (Runtime, Editor, Tests/Runtime, Tests/Editor)

### Extension Methods (Phase 2) - 11 files
- VectorExtensions (With, Flat, DirectionTo for Vector2 and Vector3)
- TransformExtensions (GetOrAdd<T>, Children)
- GameObjectExtensions (OrNull<T>, GetOrAdd<T>, HasComponent<T>)
- CollectionExtensions (Shuffle, RandomElement, IsNullOrEmpty, ForEach)
- ListExtensions (IsNullOrEmpty, Swap, RefreshWith)
- NumberExtensions (Remap, Approximately, IsOdd, IsEven)
- ColorExtensions (WithAlpha, Add, Subtract, Blend, Invert, ToHex, FromHex)
- StringExtensions (IsBlank, OrEmpty, Truncate, Slice, ToAlphanumeric, RichColor/Size/Bold/Italic)
- LayerMaskExtensions (Contains)
- RigidbodyExtensions (ChangeDirection, Stop - Unity 6 linearVelocity conditional compilation)
- WaitFor (cached FixedUpdate, EndOfFrame, Seconds dictionary with FloatComparer)

### Timer System (Phase 3) - 8 files
- Timer abstract base with IDisposable, Start/Stop/Pause/Resume/Reset
- TimerManager with sweep-list pattern for safe callback iteration
- CountdownTimer (counts down to zero, then stops)
- StopwatchTimer (counts up from zero indefinitely)
- FrequencyTimer (fires OnTick N times per second)
- IntervalTimer (countdown with periodic OnInterval events)
- PlayerLoopUtils (insert/remove systems in Unity PlayerLoop)
- TimerBootstrapper (auto-injects into Update phase, cleans up on editor exit)

### Singleton Patterns (Phase 4) - 3 files
- Singleton<T> (basic, no DontDestroyOnLoad)
- PersistentSingleton<T> (DontDestroyOnLoad, destroys newer duplicates)
- RegulatorSingleton<T> (DontDestroyOnLoad, destroys older instances)

### Gameplay Components (Phase 5) - 2 files
- LookAtCamera (4 billboard modes, cached Camera.main, LateUpdate)
- SimpleBoids (configurable flocking simulation -- birds, fish, butterflies. Refactored from NVJOB Boids ENTRY-083)

### Logging (Phase 6) - 1 file
- CategoryLogger (color-coded category prefix, Conditional attribute stripping in release builds)

### Tests (Phase 7) - 7 files
- **NUnit Tests** (6 files in Tests/Runtime/): 77 total unit tests covering extensions, timers
  - VectorExtensionsTests (12), NumberExtensionsTests (10), CollectionExtensionsTests + ListExtensionsTests (12)
  - ColorExtensionsTests (10), StringExtensionsTests (15), TimerTests (18 across 4 timer types)
- **Scene Test Runner** (1 file in Sandbox): TecVooDooTestRunner.cs - MonoBehaviour that exercises timers (via coroutine), extensions, singletons, LookAtCamera, and CategoryLogger in Play mode with colored console output

---

## Known Issues

| # | Issue | Severity | Notes |
|---|-------|----------|-------|
| 1 | ~~Not yet tested in Unity Editor~~ RESOLVED | ~~HIGH~~ | Package imported into Sandbox, all 25 scripts compiled with zero errors. |
| 2 | TimerManager uses O(n) List.Remove | LOW | Adequate for typical timer counts (< 50). Profile before optimizing. |
| 3 | Timer uses public Action fields, not events | INFO | Intentional design decision for simplicity. Callers can invoke directly but this is acceptable for utility code. |

---

## Decision Log

| Date | Decision | Rationale |
|------|----------|-----------|
| Feb 7, 2026 | Local package format | Shared across all Unity projects via DefaultUnityPackages folder. Easy to version, easy to update. |
| Feb 7, 2026 | Zero external dependencies | Package must work in any Unity project without requiring UniTask, Odin, SOAP, or any other package. Projects can use UniTask alongside this package but it cannot be a dependency. |
| Feb 7, 2026 | Full Git Amend timer absorption | Timer system is well-designed but has fixable code quality issues. Worth owning rather than depending on third-party. |
| Feb 7, 2026 | Selective Git Amend utils absorption | Only the zero-alloc helpers are worth keeping. Old Input System code, heavy reflection, and LINQ-in-hot-paths are dead weight. |
| Feb 7, 2026 | UniTask stays standalone | 70K LOC, deep PlayerLoop integration, actively maintained by Cysharp. Absorption is technically possible but practically foolish. (Sandbox ENTRY-025) |
| Feb 7, 2026 | RNGNeeds stays standalone | Burst-compiled probability distributions. Performance-critical code that's already optimized beyond manual rewrite. (Sandbox ENTRY-017) |
| Feb 7, 2026 | Category logger from scratch | Backbone Logger concept is good but implementation had no asmdef and mixed concerns. Cleaner to write from scratch. |
| Feb 7, 2026 | Code Monkey algorithms deferred | Grid/hex systems may be valuable but are not needed until a project requires them. Evaluate on-demand, not preemptively. |
| Feb 7, 2026 | Expanded extension scope to 11 files | Full source review of 42 Git Amend Utils files revealed 6 more high-value extensions: ColorExtensions, StringExtensions, LayerMaskExtensions, ListExtensions, RigidbodyExtensions (Unity 6 ready), plus expanded CollectionExtensions and NumberExtensions. |
| Feb 7, 2026 | Added RegulatorSingleton | Third singleton variant that destroys older instances. Solves hot-reload and scene restart issues where PersistentSingleton keeps the stale incumbent. |
| Feb 7, 2026 | LookAtCamera promoted from Code Monkey | Originally marked "No - trivial". Source review showed 4 useful modes (LookAt, LookAtInverted, CameraForward, CameraForwardInverted). Rewrite with cached camera ref. Common HOK need (fish labels, NPC UI). |
| Feb 7, 2026 | Skip async/Task extensions | UniTask handles all async patterns. Taking Git Amend's AsyncOperation/Awaitable/Task/UnityEvent extensions would be dead code. |
| Feb 7, 2026 | Skip Code Monkey HealthSystem | Clean standalone class but SOAP event channels handle damage/heal/death patterns at the architectural level. No need for a utility-level implementation. |
| Feb 22, 2026 | SimpleBoids added to Gameplay | Refactored NVJOB Boids (ENTRY-083) into TecVooDoo.Utilities namespace. Single 300-line script, renamed fields, added tooltips, proper SerializeField. Original asset was 90% demo content. |
| Feb 22, 2026 | Retroactive cherry-pick scan of all 135 assets | Identified Tier 1 (extension gaps ~15 lines), Tier 2 (SlowMotion + CameraShake ~55 lines), Tier 3 (Grids + Observable + EventBus ~440 lines). Ruled out BezierUtility (editor-coupled), GDS source (empty stubs), Dynamic Bone Verlet (too tangled). Confirmed Fullscreen Editor and RNGNeeds stay standalone. |

---

## Lessons Learned

| # | Lesson | Source |
|---|--------|--------|
| 1 | Do not absorb framework-scale packages (UniTask, RNGNeeds). The maintenance burden of owning deeply-integrated code exceeds the cost of an extra dependency. | Sandbox ENTRY-025, ENTRY-017 |
| 2 | Utility grab-bag packages should be treated as algorithm donors, not direct dependencies. Cherry-pick the non-trivial algorithms and rewrite to project standards. | Sandbox ENTRY-007, ENTRY-014, ENTRY-016, ENTRY-019 |
| 3 | Do not penalize third-party source code for coding style preferences (var, naming). Style standards apply only to TecVooDoo-authored code. During absorption, rewrite to standards as part of the process. | Sandbox Lesson #10 |
| 4 | After removing a local package from manifest.json, Unity's Library/Bee folder retains stale .csproj references causing CS2001 errors. Must delete Library/Bee and reopen Unity to clear. | Phase 8 Sandbox migration |

---

## Red Flags / Watch Items

| Flag | Severity | Notes |
|------|----------|-------|
| Scope creep | HIGH | Only absorb what is proven useful. Do not speculatively add "might need later" code. |
| Over-engineering patterns | MEDIUM | Singletons must stay simple. Do not build a DI framework or service locator - that is SOAP's job. |
| Breaking existing projects | HIGH | When replacing Git Amend packages, all existing references must compile without changes (or migration must be documented). |

---

## AI Rules

### Critical Protocols
1. **Verify names exist** - search before referencing files/methods/classes
2. **Read CodeReference first** - check existing APIs before writing new code
3. **Step-by-step verification** - one step at a time, wait for confirmation
4. **Read before editing** - always read files before modifying
5. **ASCII only** - no smart quotes, em-dashes, or special characters
6. **Zero dependencies** - never add a dependency on UniTask, Odin, SOAP, or any external package
7. **Be direct** - give honest assessments, don't sugar-coat
8. **Acknowledge gaps** - say explicitly when something is missing or unclear
9. **Status update = CodeReference update** - when updating Status after code changes, ALWAYS also update CodeReference

---

## Reference Documents

| Document | Path | Purpose |
|----------|------|---------|
| **Code Reference** | `TecVooDoo_Utilities_CodeReference.md` | API reference for all utilities |
| **Sandbox Asset Log** | `E:\Unity\Sandbox\Documents\Sandbox_AssetLog.md` | Source evaluations for all cherry-pick candidates |
| **HOK Status** | `E:\Unity\Hooked On Kharon\Documents\HOK_Status.md` | Primary consumer project |

---

## Cross-Project Reference

**All TecVooDoo projects:** `E:\TecVooDoo\Projects\Documents\TecVooDoo_Projects.csv`
**Sandbox (evaluations):** `E:\Unity\Sandbox`
**HOK (primary consumer):** `E:\Unity\Hooked On Kharon`
**Default Packages:** `E:\Unity\DefaultUnityPackages\`

---

## Session Close Checklist

After each work session, update this document:

- [ ] Move completed TODOs to "What Works" section
- [ ] Add any new issues to "Known Issues"
- [ ] Update "Last Session" with date and summary
- [ ] Add new lessons to "Lessons Learned" if applicable
- [ ] Increment version number in header
- [ ] **Update CodeReference** - MANDATORY if any scripts were added/changed

---

## Version History

| Version | Date | Summary |
|---------|------|---------|
| 6 | Feb 7, 2026 | Phase 8 Sandbox migration COMPLETE. Git Amend packages removed from Sandbox manifest, Sandbox compiles clean. Stale Library/Bee cache lesson learned. Migration scope expanded: HOK, DLYH, FearSteez, AssetInventory still pending. Git Amend folders kept on disk until all projects migrated. |
| 5 | Feb 7, 2026 | Phase 7 validated (85/85 NUnit, 53/53 scene). Timer constructor bug fixed. Phase 8 started: Git Amend packages removed from Sandbox manifest. HOK migration pending. |
| 4 | Feb 7, 2026 | Phase 7 complete. 77 NUnit tests across 6 test files + 1 scene test runner MonoBehaviour. All phases 1-7 complete. Ready for Phase 8 (integration - remove Git Amend packages, update manifests). |
| 3 | Feb 7, 2026 | All Phases 1-6 complete. 25 files written: package structure, 11 extensions, WaitFor, 8 timer system, 3 singletons, LookAtCamera, CategoryLogger. CodeReference updated to Implemented. Ready for Phase 7 (tests) and Phase 8 (integration). |
| 2 | Feb 7, 2026 | Deep source review of all cherry-pick candidates (42 Git Amend Utils, 9 Git Amend Timers, 33+ Code Monkey). Expanded extensions from 5 to 11 files. Added RegulatorSingleton, LookAtCamera, Gameplay namespace. Revised Code Monkey cherry-picks. Updated phases to 8. Still no code written. |
| 1 | Feb 7, 2026 | Initial documentation. Architecture planned. Cherry-pick candidates catalogued from Sandbox evaluations. No code written. |

---

**End of Project Status**
