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

- 

=== Exigences non-fonctionnelles

- 

== Architecture préliminaire

Game engine: Godot

== Mockups / Landing page préliminaire

(Figma ici)

== Description des choix techniques

Godot comme game engine parce que c'open source, à la mode et possibilité d'intégrer le build dans un pipeline CI/CD

== Description du processus de travail

- Organisation du git: branche ``` main``` avec le workflow pour créer un build
- Les PR ouvertes pour ``` main``` lancent des teste unitaires / de features qui doivent passer pour pouvoir merge la PR
- Branches individuelles pour les features suffixées avec les initiales de la personne en charge
- Branche ``` pages``` pour la landing page
