# Implementation

Unity 6 + C# + URP. 3D rendering with 2.5D gameplay on X/Z.

## Implemented core
- X/Z player movement + mobile joystick adapter
- top-down 2.5D camera
- health/damage/entity registry
- bounded recursive combat event queue
- auto-target/auto-attack Sword foundation
- manual active skill
- enemy chase/contact damage
- horde stage spawn, kill objective, victory and death
- persistent Core/campaign/Abyss progress
- upgrade cost curves
- Trait runtime foundation
- deterministic infinite Abyss floor generation
- editor prototype scene builder

## Event limits
Generation 32, 4096 descendants/root, 1200 events/frame. Deferred events preserve queue order.

## Setup
Open with Unity 6 and run **Horde Evolution > Create Prototype Scene**. Create an Enemy prefab with Health + EnemyAgent. Add StageDirector to the scene and assign Player and Enemy prefab.

## Next production layers
Data-driven ScriptableObject content, six complete weapon behavior modules, mutation/evolution proc definitions, trait fusion UI, campaign content tables, pooled VFX/damage-number renderer, boss mechanics, mobile HUD and balance content.
