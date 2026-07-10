# ORBITA AI — Implementation Order

> Ce document précise, pour chacune des 18 phases définies dans [DEVELOPMENT_PHASES.md](DEVELOPMENT_PHASES.md), l'ordre exact d'exécution, les dépendances et prérequis, les livrables attendus et les modalités de validation.

## Format

Chaque phase est présentée selon la même structure : **Prérequis** (ce qui doit déjà être validé), **Dépendances** (modules ou décisions dont elle dépend, cf. [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md)), **Livrables** (ce qui doit exister à l'issue de la phase) et **Validation** (comment son achèvement est vérifié, cf. [QUALITY_GATES.md](QUALITY_GATES.md)).

## Phase 1 — Fondations techniques

- **Prérequis** : ensemble de la documentation de conception ([branding/](../branding/), [docs/](../docs/), [architecture/](../architecture/), [ux/](../ux/), [features/](../features/), [tech/](../tech/)) finalisée.
- **Dépendances** : aucune.
- **Livrables** : environnement de développement opérationnel, structure de projet en place, base de données installée et accessible, chaîne de construction fonctionnelle, flux Git opérationnel.
- **Validation** : un contributeur peut cloner le projet, l'installer et exécuter une construction complète sans intervention manuelle non documentée.

## Phase 2 — Core

- **Prérequis** : Phase 1 validée.
- **Dépendances** : aucune (fondation).
- **Livrables** : Domaine implémenté conformément à [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md), Security, History, Audit et Settings opérationnels.
- **Validation** : couverture de tests unitaires complète du Domaine (cf. [tech/CODE_QUALITY.md](../tech/CODE_QUALITY.md) §5), cloisonnement par Organisation vérifié par test d'intégration.

## Phase 3 — Import Engine

- **Prérequis** : Phase 2 validée.
- **Dépendances** : Core.
- **Livrables** : ensemble du comportement défini dans [features/IMPORT_ENGINE.md](../features/IMPORT_ENGINE.md).
- **Validation** : tests fonctionnels couvrant l'intégralité des scénarios d'import définis (incrémental, complet, fusion, remplacement, erreurs).

## Phase 4 — Analytics Engine

- **Prérequis** : Phase 3 validée.
- **Dépendances** : Import Engine, History.
- **Livrables** : ensemble des analyses définies dans [features/ANALYTICS_ENGINE.md](../features/ANALYTICS_ENGINE.md), ainsi que Client Management, Product Analytics, Sales Analytics.
- **Validation** : conformité aux temps de calcul définis dans [tech/PERFORMANCE_TARGETS.md](../tech/PERFORMANCE_TARGETS.md) §3, sur données représentatives.

## Phase 5 — Dashboard

- **Prérequis** : Phase 4 validée.
- **Dépendances** : Analytics Engine (Forecast Engine non encore disponible ; les cartes KPI dépendant du Forecast, telles que Projection, sont livrées en état "à venir" et complétées en Phase 6).
- **Livrables** : ensemble du comportement défini dans [features/DASHBOARD.md](../features/DASHBOARD.md), Design System et navigation opérationnels.
- **Validation** : tests UI couvrant les parcours critiques définis dans [ux/USER_FLOWS.md](../ux/USER_FLOWS.md) §1, conformité à [ux/ACCESSIBILITY_UI.md](../ux/ACCESSIBILITY_UI.md).

## Phase 6 — Forecast

- **Prérequis** : Phase 5 validée.
- **Dépendances** : Analytics Engine.
- **Livrables** : ensemble du comportement défini dans [features/FORECAST_ENGINE.md](../features/FORECAST_ENGINE.md) ; complétude des cartes KPI du Dashboard dépendant du Forecast (Projection, Objectifs).
- **Validation** : tests fonctionnels sur les prévisions, hypothèses, simulations et scénarios.

## Phase 7 — Reporting

- **Prérequis** : Phase 6 validée.
- **Dépendances** : Analytics Engine, Forecast Engine.
- **Livrables** : ensemble du comportement défini dans [features/REPORTING_ENGINE.md](../features/REPORTING_ENGINE.md) et [features/EXPORT_ENGINE.md](../features/EXPORT_ENGINE.md).
- **Validation** : vérification de la fidélité de contenu entre formats (PDF, Excel, PowerPoint) définie dans [features/REPORTING_ENGINE.md](../features/REPORTING_ENGINE.md) §10.

## Phase 8 — ORION

- **Prérequis** : Phase 7 validée.
- **Dépendances** : Analytics Engine, Forecast Engine, History ; Alert System et Notification Center développés en parallèle (cf. [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md) §3).
- **Livrables** : ensemble du comportement défini dans [features/ORION.md](../features/ORION.md), architecture d'intégration IA opérationnelle (cf. [tech/AI_INTEGRATION.md](../tech/AI_INTEGRATION.md)).
- **Validation** : vérification que toute réponse d'ORION reste tracable à des données réelles (cf. [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md) §4), test des limites fonctionnelles (refus explicite hors périmètre).

## Phase 9 — Administration

- **Prérequis** : Phase 8 validée.
- **Dépendances** : Core (User Management), tous les modules déjà livrés (pour la supervision).
- **Livrables** : ensemble du comportement défini dans [features/ADMINISTRATION.md](../features/ADMINISTRATION.md) et [features/SEARCH_ENGINE.md](../features/SEARCH_ENGINE.md).
- **Validation** : tests de droits d'accès couvrant l'ensemble des rôles définis dans [features/USER_MANAGEMENT.md](../features/USER_MANAGEMENT.md) §4.

## Phase 10 — Licences

- **Prérequis** : Phase 9 validée.
- **Dépendances** : Core, ensemble des modules livrés (Phases 2 à 9) pour application de la matrice de disponibilité.
- **Livrables** : ensemble du comportement défini dans [features/LICENSE_MANAGEMENT.md](../features/LICENSE_MANAGEMENT.md), application intégrale de [features/FEATURE_MATRIX.md](../features/FEATURE_MATRIX.md), mise en place du rôle Product Owner (cf. [PRODUCT_OWNER_RULES.md](PRODUCT_OWNER_RULES.md)).
- **Validation** : test de chaque édition (Community à Enterprise) confirmant l'activation exacte du périmètre attendu ; vérification que le rôle Product Owner n'est jamais restreint par une licence cliente.

## Phase 11 — Plugins

- **Prérequis** : Phase 10 validée.
- **Dépendances** : Core, Licensing.
- **Livrables** : ensemble du comportement défini dans [features/PLUGIN_SYSTEM.md](../features/PLUGIN_SYSTEM.md), conforme à [tech/PLUGIN_ARCHITECTURE.md](../tech/PLUGIN_ARCHITECTURE.md).
- **Validation** : test d'installation, activation, désactivation d'un plugin de référence sans impact sur le noyau.

## Phase 12 — Connecteurs

- **Prérequis** : Phase 11 validée.
- **Dépendances** : Plugin System, Import Engine.
- **Livrables** : ensemble du comportement défini dans [features/CONNECTORS.md](../features/CONNECTORS.md).
- **Validation** : test de synchronisation ERP et CRM de référence, incrémental et sans duplication.

## Phase 13 — Optimisations

- **Prérequis** : Phase 12 validée.
- **Dépendances** : ensemble des modules livrés.
- **Livrables** : conformité intégrale à [tech/PERFORMANCE_TARGETS.md](../tech/PERFORMANCE_TARGETS.md), mécanismes de sauvegarde/restauration vérifiés (cf. [tech/BACKUP_AND_RECOVERY.md](../tech/BACKUP_AND_RECOVERY.md)).
- **Validation** : tests de performance et de charge sur volumétrie Enterprise représentative (cf. [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md) §6).

## Phase 14 — Tests globaux

- **Prérequis** : Phase 13 validée.
- **Dépendances** : ensemble des modules livrés.
- **Livrables** : exécution complète de [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md), rapport de couverture consolidé.
- **Validation** : critères définis dans [TEST_ACCEPTANCE.md](TEST_ACCEPTANCE.md) intégralement satisfaits.

## Phase 15 — Version Alpha

- **Prérequis** : Phase 14 validée.
- **Livrables** : première version complète interne (cf. [RELEASE_MILESTONES.md](RELEASE_MILESTONES.md)).
- **Validation** : critères du canal Alpha (cf. [tech/RELEASE_PROCESS.md](../tech/RELEASE_PROCESS.md) §3).

## Phase 16 — Version Beta

- **Prérequis** : Phase 15 validée.
- **Livrables** : version diffusée à des Organisations volontaires.
- **Validation** : critères du canal Beta, absence d'anomalie bloquante remontée.

## Phase 17 — Release Candidate

- **Prérequis** : Phase 16 validée.
- **Livrables** : version à périmètre gelé.
- **Validation** : critères du canal RC, absence d'anomalie bloquante ou de sécurité.

## Phase 18 — Version Stable

- **Prérequis** : Phase 17 validée.
- **Livrables** : publication V1 (cf. [RELEASE_MILESTONES.md](RELEASE_MILESTONES.md)).
- **Validation** : critères du canal Stable intégralement satisfaits.
