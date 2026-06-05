# Frontline Command
### Game Developer Intern Assignment — SPAAK Kai LLP

**Genre:** 2D Top-Down Wave Defense  
**Engine:** Unity 2022.3 LTS  
**Language:** C#  
**Theme:** War / Strategy

---

## How to Set Up in Unity

### Step 1 — Create a New Unity Project
1. Open Unity Hub → New Project → **2D (URP or Built-in)** → name it `FrontlineCommand`
2. Copy all files from `Assets/Scripts/` into your Unity project's `Assets/Scripts/` folder

### Step 2 — Install TextMeshPro
- Window → Package Manager → search "TextMeshPro" → Install
- When prompted, import TMP Essentials

### Step 3 — Create Scenes
Create 3 scenes in `Assets/Scenes/`:
- `MainMenu` — add a Canvas with Play button, attach `MainMenuUI.cs`
- `Game` — main gameplay scene (see hierarchy below)
- `GameOver` — add Canvas with score labels, attach `GameOverUI.cs`

Add all 3 scenes to **File → Build Settings** in order.

### Step 4 — Set Up the Game Scene Hierarchy
```
Game (scene)
├── GameManager (empty GO) → attach GameManager.cs
│     └── Set waves array with WaveData ScriptableObjects
├── SpawnManager (empty GO) → attach SpawnManager.cs
│     └── Assign spawnPoint Transforms at map edges
├── AudioManager (empty GO) → attach AudioManager.cs
│     └── Assign audio clips in Inspector
├── Tilemap → import Kenney Topdown tiles, paint ground
├── Outpost (Sprite) → tag as "Outpost", attach Outpost.cs
│     └── Add BoxCollider2D
├── FogOfWar (full-screen dark Sprite) → attach FogOfWarController.cs
├── UnitPlacer (empty GO) → attach UnitPlacer.cs
│     └── Set groundLayer and blockedLayer masks
└── Canvas (UI) → attach HUDController.cs
      └── Wire up all UI references in Inspector
```

### Step 5 — Create WaveData ScriptableObjects
- Right-click in Project window → Create → FrontlineCommand → WaveData
- Create 5-8 waves, increasing `enemyCount` and decreasing `spawnInterval`
- Assign them to `GameManager.waves[]` array

### Step 6 — Create Prefabs
Create prefabs for:
- `Soldier` — Sprite + Soldier.cs + CircleCollider2D (layer: Unit)
- `Sniper` — Sprite + Sniper.cs + CircleCollider2D (layer: Unit)
- `Barrier` — Sprite + Barrier.cs + BoxCollider2D (layer: Unit)
- `Infantry` — Sprite + EnemyAI.cs + CircleCollider2D (layer: Enemy)
- `FastUnit` — Sprite + FastUnit.cs + CircleCollider2D (layer: Enemy)
- `Bullet` — small circle Sprite + Bullet.cs

### Step 7 — Layer Setup
In Edit → Project Settings → Tags and Layers, create:
- Layer 8: `Enemy`
- Layer 9: `Unit`
- Layer 10: `Ground`
- Layer 11: `Obstacle`

### Step 8 — Physics 2D Settings
In Edit → Project Settings → Physics 2D:
- Disable collision between Unit↔Unit (they shouldn't block each other)
- Enable Enemy↔Unit collision (for bullets)

---

## Controls
| Action | Input |
|--------|-------|
| Select unit | Click UI button |
| Place unit | Left click on map |
| Cancel placement | Right click |
| Deploy Scout | Click Scout button |

---

## Free Asset Sources
- **Sprites:** https://kenney.nl/assets/topdown-shooter
- **UI:** https://kenney.nl/assets/ui-pack
- **Sounds:** https://freesound.org (search: gunshot, explosion, alert)

---

## File Structure
```
Assets/
├── Scenes/          MainMenu.unity, Game.unity, GameOver.unity
├── Scripts/
│   ├── Managers/    GameManager.cs, SpawnManager.cs, AudioManager.cs
│   ├── Units/       UnitBase.cs, Soldier.cs, Sniper.cs, Barrier.cs
│   │                UnitPlacer.cs, Bullet.cs
│   ├── Enemies/     EnemyBase.cs, EnemyAI.cs, FastUnit.cs
│   ├── UI/          HUDController.cs, MainMenuUI.cs, GameOverUI.cs
│   └── Systems/     WaveData.cs, FogOfWarController.cs, Outpost.cs
├── Prefabs/
├── Sprites/
├── Audio/
└── ScriptableObjects/
```

---

## Submission
- Build: File → Build Settings → PC, Mac & Linux → Build → exports `.exe`
- Zip: project folder + Build folder + video demo
- Email: contactus@smarrtifai.com
- Subject: `Game Developer Intern – Assignment Submission | [Your Name]`
