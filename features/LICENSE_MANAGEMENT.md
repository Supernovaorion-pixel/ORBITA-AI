# ORBITA AI — Feature Specification: License Management

> Ce document spécifie le comportement fonctionnel complet de la gestion des licences, appliquant les principes définis dans [docs/LICENSE_SYSTEM.md](../docs/LICENSE_SYSTEM.md) et [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §8, couvrant l'écran Licences (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §17).

## 1. Objectif

Déterminer, appliquer et faire évoluer le périmètre d'usage contractuel de chaque Organisation cliente.

## 2. Community

- Offre d'entrée, destinée à la découverte du produit ou à un usage très restreint.
- Périmètre fonctionnel limité aux modules essentiels (Dashboard, Analytics de base), nombre d'utilisateurs et volume de données strictement plafonnés (cf. [docs/BUSINESS_MODEL.md](../docs/BUSINESS_MODEL.md) §2).
- Détail du périmètre précis par fonctionnalité : cf. [FEATURE_MATRIX.md](FEATURE_MATRIX.md).

## 3. Starter

- Offre destinée à une équipe commerciale unique, incluant les modules du quotidien (Dashboard, Analytics, Reporting de base, Alertes), avec des limites de volume plus élevées que Community.

## 4. Business

- Offre destinée aux organisations multi-équipes ou multi-régions, incluant l'ensemble des modules fonctionnels (dont Forecast et les capacités étendues d'ORION), avec des volumes élargis.

## 5. Enterprise

- Offre destinée aux grandes organisations, incluant l'ensemble complet des fonctionnalités sans limite standard, avec des exigences renforcées de sécurité, un mode de déploiement On-Premise disponible, et un accompagnement dédié (cf. [docs/BUSINESS_MODEL.md](../docs/BUSINESS_MODEL.md) §2).

## 6. Activation

- Une licence est activée pour une Organisation dès la souscription, rendant immédiatement accessible le périmètre fonctionnel et les volumes correspondants (cf. [docs/LICENSE_SYSTEM.md](../docs/LICENSE_SYSTEM.md) §3).
- L'activation d'une licence ne nécessite aucune intervention technique sur le système : elle correspond à une mise à jour de la configuration de l'Organisation.
- L'Administrateur de l'Organisation est notifié de toute activation ou changement de licence, avec un résumé du périmètre désormais accessible.

## 7. Expiration

- À l'échéance de la période contractuelle sans renouvellement, la licence entre en état d'expiration : l'accès aux fonctionnalités est suspendu selon les conditions contractuelles, sans perte des données de l'Organisation pendant la période de grâce convenue (cf. [docs/LICENSE_SYSTEM.md](../docs/LICENSE_SYSTEM.md) §3).
- Une notification est envoyée à l'Administrateur de l'Organisation à intervalles rapprochés à l'approche de l'échéance (cf. [NOTIFICATION_CENTER.md](NOTIFICATION_CENTER.md)), afin d'éviter toute expiration non anticipée.

## 8. Renouvellement

- Le renouvellement d'une licence peut être anticipé avant échéance, avec reconduction sans interruption de service.
- Un renouvellement peut s'accompagner d'un changement d'offre (montée en gamme), pris en compte immédiatement à son activation.

## 9. Suivi d'usage et dépassement

- L'écran Licences présente en continu l'usage réel de l'Organisation (utilisateurs actifs, volume de données) rapporté aux limites de la licence active.
- Un dépassement ou une approche de limite est signalé explicitement à l'Administrateur, avec un chemin de régularisation clair (montée en gamme, ajustement), jamais une interruption brutale et non anticipée du service (cf. [docs/LICENSE_SYSTEM.md](../docs/LICENSE_SYSTEM.md) §4).

## 10. Granularité fonctionnelle

- L'activation ou la restriction d'un module fonctionnel selon l'offre souscrite est appliquée de façon transversale à l'ensemble de la plateforme : un module non inclus dans l'offre n'apparaît pas dans la navigation de l'utilisateur (cf. [ux/NAVIGATION.md](../ux/NAVIGATION.md) §2), plutôt que d'être visible mais inutilisable.

## 11. Séparation entre Organisations

- Chaque licence est strictement rattachée à une seule Organisation cliente ; aucune donnée d'usage ou de configuration de licence n'est jamais visible par une autre Organisation, quel que soit son niveau de licence respectif (cf. [docs/LICENSE_SYSTEM.md](../docs/LICENSE_SYSTEM.md) §6).
