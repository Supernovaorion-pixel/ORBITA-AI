# ORBITA AI — Feature Specification: Connectors

> Ce document spécifie le comportement fonctionnel complet des connecteurs vers des systèmes externes, couvrant l'écran Connecteurs (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §19). Les connecteurs sont une catégorie particulière d'extension du Plugin System (cf. [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md) §4).

## 1. Objectif

Permettre l'alimentation continue et automatisée de l'Import Engine depuis des systèmes externes à l'Organisation, sans intervention manuelle répétée.

## 2. Connecteurs ERP

- Un connecteur ERP synchronise automatiquement les données de gestion (ventes, produits, clients, facturation) depuis le système ERP de l'Organisation vers l'Import Engine (cf. [IMPORT_ENGINE.md](IMPORT_ENGINE.md)).
- La fréquence de synchronisation est configurable (temps réel, quotidienne, hebdomadaire) selon les besoins de l'Organisation et les capacités du système source.
- Un connecteur ERP traduit la structure de données de l'ERP source vers le format attendu par l'Import Engine, sans que celui-ci n'ait à connaître la nature du système d'origine (cf. [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md) §4).

## 3. Connecteurs CRM

- Un connecteur CRM synchronise les données relatives aux Clients, opportunités commerciales (Pipeline) et interactions vers l'Import Engine.
- Fonctionne selon les mêmes principes de synchronisation et de configuration que les connecteurs ERP (§2).

## 4. API (accès entrant/sortant futur)

- Bien qu'aucune API ne soit développée à ce stade, les connecteurs sont conçus pour préfigurer une future exposition d'API permettant à des systèmes tiers d'interagir directement avec ORBITA AI, sans remise en cause de leur fonctionnement actuel (cf. [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md) §10).
- Tout connecteur respecte dès sa conception le même principe de séparation entre logique métier et accès externe, afin de faciliter cette évolution future.

## 5. Imports via connecteur

- Les données acheminées par un connecteur suivent exactement le même processus de contrôle et de validation que tout autre Import (cf. [IMPORT_ENGINE.md](IMPORT_ENGINE.md) §5), sans dispense de contrôle du fait de leur origine automatisée.
- Le mode incrémental (cf. [IMPORT_ENGINE.md](IMPORT_ENGINE.md) §11) est le mode par défaut des imports via connecteur, cohérent avec leur fréquence de synchronisation régulière.

## 6. Exports via connecteur

- Un connecteur peut également être configuré pour transmettre des données ou rapports d'ORBITA AI vers un système externe (ex. rapport de performance transmis automatiquement vers un outil de gestion documentaire de l'Organisation), en s'appuyant sur l'Export Engine (cf. [EXPORT_ENGINE.md](EXPORT_ENGINE.md)).

## 7. Suivi de synchronisation

- L'écran Connecteurs présente, pour chaque connecteur configuré, son statut (actif, en erreur, en pause) et la date de sa dernière synchronisation réussie.
- Un échec de synchronisation déclenche une notification à l'Administrateur (cf. [NOTIFICATION_CENTER.md](NOTIFICATION_CENTER.md)), avec accès direct au détail de l'anomalie rencontrée.

## 8. Sécurité

- Toute connexion à un système externe respecte les principes de confidentialité et d'authentification définis dans [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) ; les identifiants de connexion à un système externe ne sont jamais visibles en clair au-delà de leur configuration initiale.

## 9. Restriction par licence

- Le nombre de connecteurs simultanés et l'accès aux connecteurs avancés peuvent être restreints selon l'offre de licence de l'Organisation (cf. [FEATURE_MATRIX.md](FEATURE_MATRIX.md)).
