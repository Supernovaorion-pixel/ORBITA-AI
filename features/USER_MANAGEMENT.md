# ORBITA AI — Feature Specification: User Management

> Ce document spécifie le comportement fonctionnel complet de la gestion des utilisateurs, organisations, rôles et permissions, couvrant l'écran Utilisateurs (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §16).

## 1. Objectif

Permettre à chaque Organisation cliente de gérer de façon autonome ses utilisateurs et leurs droits d'accès, conformément à [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §5.

## 2. Utilisateurs

- Un utilisateur est créé au sein d'une Organisation par un Administrateur, avec une identité, un rôle et un périmètre d'accès (territoire, équipe).
- Un utilisateur peut être invité par une invitation nominative ; son accès n'est effectif qu'après acceptation de cette invitation et authentification (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §6).
- Un utilisateur peut être suspendu (accès temporairement bloqué, données conservées) ou retiré définitivement de l'Organisation ; le retrait ne supprime jamais les données historiques dont il est l'auteur (traçabilité, cf. [AUDIT_AND_HISTORY.md](AUDIT_AND_HISTORY.md)).

## 3. Organisations

- Une Organisation est l'entité cliente racine (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §2), strictement cloisonnée de toute autre Organisation.
- La configuration propre à une Organisation (structure de territoires, objectifs, préférences) est gérée exclusivement par ses propres Administrateurs, sans intervention possible d'une autre Organisation.
- Une Organisation dispose d'une licence unique active à un instant donné (cf. [LICENSE_MANAGEMENT.md](LICENSE_MANAGEMENT.md)).

## 4. Rôles

Le catalogue officiel de rôles, aligné sur [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md), comprend :

| Rôle | Périmètre de données par défaut |
|---|---|
| **Direction Générale** | Ensemble de l'Organisation. |
| **Directeur Commercial** | Ensemble de l'Organisation, avec focus sur le pilotage commercial. |
| **Responsable Régional** | Son territoire et les Commerciaux qui y sont rattachés. |
| **Commercial** | Son portefeuille de Clients et sa performance individuelle. |
| **Contrôle de Gestion** | Ensemble de l'Organisation, avec accès renforcé aux données financières et à la traçabilité. |
| **Administrateur** | Gestion des utilisateurs, licences et configuration ; périmètre de données selon attribution complémentaire. |

Un utilisateur peut se voir attribuer un rôle complémentaire à titre exceptionnel (ex. un Responsable Régional agissant également comme Administrateur), sans que cela ne modifie la définition du rôle lui-même.

## 5. Permissions

- Les permissions découlent du rôle attribué et du périmètre de responsabilité (territoire, équipe) associé à l'utilisateur (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §Utilisateur et Permissions).
- Une permission peut être affinée au-delà du rôle standard (ex. accès en lecture seule à un module donné), configurable par un Administrateur.
- Toute modification de permission est journalisée (qui, quoi, quand), conformément à [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §5.

## 6. Cycle de vie d'un compte utilisateur

1. **Invitation** — un Administrateur crée l'invitation avec rôle et périmètre.
2. **Activation** — l'utilisateur accepte l'invitation et s'authentifie pour la première fois.
3. **Usage courant** — l'utilisateur accède à la plateforme selon ses permissions.
4. **Modification** — le rôle ou le périmètre peut être ajusté à tout moment par un Administrateur.
5. **Suspension** — accès temporairement bloqué, réversible.
6. **Retrait** — accès définitivement supprimé, historique conservé.

## 7. Limites liées à la licence

- Le nombre d'utilisateurs actifs pouvant être créés est encadré par la licence active de l'Organisation (cf. [LICENSE_MANAGEMENT.md](LICENSE_MANAGEMENT.md) §9) ; un Administrateur tentant de dépasser cette limite est informé explicitement avec un chemin de régularisation.

## 8. Auto-gestion du profil

- Chaque utilisateur gère lui-même ses informations personnelles et ses préférences depuis l'écran Profil (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §22), sans pouvoir modifier son propre rôle ou périmètre d'accès, qui restent du ressort exclusif d'un Administrateur.
