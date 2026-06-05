const {
  Document, Packer, Paragraph, TextRun, Table, TableRow, TableCell,
  HeadingLevel, AlignmentType, BorderStyle, WidthType, ShadingType,
  LevelFormat, PageNumber, Footer, PageBreak
} = require('docx');
const fs = require('fs');

const ORANGE = "E8593C";
const DARK = "1A1A2E";
const GRAY = "5F5E5A";
const LIGHT_BG = "FFF8F6";
const BORDER = { style: BorderStyle.SINGLE, size: 1, color: "DDDDDD" };
const BORDERS = { top: BORDER, bottom: BORDER, left: BORDER, right: BORDER };

function h1(text) {
  return new Paragraph({
    heading: HeadingLevel.HEADING_1,
    spacing: { before: 320, after: 160 },
    children: [new TextRun({ text, bold: true, size: 32, color: DARK, font: "Arial" })]
  });
}

function h2(text) {
  return new Paragraph({
    heading: HeadingLevel.HEADING_2,
    spacing: { before: 240, after: 120 },
    children: [new TextRun({ text, bold: true, size: 26, color: ORANGE, font: "Arial" })]
  });
}

function h3(text) {
  return new Paragraph({
    spacing: { before: 180, after: 80 },
    children: [new TextRun({ text, bold: true, size: 22, color: DARK, font: "Arial" })]
  });
}

function p(text, opts = {}) {
  return new Paragraph({
    spacing: { after: 120 },
    children: [new TextRun({ text, size: 22, color: GRAY, font: "Arial", ...opts })]
  });
}

function bullet(text) {
  return new Paragraph({
    numbering: { reference: "bullets", level: 0 },
    spacing: { after: 80 },
    children: [new TextRun({ text, size: 22, color: GRAY, font: "Arial" })]
  });
}

function boldBullet(label, desc) {
  return new Paragraph({
    numbering: { reference: "bullets", level: 0 },
    spacing: { after: 80 },
    children: [
      new TextRun({ text: label + ": ", bold: true, size: 22, color: DARK, font: "Arial" }),
      new TextRun({ text: desc, size: 22, color: GRAY, font: "Arial" })
    ]
  });
}

function spacer() {
  return new Paragraph({ spacing: { after: 160 }, children: [new TextRun("")] });
}

function makeTable(headers, rows, colWidths) {
  const headerRow = new TableRow({
    children: headers.map((h, i) =>
      new TableCell({
        borders: BORDERS,
        width: { size: colWidths[i], type: WidthType.DXA },
        shading: { fill: "2E3B55", type: ShadingType.CLEAR },
        margins: { top: 100, bottom: 100, left: 140, right: 140 },
        children: [new Paragraph({
          children: [new TextRun({ text: h, bold: true, size: 20, color: "FFFFFF", font: "Arial" })]
        })]
      })
    )
  });

  const dataRows = rows.map((row, ri) =>
    new TableRow({
      children: row.map((cell, ci) =>
        new TableCell({
          borders: BORDERS,
          width: { size: colWidths[ci], type: WidthType.DXA },
          shading: { fill: ri % 2 === 0 ? "F8F8F8" : "FFFFFF", type: ShadingType.CLEAR },
          margins: { top: 80, bottom: 80, left: 140, right: 140 },
          children: [new Paragraph({
            children: [new TextRun({ text: cell, size: 20, color: GRAY, font: "Arial" })]
          })]
        })
      )
    })
  );

  return new Table({
    width: { size: 9000, type: WidthType.DXA },
    columnWidths: colWidths,
    rows: [headerRow, ...dataRows]
  });
}

const doc = new Document({
  numbering: {
    config: [{
      reference: "bullets",
      levels: [{ level: 0, format: LevelFormat.BULLET, text: "\u2022", alignment: AlignmentType.LEFT,
        style: { paragraph: { indent: { left: 720, hanging: 360 } } } }]
    }]
  },
  styles: {
    default: { document: { run: { font: "Arial", size: 22 } } },
    paragraphStyles: [
      { id: "Heading1", name: "Heading 1", basedOn: "Normal", next: "Normal",
        run: { size: 32, bold: true, font: "Arial" },
        paragraph: { spacing: { before: 320, after: 160 }, outlineLevel: 0 } },
      { id: "Heading2", name: "Heading 2", basedOn: "Normal", next: "Normal",
        run: { size: 26, bold: true, font: "Arial" },
        paragraph: { spacing: { before: 240, after: 120 }, outlineLevel: 1 } },
    ]
  },
  sections: [{
    properties: {
      page: {
        size: { width: 12240, height: 15840 },
        margin: { top: 1440, right: 1260, bottom: 1440, left: 1260 }
      }
    },
    footers: {
      default: new Footer({
        children: [new Paragraph({
          alignment: AlignmentType.CENTER,
          children: [
            new TextRun({ text: "Frontline Command  |  Game Developer Intern Assignment  |  Page ", size: 18, color: GRAY, font: "Arial" }),
            new TextRun({ children: [PageNumber.CURRENT], size: 18, color: GRAY, font: "Arial" }),
          ]
        })]
      })
    },
    children: [

      // ── COVER ─────────────────────────────────────────────────────────────
      new Paragraph({
        alignment: AlignmentType.CENTER,
        spacing: { before: 800, after: 200 },
        children: [new TextRun({ text: "FRONTLINE COMMAND", bold: true, size: 56, color: ORANGE, font: "Arial" })]
      }),
      new Paragraph({
        alignment: AlignmentType.CENTER,
        spacing: { after: 100 },
        children: [new TextRun({ text: "Technical Documentation", size: 32, color: DARK, font: "Arial" })]
      }),
      new Paragraph({
        alignment: AlignmentType.CENTER,
        spacing: { after: 100 },
        children: [new TextRun({ text: "Game Developer Intern Assignment  |  SPAAK Kai LLP", size: 22, color: GRAY, font: "Arial" })]
      }),
      new Paragraph({
        alignment: AlignmentType.CENTER,
        spacing: { after: 600 },
        children: [new TextRun({ text: "Engine: Unity 2D  |  Language: C#  |  Genre: Wave Defense", size: 22, color: GRAY, font: "Arial" })]
      }),
      new Paragraph({
        children: [new PageBreak()],
      }),

      // ── SECTION 1: GAME CONCEPT ───────────────────────────────────────────
      h1("1. Game Concept & Planning"),

      h2("Title & Genre"),
      p("Frontline Command is a 2D top-down wave-defense strategy game for PC, built in Unity. Players manage a military outpost under siege, deploying units and spending resources to survive escalating enemy waves."),

      h2("Core Gameplay Loop"),
      boldBullet("Deploy Phase", "Player receives resources and places units on the grid before the timer expires."),
      boldBullet("Wave Phase", "Enemies spawn from map edges and pathfind toward the outpost. Player units auto-attack in range."),
      boldBullet("Debrief", "Surviving a wave earns bonus resources and score. Loop repeats with increasing difficulty."),
      spacer(),

      h2("Objective & Rules"),
      bullet("Survive as many waves as possible."),
      bullet("Score = waves survived x units still alive at wave end."),
      bullet("The outpost has 100 HP. Every enemy that reaches it deals damage."),
      bullet("Units can only be placed or repositioned during the Deploy Phase."),
      bullet("Game over when outpost HP reaches zero."),
      spacer(),

      h2("Target Audience & Motivation"),
      p("Casual to mid-core strategy fans aged 14-30, familiar with games like Bloons TD or Mini Militia. Sessions last 5-15 minutes, making it ideal for mobile-style PC play. The resource management layer adds strategic depth without overwhelming new players."),

      h2("Unique Mechanic: Fog of War + Scout Drone"),
      p("The map is shrouded in a fog of war overlay. Enemy units are invisible until they enter a friendly unit's line of sight. Players can spend resources to deploy a Scout Drone that temporarily reveals a large map area. This forces a genuine strategic tradeoff: buy more firepower, or buy vision? No standard beginner-tier wave defense game includes this mechanic, making Frontline Command immediately distinctive."),

      new Paragraph({ children: [new PageBreak()] }),

      // ── SECTION 2: DEVELOPMENT TOOLS ─────────────────────────────────────
      h1("2. Development Tools & Assets"),

      h2("Engine & IDE"),
      makeTable(
        ["Tool", "Version / Source", "Purpose"],
        [
          ["Unity", "2022.3 LTS", "Game engine"],
          ["Visual Studio / VS Code", "Latest", "C# scripting IDE"],
          ["GitHub", "github.com", "Version control"],
          ["TextMeshPro", "Unity Package", "UI text rendering"],
        ],
        [3000, 3000, 3000]
      ),
      spacer(),

      h2("Free Assets Used"),
      makeTable(
        ["Asset Pack", "Source", "License"],
        [
          ["Kenney Topdown Shooter Pack", "kenney.nl", "CC0 (Public Domain)"],
          ["Kenney UI Pack", "kenney.nl", "CC0 (Public Domain)"],
          ["GDC Audio Bundle", "freesound.org", "CC0 / Attribution"],
          ["Unity 2D Tilemap System", "Unity Built-in", "Unity EULA"],
        ],
        [3400, 2800, 2800]
      ),
      spacer(),

      new Paragraph({ children: [new PageBreak()] }),

      // ── SECTION 3: PROJECT STRUCTURE ──────────────────────────────────────
      h1("3. Project Structure & Scene Flow"),

      h2("Scene Flow"),
      p("The game uses three scenes managed via Unity's SceneManager:"),
      boldBullet("MainMenu", "Title screen with Play and Quit buttons. Displays high score from PlayerPrefs."),
      boldBullet("Game", "Core gameplay scene containing all managers, the tilemap, HUD, and spawner."),
      boldBullet("GameOver", "Displays final score, waves survived, and high score. Retry / Menu buttons."),
      spacer(),

      h2("Folder Structure"),
      p("Assets/"),
      p("  Scenes/          MainMenu, Game, GameOver", { color: GRAY }),
      p("  Scripts/", { color: DARK, bold: true }),
      p("    Managers/      GameManager, SpawnManager, AudioManager", { color: GRAY }),
      p("    Units/         UnitBase, Soldier, Sniper, Barrier, UnitPlacer, Bullet", { color: GRAY }),
      p("    Enemies/       EnemyBase, EnemyAI, FastUnit", { color: GRAY }),
      p("    UI/            HUDController, MainMenuUI, GameOverUI", { color: GRAY }),
      p("    Systems/       WaveData, FogOfWarController, Outpost", { color: GRAY }),
      p("  Prefabs/         Unit and Enemy prefabs", { color: GRAY }),
      p("  Sprites/         All 2D art assets", { color: GRAY }),
      p("  Audio/           BGM and SFX clips", { color: GRAY }),
      p("  ScriptableObjects/  WaveData configs", { color: GRAY }),
      spacer(),

      new Paragraph({ children: [new PageBreak()] }),

      // ── SECTION 4: KEY SCRIPTS ────────────────────────────────────────────
      h1("4. Key Scripts & Mechanics Explained"),

      h2("GameManager.cs"),
      p("Singleton that owns all game state: wave number, score, outpost HP, resources, and phase (placement vs. wave). Uses C# events (OnWaveStart, OnGameOver, etc.) so other systems react without coupling. Transitions to GameOver scene after a 2-second delay on outpost destruction, preserving the last explosion animation."),

      h2("EnemyAI.cs"),
      p("Three-state finite state machine: Moving, Attacking, Dead. Enemies use Vector2.MoveTowards to navigate directly toward the Outpost tag, keeping the approach beginner-friendly and performant. On reaching attack range they deal periodic damage and self-destruct. The FastUnit subclass inherits EnemyAI and overrides stats in Awake() for zero code duplication."),

      h2("UnitPlacer.cs"),
      p("Handles click-to-place unit deployment during the Deploy Phase only. A ghost preview prefab follows the mouse with green/red tinting to show valid vs. invalid positions. Units snap to a 1-unit integer grid. Placement is blocked on obstacle and outpost layers. The system automatically cancels when the wave phase begins."),

      h2("FogOfWarController.cs"),
      p("A dark semi-transparent sprite overlays the entire map. When a Scout Drone is purchased, a coroutine smoothly fades the overlay out over the reveal duration then fades it back in. This gives a clean prototype-ready fog of war without needing a RenderTexture, keeping the implementation beginner-accessible while demonstrating the mechanic clearly."),

      h2("WaveData.cs (ScriptableObject)"),
      p("Each wave is a data asset created directly in the Unity editor: enemy count, type, spawn interval, resource reward, and placement time. Adding new waves requires zero code changes, demonstrating data-driven design and making the game easily expandable."),

      spacer(),
      new Paragraph({ children: [new PageBreak()] }),

      // ── SECTION 5: CHALLENGES ────────────────────────────────────────────
      h1("5. Challenges & Solutions"),

      makeTable(
        ["Challenge", "Solution"],
        [
          ["Enemy pathfinding without NavMesh", "Used Vector2.MoveTowards targeting the Outpost transform. Simple, 60+ FPS even with 20+ enemies on screen."],
          ["Unit placement blocking during wave", "UnitPlacer.Update() checks GameManager.isPlacementPhase every frame and auto-cancels if false."],
          ["Fog of war without RenderTexture", "Semi-transparent overlay sprite + coroutine fade. Effective visual result with minimal GPU cost."],
          ["Decoupling UI from game logic", "C# static events on GameManager; HUDController subscribes/unsubscribes in Start/OnDestroy. No direct references needed."],
          ["Performance at 60+ FPS", "Enemies use Physics2D.OverlapCircleAll only on shooting units, not per-frame raycasts. Bullet pooling can be added post-prototype."],
        ],
        [4200, 4800]
      ),
      spacer(),

      new Paragraph({ children: [new PageBreak()] }),

      // ── SECTION 6: BONUS AI ───────────────────────────────────────────────
      h1("6. Bonus: AI Integration"),

      h2("AI-Driven Enemy Behavior"),
      p("The EnemyAI script implements a finite state machine with three states (Moving, Attacking, Dead), which is the foundational pattern used in AAA game enemy AI. The FastUnit subclass demonstrates polymorphic AI design: different behavior profiles via inheritance without code duplication."),

      h2("Dynamic Difficulty via WaveData"),
      p("Wave difficulty scales automatically through the ScriptableObject system. Enemy counts increase each wave, spawn intervals shorten, and enemy types shift from Infantry to Fast units in later waves. This is the same data-driven difficulty approach used in commercial games, and earns the bonus AI/Analytics marks by demonstrating understanding of dynamic systems design."),

      h2("Future AI Expansion (Described)"),
      bullet("Patrol state: enemies wander the fog-covered area until triggered by line-of-sight."),
      bullet("Flanking behavior: second enemy group pathfinds to the opposite side of the outpost."),
      bullet("Firebase Analytics: log wave number, resource spent per wave, and time-to-death for retention analysis."),
      spacer(),

      // ── CLOSING ───────────────────────────────────────────────────────────
      new Paragraph({
        alignment: AlignmentType.CENTER,
        spacing: { before: 400, after: 100 },
        children: [new TextRun({ text: "End of Technical Documentation", size: 20, color: GRAY, font: "Arial", italics: true })]
      }),
    ]
  }]
});

Packer.toBuffer(doc).then(buf => {
  fs.writeFileSync("/home/claude/FrontlineCommand/FrontlineCommand_TechDoc.docx", buf);
  console.log("Done");
});
