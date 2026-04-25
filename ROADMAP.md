# U.Roguelike - Roadmap

Development roadmap for U.Roguelike project.

## Overview

| Phase | Description | Est. Duration |
|-------|-------------|---------------|
| Phase 0 | Cleanup & Foundation | 1 week |
| Phase 1 | Core Gameplay | 3 weeks |
| Phase 2 | Combat & Enemies | 4 weeks |
| Phase 3 | Progression | 4 weeks |
| Phase 4 | UX & Polish | 4 weeks |
| Phase 5 | Content | 5+ weeks |

**Total estimated: 20-24 weeks**

---

## Phase 0: Cleanup & Foundation

*Pre-requisites for everything else*

| # | Task | Priority |
|---|------|----------|
| 0.1 | Delete `Assets/Scripts/Antiguo/` folder | 🔴 High |
| 0.2 | Move root scripts to correct folders (`Assets/Scripts/Utils/`, `Assets/Scripts/Systems/`) | 🔴 High |
| 0.3 | Rename temporary prefabs (`prueba76`, `TR`, `T`, etc.) to descriptive names | 🟡 Medium |
| 0.4 | Standardize naming: all PascalCase (classes, files, methods) | 🟡 Medium |
| 0.5 | Improve `.gitignore` if needed | 🟢 Low |

---

## Phase 1: Core Gameplay

*Fundamental systems for a playable game*

| # | Task | Depends |
|---|------|---------|
| 1.1 | **Refactor PlayerMovement** — extract to clean component, no Input logic | 0.2 |
| 1.2 | **Player stats system** — HP, Mana, Stamina, Speed, Damage | 1.1 |
| 1.3 | **Basic inventory system** — slots, basic items | 1.2 |
| 1.4 | **Collectible items system** — health, mana, damage (prefab exists, logic needed) | 1.3 |
| 1.5 | **Room system** — clean generation, make deterministic | 0.1, 0.2 |

---

## Phase 2: Combat & Enemies

*Robust combat system*

| # | Task | Depends |
|---|------|---------|
| 2.1 | **Abstract CombatSystem** — DamageType, defense, resistances | 1.2 |
| 2.2 | **Refactor all AI** — create EnemyBase with states (StateMachine) | 1.1 |
| 2.3 | **Add damage types** — physical, magic, fire, etc. | 2.1 |
| 2.4 | **Status effects system** — poison, burn, stun | 2.3 |
| 2.5 | **Boss improvement** — phases, patterns, basic cinemachine | 2.2 |
| 2.6 | **Initial balancing** — base values for all stats | 2.5 |

---

## Phase 3: Progression

*Roguelike elements that make it replayable*

| # | Task | Depends |
|---|------|---------|
| 3.1 | **Runes/pickups system** — objects that grant temporary abilities | 2.3 |
| 3.2 | **Weapon upgrade** — upgrade system (3 stars, affine, etc.) | 2.1 |
| 3.3 | **Loot table** — drop probability by enemy type | 2.2 |
| 3.4 | **Rune persistence** — save progression between rooms | 3.1 |
| 3.5 | **Meta-progression** — unlock things between runs (optional for prototyping) | 3.4 |

---

## Phase 4: UX & Polish

*What makes the game enjoyable*

| # | Task | Depends |
|---|------|---------|
| 4.1 | **Loading screens** between rooms | 1.5 |
| 4.2 | **Visual feedback** — hit effects, screen shake, particles | 2.1 |
| 4.3 | **Audio feedback** — SFX for attacks, hits, pickups | 4.2 |
| 4.4 | **In-game tutorial** — tooltips, hints | 1.3 |
| 4.5 | **Mini-map** or position indicator | 1.5 |
| 4.6 | **Settings menu** — volume, quality, key rebinding | 4.4 |

---

## Phase 5: Content

*Stuff that makes the game interesting*

| # | Task |
|---|------|
| 5.1 | More room types (trap, treasure, puzzle) |
| 5.2 | More enemy types (5-7 more) |
| 5.3 | More bosses (2-3) |
| 5.4 | Progressive biomes/difficulty (floors) |
| 5.5 | Achievements |

---

## Language

- English: [README.md](./README.md)
- Español: [README_ES.md](./README_ES.md)
- Roadmap: [ROADMAP.md](./ROADMAP.md)
- Roadmap ES: [ROADMAP_ES.md](./ROADMAP_ES.md)