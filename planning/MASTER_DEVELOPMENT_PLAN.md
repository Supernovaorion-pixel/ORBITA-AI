# ORBITA AI — Master Development Plan

> Ce document est la feuille de route officielle et unique du développement d'ORBITA AI. Il ne modifie aucune décision déjà prise dans [branding/](../branding/), [docs/](../docs/), [architecture/](../architecture/), [ux/](../ux/), [features/](../features/) ou [tech/](../tech/) : il en organise exclusivement la mise en œuvre dans le temps.

## 1. Objectif du plan

Découper le développement d'ORBITA AI en phases séquentielles, chacune indépendante et vérifiable, garantissant qu'aucune fondation instable ne soit jamais construite sur une base non validée.

## 2. Principe fondamental — séquentialité stricte

- Le projet est découpé en **18 phases**, détaillées dans [DEVELOPMENT_PHASES.md](DEVELOPMENT_PHASES.md).
- **Aucune phase ne peut commencer tant que la phase précédente n'a pas été formellement validée** au regard des critères définis dans [QUALITY_GATES.md](QUALITY_GATES.md).
- Chaque phase est **indépendante** : elle produit un livrable complet et vérifiable en lui-même, sans dépendre d'un développement anticipé d'une phase ultérieure.
- Cette séquentialité n'interdit pas le travail en parallèle *au sein* d'une même phase, lorsque plusieurs modules de cette phase n'ont pas de dépendance entre eux (cf. [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md)).

## 3. Structure du plan

| Document | Rôle |
|---|---|
| [DEVELOPMENT_PHASES.md](DEVELOPMENT_PHASES.md) | Définition précise des 18 phases du projet. |
| [IMPLEMENTATION_ORDER.md](IMPLEMENTATION_ORDER.md) | Ordre exact d'exécution, dépendances, prérequis, livrables et validation de chaque étape. |
| [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md) | Modules pouvant être développés en parallèle et modules devant attendre. |
| [DEVELOPMENT_RULES.md](DEVELOPMENT_RULES.md) | Règles de conduite du développement, valables à toute phase. |
| [DEFINITION_OF_DONE.md](DEFINITION_OF_DONE.md) | Définition unique et non négociable de "module terminé". |
| [QUALITY_GATES.md](QUALITY_GATES.md) | Critères obligatoires de passage d'une phase à la suivante. |
| [TEST_ACCEPTANCE.md](TEST_ACCEPTANCE.md) | Critères de validation utilisateur d'un livrable. |
| [RELEASE_MILESTONES.md](RELEASE_MILESTONES.md) | Jalons de publication (Alpha à V2). |
| [PRODUCT_OWNER_RULES.md](PRODUCT_OWNER_RULES.md) | Rôle et prérogatives officielles du Product Owner. |

## 4. Origine du plan

Ce plan n'invente aucune fonctionnalité ni aucune règle technique nouvelle : il organise chronologiquement la mise en œuvre de ce qui est déjà spécifié dans :
- [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) pour les modules à construire,
- [features/](../features/) pour leur comportement attendu,
- [tech/](../tech/) pour le socle technique à utiliser,
- [ux/](../ux/) pour l'expérience à livrer,
- [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) pour les règles de gouvernance du projet.

## 5. Portée

Ce plan couvre le développement jusqu'à la publication de la version Stable (V1, cf. [RELEASE_MILESTONES.md](RELEASE_MILESTONES.md)). Les évolutions post-V1 relèvent de [features/FUTURE_FEATURES.md](../features/FUTURE_FEATURES.md) et feront l'objet d'une planification ultérieure distincte, selon les mêmes principes de rigueur que ceux définis ici.

## 6. Autorité du document

Ce plan constitue la référence unique de séquencement du développement. Toute proposition de développement qui s'écarterait de l'ordre défini dans [IMPLEMENTATION_ORDER.md](IMPLEMENTATION_ORDER.md) doit être refusée, sauf décision explicite et documentée de révision du plan lui-même, jamais par contournement informel.
