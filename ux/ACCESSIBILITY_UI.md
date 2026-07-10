# ORBITA AI — Accessibility (UI Application)

> Ce document précise comment les principes d'accessibilité définis dans [branding/ACCESSIBILITY.md](../branding/ACCESSIBILITY.md) s'appliquent concrètement aux écrans et composants du produit. Il ne redéfinit aucune valeur déjà fixée par le branding (ratios de contraste, tailles minimales) — il en précise l'application par contexte d'interface.

## 1. Navigation clavier

- L'ensemble de la Sidebar, de la Topbar et de toute zone de contenu est accessible par tabulation, dans l'ordre défini dans [NAVIGATION.md](NAVIGATION.md) et [INTERACTION_RULES.md](INTERACTION_RULES.md) §6.
- Les raccourcis clavier officiels (cf. [NAVIGATION.md](NAVIGATION.md) §5) constituent un accès alternatif à la tabulation, jamais un remplacement qui rendrait la tabulation seule insuffisante.
- Un tableau de données (cf. [COMPONENT_LIBRARY.md](COMPONENT_LIBRARY.md) §5) est navigable au clavier ligne par ligne et colonne par colonne, avec activation des actions par `Entrée`.
- Le panneau ORION est ouvrable, utilisable (saisie et lecture des réponses) et refermable intégralement au clavier.

## 2. Focus

- Tout élément interactif affiche l'indicateur de focus défini dans [branding/ACCESSIBILITY.md](../branding/ACCESSIBILITY.md) §5, sans exception, y compris les cartes KPI cliquables, les lignes de tableau sélectionnables et les points de graphique interactifs.
- L'ouverture d'une modale ou d'un panneau latéral déplace immédiatement le focus vers son premier élément interactif, et le restitue à l'élément déclencheur à sa fermeture.

## 3. Contraste

- Toute combinaison texte/fond utilisée dans une carte, un badge, un graphique ou un état (vide, chargement, erreur) respecte les ratios minimaux définis dans [branding/ACCESSIBILITY.md](../branding/ACCESSIBILITY.md) §1.
- Les couleurs sémantiques (succès, avertissement, critique) ne sont jamais le seul vecteur d'information : chaque badge, indicateur de variation ou alerte est toujours doublé d'un libellé textuel ou d'une icône (cf. [COMPONENT_LIBRARY.md](COMPONENT_LIBRARY.md) §10).

## 4. Lisibilité

- Les tailles de police minimales définies dans [branding/TYPOGRAPHY.md](../branding/TYPOGRAPHY.md) et [branding/ACCESSIBILITY.md](../branding/ACCESSIBILITY.md) §3 s'appliquent sans exception, y compris dans les tableaux en mode de densité compacte (cf. [DESIGN_SYSTEM.md](DESIGN_SYSTEM.md) §2).
- Les valeurs chiffrées critiques (KPI, montants) visent le ratio de contraste renforcé (AAA) défini dans [branding/ACCESSIBILITY.md](../branding/ACCESSIBILITY.md) §1.

## 5. Feedback utilisateur

- Toute action de l'utilisateur (clic, saisie, sélection) produit un retour perceptible non exclusivement visuel lorsque cela est pertinent (ex. changement d'état annoncé pour les technologies d'assistance, message d'erreur associé programmatiquement au champ concerné).
- Un message d'erreur ou de confirmation n'est jamais communiqué uniquement par une variation de couleur (cf. [ERROR_STATES.md](ERROR_STATES.md) §2.1).

## 6. Alternative textuelle aux graphiques

- Tout graphique (cf. [DATA_VISUALIZATION.md](DATA_VISUALIZATION.md)) dispose d'un accès à une vue tabulaire équivalente des données représentées, consultable indépendamment de la perception visuelle du graphique.
- Les infobulles (tooltips) de graphique sont accessibles au clavier (focus) et pas uniquement au survol de souris.

## 7. Réduction des animations

- Le produit respecte la préférence système de réduction des animations (`prefers-reduced-motion`) définie dans [branding/MOTION_GUIDELINES.md](../branding/MOTION_GUIDELINES.md) §8 : les transitions, squelettes de chargement et animation orbitale d'ORION basculent vers une version statique équivalente.

## 8. Formulaires

- Chaque champ de saisie (cf. [COMPONENT_LIBRARY.md](COMPONENT_LIBRARY.md) §3) est associé programmatiquement à son libellé, jamais dépendant uniquement d'un placeholder.
- Les messages d'erreur de formulaire sont annoncés de façon explicite et associés au champ concerné, conformément à [branding/ACCESSIBILITY.md](../branding/ACCESSIBILITY.md) §6.

## 9. Principe transversal

Aucun écran documenté dans [SCREEN_STRUCTURE.md](SCREEN_STRUCTURE.md) n'est considéré comme terminé s'il ne respecte pas l'intégralité des règles de ce document et de [branding/ACCESSIBILITY.md](../branding/ACCESSIBILITY.md) : l'accessibilité est une condition de livraison, jamais une amélioration ultérieure optionnelle.
