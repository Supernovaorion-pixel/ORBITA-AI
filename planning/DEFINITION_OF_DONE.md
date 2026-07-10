# ORBITA AI — Definition of Done

> Ce document définit précisément et de façon non négociable ce que signifie **"module terminé"** pour tout module développé dans le cadre de ce plan. Aucun module n'est considéré comme terminé si l'une des conditions suivantes n'est pas satisfaite.

## 1. Code

- Le code du module respecte intégralement [tech/CODE_QUALITY.md](../tech/CODE_QUALITY.md) : style conforme, complexité et duplication sous les seuils définis.
- Le code respecte les règles de dépendance définies dans [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md) : aucune dépendance interdite, aucune dépendance circulaire.
- Le code implémente l'intégralité du comportement décrit dans le document [features/](../features/) correspondant, sans omission ni ajout non spécifié.
- Le code respecte les conventions de nommage définies dans [architecture/NAMING_CONVENTIONS.md](../architecture/NAMING_CONVENTIONS.md).

## 2. Documentation

- Toute interface publique du module est documentée conformément à [tech/DEVELOPMENT_ENVIRONMENT.md](../tech/DEVELOPMENT_ENVIRONMENT.md) §6.
- Toute règle métier non évidente est documentée à l'endroit où elle est implémentée (cf. [architecture/CODING_PRINCIPLES.md](../architecture/CODING_PRINCIPLES.md) §8).
- Si le développement a révélé une clarification nécessaire d'un document de conception existant, cette clarification a été intégrée au document source concerné.

## 3. Tests

- L'ensemble des règles métier du module est couvert par des tests unitaires (cf. [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md) §2), atteignant le seuil de couverture défini dans [tech/CODE_QUALITY.md](../tech/CODE_QUALITY.md) §5.
- Les interactions du module avec l'infrastructure (base de données) et avec les autres modules dont il dépend sont couvertes par des tests d'intégration (cf. [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md) §3).
- Chaque comportement métier significatif décrit dans la spécification fonctionnelle du module est couvert par au moins un test fonctionnel (cf. [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md) §4).
- Si le module comporte une interface utilisateur, les parcours critiques associés (cf. [ux/USER_FLOWS.md](../ux/USER_FLOWS.md)) sont couverts par des tests UI (cf. [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md) §5).
- L'ensemble des tests de non-régression existants continue de réussir après intégration du module (cf. [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md) §7).

## 4. Performances

- Le module respecte les cibles chiffrées qui le concernent dans [tech/PERFORMANCE_TARGETS.md](../tech/PERFORMANCE_TARGETS.md), vérifiées sur un jeu de données représentatif d'une Organisation Enterprise à pleine échelle.
- Aucune régression de performance n'est constatée sur les fonctionnalités déjà livrées par les phases précédentes du fait de l'introduction de ce module.

## 5. Qualité

- Le module a passé l'analyse statique et le formatage automatisés sans avertissement non résolu (cf. [tech/DEVELOPMENT_ENVIRONMENT.md](../tech/DEVELOPMENT_ENVIRONMENT.md) §4-5).
- Le module respecte l'ensemble des exigences de sécurité qui le concernent dans [tech/SECURITY_REQUIREMENTS.md](../tech/SECURITY_REQUIREMENTS.md).
- Si le module comporte une interface utilisateur, il respecte intégralement [ux/ACCESSIBILITY_UI.md](../ux/ACCESSIBILITY_UI.md) et le [ux/COMPONENT_LIBRARY.md](../ux/COMPONENT_LIBRARY.md).

## 6. Validation

- Le module a fait l'objet d'une revue de code approuvée par au moins un contributeur distinct de son auteur (cf. [DEVELOPMENT_RULES.md](DEVELOPMENT_RULES.md) §5).
- Le module a satisfait les critères d'acceptation définis dans [TEST_ACCEPTANCE.md](TEST_ACCEPTANCE.md) pour son périmètre.
- Le module a été fusionné dans la branche principale selon les conditions définies dans [tech/GIT_WORKFLOW.md](../tech/GIT_WORKFLOW.md) §4.

## 7. Principe transversal

Un module qui satisfait cinq des six catégories ci-dessus mais pas la sixième n'est **pas terminé**. Il n'existe aucune notion de "presque terminé" dans ce projet : un module est soit conforme à l'intégralité de cette définition, soit en cours de développement. Cette définition unique s'applique identiquement à tout module, quelle que soit la phase à laquelle il appartient.
