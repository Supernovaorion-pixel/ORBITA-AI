# ORBITA AI — Module Architecture

> Ce document définit les modules officiels du système et la responsabilité unique de chacun. Aucune technologie ni implémentation n'est décrite.

## Principe

Chaque module ci-dessous répond à une **responsabilité unique** (Single Responsibility, cf. [CODING_PRINCIPLES.md](CODING_PRINCIPLES.md)). Un module ne doit jamais absorber une responsabilité qui appartient à un autre module de cette liste. Les dépendances autorisées entre ces modules sont définies dans [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md).

## 1. Core

**Responsabilité unique** : porter le Domaine métier commun (cf. [DOMAIN_MODEL.md](DOMAIN_MODEL.md)), les règles transversales et les mécanismes partagés (identité, événements, configuration de base) sur lesquels s'appuient tous les autres modules.

## 2. Import Engine

**Responsabilité unique** : réceptionner, contrôler et intégrer les données commerciales externes dans la source de référence de l'organisation.

## 3. Analytics Engine

**Responsabilité unique** : produire des analyses (comparaisons, décompositions, tendances) à partir des données consolidées, sans se charger ni de leur import ni de leur restitution visuelle.

## 4. Forecast Engine

**Responsabilité unique** : produire des projections et estimations de performance future à partir des données historiques et courantes.

## 5. Reporting Engine

**Responsabilité unique** : assembler les données et analyses en rapports formalisés, destinés au partage ou à l'archivage.

## 6. Dashboard

**Responsabilité unique** : organiser et restituer visuellement les indicateurs de performance pertinents pour chaque rôle utilisateur, à partir des résultats produits par les moteurs d'analyse.

## 7. ORION

**Responsabilité unique** : interpréter les données et interactions utilisateur pour fournir des réponses, synthèses et recommandations en langage naturel (cf. [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md)), en s'appuyant sur les résultats des autres moteurs sans dupliquer leur logique.

## 8. Licensing

**Responsabilité unique** : déterminer et faire respecter le périmètre fonctionnel et les volumes autorisés pour chaque organisation cliente, selon sa licence (cf. [docs/LICENSE_SYSTEM.md](../docs/LICENSE_SYSTEM.md)).

## 9. Administration

**Responsabilité unique** : permettre la gestion des utilisateurs, rôles et structure organisationnelle d'une organisation cliente.

## 10. Settings

**Responsabilité unique** : porter la configuration et les préférences propres à chaque organisation et à chaque utilisateur (objectifs, affichages, notifications).

## 11. Security

**Responsabilité unique** : garantir l'authentification, les droits d'accès et la protection des données, de façon transversale à l'ensemble du système (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md)).

## 12. Update Manager

**Responsabilité unique** : gérer la distribution, l'application et la traçabilité des mises à jour du logiciel, en environnement Cloud comme On-Premise.

## 13. Plugin System

**Responsabilité unique** : permettre l'ajout, l'activation et l'isolation de modules tiers ou additionnels (connecteurs, moteurs, rapports) sans modification du noyau (cf. [EXTENSIBILITY.md](EXTENSIBILITY.md)).

## 14. Export Engine

**Responsabilité unique** : extraire des données ou rapports de la plateforme vers des formats exploitables en dehors du système.

## 15. Notification Center

**Responsabilité unique** : détecter et acheminer les alertes et notifications vers les utilisateurs concernés, selon leur rôle et leurs préférences.

## 16. History

**Responsabilité unique** : conserver et restituer l'évolution des données dans le temps, permettant la reconstitution d'un état passé.

## 17. Audit

**Responsabilité unique** : journaliser toute action significative du système (accès, modification, administration) à des fins de traçabilité et de conformité (cf. [LOGGING_STRATEGY.md](LOGGING_STRATEGY.md)).

## Synthèse des relations fonctionnelles

| Module | S'appuie typiquement sur | Fournit à |
|---|---|---|
| Core | — | tous les modules |
| Import Engine | Core, Security | Analytics Engine, History |
| Analytics Engine | Core, Import Engine | Dashboard, Reporting Engine, ORION |
| Forecast Engine | Core, Analytics Engine | Dashboard, Reporting Engine, ORION |
| Reporting Engine | Analytics Engine, Forecast Engine | Export Engine |
| Dashboard | Analytics Engine, Forecast Engine | Utilisateur (Présentation) |
| ORION | Analytics Engine, Forecast Engine, History | Dashboard, Notification Center |
| Licensing | Core | tous les modules (vérification de périmètre) |
| Administration | Core, Security | Licensing, Settings |
| Settings | Core | Analytics Engine, Dashboard, Notification Center |
| Security | Core | tous les modules |
| Update Manager | Core | — (opère sur le système entier) |
| Plugin System | Core | Import Engine, Analytics Engine, Reporting Engine |
| Export Engine | Reporting Engine, Analytics Engine | Utilisateur (Présentation) |
| Notification Center | Analytics Engine, Forecast Engine, Settings | Utilisateur (Présentation) |
| History | Import Engine, Core | Analytics Engine, Forecast Engine, Dashboard |
| Audit | Core, Security | Administration |

Cette table est une synthèse fonctionnelle ; les règles formelles de dépendance autorisée sont définies dans [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md).
