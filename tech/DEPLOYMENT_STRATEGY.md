# ORBITA AI — Deployment Strategy

> Ce document définit la stratégie de déploiement technique d'ORBITA AI pour ses deux modes officiels, Cloud et On-Premise, à partir de la construction unique définie dans [BUILD_AND_PACKAGING.md](BUILD_AND_PACKAGING.md).

## 1. Principe directeur

Un seul et même artefact applicatif sert les deux modes de déploiement : aucune divergence de code entre Cloud et On-Premise, seule la configuration système (cf. [CONFIGURATION_MANAGEMENT.md](CONFIGURATION_MANAGEMENT.md) §2) et l'infrastructure d'exécution diffèrent.

## 2. Cloud

- La plateforme est exploitée pour le compte des Organisations clientes sur une infrastructure mutualisée, avec cloisonnement strict par Organisation garanti au niveau du Domaine (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §4) et de la base de données (cf. [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md) §3).
- Le déploiement s'appuie sur des images de conteneur standardisées (cf. [BUILD_AND_PACKAGING.md](BUILD_AND_PACKAGING.md) §3), orchestrées pour permettre une montée en charge horizontale (ajout d'instances applicatives) indépendante de la montée en charge de la base de données (cf. [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md) §4).
- La montée en charge horizontale est rendue possible par l'absence d'état conservé en mémoire d'une instance applicative au-delà d'une requête : toute donnée durable réside dans la base de données de référence, jamais uniquement dans la mémoire d'une instance.

## 3. On-Premise

- La plateforme est installée au sein de l'infrastructure de l'Organisation cliente (cf. [BUILD_AND_PACKAGING.md](BUILD_AND_PACKAGING.md) §2), avec priorité donnée à un déploiement sur Windows Server, une prise en charge Linux prévue via conteneur standardisé.
- L'Organisation cliente maîtrise l'intégralité de son infrastructure d'exécution et de ses données (cf. [SECRET_MANAGEMENT.md](SECRET_MANAGEMENT.md) §6), condition recherchée par les Organisations Enterprise à exigence de gouvernance renforcée.
- Le mode On-Premise reste fonctionnellement identique au mode Cloud dans son périmètre de licence souscrit (cf. [features/FEATURE_MATRIX.md](../features/FEATURE_MATRIX.md)), à l'exception des capacités qui supposent intrinsèquement une infrastructure mutualisée (ex. mise à jour continue automatique, remplacée par une mise à jour maîtrisée par l'Organisation, cf. [UPDATE_SYSTEM.md](UPDATE_SYSTEM.md) §1).

## 4. Migration entre modes

- Une Organisation cliente peut migrer d'un mode de déploiement à l'autre (ex. d'On-Premise vers Cloud lors d'une évolution de sa stratégie d'infrastructure), par un export complet de ses données (cf. [features/EXPORT_ENGINE.md](../features/EXPORT_ENGINE.md)) suivi d'une réintégration dans l'environnement cible, sans transformation du modèle de données, celui-ci étant strictement identique entre les deux modes (cf. [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md) §3).
- Une migration est toujours réalisée avec une période de double disponibilité contrôlée, permettant de vérifier l'intégrité de la donnée migrée avant bascule définitive et retrait de l'environnement d'origine.

## 5. Mise à jour

- Détaillée dans [UPDATE_SYSTEM.md](UPDATE_SYSTEM.md) : bascule progressive en Cloud, fenêtre maîtrisée par l'Administrateur en On-Premise, avec dans les deux cas une vérification de compatibilité et une possibilité de retour arrière (§6).

## 6. Retour arrière (rollback d'infrastructure)

- Tout déploiement d'une nouvelle version conserve la version précédente disponible pendant une période de transition, permettant un retour immédiat en cas d'anomalie détectée après bascule, conformément à [UPDATE_SYSTEM.md](UPDATE_SYSTEM.md) §2.
- En Cloud, le retour arrière est automatisable sur détection d'anomalie ; en On-Premise, il est déclenché par l'Administrateur selon une procédure documentée et testée avant chaque publication majeure (cf. [RELEASE_PROCESS.md](RELEASE_PROCESS.md)).

## 7. Observabilité du déploiement

- Chaque environnement déployé (Cloud ou On-Premise) expose des indicateurs de bon fonctionnement (disponibilité, temps de réponse) vérifiables en continu, alimentant le Journal technique (cf. [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §2) et permettant la détection automatique des anomalies justifiant un retour arrière (§6).

## 8. Principe transversal

Aucune décision de déploiement ne doit jamais introduire de divergence fonctionnelle entre Cloud et On-Premise au-delà de ce qui est explicitement documenté dans ce document et dans [features/FEATURE_MATRIX.md](../features/FEATURE_MATRIX.md) : une Organisation cliente doit pouvoir changer de mode de déploiement sans surprise fonctionnelle.
