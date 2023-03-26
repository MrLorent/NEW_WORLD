# 🐻 TOTOROLAND 🐻
Unity autonom world rules by a combination of 4 biological inspired algorithms :
- Fractals (Diamond Square map generation, LSystem based trees);
- Genetic Algorithm (Tree population evolution);
- Game Of Life (Flowers evolution);
- Boids (Totoros);

## 🎮 COMMANDS 🎮

### ➡️ MOVEMENTS
- <kbd>Z</kbd> : Forward;
- <kbd>S</kbd> : Back;
- <kbd>Q</kbd> : Left;
- <kbd>D</kbd> : Right.  

On free fly mode only :
- <kbd>Space</kbd> : Up;
- <kbd>Left Shift</kbd> : Down.  

### ➡️ OTHERS
- <kbd>C</kbd> : Change camera mode (Constain | Free fly);
- <kbd>R</kbd> : Reset camera to initial position and mode;

## 🎮 FOLDER ORGANISATION 🎮

### ➡️ Index

```
+---Art
+---Editor
+---Prefabs
| +---Boids
+---Ressources
| +---GA
| +---LSystem
+---Scenes
| \---Main
| \---Diamond Square
| \---LSystem
+---Scripts
| +---Boids
| +---Diamond Square
| +---GA
| +---LSystem
| +---Managers
| +---Scriptables
| +---Shaders
| +---System
| +---Utilities
```

### ➡️ Folders description

- **Art**: All the art materials of the project, from game assets to menu assets.

- **Editor**: Unity Editor modifier scripts.

- **Prefabs**: All the Unity prefabs of the project.

    - **Boids**: Boids prefabs.

- **Ressources**: All the Unity Scriptable Objects use to instanciate IA Algorithm scripts.

    - **GA**: All Genetic Algorithm attributs objects (Trees attributs).

    - **LSystem**: All LSystem models objects (LSystem bases).

- **Scenes**: Contains all the Unity scenes created during development.

    - **Main**: The actual project scene.

    - **Diamond Square**: Map generation experimentation scene.

    - **LSystem**: Tree generation experimentation scene.

- **Scripts**: Basically contains all the C# code of the project.

    - **Boids**: Boids scripts.

    - **Diamond Square**: Diamond square script.

    - **GA**: Genetic Algorithm scripts.

    - **LSystem**: LSystem scripts.

    - **Managers**: Bio IA algorithm Unity implementation managers.

    - **Scriptables**: Scripts defining ressources objects.

    - **Shaders**: Terrain shader script.

    - **System**: Core managers scripts.

    - **Utilities**: Unity helpers scripts.