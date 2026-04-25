# U.Roguelike

A 2D roguelike game built with Unity.

## About

**U.Roguelike** is a procedurally generated dungeon crawler where each run offers a new experience. Fight through rooms, defeat enemies, collect items, and face challenging bosses.

## Features

- Procedural room generation
- Multiple enemy types with unique behaviors
- Boss fights
- Player combat system (melee & ranged)
- Health, Mana, and Stamina systems
- Inventory system
- Multiple game menus (pause, death, game over)

## Requirements

- Unity 2022.3 LTS or higher
- .NET Standard 2.1

## Installation

1. Clone the repository
2. Open the project in Unity Hub
3. Wait for package installation
4. Open `Assets/Scenes/In Game/InGame.unity`
5. Press Play

## Project Structure

```
Assets/
├── Scripts/
│   ├── Manager/          # Game management systems
│   ├── Characters/
│   │   ├── Player/       # Player scripts
│   │   └── Enemies/      # Enemy AI and behaviors
│   ├── Projectiles/      # Projectile systems
│   ├── Canvas/           # UI components
│   ├── Camera/           # Camera effects
│   └── Menu/             # Menu systems
├── Prefabs/
│   ├── Player/
│   ├── Enemies/
│   ├── Projectiles/
│   ├── Dungeon/
│   └── HUD/
└── Scenes/
    └── In Game/
        ├── InGame.unity
        └── BossFight.unity
```

## Technology Stack

- **Engine**: Unity 2022.x
- **Pathfinding**: A* Pathfinding Project
- **Input**: Unity Input System
- **Addressables**: For scene management
- **2D Features**: Unity 2D Feature Set

## Roadmap

See [ROADMAP.md](./ROADMAP.md) for development phases and milestones.

## Language

- English: [README.md](./README.md)
- Español: [README_ES.md](./README_ES.md)
- Roadmap: [ROADMAP.md](./ROADMAP.md)
- Roadmap ES: [ROADMAP_ES.md](./ROADMAP_ES.md)

---

*Built with Unity*