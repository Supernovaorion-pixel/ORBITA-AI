# ORBITA AI — AI Integration

> Ce document définit l'architecture technique d'intégration des capacités d'intelligence artificielle mobilisées par ORION (cf. [features/ORION.md](../features/ORION.md), [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md)). Conformément à l'exigence de la mission, ORBITA AI ne dépend d'aucun fournisseur unique.

## 1. Principe directeur

L'intelligence artificielle est traitée comme une **capacité externe interchangeable**, jamais comme une dépendance structurante du Domaine. ORION (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §7) ignore, dans son fonctionnement, quel fournisseur traite une requête donnée : il ne connaît qu'une interface d'abstraction stable.

## 2. Architecture d'intégration

- Une **couche d'abstraction** définit un contrat unique (interrogation en langage naturel, production de synthèse, détection de signal) auquel tout fournisseur d'intelligence artificielle doit se conformer pour être intégré.
- Chaque fournisseur est intégré via un **adaptateur** dédié, traduisant le contrat d'abstraction vers l'interface propre de ce fournisseur, selon le même principe d'extension que les Connecteurs ERP/CRM (cf. [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md) §6, [features/CONNECTORS.md](../features/CONNECTORS.md)).
- Le module ORION consomme exclusivement la couche d'abstraction, jamais directement l'interface propre d'un fournisseur : un changement ou un ajout de fournisseur ne modifie jamais la logique métier d'ORION.

## 3. Compatibilité multi-modèles

- Plusieurs fournisseurs ou modèles peuvent être actifs simultanément pour une même Organisation, chacun pouvant être sollicité selon la nature de la tâche (ex. un modèle pour la synthèse rapide, un autre pour l'analyse approfondie), sans que cette répartition ne soit visible de l'utilisateur final au-delà du résultat produit.
- Le choix du ou des modèles actifs est un paramètre de configuration (cf. [CONFIGURATION_MANAGEMENT.md](CONFIGURATION_MANAGEMENT.md) §3), ajustable sans modification du code applicatif.
- Un nouveau fournisseur ou modèle est intégré par l'ajout d'un nouvel adaptateur (§2), sans modification de la couche d'abstraction ni du module ORION, conformément au principe d'extensibilité par inversion défini dans [architecture/ARCHITECTURE_DECISIONS.md](../architecture/ARCHITECTURE_DECISIONS.md) (ADR-005).

## 4. Gestion des fournisseurs

- Chaque adaptateur documente explicitement ses capacités et ses limites propres (longueur de contexte, langues supportées), permettant à la couche d'abstraction d'orienter une requête vers le fournisseur le plus adapté lorsque plusieurs sont actifs.
- Un fournisseur devenu indisponible (incident technique, fin de contrat) est retiré ou remplacé par simple désactivation de son adaptateur, sans interruption du contrat d'abstraction ni du fonctionnement des autres capacités d'ORION non concernées.
- Aucune règle métier ou recommandation définie dans [features/ORION.md](../features/ORION.md) ne dépend de la formulation propre à un fournisseur donné : le comportement fonctionnel attendu d'ORION reste strictement identique quel que soit le fournisseur mobilisé en arrière-plan.

## 5. Sécurité

- Toute donnée transmise à un fournisseur externe respecte le périmètre de droits de l'utilisateur à l'origine de la requête (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §1) : aucune donnée hors de ce périmètre n'est jamais transmise.
- Les identifiants d'accès à chaque fournisseur sont gérés selon les principes définis dans [SECRET_MANAGEMENT.md](SECRET_MANAGEMENT.md), jamais partagés entre Organisations.
- Une Organisation cliente peut restreindre ou interdire l'usage de fournisseurs externes pour ses données (exigence de gouvernance renforcée, notamment en mode On-Premise, cf. [docs/BUSINESS_MODEL.md](../docs/BUSINESS_MODEL.md) §5), auquel cas seule une capacité d'intelligence artificielle exécutée localement à son infrastructure, si disponible, est mobilisée.
- Toute transmission de donnée à un fournisseur externe est journalisée conformément à [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §3, garantissant la traçabilité exigée par [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md).

## 6. Cohérence avec les limites fonctionnelles d'ORION

Cette architecture garantit techniquement les limites fonctionnelles définies dans [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md) §4 : ORION ne peut mobiliser que les données du périmètre autorisé, ne dépend d'aucun fournisseur unique dont la disparition compromettrait le produit, et son comportement reste stable et prévisible indépendamment de l'évolution du marché des fournisseurs d'intelligence artificielle sur la durée de vie du produit.
