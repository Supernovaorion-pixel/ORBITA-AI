# ORBITA AI — Development Phases

> Ce document définit précisément les 18 phases officielles du développement d'ORBITA AI. Chaque phase est indépendante et ne peut débuter tant que la phase précédente n'est pas validée (cf. [MASTER_DEVELOPMENT_PLAN.md](MASTER_DEVELOPMENT_PLAN.md) §2, [QUALITY_GATES.md](QUALITY_GATES.md)).

## Phase 1 — Fondations techniques

**Objectif** : mettre en place l'environnement et le socle technique commun à tout développement ultérieur.
**Contenu** : environnement de développement (cf. [tech/DEVELOPMENT_ENVIRONMENT.md](../tech/DEVELOPMENT_ENVIRONMENT.md)), structure du projet (cf. [tech/PROJECT_STRUCTURE.md](../tech/PROJECT_STRUCTURE.md)), mise en place de la base de données (cf. [tech/DATABASE_STRATEGY.md](../tech/DATABASE_STRATEGY.md)), chaîne de construction et de packaging (cf. [tech/BUILD_AND_PACKAGING.md](../tech/BUILD_AND_PACKAGING.md)), gestion des secrets (cf. [tech/SECRET_MANAGEMENT.md](../tech/SECRET_MANAGEMENT.md)), flux Git (cf. [tech/GIT_WORKFLOW.md](../tech/GIT_WORKFLOW.md)).
**Aucun module fonctionnel n'est développé durant cette phase.**

## Phase 2 — Core

**Objectif** : construire le socle applicatif commun.
**Contenu** : Domaine (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md)), module Security (authentification, autorisation, cf. [tech/SECURITY_REQUIREMENTS.md](../tech/SECURITY_REQUIREMENTS.md)), système d'événements (cf. [architecture/EVENT_SYSTEM.md](../architecture/EVENT_SYSTEM.md)), module History et Audit (cf. [features/AUDIT_AND_HISTORY.md](../features/AUDIT_AND_HISTORY.md)), module Settings de base (cf. [features/SETTINGS.md](../features/SETTINGS.md)), module User Management (cf. [features/USER_MANAGEMENT.md](../features/USER_MANAGEMENT.md)).

## Phase 3 — Import Engine

**Objectif** : rendre la plateforme capable d'intégrer des données commerciales.
**Contenu** : ensemble des comportements définis dans [features/IMPORT_ENGINE.md](../features/IMPORT_ENGINE.md) (formats, détection, mapping, validation, prévisualisation, fusion, remplacement, rapport d'import).

## Phase 4 — Analytics Engine

**Objectif** : rendre la donnée importée exploitable analytiquement.
**Contenu** : ensemble des analyses définies dans [features/ANALYTICS_ENGINE.md](../features/ANALYTICS_ENGINE.md), ainsi que les modules dérivés Client Management, Product Analytics et Sales Analytics (cf. [features/CLIENT_MANAGEMENT.md](../features/CLIENT_MANAGEMENT.md), [features/PRODUCT_ANALYTICS.md](../features/PRODUCT_ANALYTICS.md), [features/SALES_ANALYTICS.md](../features/SALES_ANALYTICS.md)).

## Phase 5 — Dashboard

**Objectif** : restituer visuellement les analyses produites.
**Contenu** : ensemble du comportement défini dans [features/DASHBOARD.md](../features/DASHBOARD.md), conforme à [ux/DASHBOARD_SPECIFICATION.md](../ux/DASHBOARD_SPECIFICATION.md) ; mise en place de la couche Présentation et du Design System (cf. [ux/DESIGN_SYSTEM.md](../ux/DESIGN_SYSTEM.md), [ux/COMPONENT_LIBRARY.md](../ux/COMPONENT_LIBRARY.md)) et de la navigation (cf. [ux/NAVIGATION.md](../ux/NAVIGATION.md)).

## Phase 6 — Forecast

**Objectif** : ajouter la capacité de projection.
**Contenu** : ensemble du comportement défini dans [features/FORECAST_ENGINE.md](../features/FORECAST_ENGINE.md) (prévisions, hypothèses, simulations, objectifs, scénarios).

## Phase 7 — Reporting

**Objectif** : permettre la restitution formalisée et l'export.
**Contenu** : ensemble du comportement défini dans [features/REPORTING_ENGINE.md](../features/REPORTING_ENGINE.md) et [features/EXPORT_ENGINE.md](../features/EXPORT_ENGINE.md).

## Phase 8 — ORION

**Objectif** : intégrer l'assistant intelligent transversal.
**Contenu** : ensemble du comportement défini dans [features/ORION.md](../features/ORION.md), s'appuyant sur l'architecture d'intégration définie dans [tech/AI_INTEGRATION.md](../tech/AI_INTEGRATION.md) ; intégration du système d'Alertes (cf. [features/ALERT_SYSTEM.md](../features/ALERT_SYSTEM.md)) et du Notification Center (cf. [features/NOTIFICATION_CENTER.md](../features/NOTIFICATION_CENTER.md)), tous deux consommés par ORION.

## Phase 9 — Administration

**Objectif** : permettre la gestion autonome d'une Organisation cliente.
**Contenu** : ensemble du comportement défini dans [features/ADMINISTRATION.md](../features/ADMINISTRATION.md), incluant la Recherche globale (cf. [features/SEARCH_ENGINE.md](../features/SEARCH_ENGINE.md)).

## Phase 10 — Licences

**Objectif** : rendre le système commercialisable selon les quatre éditions officielles.
**Contenu** : ensemble du comportement défini dans [features/LICENSE_MANAGEMENT.md](../features/LICENSE_MANAGEMENT.md), application de la [features/FEATURE_MATRIX.md](../features/FEATURE_MATRIX.md) à l'ensemble des modules déjà développés (Phases 2 à 9), mise en place du rôle Product Owner (cf. [PRODUCT_OWNER_RULES.md](PRODUCT_OWNER_RULES.md)).

## Phase 11 — Plugins

**Objectif** : rendre le système extensible.
**Contenu** : ensemble du comportement défini dans [features/PLUGIN_SYSTEM.md](../features/PLUGIN_SYSTEM.md), implémenté selon [tech/PLUGIN_ARCHITECTURE.md](../tech/PLUGIN_ARCHITECTURE.md).

## Phase 12 — Connecteurs

**Objectif** : permettre l'alimentation automatisée depuis des systèmes externes.
**Contenu** : ensemble du comportement défini dans [features/CONNECTORS.md](../features/CONNECTORS.md), construits comme des extensions du Plugin System (Phase 11).

## Phase 13 — Optimisations

**Objectif** : garantir que le système respecte les objectifs chiffrés définis dans [tech/PERFORMANCE_TARGETS.md](../tech/PERFORMANCE_TARGETS.md) à pleine échelle.
**Contenu** : revue de performance transversale à l'ensemble des modules déjà développés, mise en place de la montée en charge définie dans [tech/DEPLOYMENT_STRATEGY.md](../tech/DEPLOYMENT_STRATEGY.md), vérification des mécanismes de sauvegarde et de restauration (cf. [tech/BACKUP_AND_RECOVERY.md](../tech/BACKUP_AND_RECOVERY.md)).

## Phase 14 — Tests globaux

**Objectif** : vérifier l'ensemble du système de bout en bout.
**Contenu** : exécution complète de la stratégie définie dans [tech/TESTING_STRATEGY.md](../tech/TESTING_STRATEGY.md) sur le système assemblé dans son intégralité, incluant les parcours définis dans [ux/USER_FLOWS.md](../ux/USER_FLOWS.md) et les scénarios d'acceptation définis dans [TEST_ACCEPTANCE.md](TEST_ACCEPTANCE.md).

## Phase 15 — Version Alpha

**Objectif** : première version complète, vérifiée en interne exclusivement.
**Contenu** : conforme au canal Alpha défini dans [tech/RELEASE_PROCESS.md](../tech/RELEASE_PROCESS.md) et [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md) §3. Aucune diffusion externe.

## Phase 16 — Version Beta

**Objectif** : vérification élargie auprès d'Organisations clientes volontaires.
**Contenu** : conforme au canal Beta ; collecte structurée des anomalies remontées, cf. [RELEASE_MILESTONES.md](RELEASE_MILESTONES.md).

## Phase 17 — Release Candidate

**Objectif** : dernière vérification avant publication générale, périmètre gelé.
**Contenu** : conforme au canal RC ; seules les corrections d'anomalies bloquantes sont admises, aucune nouvelle fonctionnalité.

## Phase 18 — Version Stable

**Objectif** : publication en disponibilité générale (V1).
**Contenu** : conforme au canal Stable ; jalon détaillé dans [RELEASE_MILESTONES.md](RELEASE_MILESTONES.md).

## Principe transversal

Chaque phase produit un livrable conforme à la [DEFINITION_OF_DONE.md](DEFINITION_OF_DONE.md) avant d'être soumise aux [QUALITY_GATES.md](QUALITY_GATES.md) qui conditionnent l'ouverture de la phase suivante.
