# U.Roguelike - Roadmap

Hoja de ruta del proyecto U.Roguelike.

## Visión General

| Fase | Descripción | Duración Est. |
|------|-------------|---------------|
| Fase 0 | Limpieza y Base | 1 semana |
| Fase 1 | Core Gameplay | 3 semanas |
| Fase 2 | Combate y Enemigos | 4 semanas |
| Fase 3 | Progresión | 4 semanas |
| Fase 4 | UX y Polish | 4 semanas |
| Fase 5 | Contenido | 5+ semanas |

**Total estimado: 20-24 semanas**

---

## Fase 0: Limpieza y Base

*Pre-requisitos para todo lo demás*

| # | Tarea | Prioridad |
|---|-------|-----------|
| 0.1 | Eliminar carpeta `Assets/Scripts/Antiguo/` | 🔴 Alta |
| 0.2 | Mover scripts root a carpetas correctas (`Assets/Scripts/Utils/`, `Assets/Scripts/Systems/`) | 🔴 Alta |
| 0.3 | Renombrar prefabs temporales (`prueba76`, `TR`, `T`, etc.) a nombres descriptivos | 🟡 Media |
| 0.4 | Estandarizar naming: todo PascalCase (clases, archivos, métodos) | 🟡 Media |
| 0.5 | Mejorar `.gitignore` si es necesario | 🟢 Baja |

---

## Fase 1: Core Gameplay

*Sistemas fundamentales para que el juego sea jugable*

| # | Tarea | Depende |
|---|-------|---------|
| 1.1 | **Refactorizar PlayerMovement** — extraer a componente limpio, sin lógica de Input | 0.2 |
| 1.2 | **Sistema de estadísticas del jugador** — HP, Mana, Stamina, Speed, Damage | 1.1 |
| 1.3 | **Sistema de inventario básico** — slots, items básicos | 1.2 |
| 1.4 | **Sistema de objetos recolectables** — vida, mana, daño (ya existen prefabs, falta lógica) | 1.3 |
| 1.5 | **Sistema de habitaciones** — limpiar generación, hacer determinista | 0.1, 0.2 |

---

## Fase 2: Combate y Enemigos

*Sistema de combate robusto*

| # | Tarea | Depende |
|---|-------|---------|
| 2.1 | **CombatSystem abstracto** — DamageType, defense, resistencias | 1.2 |
| 2.2 | **Refactorizar todos los AI** — crear EnemyBase con estados (StateMachine) | 1.1 |
| 2.3 | **Añadir tipos de daño** — físico, magia, fuego, etc. | 2.1 |
| 2.4 | **Sistema de efectos** — veneno, quemadura, aturdimiento | 2.3 |
| 2.5 | **Mejora del Boss** — fases, patrones, cinematica básica | 2.2 |
| 2.6 | **Balancing inicial** — valores base para todas las estadísticas | 2.5 |

---

## Fase 3: Progresión

*Elementos roguelike que hacen rejugable*

| # | Tarea | Depende |
|---|-------|---------|
| 3.1 | **Sistema de runas/pickups** — objetos que dan habilidades temporales | 2.3 |
| 3.2 | **Upgrade de arma** — sistema de mejoras (3 estrellas, affine, etc.) | 2.1 |
| 3.3 | **Tabla de drops** — probabilidad por tipo de enemigo | 2.2 |
| 3.4 | **Persistencia de runas** — guardar progresión entre habitaciones | 3.1 |
| 3.5 | **Meta-progresión** — desbloquear cosas entre partidas (opcional para prototipado) | 3.4 |

---

## Fase 4: UX y Polish

*Lo que hace el juego enjoynable*

| # | Tarea | Depende |
|---|-------|---------|
| 4.1 | **Pantallas de carga** entre habitaciones | 1.5 |
| 4.2 | **Feedback visual** — hit effects, screen shake, particles | 2.1 |
| 4.3 | **Feedback de audio** — SFX para attacks, hits, pickups | 4.2 |
| 4.4 | **Tutorial in-game** — tooltips, hints | 1.3 |
| 4.5 | **Mini-map** o indicador de posición | 1.5 |
| 4.6 | **Settings menu** — volumen, calidad, rebinding | 4.4 |

---

## Fase 5: Contenido

*Cosas que hacen el juego interesante*

| # | Tarea |
|---|-------|
| 5.1 | Más tipos de habitaciones (trap, treasure, puzzle) |
| 5.2 | Más tipos de enemigos (5-7 más) |
| 5.3 | Más bosses (2-3) |
| 5.4 | Biome/dificultad progresiva (floors) |
| 5.5 | Logros |

---

## Idioma

- English: [README.md](./README.md)
- Español: [README_ES.md](./README_ES.md)
- Roadmap: [ROADMAP.md](./ROADMAP.md)
- Roadmap ES: [ROADMAP_ES.md](./ROADMAP_ES.md)