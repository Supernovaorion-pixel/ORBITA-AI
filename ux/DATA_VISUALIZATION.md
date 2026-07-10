# ORBITA AI — Data Visualization

> Ce document définit les règles officielles de choix et de construction des graphiques du produit. Aucune implémentation, aucune bibliothèque graphique n'est prescrite — uniquement les règles d'usage. Les fondations visuelles communes (couleurs, épaisseurs de trait, grille de fond) sont définies dans [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §8 et [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md) §10.

## 1. Principe de choix d'un graphique

Un graphique n'est jamais choisi pour son esthétique : il est choisi parce qu'il est la représentation la plus directe de la question posée par l'utilisateur. Un mauvais choix de graphique (par exemple un Donut pour une série temporelle) est considéré comme une erreur de conception, pas une variante acceptable.

## 2. Règles par type de graphique

### Bar (barres)
- **Usage** : comparaison de valeurs entre catégories discrètes (ex. CA par territoire, par commercial).
- **Règle** : barres toujours triées par valeur décroissante sauf lorsque l'ordre catégoriel a un sens propre (ex. mois de l'année) ; jamais plus de 12 catégories affichées simultanément sans regroupement "Autres".

### Line (ligne)
- **Usage** : évolution d'un indicateur dans le temps (tendance, historique, projection).
- **Règle** : axe temporel toujours en abscisse ; jamais plus de 5 séries simultanées sur un même graphique en ligne, au-delà la lisibilité devient insuffisante et un filtrage ou une décomposition est nécessaire.

### Area (aire)
- **Usage** : évolution dans le temps avec emphase sur le volume cumulé ou la magnitude (ex. CA cumulé sur la période).
- **Règle** : réservée à une série unique ou à des séries empilées (stacked) représentant une décomposition d'un tout ; jamais utilisée pour comparer des séries indépendantes qui se chevauchent visuellement.

### Pie (camembert)
- **Usage** : répartition d'un tout en un nombre très restreint de parts (2 à 4 maximum).
- **Règle** : jamais utilisé au-delà de 4 catégories ; au-delà, un graphique en barres est systématiquement préféré, plus lisible pour la comparaison.

### Donut (anneau)
- **Usage** : identique au Pie, avec une valeur centrale mise en avant (ex. total ou indicateur clé au centre de l'anneau).
- **Règle** : mêmes limites que le Pie (2 à 4 catégories) ; le centre affiche toujours une valeur agrégée pertinente, jamais laissé vide par défaut.

### Pareto
- **Usage** : identification des contributeurs principaux à un phénomène (ex. quels clients ou produits concentrent l'essentiel du CA), combinant barres triées et courbe cumulative.
- **Règle** : toujours trié par ordre décroissant de contribution ; la courbe cumulative est systématiquement affichée en complément des barres, jamais l'une sans l'autre.

### Heatmap (carte de chaleur)
- **Usage** : visualisation d'une intensité croisant deux dimensions catégorielles (ex. performance par territoire et par mois).
- **Règle** : l'échelle de couleur utilise exclusivement un dégradé séquentiel à deux teintes de la palette de marque (ex. de `bg.raised` vers `brand.primary`), jamais un dégradé multicolore de type arc-en-ciel ; une légende d'échelle est toujours visible.

### Treemap
- **Usage** : représentation de la part relative d'un ensemble hiérarchique (ex. répartition du CA par Famille puis par Produit).
- **Règle** : deux niveaux de hiérarchie maximum affichés simultanément ; au-delà, un second niveau s'ouvre par interaction (clic) plutôt que d'être empilé visuellement.

### Waterfall (cascade)
- **Usage** : décomposition d'une variation en ses facteurs contributifs (ex. passage du CA d'une période à l'autre, décomposé par effet volume, prix, mix produit).
- **Règle** : les contributions positives et négatives utilisent systématiquement les couleurs sémantiques de succès et de critique (cf. [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md) §4) ; le total de départ et d'arrivée est toujours visuellement distinct des contributions intermédiaires.

### Scatter (nuage de points)
- **Usage** : mise en évidence d'une corrélation entre deux indicateurs (ex. fréquence d'achat et panier moyen par client).
- **Règle** : chaque point reste identifiable individuellement au survol/focus (cf. tooltip, [COMPONENT_LIBRARY.md](COMPONENT_LIBRARY.md) §6) ; au-delà d'un volume de points rendant la lecture individuelle impossible, un regroupement visuel (densité) est utilisé plutôt qu'une accumulation illisible de points.

### Gauge (jauge)
- **Usage** : représentation d'un taux d'atteinte par rapport à un objectif unique (ex. carte KPI "Objectifs").
- **Règle** : toujours accompagnée de la valeur numérique exacte, jamais laissée comme seule représentation visuelle ; les seuils de couleur (proche, atteint, dépassé) suivent les couleurs sémantiques standards.

## 3. Règles transversales à tous les graphiques

- **Couleur** : toujours issue de la palette graphique officielle (cf. [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md) §10), dans l'ordre défini, jamais une couleur choisie ponctuellement.
- **Axes et grille** : discrets, jamais plus visibles que les données elles-mêmes (cf. [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §8).
- **Absence de volume** : tout graphique doit prévoir un état vide explicite lorsque la donnée n'est pas disponible (cf. [EMPTY_STATES.md](EMPTY_STATES.md)), jamais un graphique vide sans explication.
- **Accessibilité** : chaque graphique est doublé d'un accès à la donnée sous forme de tableau (cf. [ACCESSIBILITY_UI.md](ACCESSIBILITY_UI.md) §6).
- **Aucun effet visuel superflu** : pas de volume 3D, pas d'ombre portée sur les éléments de graphique, pas d'animation d'entrée superflue au-delà de ce que définit [branding/MOTION_GUIDELINES.md](../branding/MOTION_GUIDELINES.md).

## 4. Correspondance avec les écrans

Le choix par défaut du graphique pour chaque écran est précisé dans [SCREEN_STRUCTURE.md](SCREEN_STRUCTURE.md) et [DASHBOARD_SPECIFICATION.md](DASHBOARD_SPECIFICATION.md) ; ce document définit la règle de choix, ces derniers en précisent l'application concrète par contexte.
