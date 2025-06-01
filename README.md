# AR Bowling

##### Auteurs: GRELET--CARTIER Suzanne & VON AESCH Nicolas
##### Année universitaire: 2025/2026


## Description

Ce projet est constitué d'une application en réalité augmentée développée en C# avec Unity, dans le cadre du cours Réalité augmentée et interactions hybrides.
L'objectif de ce projet est de découvrir la technologie de réalité augmentée et de développer des interactions avec celle-ci.
Nous utiliserons dans le cadre de ce projet cette technologie afin de simuler un Bowling. 
## Build
### Prérequis

Pour exécuter ce projet, vous aurez besoin de:

- [Unity 2022.3.37](https://unity.com/fr/releases/editor/whats-new/2022.3.37#installs)

### Installation

1. Clonez ce dépôt sur votre machine locale :
```sh
git clone https://github.com/nvonaesch/Technologie_Facteurs_Humains main
```
2. Ouvrez le projet avec Unity

3. Connecter votre téléphone mobile Android via câble USB avec le mode développeur actif et le débogage USB.

4. File → Build Settings... → Run device (Votre téléphone mobile) → Build And Run.

## Structure du projet

Le projet est organisé comme suit :

- `Assets/`: Contient tous les assets du projet (modèles 3D, textures, scripts, scènes Unity, etc.).  
├── `Materials/`: Liste les matériaux utilisés dans les scènes.  
├── `Models/`: Contient les modèles 3D dont la Quille, l'afficheur de score et la Boule de bowling.  
├── `Scenes/` : Contient les différentes scènes du projet, celle de calibration, le menu de démarrage et de jeu.  
├── `Scripts/`: Liste les différents scripts décrivant les interactions.  
│&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└──`Calibration/`: Contient les scripts décrivant les interactions de la scène de calibration.  
│&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└──`Menu/`: Contient les scripts décrivant les interactions du menu.  
│&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└──`SampleScene/`: Contient les scripts décrivant les interactions du jeu.  
├── `Videos/`: Contient les différentes vidéos jouées dans le jeu  
├── `XR/`: Contient les éléments liés à l’écosystème XR de Unity  
└── `XRI/`:  Contient des éléments spécifiques à l’interaction utilisateur dans un environnement XR  
- `Packages/` : Liste des dépendances utilisées dans le projet via le Package Manager de Unity.  
- `ProjectSettings/` : Contient la configuration du projet (qualité, build, input, etc.).  