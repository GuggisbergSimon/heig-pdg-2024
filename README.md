# Musicz

Répertoire concernant le module PDG d'été 2024 à la HEIG-VD.

## Équipe

| Nom | Prénom | Github |
| --- | ------ | ----- |
| Guggisberg   | Simon | [@GuggisbergSimon](https://github.com/GuggisbergSimon) |
| Bourquin	| Olin	| [@OlinB](https://github.com/OlinB) |
| Troeltsch	| Jonas	| [@jonas-tr](https://github.com/jonas-tr) |
| Demont	| Kilian | [@kdemont](https://github.com/kdemont) |

## Description

Un voisin particulièrement bruyant, des bruits de travaux qui vous percent les tympans. Il est temps de lui rendre la pareille. 

Musicz est un jeu d'automatisation de notes de musique. Le joueur doit placer des éléments sur un circuit pour que les notes de musique puissent être jouées. Le jeu est basé sur le moteur Godot et est écrit en C#.

## Landing Page

La landing page est hébergée par Github Pages et se trouve à cette adresse : [https://guggisbergsimon.github.io/heig-pdg-2024](https://guggisbergsimon.github.io/heig-pdg-2024)



## Comment contribuer au project

Si vous souhaitez contribuer au projet, vous pouvez suivre les instructions ci-dessous.

### Prérequis

- [Godot Engine](https://godotengine.org/download/) 4.3 .NET
- [.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) SDK 8.0.x

### Installation

1. Cloner le repository
2. Importer le projet dans Godot en choisissant le fichier project.godot situé à la racine de ce répertoire
3. Attendre la fin de l'importation des assets. En cas d'erreur ou d'avertissement lors de l'import initial, il suffit de reload le projet godot via le menu Project -> Reload Current Project

Pour une prise en main complète de Godot, veuillez vous référez à la [documentation officielle](https://docs.godotengine.org/en/stable/getting_started/introduction/index.html). À noter néanmoins que F5 permet de lancer le jeu en mode debug.

### Contribution

Regardez les [issues](https://github.com/GuggisbergSimon/heig-pdg-2024/issues) pour voir ce qui doit être fait. Si vous avez une nouvelle idée, ou rencontrez un problème, n'hésitez pas à ouvrir une nouvelle issue.

Pour contribuer, créez une fork du projet, faites vos modifications, créez une pull request et lier la à l'issue que vous resolvez. Si les tests passent et que que un membre du groupe a approuvé, votre code sera mergé.

### Mockups et recherche

Nous avons d'abord réfléchi au Game design en exprimant les éléments au travers via une mindmap :
![Game design](/rapport/img/game-design.png)

Nous avons également représenté un exemple de circuit que les joueurs pourraient être amenés à créer :
![Game design sequence](/rapport/img/game-design-sequence.png)

Ci-dessous se trouvent les mockups représentant le tutoriel, d'un niveau simple et d'un plus complexe.
![Tutorial](/rapport/img/mockup-tutorial.png)
![Early level](/rapport/img/mockup-early-level.png)
![Later level](/rapport/img/mockup-late-level.png)

## Rapport

Plus d'informations se trouvent sur le rapport, réalisé à l'aide de [Typst](https://typst.app/) et l'extension VSCode [tinymist Typst](https://marketplace.visualstudio.com/items?itemName=myriad-dreamin.tinymist).

Le rapport est disponible [en .pdf](/rapport/rapport.pdf) pour la lecture et [en .typ](/rapport/rapport.typ) pour l'édition.

## Mentions légales

Certains sons utilisés dans le jeu sont sous licence Creative Commons By Attribution. Voici la liste des sons utilisés :

- [ui-sounds](https://ellr.itch.io/universal-ui-soundpack) par Nathan Gibson
- [level-up](https://pixabay.com/users/universfield-28281460/) par universfield
