# ORBITA AI — Responsive Rules

> Ce document définit le comportement du produit selon la taille d'écran. ORBITA AI est un outil de décision professionnel avant tout utilisé sur poste de travail ; ces règles garantissent une expérience cohérente à travers les formats sans jamais sacrifier la densité d'information nécessaire à l'analyse.

## 1. Principe général

- L'expérience de référence est **desktop / laptop** : c'est le format sur lequel la totalité des fonctionnalités et de la densité analytique est disponible.
- Les formats plus petits (tablette) adaptent la disposition sans jamais retirer de fonctionnalité, uniquement en réorganisant ou en permettant un défilement supplémentaire.
- Aucune fonctionnalité n'est exclusive à un format : un utilisateur en tablette doit pouvoir accomplir la même tâche qu'un utilisateur en desktop, avec une disposition adaptée.

## 2. Paliers officiels

| Palier | Largeur de référence | Comportement |
|---|---|---|
| **Écran ultra-large** | ≥ 1920px | Grille 12 colonnes pleinement exploitée ; les bandes de Dashboard peuvent afficher davantage de graphiques secondaires côte à côte sans réduire leur taille de lecture. |
| **Desktop** | 1280–1919px | Format de référence : disposition standard décrite dans [DASHBOARD_SPECIFICATION.md](DASHBOARD_SPECIFICATION.md) et [SCREEN_STRUCTURE.md](SCREEN_STRUCTURE.md). |
| **Laptop** | 1024–1279px | Identique au Desktop, avec réduction de l'espacement (`space.lg` → `space.md`) et passage automatique de la Sidebar en mode réduit (icônes seules, cf. [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §12) si l'espace de contenu devient contraint. |
| **Tablette** | 768–1023px | Sidebar repliée par défaut (accessible par bouton d'ouverture) ; les bandes de graphiques secondaires du Dashboard passent d'une disposition côte à côte à un empilement vertical ; les tableaux larges deviennent défilables horizontalement dans leur propre conteneur, jamais le corps entier de la page. |

## 3. Règles par palier

### Écran ultra-large
- La largeur additionnelle ne doit jamais être utilisée pour agrandir démesurément les composants (texte, cartes) au-delà des tailles définies dans [branding/TYPOGRAPHY.md](../branding/TYPOGRAPHY.md) : l'espace supplémentaire sert à afficher davantage d'éléments côte à côte, jamais à les dilater.
- Une largeur maximale de contenu peut s'appliquer aux vues exclusivement textuelles (À propos, Support) pour préserver la lisibilité (cf. [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §1).

### Desktop / Laptop
- Disposition de référence de tous les écrans documentés dans [SCREEN_STRUCTURE.md](SCREEN_STRUCTURE.md).
- Le passage de Desktop à Laptop ne doit jamais faire disparaître d'information, uniquement la rendre plus compacte (cf. mode de densité, [DESIGN_SYSTEM.md](DESIGN_SYSTEM.md) §2).

### Tablette
- Priorité à la consultation : les écrans de gestion nécessitant une saisie complexe (Administration, Connecteurs, configuration de Plugins) restent accessibles, mais leur usage principal en tablette est la consultation et l'approbation rapide, non la configuration approfondie.
- Les graphiques (cf. [DATA_VISUALIZATION.md](DATA_VISUALIZATION.md)) conservent leur légende et leurs axes lisibles ; un graphique dont la lisibilité ne peut être garantie à cette largeur est remplacé par sa vue tabulaire équivalente.
- Le panneau ORION s'ouvre en plein écran temporaire plutôt qu'en panneau latéral partiel, pour préserver un espace de lecture suffisant.

## 4. Ce qui ne change jamais entre paliers

- La hiérarchie de priorité de l'information (cf. [DASHBOARD_SPECIFICATION.md](DASHBOARD_SPECIFICATION.md) §7) reste strictement identique.
- Le vocabulaire, les couleurs sémantiques et les libellés restent identiques (cf. [DESIGN_SYSTEM.md](DESIGN_SYSTEM.md) §5-6).
- Les raccourcis clavier (cf. [NAVIGATION.md](NAVIGATION.md) §5) restent disponibles sur tout palier où une saisie clavier est possible.

## 5. Principe d'adaptation, jamais de duplication

L'adaptation à un palier est toujours une réorganisation de la même information et des mêmes composants (cf. [COMPONENT_LIBRARY.md](COMPONENT_LIBRARY.md)), jamais la création d'un écran ou d'un composant distinct propre à un format. Un composant qui nécessiterait une version fondamentalement différente selon le palier doit être reconsidéré dans sa conception d'origine.
