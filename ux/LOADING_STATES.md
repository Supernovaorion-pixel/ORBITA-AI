# ORBITA AI — Loading States

> Ce document définit les règles officielles de représentation des états de chargement. Aucune implémentation n'est décrite ; les durées et courbes d'animation de référence sont définies dans [branding/MOTION_GUIDELINES.md](../branding/MOTION_GUIDELINES.md).

## 1. Principe général

Un chargement n'est jamais un espace figé ou ambigu : l'utilisateur doit toujours percevoir que le système travaille, sans jamais douter d'un blocage. Le choix du type d'indicateur dépend de la durée attendue et de la nature du contenu chargé.

## 2. Catégories de chargement

### 2.1 Chargement instantané à court (< 1 seconde perçue)
- Aucun indicateur dédié n'est nécessaire au-delà d'une micro-transition (cf. [MICRO_INTERACTIONS.md](MICRO_INTERACTIONS.md) §5) : un changement trop appuyé pour une attente très courte créerait une impression de lenteur artificielle.

### 2.2 Chargement court (1 à 3 secondes)
- Utilise un **squelette de chargement** (skeleton) reproduisant la structure du contenu à venir (carte KPI, ligne de tableau, graphique), conformément à [branding/MOTION_GUIDELINES.md](../branding/MOTION_GUIDELINES.md) §7.
- Le squelette ne doit jamais induire en erreur sur la nature du contenu réel à venir (proportions cohérentes avec le contenu final).

### 2.3 Chargement long (> 3 secondes)
- Utilise l'indicateur orbital de statut (cf. [branding/MOTION_GUIDELINES.md](../branding/MOTION_GUIDELINES.md) §6), accompagné d'un message factuel sur la nature de l'opération en cours (ex. "Analyse en cours", "Import en cours").
- Pour les opérations dont la durée est estimable (import volumineux, génération de rapport complexe), une progression explicite (pourcentage ou étape) est affichée plutôt qu'un indicateur indéterminé seul.

### 2.4 Chargement en arrière-plan (asynchrone)
- Réservé aux opérations initiées par l'utilisateur qui ne bloquent pas la poursuite de son travail sur d'autres écrans (ex. génération d'un Export, Import volumineux).
- L'état d'avancement reste consultable depuis l'écran d'origine (Exports, Import) et signalé par une notification à son achèvement (cf. [SCREEN_STRUCTURE.md](SCREEN_STRUCTURE.md) §13).

## 3. Cohérence des données pendant le chargement

- Lorsqu'un Dashboard ou un écran d'analyse est consulté pendant qu'un Import est en cours de traitement, un indicateur explicite signale que les données affichées ne reflètent pas encore ce traitement (cf. [USER_FLOWS.md](USER_FLOWS.md) §3), plutôt que de laisser l'utilisateur supposer que la donnée est à jour.
- Un rechargement de données ne doit jamais faire disparaître brutalement le contenu déjà affiché avant que le nouveau contenu ne soit disponible : le contenu précédent reste visible, éventuellement atténué, jusqu'au remplacement effectif.

## 4. Chargement partiel

- Un écran composé de plusieurs zones indépendantes (ex. Dashboard : cartes KPI, graphiques, tableau) charge chaque zone indépendamment : une zone dont la donnée est disponible s'affiche immédiatement, sans attendre les zones plus lentes à charger.

## 5. Échec de chargement

- Un chargement qui échoue ne doit jamais rester indéfiniment dans un état de chargement : il bascule explicitement vers un état d'erreur (cf. [ERROR_STATES.md](ERROR_STATES.md)) après un délai raisonnable, avec une option de nouvelle tentative.

## 6. Principe transversal

Le chargement est conçu comme une phase transitoire brève et informative, jamais comme un état par défaut acceptable : toute optimisation de performance définie dans [architecture/PERFORMANCE_GUIDELINES.md](../architecture/PERFORMANCE_GUIDELINES.md) vise directement à réduire la fréquence et la durée des états couverts par ce document.
