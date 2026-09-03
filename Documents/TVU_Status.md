# TecVooDoo Utilities - Status

**Package:** `com.tecvoodoo.utilities` v1.2.0
**Type:** UPM local package (shared library)
**Source:** `E:\Unity\DefaultUnityPackages\com.tecvoodoo.utilities\`
**Namespace:** `TecVooDoo.Utilities`
**Installed in:** All TecVooDoo Unity projects via `file:` reference in manifest.json

**Reference doc:** `TVU_Reference.md` -- full API

---

## Current Contents

| Module | Files | Status |
|--------|-------|--------|
| Extensions | 10 extension classes (expanded with Tier 1 methods) | Stable |
| Timers | Timer (base), Countdown, Stopwatch, Frequency, Interval, TimerManager, TimerBootstrapper, PlayerLoopUtils | Stable |
| Patterns | Singleton, PersistentSingleton, RegulatorSingleton, CharacterStateMachine, Transition | Stable |
| Gameplay | LookAtCamera | Stable |
| Logging | CategoryLogger | Stable |
| Collections | CircularBuffer | Stable |
| Debug | AllocCounter | Stable |
| UI | DataBindingHelper | Stable |
| Root | WaitFor | Stable |

---

## Tests

**85 tests, 85 passing** -- last run 2026-08-02 in TVD on Unity 6000.5.5f1 (0 failed, 0 skipped, 0.16s). Six fixtures under `Tests/Runtime/`: `CollectionExtensionsTests`, `ColorExtensionsTests`, `NumberExtensionsTests`, `StringExtensionsTests`, `TimerTests`, `VectorExtensionsTests`. Assembly `TecVooDoo.Utilities.Tests` (`defineConstraints: UNITY_INCLUDE_TESTS`, `nunit.framework.dll`).

**How to actually run them -- two non-obvious gates:**

1. **The consuming project must list this package in `testables`.** UPM does not compile a package's test assemblies unless `Packages/manifest.json` carries `"testables": ["com.tecvoodoo.utilities"]`. Without it the tests are invisible and the Test Runner reports *"No tests found"* -- not a missing-test error, just silence. TVD had no `testables` array at all until 2026-08-02, so **these 85 tests had never been run there.**
2. **After editing `testables`, force a package resolve.** An `assets-refresh` / `AssetDatabase.Refresh()` is **not** enough -- the assembly stays absent from `CompilationPipeline.GetAssemblies()`. `UnityEditor.PackageManager.Client.Resolve()` registers it.

**They run in PlayMode, not EditMode.** `TecVooDoo.Utilities.Tests.asmdef` uses `includePlatforms: []` (all platforms) rather than `["Editor"]`, so Unity classifies the suite as PlayMode -- an EditMode run reports "No tests found" even once the assembly compiles. Via MCP: `tests-run` with `testMode: PlayMode` and `testAssembly: TecVooDoo.Utilities.Tests` (filter by assembly, or you also pull in unrelated third-party PlayMode suites). Switching the asmdef to `includePlatforms: ["Editor"]` would make these EditMode-runnable and faster; not done yet, since it affects every consuming project.

---

## Sessions

**Session 0 (pre-2026) -- Initial Build:**
Package created with core extension methods, PlayerLoop-based timer system, singleton variants, LookAtCamera, SimpleBoids, CategoryLogger, WaitFor. Installed across HOK, FearSteez, Sandbox via DefaultUnityPackages file reference.

**Session 1 (2026-03-16) -- Adam Myhre integrations + SimpleBoids migration:**
SimpleBoids moved to com.tecvoodoo.games (game logic, not a utility). Added from adammyhre gists: CharacterStateMachine + Transition (Patterns/), CircularBuffer (Collections/), AllocCounter (Debug/), DataBindingHelper (UI/). All adapted to TecVooDoo standards (namespace, header, no var). Version bumped to 1.1.0.

**Session 2 (2026-04-09) -- Tier 1 extension methods:**
Added all Tier 1 pending candidates from Sandbox AssetLog:
- **NumberExtensions:** InRangeOf (float+int), Quantize (float+int), RoundToInt
- **VectorExtensions:** ToVector2XY, ToVector2XZ
- **TransformExtensions:** ResetPosition, ResetRotation, ResetScale, DestroyChildren, HierarchyPath
- **GameObjectExtensions:** SetLayerRecursively
- **ColorExtensions:** ToHexRGB (RGB without alpha), TryFromHex (safe parse)
All zero-dependency, high-reuse methods. Version bumped to 1.2.0.

**Session 3 (2026-09-03) -- Unity 6000.6.0f1 deprecation pass (driven from TVD S44):**
- **`RegulatorSingleton.cs:69` -- `CS0618` x2 FIXED.** `FindObjectsByType<T>(FindObjectsSortMode.None)` -> `FindObjectsByType<T>(FindObjectsInactive.Exclude)`. **Behaviour unchanged** -- `Exclude` is the sort-mode overload's implicit default, so the previously-implicit choice is now explicit. Verified against the live 6000.6 API by reflection first (**both** sort-mode overloads carry `[Obsolete]`; the no-arg and `FindObjectsInactive` forms are clean), then verified by **rebuild** -- `TecVooDoo.Utilities.dll` recompiled with zero `CS0618`.
- **Version deliberately NOT bumped.** A warning-only fix with no API and no behaviour change; every project file-refs this package, so a bump would churn manifests for nothing. Revisit if the `UAC0009` item below lands, since that one *can* change release-build behaviour.
- **NEW and OPEN -- `UAC0009` x2 in `CategoryLogger.cs(27,22)` + `(42,22)`:** *"DEVELOPMENT_BUILD preprocessor directive has been deprecated"* on Unity 6000.6. Both are `[Conditional("DEVELOPMENT_BUILD")]` on `Log` / `LogWarning` (a third mention is prose at line 14). Unity suggests `DEBUG` / `UNITY_ENABLE_CHECKS` / `UNITY_INCLUDE_INSTRUMENTATION`, or runtime `Debug.isDebugBuild`. **Not auto-fixed -- this is a semantics decision, not a rename:** `[Conditional]` *strips the call site* from release player builds, whereas `Debug.isDebugBuild` only branches at runtime (no stripping, and the arguments still evaluate). Changing it changes what every consuming project ships. Needs Rune's direction.
- **Tests NOT re-run on 6000.6** -- still the 2026-08-02 result (85/85 on 6000.5.5f1). The 6000.6 crossing is verified for compile only, not runtime.

---

## Active TODO

| Task | Priority | Notes |
|------|----------|-------|
| **Decide the `DEVELOPMENT_BUILD` replacement** (`CategoryLogger.cs` 27 + 42) | **Rune's call** | `UAC0009` on Unity 6000.6. `[Conditional]` strips call sites in release builds; `Debug.isDebugBuild` does not. Affects every consuming project's shipped logging. See Session 3 |
| Library otherwise stable at v1.2.0 | -- | `CS0618` cleared 2026-09-03 |
| Monitor for candidates in Sandbox sessions | Ongoing | See Sandbox_DevReference.md candidate criteria |

---

## Candidate Backlog

Utilities identified during Sandbox sessions but not yet promoted:

| Candidate | Source Session | Notes |
|-----------|---------------|-------|
| (none yet) | -- | -- |

---

## Add Process

1. Write and test code in Sandbox (`Assets/_Sandbox/UtilitiesDev/` scratch area)
2. Confirm zero dependencies beyond `UnityEngine` (or `UnityEditor` for editor-only)
3. Copy to correct module in `Runtime/` (or create new module subfolder)
4. Bump `package.json`: patch (x.x.1) for additions, minor (x.1.0) for new modules
5. Update `TVU_Reference.md` module table
6. Update this doc's Contents table and Sessions entry
7. `file:` reference projects pick up changes on next domain reload -- no extra steps

---

## Session Close Checklist

- [ ] Update Sessions with summary of changes
- [ ] Update Contents table if modules changed
- [ ] Bump version in package.json
- [ ] Update TVU_Reference.md if APIs added or changed
- [ ] Move promoted candidates from backlog to Contents

---

**End of Status**
