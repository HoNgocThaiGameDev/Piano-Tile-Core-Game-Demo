Piano-Tile-Core-Game-Demo
🎹 Piano Tile – Unity Game
🕹️ Description
Piano Tile is a rhythm-based music game where players must tap the correct black tiles in time with the music. The game challenges the player's reflexes and musical timing.

🚀 How to Play

Tap the black tiles when they reach the designated zone.

Avoid tapping empty spaces or bomb tiles.

Maintain combo chains and aim for the highest score!

🛠️ Project Structure

Framework
A collection of core components that power the game’s functionality.

ViewManager
Manages the display of user interfaces (UI, parallax), including the main menu, gameplay screen, and result screen.

ConfigManager
Handles and loads configuration data from files (e.g., song list, tempo, number of notes, etc.).

Database (MVC Model)
Uses the MVC pattern for data models. Classes like ConfigSongRecord and ConfigSong store song data.

GameDesign
Contains the core game logic such as note generation, collision handling, scoring, movement speed, and effects.

PoolManager
Uses object pooling to optimize performance, especially for frequently spawning nodes.

📁 Requirements

Unity 2022.3.51f1 or higher

DOTween

Newtonsoft Json

Advanced PlayerPrefs Window
