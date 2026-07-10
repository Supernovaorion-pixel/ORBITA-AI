# ORBITA AI — Backup and Recovery

> Ce document définit la stratégie technique de sauvegarde et de restauration, appliquant les principes définis dans [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §3 au socle retenu dans [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md).

## 1. Sauvegardes

- La base PostgreSQL de chaque environnement fait l'objet de sauvegardes complètes régulières, complétées par une conservation continue des journaux de transactions, permettant une restauration à un instant précis (point-in-time recovery) plutôt qu'au seul instant de la dernière sauvegarde complète.
- **Cloud** — les sauvegardes sont automatisées, chiffrées, et répliquées sur un site distinct du site d'exploitation principal, sans action requise de l'Organisation cliente.
- **On-Premise** — la fréquence et la destination des sauvegardes sont configurées par l'Administrateur de l'Organisation (cf. [features/ADMINISTRATION.md](../features/ADMINISTRATION.md) §4), avec une configuration par défaut raisonnable fournie nativement à l'installation.
- Les secrets et clés de chiffrement des sauvegardes sont gérés selon les principes définis dans [SECRET_MANAGEMENT.md](SECRET_MANAGEMENT.md), jamais conservés au même emplacement que la sauvegarde elle-même.

## 2. Restauration

- Une restauration peut être ciblée à un instant précis dans le passé (point-in-time recovery), permettant de revenir à l'état exact des données précédant un incident, sans perdre l'intégralité de l'activité intervenue depuis la dernière sauvegarde complète.
- Une demande de restauration est une action tracée et réservée aux Administrateurs habilités (cf. [features/ADMINISTRATION.md](../features/ADMINISTRATION.md) §4), avec confirmation explicite de son caractère irréversible pour les données postérieures au point de restauration choisi (cf. [ux/MICRO_INTERACTIONS.md](../ux/MICRO_INTERACTIONS.md) §9).
- La capacité de restauration est vérifiée périodiquement par un exercice de restauration effective sur un environnement isolé, conformément à [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §3 : une sauvegarde jamais testée n'est pas considérée comme fiable.

## 3. Résilience

- **Cloud** — l'infrastructure d'exploitation est conçue pour tolérer la défaillance d'un composant unique (instance applicative, nœud de base de données) sans interruption de service perceptible, grâce à la réplication définie dans [DATABASE_STRATEGY.md](DATABASE_STRATEGY.md) §4 et à la montée en charge horizontale définie dans [DEPLOYMENT_STRATEGY.md](DEPLOYMENT_STRATEGY.md) §2.
- **On-Premise** — la résilience dépend de l'infrastructure propre à l'Organisation cliente ; ORBITA AI documente et recommande une configuration minimale de résilience (réplication de la base de données, sauvegarde régulière), sans imposer une infrastructure spécifique à l'Organisation.
- Toute défaillance d'un module fonctionnel isolé (cf. [architecture/SYSTEM_ARCHITECTURE.md](../architecture/SYSTEM_ARCHITECTURE.md) §6) n'affecte jamais le fonctionnement des autres modules, limitant l'impact d'un incident à son périmètre réel.

## 4. Continuité d'activité

- Un objectif de temps de reprise (durée maximale d'interruption acceptable) et un objectif de perte de données maximale (durée maximale de donnée non sauvegardée pouvant être perdue lors d'un incident) sont définis pour chaque mode de déploiement et chaque niveau de licence, les Organisations Enterprise bénéficiant des objectifs les plus stricts, cohérents avec les exigences de gouvernance renforcée de cette offre (cf. [docs/BUSINESS_MODEL.md](../docs/BUSINESS_MODEL.md) §2).
- La continuité d'activité est vérifiée par des exercices réguliers simulant un incident majeur (perte totale d'un site d'exploitation en Cloud), garantissant que la procédure de reprise documentée reste effectivement opérationnelle et pas seulement théorique.

## 5. Traçabilité

- Toute opération de sauvegarde et de restauration est journalisée conformément à [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §3, incluant son origine, son étendue et son résultat.

## 6. Cloisonnement par Organisation

Une restauration effectuée pour une Organisation cliente n'affecte jamais les données d'une autre Organisation, y compris dans un environnement Cloud mutualisé, conformément au principe de cloisonnement défini dans [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §4.

## 7. Principe transversal

La sauvegarde et la restauration ne sont jamais considérées comme une fonctionnalité secondaire : elles constituent, avec la sécurité (cf. [SECURITY_REQUIREMENTS.md](SECURITY_REQUIREMENTS.md)), la condition de confiance fondamentale sur laquelle repose l'ensemble de la proposition de valeur d'ORBITA AI (cf. [docs/PROJECT_VISION.md](../docs/PROJECT_VISION.md)).
