# ORBITA AI — Update System

> Ce document définit le comportement fonctionnel et technique du module Update Manager (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §12), garantissant des mises à jour automatiques sans reconstruction du système (cf. [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md) §9).

## 1. Mises à jour

- **Cloud** — les mises à jour sont appliquées de façon continue et progressive sur l'infrastructure d'exploitation, sans action requise de l'Organisation cliente, avec bascule progressive (une fraction des instances à la fois) pour limiter l'impact d'une anomalie non détectée avant publication.
- **On-Premise** — une mise à jour disponible est signalée à l'Administrateur de l'Organisation (cf. [features/NOTIFICATION_CENTER.md](../features/NOTIFICATION_CENTER.md)), qui choisit la fenêtre d'application (cf. [features/ADMINISTRATION.md](../features/ADMINISTRATION.md) §6) ; une application automatique peut être configurée par l'Organisation pour les corrections de sécurité critiques.
- Toute mise à jour est vérifiée par signature avant application (cf. [BUILD_AND_PACKAGING.md](BUILD_AND_PACKAGING.md) §4).

## 2. Rollback (retour arrière)

- Toute mise à jour applique ses changements de façon réversible : l'état précédent (code applicatif et schéma de données, cf. [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md) §6) reste restaurable en cas d'anomalie détectée après application.
- Un rollback est une opération de dernier recours, jamais un mode de fonctionnement attendu ; toute mise à jour ayant nécessité un rollback fait l'objet d'une analyse avant nouvelle tentative de publication (cf. [RELEASE_PROCESS.md](RELEASE_PROCESS.md) §3).
- En Cloud, un rollback peut être déclenché automatiquement si des indicateurs de bon fonctionnement se dégradent significativement après une bascule progressive (§1).
- En On-Premise, un rollback est déclenché par l'Administrateur, avec une procédure documentée et testée au même titre que la mise à jour elle-même.

## 3. Notifications

- Toute mise à jour disponible, appliquée avec succès, ou ayant échoué, génère une notification à destination de l'Administrateur de l'Organisation (cf. [features/NOTIFICATION_CENTER.md](../features/NOTIFICATION_CENTER.md)).
- Une mise à jour majeure (rupture de compatibilité, cf. [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md) §2) est annoncée suffisamment à l'avance pour permettre à l'Organisation cliente de s'y préparer, jamais appliquée par surprise.

## 4. Canaux

- **Canal Stable** — canal par défaut de toute Organisation cliente en production, recevant les versions ayant traversé l'intégralité du processus de publication (cf. [RELEASE_PROCESS.md](RELEASE_PROCESS.md)).
- **Canal LTS** — canal réservé aux Organisations Enterprise ou On-Premise recherchant une stabilité prolongée, ne recevant que des corrections et évolutions mineures rétrocompatibles (cf. [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md) §4).
- **Canal Beta** — canal optionnel, réservé aux Organisations volontaires souhaitant accéder par anticipation aux prochaines fonctionnalités, dans un cadre encadré et réversible.

## 5. Versionnement

- Chaque mise à jour applique une transition de version strictement conforme au Semantic Versioning défini dans [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md) : une mise à jour CORRECTIF ou MINEURE ne requiert jamais d'action de l'Administrateur au-delà de sa validation d'application ; une mise à jour MAJEURE requiert une confirmation explicite et informée avant application, y compris en Cloud.
- La version exacte actuellement active pour une Organisation est consultable à tout moment (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §26 — À propos).

## 6. Mise à jour des extensions

- L'Update Manager coordonne également la compatibilité des Plugins et Connecteurs actifs (cf. [features/PLUGIN_SYSTEM.md](../features/PLUGIN_SYSTEM.md), [features/CONNECTORS.md](../features/CONNECTORS.md)) avec la nouvelle version du noyau, signalant explicitement toute extension qui deviendrait incompatible avant d'appliquer la mise à jour, plutôt que de la désactiver silencieusement après coup.

## 7. Principe transversal

Aucune mise à jour n'est appliquée sans que l'ensemble des vérifications définies dans [TESTING_STRATEGY.md](TESTING_STRATEGY.md) et [RELEASE_PROCESS.md](RELEASE_PROCESS.md) n'ait été satisfait au préalable pour la version concernée : le module Update Manager est un mécanisme de distribution, jamais un substitut au processus de vérification.
