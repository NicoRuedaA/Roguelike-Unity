# U.Roguelike

Un juego roguelike 2D construido con Unity.

## Acerca de

**U.Roguelike** es un dungeon crawler generado procedimentalmente donde cada partida ofrece una nueva experiencia. Lucha a través de habitaciones, derrota enemigos, colecciona objetos y enfréntate a jefes desafiantes.

## Características

- Generación procedural de habitaciones
- Múltiples tipos de enemigos con comportamientos únicos
- Combates contra jefes
- Sistema de combate del jugador (melee y a distancia)
- Sistemas de Vida, Mana y Estamina
- Sistema de inventario
- Múltiples menús de juego (pausa, muerte, game over)

## Requisitos

- Unity 2022.3 LTS o superior
- .NET Standard 2.1

## Instalación

1. Clona el repositorio
2. Abre el proyecto en Unity Hub
3. Espera a que se instalen los paquetes
4. Abre `Assets/Scenes/In Game/InGame.unity`
5. Presiona Play

## Estructura del Proyecto

```
Assets/
├── Scripts/
│   ├── Manager/          # Sistemas de gestión del juego
│   ├── Characters/
│   │   ├── Player/       # Scripts del jugador
│   │   └── Enemies/      # IA y comportamientos de enemigos
│   ├── Projectiles/      # Sistemas de proyectiles
│   ├── Canvas/           # Componentes de UI
│   ├── Camera/           # Efectos de cámara
│   └── Menu/             # Sistemas de menús
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

## Tecnologías

- **Motor**: Unity 2022.x
- **Pathfinding**: A* Pathfinding Project
- **Input**: Unity Input System
- **Addressables**: Para gestión de escenas
- **2D Features**: Unity 2D Feature Set

## Roadmap

Ver [ROADMAP_ES.md](./ROADMAP_ES.md) para las fases de desarrollo y objetivos.

## Idioma

- English: [README.md](./README.md)
- Español: [README_ES.md](./README_ES.md)
- Roadmap: [ROADMAP.md](./ROADMAP.md)
- Roadmap ES: [ROADMAP_ES.md](./ROADMAP_ES.md)

---

*Hecho con Unity*