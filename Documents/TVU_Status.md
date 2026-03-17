# TecVooDoo Utilities - Status

**Package:** `com.tecvoodoo.utilities` v1.0.0
**Type:** UPM local package (shared library)
**Source:** `E:\Unity\DefaultUnityPackages\com.tecvoodoo.utilities\`
**Namespace:** `TecVooDoo.Utilities`
**Installed in:** All TecVooDoo Unity projects via `file:` reference in manifest.json

**Reference doc:** `TVU_Reference.md` -- full API

---

## Current Contents

| Module | Files | Status |
|--------|-------|--------|
| Extensions | 10 extension classes | Stable |
| Timers | Timer (base), Countdown, Stopwatch, Frequency, Interval, TimerManager, TimerBootstrapper, PlayerLoopUtils | Stable |
| Patterns | Singleton, PersistentSingleton, RegulatorSingleton, CharacterStateMachine, Transition | Stable |
| Gameplay | LookAtCamera | Stable |
| Logging | CategoryLogger | Stable |
| Collections | CircularBuffer | Stable |
| Debug | AllocCounter | Stable |
| UI | DataBindingHelper | Stable |
| Root | WaitFor | Stable |

---

## Sessions

**Session 0 (pre-2026) -- Initial Build:**
Package created with core extension methods, PlayerLoop-based timer system, singleton variants, LookAtCamera, SimpleBoids, CategoryLogger, WaitFor. Installed across HOK, FearSteez, Sandbox via DefaultUnityPackages file reference.

**Session 1 (2026-03-16) -- Adam Myhre integrations + SimpleBoids migration:**
SimpleBoids moved to com.tecvoodoo.games (game logic, not a utility). Added from adammyhre gists: CharacterStateMachine + Transition (Patterns/), CircularBuffer (Collections/), AllocCounter (Debug/), DataBindingHelper (UI/). All adapted to TecVooDoo standards (namespace, header, no var). Version bumped to 1.1.0.

---

## Active TODO

| Task | Priority | Notes |
|------|----------|-------|
| No active work | -- | Library is stable at v1.0.0 |
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
