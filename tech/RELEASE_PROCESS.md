# ORBITA AI — Release Process

> Ce document définit le processus officiel de publication d'une version, appliquant concrètement [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md).

## 1. Objectif

Garantir qu'une version publiée d'ORBITA AI, quelle que soit l'édition ou le mode de déploiement, a traversé un processus de vérification homogène avant d'atteindre une Organisation cliente.

## 2. Étapes du processus

1. **Gel du périmètre** — le contenu fonctionnel prévu pour la version est arrêté ; toute nouvelle fonctionnalité non prête est reportée à la version suivante plutôt que d'introduire un risque de dernière minute.
2. **Canal Alpha** — vérification interne, exécution complète de la suite de tests (cf. [TESTING_STRATEGY.md](TESTING_STRATEGY.md)), aucune diffusion externe.
3. **Canal Beta** — mise à disposition auprès d'Organisations clientes volontaires dans un cadre encadré, avec collecte structurée des anomalies rencontrées.
4. **Canal RC (Release Candidate)** — périmètre fonctionnel gelé, dernière vérification complète, aucune nouvelle fonctionnalité ajoutée à ce stade, seules des corrections d'anomalies bloquantes sont tolérées.
5. **Canal Stable** — publication en disponibilité générale, accessible à l'ensemble des Organisations clientes selon leur mode de déploiement et leur politique de mise à jour (cf. [UPDATE_SYSTEM.md](UPDATE_SYSTEM.md)).

Aucune étape n'est ignorée ou raccourcie, quelle que soit la pression de calendrier commercial.

## 3. Critères de passage entre canaux

- Le passage d'Alpha à Beta exige la réussite complète des tests unitaires, d'intégration et fonctionnels (cf. [TESTING_STRATEGY.md](TESTING_STRATEGY.md) §2-4).
- Le passage de Beta à RC exige en complément la réussite des tests UI et de performance (cf. [TESTING_STRATEGY.md](TESTING_STRATEGY.md) §5-6) et l'absence d'anomalie bloquante remontée durant la phase Beta.
- Le passage de RC à Stable exige l'absence de toute anomalie bloquante ou de sécurité non résolue.

## 4. Notes de version

- Chaque publication est accompagnée de notes de version destinées aux Organisations clientes, résumant : les nouvelles fonctionnalités, les corrections apportées, et tout changement de compatibilité (cf. [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md) §2).
- Une rupture de compatibilité ascendante (incrément de version MAJEUR) est systématiquement accompagnée d'une note explicite sur son impact et, lorsque pertinent, d'un chemin de migration recommandé.

## 5. Application aux éditions commerciales

- Une seule publication de version couvre l'ensemble des éditions (Community à Enterprise, cf. [features/FEATURE_MATRIX.md](../features/FEATURE_MATRIX.md)) : le périmètre fonctionnel activé dépend de la licence, jamais d'une version distincte du logiciel (cf. [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md) §5).

## 6. Application aux modes de déploiement

- **Cloud** — une version Stable est déployée progressivement (cf. [DEPLOYMENT_STRATEGY.md](DEPLOYMENT_STRATEGY.md)) sur l'infrastructure exploitée pour le compte des Organisations clientes, sans action requise de leur part.
- **On-Premise** — une version Stable ou LTS est mise à disposition des Organisations clientes, qui choisissent la fenêtre d'application selon leur propre politique de maintenance (cf. [features/ADMINISTRATION.md](../features/ADMINISTRATION.md) §6).

## 7. Rôles et responsabilités

- La décision de passage d'un canal à l'autre est prise collectivement par les responsables techniques du projet sur la base des critères objectifs du §3, jamais sur une base calendaire seule.
- Toute anomalie bloquante détectée à un stade quelconque suspend le passage au canal suivant jusqu'à sa résolution et sa vérification.

## 8. Traçabilité

Chaque publication est journalisée avec sa date, son numéro de version, son canal et son contenu exact, conformément aux principes de [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md), garantissant qu'il est toujours possible de déterminer précisément ce qu'une Organisation cliente donnée exécute à un instant donné.
