# ORBITA AI — Feature Specification: Administration

> Ce document spécifie le comportement fonctionnel complet de l'écran Administration (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §15), point d'entrée transversal de la gestion d'une Organisation cliente.

## 1. Objectif

Offrir à l'Administrateur d'une Organisation une vision consolidée et un accès centralisé à la gestion de son environnement : organisation, licence, sauvegardes, utilisateurs et maintenance.

## 2. Organisation

- Consultation et modification des informations de référence de l'Organisation (identité, structure de territoires et d'équipes, cf. [SETTINGS.md](SETTINGS.md) §2.2).
- Vue de synthèse de l'état général de l'Organisation : nombre d'utilisateurs actifs, dernier Import réalisé, alertes critiques en cours.

## 3. Licence

- Accès direct à l'état de la licence active (offre souscrite, périmètre, volumes utilisés, échéance), en synthèse depuis Administration et en détail depuis l'écran Licences (cf. [LICENSE_MANAGEMENT.md](LICENSE_MANAGEMENT.md)).
- Toute situation nécessitant une action (dépassement, échéance proche) est mise en avant en priorité sur cet écran (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §15).

## 4. Sauvegardes

- Consultation de l'état des sauvegardes de données de l'Organisation, conformément à [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §3 : date de la dernière sauvegarde réussie, fréquence appliquée.
- En mode de déploiement On-Premise (cf. [docs/BUSINESS_MODEL.md](../docs/BUSINESS_MODEL.md) §5), l'Administrateur dispose d'une visibilité complémentaire sur la configuration de sauvegarde propre à son infrastructure.
- Une demande de restauration à partir d'une sauvegarde antérieure est une action tracée, réservée aux Administrateurs habilités, avec confirmation explicite (cf. [ux/MICRO_INTERACTIONS.md](../ux/MICRO_INTERACTIONS.md) §9).

## 5. Utilisateurs

- Accès rapide à la gestion des utilisateurs depuis Administration, renvoyant vers l'écran dédié (cf. [USER_MANAGEMENT.md](USER_MANAGEMENT.md)) pour le détail complet des opérations.
- Mise en évidence des situations nécessitant une action (invitation en attente depuis longtemps, utilisateur suspendu à réactiver ou retirer).

## 6. Maintenance

- Information sur les opérations de maintenance planifiées de la plateforme (mises à jour, cf. [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md)), avec anticipation communiquée à l'Administrateur avant toute interruption de service programmée.
- En mode Cloud, les mises à jour sont appliquées en continu avec un impact minimal signalé à l'avance ; en mode On-Premise, l'Administrateur choisit la fenêtre d'application d'une mise à jour au sein d'une période proposée.
- Accès à l'état de version actuellement déployé pour l'Organisation (cf. écran À propos, [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §26).

## 7. Connecteurs et Plugins

- Accès rapide depuis Administration vers la gestion des Connecteurs (cf. [CONNECTORS.md](CONNECTORS.md)) et des Plugins (cf. [PLUGIN_SYSTEM.md](PLUGIN_SYSTEM.md)), avec mise en évidence de toute anomalie de synchronisation active.

## 8. Traçabilité

- Toute action réalisée depuis l'écran Administration est journalisée selon les principes définis dans [AUDIT_AND_HISTORY.md](AUDIT_AND_HISTORY.md) §4.

## 9. Droits d'accès

- L'écran Administration et l'ensemble de ses sous-écrans ne sont accessibles qu'aux utilisateurs disposant du rôle Administrateur, ou d'une permission complémentaire explicitement attribuée par un Administrateur (cf. [USER_MANAGEMENT.md](USER_MANAGEMENT.md) §4-5).
