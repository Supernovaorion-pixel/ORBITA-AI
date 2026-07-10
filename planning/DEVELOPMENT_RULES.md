# ORBITA AI — Development Rules

> Ce document fixe les règles de conduite du développement, applicables à toute phase définie dans [DEVELOPMENT_PHASES.md](DEVELOPMENT_PHASES.md). Ces règles complètent, sans les remplacer, celles déjà définies dans [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md), [architecture/CODING_PRINCIPLES.md](../architecture/CODING_PRINCIPLES.md) et [tech/GIT_WORKFLOW.md](../tech/GIT_WORKFLOW.md).

## 1. Un seul module développé à la fois par contributeur

- Un contributeur ne travaille que sur un seul module à la fois, jusqu'à sa fusion complète, avant d'en commencer un autre. Le travail simultané sur plusieurs modules non liés par un même contributeur dilue l'attention et augmente le risque d'incohérence.
- Plusieurs contributeurs distincts peuvent travailler simultanément sur des modules différents, dans les limites du parallélisme autorisé par [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md).

## 2. Aucun code expérimental

- Aucun code exploratoire, provisoire ou "à corriger plus tard" n'est intégré à une branche destinée à la fusion (cf. [tech/GIT_WORKFLOW.md](../tech/GIT_WORKFLOW.md) §1).
- Toute expérimentation technique nécessaire à la prise de décision (ex. comparer deux approches) est menée en dehors du dépôt principal ou sur une branche jetable jamais destinée à fusion, et donne lieu, si elle aboutit, à une décision documentée avant implémentation définitive (cf. [architecture/ARCHITECTURE_DECISIONS.md](../architecture/ARCHITECTURE_DECISIONS.md)).

## 3. Documentation obligatoire

- Aucune fonctionnalité n'est développée sans que sa spécification existe déjà dans [features/](../features/) (cf. [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §4).
- Toute interface publique de module créée pendant le développement est documentée dans le code selon [tech/DEVELOPMENT_ENVIRONMENT.md](../tech/DEVELOPMENT_ENVIRONMENT.md) §6, avant d'être proposée à la fusion.
- Si le développement révèle une ambiguïté ou une lacune dans une spécification existante, celle-ci est corrigée dans le document source concerné avant la poursuite du développement, jamais contournée par une interprétation locale non documentée.

## 4. Tests obligatoires

- Aucun module n'est proposé à la fusion sans les tests correspondant à son niveau de criticité, conformément à [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md).
- Un changement qui corrige une anomalie est systématiquement accompagné d'un test reproduisant cette anomalie avant correction, garantissant sa non-réapparition future.

## 5. Revue obligatoire

- Toute proposition de changement est revue par au moins un autre contributeur que son auteur, sans exception, quelle que soit la taille du changement (cf. [tech/GIT_WORKFLOW.md](../tech/GIT_WORKFLOW.md) §4).
- La revue vérifie la conformité à la spécification fonctionnelle d'origine ([features/](../features/)), à l'architecture ([architecture/](../architecture/)) et aux exigences de qualité (cf. [tech/CODE_QUALITY.md](../tech/CODE_QUALITY.md)).

## 6. Validation avant fusion

- Aucune fusion n'intervient sans que l'ensemble des conditions de [tech/GIT_WORKFLOW.md](../tech/GIT_WORKFLOW.md) §4 soit satisfait.
- Un module n'est considéré comme terminé qu'au regard de la [DEFINITION_OF_DONE.md](DEFINITION_OF_DONE.md), jamais sur la seule base d'une fusion technique réussie.

## 7. Respect strict de l'ordre de développement

- Aucun contributeur ne débute le développement d'un module d'une phase ultérieure avant que la phase courante ne soit close selon [QUALITY_GATES.md](QUALITY_GATES.md), sauf exception explicitement prévue par [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md).
- Toute tentative de contourner cet ordre (développement anticipé non prévu) est refusée en revue, quelle que soit la motivation invoquée (gain de temps perçu, disponibilité momentanée d'un contributeur).

## 8. Aucune déviation non documentée

- Toute déviation par rapport à la documentation de conception existante ([branding/](../branding/), [docs/](../docs/), [architecture/](../architecture/), [ux/](../ux/), [features/](../features/), [tech/](../tech/)) constatée nécessaire en cours de développement doit être proposée comme une mise à jour explicite du document concerné, validée avant implémentation, jamais implémentée en silence en s'écartant du document existant.

## 9. Principe transversal

Ces règles s'appliquent identiquement à tout contributeur, sans exception liée à l'ancienneté, au rôle ou à l'urgence perçue d'un changement. Une exception non documentée à ces règles constitue en elle-même un défaut de qualité du projet.
