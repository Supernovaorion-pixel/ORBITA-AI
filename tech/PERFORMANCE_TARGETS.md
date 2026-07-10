# ORBITA AI — Performance Targets

> Ce document fixe les objectifs chiffrés de performance du logiciel, déclinant en cibles mesurables les principes définis dans [architecture/PERFORMANCE_GUIDELINES.md](../architecture/PERFORMANCE_GUIDELINES.md). Ces cibles sont vérifiées systématiquement avant chaque publication de version (cf. [TESTING_STRATEGY.md](TESTING_STRATEGY.md) §6).

## 1. Temps de démarrage

- **Cloud** — une instance applicative est disponible à recevoir des requêtes en moins de **10 secondes** après son lancement, condition nécessaire à une bascule de mise à jour progressive fluide (cf. [UPDATE_SYSTEM.md](UPDATE_SYSTEM.md) §1).
- **On-Premise** — le service applicatif est pleinement opérationnel en moins de **30 secondes** après le démarrage du système d'exploitation hôte.

## 2. Temps d'import

- Un import de **100 000 lignes** est intégralement traité (contrôle, validation, intégration) en moins de **60 secondes** en mode incrémental.
- Un import complet de **1 million de lignes** (cf. [features/IMPORT_ENGINE.md](../features/IMPORT_ENGINE.md) §12) est traité en moins de **10 minutes**, en traitement asynchrone n'empêchant jamais la consultation du reste de la plateforme (cf. [architecture/PERFORMANCE_GUIDELINES.md](../architecture/PERFORMANCE_GUIDELINES.md) §3).
- La prévisualisation d'un import (cf. [features/IMPORT_ENGINE.md](../features/IMPORT_ENGINE.md) §6) est affichée en moins de **3 secondes**, quel que soit le volume total du fichier, par échantillonnage représentatif.

## 3. Temps de calcul (Analytics et Forecast)

- Une analyse standard (CA, Marge, Évolution sur un périmètre donné, cf. [features/ANALYTICS_ENGINE.md](../features/ANALYTICS_ENGINE.md)) est restituée en moins de **2 secondes**, pour une Organisation disposant de plusieurs années d'historique.
- Une analyse de décomposition complexe (Pareto, analyse croisée multi-axes) est restituée en moins de **5 secondes**.
- Une projection du Forecast Engine (cf. [features/FORECAST_ENGINE.md](../features/FORECAST_ENGINE.md)) pour un périmètre standard est calculée en moins de **5 secondes** ; un recalcul global consécutif à un import majeur s'exécute en tâche asynchrone sans bloquer la consultation des projections déjà disponibles.

## 4. Temps d'ouverture des Dashboards

- Le Dashboard (cf. [features/DASHBOARD.md](../features/DASHBOARD.md)) affiche l'intégralité de ses cartes KPI et graphiques en moins de **2 secondes** après authentification, pour un périmètre standard.
- Un changement de filtre (période, périmètre) sur le Dashboard déjà ouvert applique le résultat en moins de **1 seconde** (cf. [ux/DASHBOARD_SPECIFICATION.md](../ux/DASHBOARD_SPECIFICATION.md) §5).
- Un chargement partiel par zone (cf. [architecture/PERFORMANCE_GUIDELINES.md](../architecture/PERFORMANCE_GUIDELINES.md) §4) garantit qu'aucune carte ne retarde l'affichage des autres cartes déjà disponibles.

## 5. Consommation mémoire

- Une instance applicative traitant une charge Enterprise standard (plusieurs centaines d'utilisateurs simultanés, plusieurs Organisations) opère dans une enveloppe mémoire dimensionnée et prévisible, sans croissance non maîtrisée dans le temps (absence de fuite mémoire vérifiée lors des tests de performance, cf. [TESTING_STRATEGY.md](TESTING_STRATEGY.md) §6).
- La consommation mémoire d'une instance ne doit jamais croître de façon linéaire avec le volume total d'historique d'une Organisation : seule la donnée activement consultée doit peser sur la mémoire à un instant donné (cf. [architecture/PERFORMANCE_GUIDELINES.md](../architecture/PERFORMANCE_GUIDELINES.md) §2).

## 6. Capacité maximale

- La plateforme doit soutenir, pour une Organisation Enterprise, un volume de **plusieurs dizaines de millions de lignes** de données transactionnelles cumulées sur plusieurs années d'historique, sans dégradation des temps définis aux §2-4.
- La plateforme doit soutenir plusieurs centaines d'utilisateurs simultanés au sein d'une même Organisation, et plusieurs milliers d'Organisations distinctes sur une même infrastructure Cloud, sans qu'une Organisation n'affecte la performance perçue par une autre (cf. cloisonnement, [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §4).
- Ces capacités doivent être atteignables par une montée en charge horizontale de l'infrastructure (cf. [DEPLOYMENT_STRATEGY.md](DEPLOYMENT_STRATEGY.md)), sans nécessiter de refonte du Domaine ou de l'Application.

## 7. Principe de vérification

Ces cibles sont vérifiées sur un jeu de données représentatif d'une Organisation Enterprise à pleine échelle (cf. [architecture/PERFORMANCE_GUIDELINES.md](../architecture/PERFORMANCE_GUIDELINES.md) §7), jamais sur un jeu de données réduit non représentatif. Un dépassement constaté d'une cible de ce document bloque la publication de la version concernée (cf. [RELEASE_PROCESS.md](RELEASE_PROCESS.md) §3).
