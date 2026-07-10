# ORBITA AI — Accessibility Guidelines

L'accessibilité est un prérequis de crédibilité pour un outil de décision destiné à des dirigeants et analystes : une interface illisible ou inutilisable au clavier nuit directement au positionnement premium de la marque.

## 1. Contraste

Conformité minimale visée : **WCAG 2.1 niveau AA**, avec ambition **AAA** pour les textes critiques (valeurs de décision, alertes).

| Élément | Ratio minimum |
|---|---|
| Texte courant (< 18px, poids normal) | 4.5:1 |
| Texte large (≥ 18px, ou ≥ 14px en gras) | 3:1 |
| Composants d'interface et icônes porteuses de sens | 3:1 |
| Texte critique (KPI, alertes) — cible AAA | 7:1 |

- Toutes les combinaisons texte/fond définies dans [COLOR_SYSTEM.md](COLOR_SYSTEM.md) doivent être vérifiées avant intégration ; `text.tertiary` et `text.disabled` ne doivent jamais porter une information indispensable à la compréhension.
- La couleur ne doit jamais être le seul vecteur d'information : tout état sémantique (succès/avertissement/critique) est systématiquement doublé d'une icône et/ou d'un libellé textuel.

## 2. Lisibilité

- Interlignage minimum : 1.4× la taille de police pour tout texte courant (cf. échelle dans [TYPOGRAPHY.md](TYPOGRAPHY.md)).
- Largeur de ligne de texte confortable : 60 à 80 caractères maximum pour les paragraphes longs.
- Contraste de fond suffisant derrière tout texte superposé à une image, un graphique ou une vidéo (overlay scrim minimum 48% d'opacité en `bg.base`).
- Éviter tout texte en majuscules pour des blocs de plus d'une courte étiquette (`overline`/badge).

## 3. Tailles minimales

| Élément | Taille minimale |
|---|---|
| Texte porteur d'information | 12px |
| Cible tactile / cliquable (bouton, item de liste, case à cocher) | 40 × 40px |
| Espacement minimum entre deux cibles tactiles adjacentes | 8px |
| Icône porteuse de sens seule (sans texte) | 16px, avec zone cliquable étendue à 40px |

## 4. Navigation au clavier

- L'intégralité des fonctions de l'interface doit être accessible sans souris.
- Ordre de tabulation (`tab order`) toujours cohérent avec l'ordre de lecture visuel (gauche à droite, haut en bas).
- Les modales et menus contextuels doivent piéger le focus (focus trap) le temps de leur ouverture, et restituer le focus à l'élément déclencheur à leur fermeture.
- Raccourcis clavier standards respectés : `Échap` ferme toute modale/popover, `Entrée`/`Espace` activent un élément focalisé, flèches directionnelles pour naviguer dans les listes et menus.
- Aucun contenu ou action ne doit être accessible exclusivement au survol de souris (hover-only) sans équivalent clavier/focus.

## 5. Indicateurs de focus

- Tout élément interactif affiche un indicateur de focus visible et distinct : anneau de 2px en `accent.cyan` (`#22D3EE`) avec un décalage (offset) de 2px par rapport au composant, garantissant un contraste suffisant sur fond clair comme sur fond sombre.
- L'indicateur de focus n'est jamais supprimé (pas de `outline: none` sans remplacement explicite) — il peut être stylisé mais jamais retiré.
- Le focus doit rester visible y compris sur les composants personnalisés (cartes cliquables, lignes de tableau sélectionnables).

## 6. Autres exigences

- Toute information transmise par une animation (cf. [MOTION_GUIDELINES.md](MOTION_GUIDELINES.md)) doit avoir un équivalent statique respectant `prefers-reduced-motion`.
- Les graphiques de données doivent proposer une alternative textuelle ou tabulaire pour les lecteurs d'écran (résumé des valeurs clés, pas uniquement le rendu visuel).
- Les libellés de formulaire sont toujours associés programmatiquement à leur champ ; aucun champ ne repose uniquement sur un placeholder comme libellé.
- Les messages d'erreur sont annoncés de façon explicite (texte associé au champ), jamais uniquement par un changement de couleur de bordure.
