#set page(
  footer: [
    #set align(center)
    #set text(8pt)
    _#counter(page).display(
      "1 / 1",
      both: true,
    )_
  ]
)

#grid(
  columns: (2fr, 1fr),
  align(left)[
    _Bourquin Olin Christopher, Demont Kilian, Guggisberg Simon, Troeltsch Jonas_
  ],
  align(right)[
    _été 2024 \ Rapport - PDG_
  ]
)

#align(center, text(20pt)[
  *Automation game*
])

#set par(justify: true)

= Rapport

== Description du projet

=== Objectifs

Créer un jeu d'automation (comme. Factorio, satisfactory, shapez, etc.) dans le thème de la musique. Le joueur doit livrer des notes individuelles, puis des accords et enfin des suites de notes pour créer des courtes mélodies.

=== Exigences fonctionnelles

- Le joueur doit recevoir des objectifs spécifiques à accomplir pour pouvoir progresser dans le jeu.
- Le jeu doit avoir une interface fonctionnelle à la souris.
- Le jeu doit posséder du son.
- Le jeu doit contenir des instructions, sous la forme d'un texte explicatif ou autre.
- Le joueur peut placer des éléments de production sur une grille
  - Générateur de notes
  - Changement de pitch (fréquence), monte ou descend la note selon une gamme de do
  - Changement de tempo (rythme), accélère ou ralentit la note, croche, noire, blanche, etc
  - Instrument, altère la note selon un instrument donné
  - Combineur de notes, produit des accords qui sont considérés comme une seule note
  - Combineur de notes alternés, produit une séquence
  - Sortie haut parleur, crée le son
- Le joueur peut supprimer des éléments de production existants

=== Exigences non-fonctionnelles

- Le jeu doit avoir des performances acceptable. 30-60fps minimum.
- Le jeu doit être compatible avec différents systèmes d'exploitation. Windows et Linux.
- Le jeu sera crée avec Godot 4.x.
- Le jeu sera crée avec .NET et donc C\# comme language.
- Le joueur ne doit pas perdre sa progression lors de l'exécution du jeu.

== Architecture préliminaire

- Rédaction rapport : Typst, et l'extension VSCode #link("https://marketplace.visualstudio.com/items?itemName=myriad-dreamin.tinymist")[
  tinymist Typst
]
- Création mockups et schémas : Figma
- Game Engine : Godot .Net 4.3.0.0
- IDE : Jetbrains Rider 2024.2

== Mockups / Landing page préliminaire

(Figma ici)

== Description des choix techniques


=== Prérequis

- C\# 8.0.x est un prérequis pour Godot 4.x ainsi que le framework de tests unitaires GdUnit4 que nous avons choisi.
- Godot 4.3 est la version la plus récente de Godot et est stable. Elle est incidentalement sortie 4 jours avant le début du module PDG.
- Typst pour éditer le rapport.

=== Outils utilisés

- Typst pour de plus grandes possibilités de mise en page
- Figma pour une collaboration plus facile dans un premier temps
- Godot pour sa nature open source et sa montée en popularité très récente. 
  - Pour la facilité d'intégration dans un pipeline non CI/CD à l'opposé de Unity, qui était l'autre solution envisagée pour un jeu à petite échelle en 2D. PixiJS était également une option mais nous avons préféré Godot en raison de sa popularité.
  - Nous avons choisi .Net en raison de l'utilité de C\# pour le futur, de notre familiarité avec Java qui partage beaucoup de similarité, et la réticence d'apprendre un langage unique à Godot tel que GDScript.
  - Godot support et possède de nombreux outils pour la création de jeu 2D, et en raison du scope de ce projet, réaliser un projet en 3D apporterait maintes complications sans apporter de plus value.
  - Godot permet facilement d'exporter un projet sur plusieurs plateformes, que ce soit ordinateur Windows/Linux/Mac, sur le web directement, ou même sur console via des outils tierces (en raison de la nature open source de Godot), les possibilités sont nombreuses même si nous ne visons, dans un premier temps, que les plate-formes Windows et Linux.
- Rider pour l'habitude de l'équipe à travailler avec les produits Jetbrains et sa comptabilité avec Godot.
- GDUnit4 est un framework de tests unitaires pour Godot 4.x supportant tant GDScript que le C\#. Il dispose également d'une intégration CI/CD avec les Github Actions.

#figure(
  image("gmtk-top-game-engine-2017-2024.jpg", width: 100%),
  caption: [
    pourcentage de game engines utilisés lors de la GMTK game jam
  ],
)

== Description du processus de travail

=== Git workflow

- Organisation du git: branche ``` main``` avec le workflow pour créer un build
- Les PR ouvertes pour ``` main``` lancent des teste unitaires / de features qui doivent passer pour pouvoir merge la PR
- Branches individuelles pour les features suffixées avec les initiales de la personne en charge
- Branche ``` pages``` pour la landing page

=== CI/CD

- Tests unitaires lors d'une PR.
- Builds Windows et Linux suivis d'une release lors de la parution d'un tag sous la form `vx.y` sur `main`