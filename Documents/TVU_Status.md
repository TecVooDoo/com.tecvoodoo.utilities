# TecVooDoo Utilities - Status

**Package:** `com.tecvoodoo.utilities` v1.4.0
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
| Patterns | Singleton, PersistentSingleton, RegulatorSingleton, CharacterStateMachine, Transition | Stable -- the fleet's ONLY state machine since 2026-09-26 (TVG's duplicate deleted) |
| Gameplay | LookAtCamera | Stable |
| Logging | CategoryLogger | Stable |
| Collections | CircularBuffer | Stable |
| Debug | AllocCounter | Stable |
| UI | DataBindingHelper | Stable |
| Root | WaitFor | Stable |
| Input | LastInputDevice (`TVU_INPUT_SYSTEM`-gated) | New 1.4.0 -- promoted from BloodMiner; 11 EditMode tests via `InputTestFixture` |
| Terrain | HeightmapFileName, HeightmapSampler, HeightmapTerrain (`TVU_TERRAIN_MODULE`-gated) | New 1.3.0 -- **live-verified on a real pack tile in SetDesign 2026-10-10** (32,711 levels = the 15-bit TerrainData ceiling; 1.24 s) |

---

## Tests

**131 tests, 131 passing** -- last run 2026-10-10 (TVD S52) on Unity 6000.6.3f1 / MCP 0.93.3, **EditMode**. **+11 in 1.4.0:** `LastInputDeviceTests` (real Input System events through `InputTestFixture`; the test asmdef now references `Unity.InputSystem` + `Unity.InputSystem.TestFramework`). Before that: **120 tests**, (was PlayMode until the S52 switch), 0 failed / 0 skipped. **+25 in 1.3.0:** `HeightmapFileNameTests` (9), `HeightmapSamplerTests` (6), `HeightmapTerrainTests` (10, gated on `TVU_TERRAIN_MODULE`). Before that: **95 tests, 95 passing** -- run 2026-09-26 (TVD S49) on Unity 6000.6.2f1, PlayMode, 0 failed / 0 skipped. 85 from the 2026-08-02 suite + **`CharacterStateMachineTests` (10)**, moved in from TVG with the state machine. The 2026-08-02 baseline (85/85 on 6000.5.5f1) is superseded. Six fixtures under `Tests/Runtime/`: `CollectionExtensionsTests`, `ColorExtensionsTests`, `NumberExtensionsTests`, `StringExtensionsTests`, `TimerTests`, `VectorExtensionsTests`. Assembly `TecVooDoo.Utilities.Tests` (`defineConstraints: UNITY_INCLUDE_TESTS`, `nunit.framework.dll`).

**How to actually run them -- two non-obvious gates:**

1. **The consuming project must list this package in `testables`.** UPM does not compile a package's test assemblies unless `Packages/manifest.json` carries `"testables": ["com.tecvoodoo.utilities"]`. Without it the tests are invisible and the Test Runner reports *"No tests found"* -- not a missing-test error, just silence. TVD had no `testables` array at all until 2026-08-02, so **these 85 tests had never been run there.**
2. **After editing `testables`, force a package resolve.** An `assets-refresh` / `AssetDatabase.Refresh()` is **not** enough -- the assembly stays absent from `CompilationPipeline.GetAssemblies()`. `UnityEditor.PackageManager.Client.Resolve()` registers it.

**They run in EDITMODE (since 2026-10-10, TVD S52; Rune's call).** `TecVooDoo.Utilities.Tests.asmdef` is `includePlatforms: ["Editor"]` and lives in `Tests/Editor/` (moved from `Tests/Runtime/` with `git mv`, GUIDs kept). Via MCP: `tests-run` with `testMode: EditMode` and `testAssembly: TecVooDoo.Utilities.Tests`. **120/120 in 1.9 s** -- the whole suite was plain C# or edit-safe (no `[UnityTest]`; the heightmap GPU blit and `TerrainData` work in edit mode). Before: PlayMode, which crossed a play-mode domain reload per run. Affects only projects that list TVU in `testables` (TVD), since UPM compiles package tests nowhere else. Keep a new test that needs frames/physics/`Awake` in a separate PlayMode asmdef, as TVG now does.

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

**Session 4 (2026-09-26) -- `UAC0009` closed + empty asmdefs removed (driven from TVD S49):**
- **`CategoryLogger` `UAC0009` x2 FIXED -- Rune chose `[Conditional("DEBUG")]`.** `Log` / `LogWarning` had `[Conditional("UNITY_EDITOR")]` + `[Conditional("DEVELOPMENT_BUILD")]` (deprecated); both replaced by one `[Conditional("DEBUG")]`. **Semantics unchanged:** the Unity 6000.6 scripting-symbol reference states `#if DEBUG` is *"equivalent to `#if UNITY_EDITOR || DEVELOPMENT_BUILD`"*. Measured: the editor compile passes `-define:DEBUG` **with Code Optimization = Release** (the `.rsp` in `Library/Bee`), so editor logging survives either optimization mode. `[Conditional]` binds in the *caller's* compilation, so release players still strip the call sites. **Ruled out:** `UNITY_ASSERTIONS` (broader -- also Debug/Checked managed-code variants), runtime `Debug.isDebugBuild` (no stripping; args evaluate in release). No callers exist in any fleet tree outside this file (whole-`E:\Unity` grep).
- **Verified by a `CleanBuildCache` rebuild with a positive control** (CodeStage `UAC0005` fired): zero `UAC0009`. `Library/ScriptAssemblies/TecVooDoo.Utilities.dll` was NOT restamped -- Bee skips the copy for byte-identical output; the Bee artifact (`Library/Bee/artifacts/*/TecVooDoo.Utilities.dll`) is the rebuild oracle.
- **Removed 2 empty asmdefs** (0 `.cs` each, only self-references): `Editor/TecVooDoo.Utilities.Editor.asmdef` and `Tests/Editor/TecVooDoo.Utilities.Editor.Tests.asmdef`, with their folders. The clean rebuild logged 0 *"no scripts associated"* warnings. Recreate them when real editor code / EditMode tests arrive.
- **Version NOT bumped** -- no API or behaviour change.
- **Tests RE-RUN on 6000.6: 85/85 passing, then 95/95 after the state machine's 10 tests moved in from TVG** (PlayMode, whole `TecVooDoo.Utilities.Tests` assembly, 6000.6.2f1 / MCP 0.93.1) -- closes the Session 3 *compile-only* caveat above.

**Session 6 (2026-10-10) -- Input module, v1.4.0 (driven from TVD S52; Rune: "go ahead"):**
- **NEW `Runtime/Input/LastInputDevice`**, promoted from BloodMiner's `BM.Game.LastInputDevice` (BM S306, 63 lines; flagged as a TVU candidate by Sandbox). Four changes on the way in: (1) BM's `GamepadCursor.IsVirtualDevice` dependency -> `RegisterVirtualGamepadDevice` / `UnregisterVirtualGamepadDevice`; (2) LINQ `.Any()` on the per-event path -> a `foreach` with early return over the struct collection (no allocation); (3) `PadThreshold` configurable (default 0.5); (4) public `Enable` / `Disable` / `IsEnabled`. Statics reset at `SubsystemRegistration` (domain-reload-off safe).
- **Gated:** `versionDefines` `com.unity.inputsystem` -> `TVU_INPUT_SYSTEM` on both asmdefs; `Unity.InputSystem` referenced. **Offline proof:** TVU compiles with the Input System ref + define removed (0 errors); the control (define kept) fails `CS0234`.
- **Tests 120 -> 131 (EditMode).** One test first passed for the WRONG reason: a 0.1 stick value sits inside the Input System's default 0.125 stick deadzone, so it never reached `PadThreshold`. Now 0.3 is rejected at 0.5 and accepted at 0.1, so the threshold is what decides.
- **BloodMiner MIGRATED (bloodminer-a9, uncommitted in BM pending Rune's real-pad check):** BM's copy deleted; `GamepadCursor` registers its virtual mouse after `AddDevice` and unregisters before BOTH removals; BM's `IsVirtualDevice` removed (its only caller). Compile clean. Scripted play check PASS: idle-noise 0.3 stick stays pointer; the registered virtual cursor flips to pad with ONE `Changed`. **Not verified there:** a real stick past 0.5 -- the editor was UNFOCUSED (Rune was playing the standalone exe) and BM's `backgroundBehavior = ResetAndDisableNonBackgroundDevices` meant synthetic pad events never reached `onEvent` (seen=0). An environment gap, not a TVU difference; `InputTestFixture` covers that path. Rune will check with a real pad. **Gotcha for anyone scripting input checks: with that background behaviour, an unfocused editor drops pad events entirely.**

**Session 5 (2026-10-10) -- Terrain module, v1.3.0 (driven from TVD S52):**
- **NEW `Runtime/Terrain/`:** `HeightmapFileName` (parse `WL_` / `Height_` metres from a filename), `HeightmapSampler` (GPU blit of an R16 texture to an `RFloat` grid, bilinear, corner-pinned so 4096 fills 4097), `HeightmapTerrain` (`ResolutionFor`, `Apply`, `TryApply`). Source: Sandbox ENTRY-437's `heightmap-to-terrain(png)` candidate, which flagged the 8-bit `LoadImage` trap, the 4097 -> 4096 import resample, and filename-only scale.
- **`versionDefines` added to BOTH asmdefs:** `com.unity.modules.terrain` -> `TVU_TERRAIN_MODULE`, guarding `HeightmapTerrain` + its tests. **CORRECTED 2026-10-10 (same session):** BloodMiner and HideNReap DO resolve `com.unity.modules.terrain` -- it is not in their manifests but arrives as a DEPENDENCY (`packages-lock.json`: pulled in by `com.ivanmurzak.unity.mcp.terrain`, `com.unity.modules.terrainphysics`, and in HNR `com.unity.terrain-tools`). BM's live `TecVooDoo.Utilities.rsp` carries both `TerrainModule` refs and `-define:TVU_TERRAIN_MODULE`. The earlier claim came from a MANIFEST-only grep. **So the gate is defensive, not needed by any fleet tree today** (any tree with MCP's terrain Extension has the module). It stays: it costs nothing, and the offline compile proves it works if a tree ever drops the module. Lesson: read a package's presence from the LOCKFILE, never the manifest.
- **Bug caught by the suite before commit:** the parser first used `Path.GetFileNameWithoutExtension`, which read `Height_12.25` (no extension) as `Height_12` + extension `.25`. Now `GetFileName`; the extension never matches the patterns.
- **Kept as a TVU utility, not a TMCP tool:** TMCP is for third-party integrations, and `script-execute` can call `HeightmapTerrain.TryApply` in one line.
- **Tests: 120/120** (PlayMode, whole assembly, 6000.6.3f1). **Minor bump 1.2.0 -> 1.3.0** (new module, per Add Process step 4).

**State-machine duplication RESOLVED 2026-09-26 (TVD S49) -- Rune kept TVU's copy.** `CharacterStateMachine` / `CharacterState` / `CharacterState<TState>` / `Transition` / `Transition<TState>` had existed, behaviour-identical, in both `com.tecvoodoo.utilities/Runtime/Patterns/` (`TecVooDoo.Utilities`) and `com.tecvoodoo.games/Runtime/StateMachine/` (`TecVooDoo.Games`); a file importing both namespaces hit `CS0104`. **TVG's copy was deleted (TVG 1.3.0 -> 1.4.0)**; its 10 `CharacterStateMachineTests` moved to TVU (namespace `TecVooDoo.Utilities.Tests`) and pass there unchanged, and the usage docs moved from `TVG_Reference.md` to `TVU_Reference.md` § Patterns. Zero fleet `Assets/` callers existed, so nothing downstream broke.

---

## Active TODO

| Task | Priority | Notes |
|------|----------|-------|
| ~~Run `HeightmapTerrain` on a REAL pack tile~~ DONE 2026-10-10 (SetDesign): TryApply = True, 8192 x 763 x 8192 @ 4097, 32,711 distinct (15-bit TerrainData ceiling), 1.24 s; bare filename also True; no-scale name False with data untouched | -- | 1.3.0 is verified on synthetic R16 ramps only. SetDesign holds ENTRY-437's pack: `TryApply` on `California/USA_24_WL_8192_Height_763.png` should give 8192 x 763 x 8192 at 4097 with ~65k distinct levels (ENTRY-437 measured 65,193) |
| ~~Confirm a no-terrain-module project still compiles~~ DONE 2026-10-10 (TVD S52), OFFLINE: Unity 6000.6.3f1 Roslyn on TVD's own `TecVooDoo.Utilities.rsp` with both `*.TerrainModule.dll` refs and `-define:TVU_TERRAIN_MODULE` removed (= the BloodMiner / HideNReap case) -> **0 errors**. Control: module removed, define KEPT -> `CS1069` x2 on `TerrainData` in `HeightmapTerrain.cs` -- so the gate is what protects them | -- |
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
