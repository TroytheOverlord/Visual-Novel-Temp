What If

What If is a narrative-driven visual novel prototype built in Unity (C#). It explores themes of choice, consequence, and identity, giving players branching dialogue and immersive story segments presented with layered characters, backgrounds, and dynamic transitions.

🎮 Overview

The project blends visual novel storytelling with light exploratory gameplay elements. Players engage in dialogue sequences where their choices influence pacing and tone, while background transitions, character movement, and layered sprites create a cinematic presentation.

The goal of What If is to experiment with interactive narrative systems, developing tools and structures that can be expanded into a full-scale story-driven game.

✨ Features

Dialogue System

Text-based conversations with branching paths

Supports commands embedded in dialogue (e.g., scene changes, animations, events)

Dialogue progression tracked through a Conversation manager

Character System

Characters displayed as layered PNGs on backgrounds

Visibility, movement, and transitions handled dynamically

Commands for creating/removing characters at runtime

Background & Scene Transitions

Smooth fades between locations

Integration with narrative commands for cinematic flow

Exploratory Gameplay Elements

Player character can walk across backgrounds (towns, stores, etc.)

Interaction triggers (e.g., entering a shop transitions to dialogue, then back to overworld)

🛠️ Tech & Systems

Engine: Unity (C#)

Dialogue Parser: Custom text file parser that reads dialogue lines + commands

Namespace: DIALOGUE – organizes systems like Conversation, Character, and Character_Sprite

Commands: Modular commands trigger actions like scene transitions, character animations, or spawning dialogue

🚀 Setup & Run

Clone/download the repository.

Open in Unity (2022.x or later recommended).

Play directly from the Unity Editor.

📸 Demo (Optional)

(Screenshots or GIFs showing dialogue boxes, character layering, and transitions would strengthen this section.)

🌱 Future Improvements

Expand branching dialogue options

Implement choices that persist across multiple scenes

Add animations to characters for emotional expression

Integrate sound design (music & SFX)

Create a save/load system for longer play sessions
