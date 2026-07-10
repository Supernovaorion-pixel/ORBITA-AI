# ORBITA AI — Design System (UX Application)

> Ce document indique comment les fondations de marque (dossier [branding/](../branding/)) s'appliquent concrètement aux règles UX du produit. Il ne redéfinit aucune valeur déjà fixée par le branding (couleurs, typographie, iconographie, grille) — il en précise l'usage dans les patterns d'interface. Aucun code, aucun composant graphique n'est produit ici.

## 1. Principe de non-duplication

Les tokens visuels de référence (couleurs, typographie, espacements, rayons, ombres) sont définis une seule fois, dans le dossier `branding/` :

- Couleurs : [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md)
- Typographie : [branding/TYPOGRAPHY.md](../branding/TYPOGRAPHY.md)
- Icônes : [branding/ICONOGRAPHY.md](../branding/ICONOGRAPHY.md)
- Grille, espacements, rayons, ombres, composants de base : [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md)
- Animations : [branding/MOTION_GUIDELINES.md](../branding/MOTION_GUIDELINES.md)
- Accessibilité de base : [branding/ACCESSIBILITY.md](../branding/ACCESSIBILITY.md)

Ce document ne fait que préciser **comment** ces éléments s'assemblent dans des patterns d'interface complets. Toute règle UI plus fine et spécifique à un composant est détaillée dans [COMPONENT_LIBRARY.md](COMPONENT_LIBRARY.md).

## 2. Modes de densité

L'interface ORBITA AI propose deux modes de densité, appliqués de façon cohérente à tous les écrans :

- **Densité standard** — hauteur de ligne 44px (tableaux), espacement `space.lg` entre blocs. Mode par défaut, adapté à la lecture de synthèse (Dashboard, Analytics).
- **Densité compacte** — hauteur de ligne 36px, espacement `space.md`. Réservée aux vues à fort volume de données (Historique, Journal, Audit, listes de Clients/Produits).

Le choix du mode de densité n'est jamais laissé à une préférence esthétique ponctuelle : il découle de la nature de l'écran (cf. [SCREEN_STRUCTURE.md](SCREEN_STRUCTURE.md)).

## 3. Hiérarchie d'élévation appliquée aux écrans

Trois plans d'élévation structurent chaque écran, cohérents avec [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §4 :

- **Plan 0 — Fond de page** (`bg.base`) : jamais porteur d'action directe.
- **Plan 1 — Blocs de contenu** (cartes, tableaux, graphiques, `surface.default`) : unité de consultation principale.
- **Plan 2 — Éléments flottants** (menus, popovers, modales, `bg.overlay`) : toujours temporaires, jamais un mode de consultation permanent.

Un écran ne doit jamais dépasser ces trois plans simultanément visibles.

## 4. Grille d'écran officielle

- Structure standard d'un écran : **Topbar** (fixe) + **Sidebar** (fixe) + **zone de contenu** (défilante), conformément à [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §12-13.
- La zone de contenu suit la grille 12 colonnes définie dans [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §1.
- Largeur de contenu : pleine largeur pour les tableaux et graphiques ; largeur contrainte (720px) réservée aux contenus exclusivement textuels (ex. écran À propos, aide contextuelle).

## 5. Cohérence chromatique fonctionnelle

- La couleur primaire (`brand.primary`) signale exclusivement une action ou un élément actif — jamais une simple mise en avant décorative.
- Les couleurs sémantiques (succès, information, avertissement, critique — cf. [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md) §4) ont un sens unique et constant à travers tous les écrans : un même badge orange signifie toujours un avertissement, jamais autre chose selon le contexte.
- La palette graphique (cf. [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md) §10) est réservée aux séries de données ; elle n'est jamais utilisée pour des éléments d'interface statiques (boutons, menus).

## 6. Cohérence typographique fonctionnelle

- Les valeurs chiffrées (KPI, montants, pourcentages) utilisent systématiquement le style `type.data` avec chiffres tabulaires (cf. [branding/TYPOGRAPHY.md](../branding/TYPOGRAPHY.md) §6).
- Les titres d'écran utilisent systématiquement `H1` en topbar ou en tête de page ; les titres de bloc (cartes, sections) utilisent systématiquement `H3` ou `H4`, jamais l'inverse.

## 7. Principe d'unicité du composant

Un même besoin fonctionnel (ex. sélectionner une période, filtrer une liste) est toujours résolu par le même composant à travers tout le produit, tel que défini dans [COMPONENT_LIBRARY.md](COMPONENT_LIBRARY.md). Aucun écran ne doit introduire une variante locale d'un composant déjà existant pour un même usage.
