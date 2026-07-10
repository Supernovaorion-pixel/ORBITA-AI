# ORBITA AI — Database Strategy

> Ce document justifie et détaille la stratégie de stockage retenue pour ORBITA AI, introduite dans [TECHNOLOGY_STACK.md](TECHNOLOGY_STACK.md) §4.

## 1. Choix retenu — PostgreSQL

**Décision** : PostgreSQL est le système de gestion de base de données de référence pour l'ensemble des éditions et modes de déploiement d'ORBITA AI.

## 2. Justification

- **Pérennité** — PostgreSQL est développé et maintenu en continu depuis plus de vingt-cinq ans, sous une gouvernance communautaire indépendante de tout acteur commercial unique, éliminant le risque de dépendance à une entreprise dont la stratégie pourrait changer.
- **Performances** — moteur relationnel offrant des capacités analytiques avancées (fonctions de fenêtrage, agrégations complexes, indexation avancée), directement pertinentes pour les besoins de l'Analytics Engine et du Forecast Engine (cf. [features/ANALYTICS_ENGINE.md](../features/ANALYTICS_ENGINE.md), [features/FORECAST_ENGINE.md](../features/FORECAST_ENGINE.md)).
- **Communauté** — l'une des bases de données open source les plus largement adoptées et documentées au monde, garantissant disponibilité de compétences et de documentation à long terme.
- **Maintenance** — outillage mature de sauvegarde, de réplication et de supervision (cf. [BACKUP_AND_RECOVERY.md](BACKUP_AND_RECOVERY.md)).
- **Lisibilité** — modèle relationnel qui reflète directement les entités et relations définies dans [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md), sans traduction conceptuelle complexe entre le Domaine et son stockage.
- **Coût et licence** — distribution sous licence libre sans coût de redevance, condition indispensable à la viabilité économique des éditions Community et Starter et à la liberté de déploiement On-Premise sans dépendance à un éditeur de base de données tiers.
- **Disponibilité multiplateforme et multi-mode** — PostgreSQL s'exécute nativement sur Windows, Linux et macOS, et est disponible à la fois en service géré chez la plupart des fournisseurs Cloud et en installation autonome On-Premise, satisfaisant strictement l'exigence d'une seule stratégie de stockage pour tous les contextes de déploiement.

## 3. Modèle de données

- Le schéma relationnel reflète directement les entités et relations définies dans [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md), avec un cloisonnement strict par Organisation appliqué au niveau du schéma lui-même (chaque table portant une référence obligatoire à l'Organisation propriétaire, cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §4), et non uniquement au niveau applicatif.
- Les données historisées (cf. [architecture/DATA_FLOW.md](../architecture/DATA_FLOW.md) §7) sont conservées de façon append-only (ajout exclusif, jamais de modification destructive) pour les tables porteuses de traçabilité (Audit, Historique), garantissant l'immuabilité exigée par [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §3.

## 4. Montée en charge

- **Verticale (par défaut)** — pour la grande majorité des Organisations (Community à Business), une instance PostgreSQL correctement dimensionnée absorbe les volumes définis dans [PERFORMANCE_TARGETS.md](PERFORMANCE_TARGETS.md) §6 sans complexité supplémentaire.
- **Partitionnement** — les tables à très fort volume (transactions, historique) sont partitionnées par Organisation et par période, garantissant que les requêtes courantes (portant sur une période récente) restent rapides indépendamment de la profondeur totale de l'historique conservé (cf. [architecture/PERFORMANCE_GUIDELINES.md](../architecture/PERFORMANCE_GUIDELINES.md) §2 et §6).
- **Réplication en lecture** — pour les Organisations Enterprise à forte charge de consultation (Dashboard, Analytics), des répliques en lecture seule déchargent l'instance principale des requêtes de consultation, cette dernière restant dédiée à l'intégrité des écritures (Import, cf. [features/IMPORT_ENGINE.md](../features/IMPORT_ENGINE.md)).
- **Isolation par Organisation à grande échelle** — pour les déploiements Cloud à très large échelle, une Organisation Enterprise à très fort volume peut être hébergée sur une instance PostgreSQL dédiée plutôt que mutualisée, sans changement du modèle de données ni du code applicatif, uniquement par une décision de placement d'infrastructure.

## 5. Sauvegarde et restauration

- Détaillées dans [BACKUP_AND_RECOVERY.md](BACKUP_AND_RECOVERY.md), s'appuyant sur les mécanismes natifs de sauvegarde continue et de restauration à un instant précis (point-in-time recovery) offerts par PostgreSQL.

## 6. Évolution de schéma

- Toute évolution du schéma relationnel est appliquée par une migration versionnée, appliquée automatiquement lors d'une mise à jour de version (cf. [UPDATE_SYSTEM.md](UPDATE_SYSTEM.md)), toujours rétrocompatible le temps d'une transition (l'ancien et le nouveau schéma coexistant brièvement) pour permettre un rollback sans perte de données (cf. [UPDATE_SYSTEM.md](UPDATE_SYSTEM.md) §2).
- Aucune migration de schéma n'est appliquée sans avoir été vérifiée sur un jeu de données représentatif au préalable (cf. [TESTING_STRATEGY.md](TESTING_STRATEGY.md) §3).

## 7. Ce qui est explicitement écarté

- Toute base de données NoSQL comme stockage de référence unique : la nature fortement relationnelle du Domaine (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md)) et l'exigence d'intégrité transactionnelle (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §2) rendent un modèle relationnel plus adapté et plus sûr par défaut.
- Toute base de données propriétaire à coût de licence : incompatible avec la viabilité des éditions Community et Starter et la liberté de déploiement On-Premise (cf. [TECHNOLOGY_STACK.md](TECHNOLOGY_STACK.md) §7).
