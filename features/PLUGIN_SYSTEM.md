# ORBITA AI — Feature Specification: Plugin System

> Ce document spécifie le comportement fonctionnel complet du Plugin System (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §13 et [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md)), couvrant l'écran Plugins (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §18).

## 1. Objectif

Permettre l'ajout de capacités additionnelles à la plateforme (connecteurs, moteurs, rapports, dashboards) sans modification du noyau du logiciel.

## 2. Extensions

- Une extension (plugin) est une capacité additionnelle rattachée à un point d'extension précis d'un module natif (cf. [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md) §2-3).
- Chaque plugin est présenté avec son origine, sa version, sa description fonctionnelle et le module natif auquel il se rattache.
- Un plugin est soumis aux mêmes règles de cloisonnement par Organisation et de sécurité que le reste de la plateforme (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md)).

## 3. Catalogue

- L'écran Plugins présente le catalogue des extensions disponibles pour l'Organisation, selon son offre de licence (cf. [LICENSE_MANAGEMENT.md](LICENSE_MANAGEMENT.md)).
- Le catalogue distingue les plugins déjà activés des plugins disponibles mais non encore activés (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §18).

## 4. Installation

- L'installation d'un plugin est déclenchée par un Administrateur habilité, sans nécessiter d'intervention technique en dehors de la plateforme.
- Avant installation, le périmètre exact d'accès requis par le plugin (quelles données il consultera, quel point d'extension il utilise) est présenté explicitement à l'Administrateur pour validation.

## 5. Activation

- Un plugin installé peut être activé ou désactivé indépendamment, sans nécessiter de réinstallation.
- L'activation d'un plugin ne modifie jamais le comportement des modules natifs au-delà de ce que le point d'extension concerné autorise (cf. [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md) §7).
- Plusieurs plugins peuvent être actifs simultanément sur un même point d'extension (ex. plusieurs connecteurs ERP) sans conflit entre eux.

## 6. Désactivation

- La désactivation d'un plugin suspend son fonctionnement sans supprimer les données qu'il a précédemment produites ou intégrées (ex. les données déjà importées via un connecteur désactivé restent disponibles).
- Une désactivation est immédiate et réversible, sans perte de configuration du plugin concerné.

## 7. Configuration d'un plugin

- Chaque plugin dispose de ses propres paramètres de configuration, accessibles depuis sa fiche dans l'écran Plugins, sans interférer avec les paramètres généraux de l'Organisation (cf. [SETTINGS.md](SETTINGS.md)).

## 8. Suivi et traçabilité

- Toute installation, activation, désactivation ou modification de configuration d'un plugin est journalisée (cf. [AUDIT_AND_HISTORY.md](AUDIT_AND_HISTORY.md)).
- L'état de fonctionnement de chaque plugin actif (dernier traitement réalisé, éventuelles anomalies) est consultable depuis l'écran Plugins.

## 9. Restriction par licence

- La disponibilité de certains plugins ou du Plugin System lui-même peut être restreinte selon l'offre de licence de l'Organisation (cf. [FEATURE_MATRIX.md](FEATURE_MATRIX.md)), typiquement réservé aux offres Business et Enterprise.
