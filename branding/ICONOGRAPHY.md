# ORBITA AI — Iconography

## 1. Style officiel

Les icônes ORBITA AI suivent un style **linéaire (outline), fin et géométrique**, cohérent avec le symbole orbital du logo.

Caractéristiques obligatoires :
- **Trait fin uniforme** : épaisseur de trait constante sur toute icône (1.5px sur une grille de 24px de référence).
- **Angles nets et courbes régulières** : les courbes suivent des rayons standardisés (pas de courbes organiques irrégulières).
- **Style "outline"** par défaut ; un remplissage (fill) plein n'est utilisé que pour signaler un état actif/sélectionné.
- **Terminaisons de trait arrondies (round cap/join)** pour une lecture douce malgré la finesse du trait — cohérent avec le positionnement premium (jamais des angles agressifs façon interface militaire ou gaming).

## 2. Ce qui est interdit

- Aucune icône **cartoon** ou stylisée de façon ludique.
- Aucune icône **3D**, avec ombre portée, dégradé de volume ou effet skeuomorphe.
- Aucune icône **multicolore décorative** — une icône reste monochrome (couleur héritée du texte/état environnant), sauf pictogrammes de statut (succès/avertissement/critique) qui utilisent la couleur sémantique correspondante et uniquement celle-ci.
- Aucun style mixte (ne jamais combiner des icônes outline et des icônes filled pleines de fournisseurs différents dans une même vue).

## 3. Grille et construction

- Grille de conception : **24 × 24px**, avec zone de sécurité interne de 2px sur chaque bord (zone active 20×20px).
- Épaisseur de trait : **1.5px** à taille 24px ; **1.25px** à taille 16px (jamais en dessous, pour rester perceptible à l'écran).
- Tailles d'usage standard : 16px (contexte dense : tableaux, champs), 20px (boutons, menus), 24px (navigation principale, en-têtes), 32px (mise en avant, écrans vides / empty states).
- Toutes les icônes d'un même ensemble doivent être alignées optiquement (et non mathématiquement) sur la même hauteur visuelle.

## 4. Cohérence avec le symbole de marque

- Le rayon de courbure utilisé dans les icônes doit reprendre celui du symbole orbital du logo (cf. [LOGO_GUIDELINES.md](LOGO_GUIDELINES.md)), pour une cohérence géométrique perceptible entre la marque et l'interface.
- Les icônes propres à l'assistant ORION (état actif, réflexion, suggestion) doivent utiliser une variante animée sobre définie dans [MOTION_GUIDELINES.md](MOTION_GUIDELINES.md), jamais un pictogramme figuratif de robot ou de visage.

## 5. Couleur des icônes

| Contexte | Couleur |
|---|---|
| Icône par défaut (inactive) | `text.secondary` (`#A6ADC2` en mode sombre) |
| Icône active / sélectionnée | `brand.primary` (`#4C6FFF`) |
| Icône sur bouton primaire | `text.inverse` (`#FFFFFF`) |
| Icône de statut succès | `semantic.success` (`#1FD68F`) |
| Icône de statut avertissement | `semantic.warning` (`#FFB020`) |
| Icône de statut critique | `semantic.critical` (`#FF4D6A`) |
| Icône désactivée | `text.disabled` (`#454B63`) |

Référence complète des couleurs : [COLOR_SYSTEM.md](COLOR_SYSTEM.md).

## 6. États

- **Par défaut** : trait fin, couleur neutre.
- **Hover** : passage à `brand.primary`, sans changement d'épaisseur ni de taille.
- **Actif/sélectionné** : version pleine (fill) autorisée uniquement ici, en `brand.primary`.
- **Désactivé** : couleur `text.disabled`, opacité inchangée (ne pas utiliser l'opacité comme seul indicateur d'état, pour l'accessibilité).

## 7. Cohérence produit

- Utiliser un unique set d'icônes cohérent dans toute l'application (une seule bibliothèque source, pas de mélange entre fournisseurs d'icônes).
- Toute icône métier spécifique à ORBITA AI (ex. symboles d'analyse commerciale, de territoire, de pipeline) doit être redessinée pour respecter strictement la grille et l'épaisseur de trait définies ci-dessus avant intégration.
