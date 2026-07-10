# ORBITA AI — Product Owner Rules

> Ce document définit le rôle officiel et unique de **Product Owner** d'ORBITA AI. Ce rôle fait partie intégrante de l'architecture du produit, au même titre que les rôles définis dans [features/USER_MANAGEMENT.md](../features/USER_MANAGEMENT.md) §4 et le système de licences défini dans [features/LICENSE_MANAGEMENT.md](../features/LICENSE_MANAGEMENT.md) — il n'en constitue jamais un contournement.

## 1. Nature du rôle

- Le rôle **Product Owner** est un rôle unique, distinct des rôles commerciaux définis dans [features/USER_MANAGEMENT.md](../features/USER_MANAGEMENT.md) §4 (Direction Générale, Directeur Commercial, Responsable Régional, Commercial, Contrôle de Gestion, Administrateur).
- Il est réservé exclusivement au créateur d'ORBITA AI et n'est jamais attribué à un utilisateur d'une Organisation cliente, quel que soit son niveau de licence ou de rôle au sein de son Organisation.
- Ce rôle est défini au niveau du Core et du module Security (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §1 et §11) : il fait partie de l'architecture officielle du produit, documentée et vérifiable, jamais une porte dérobée non documentée.

## 2. Accès permanent à toutes les fonctionnalités

- Le Product Owner dispose d'un accès permanent à l'intégralité des modules fonctionnels définis dans [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md), sans restriction, y compris ceux non encore commercialisés ou en cours de développement.
- Cet accès n'est jamais soumis aux règles d'activation par module définies dans [features/LICENSE_MANAGEMENT.md](../features/LICENSE_MANAGEMENT.md) §10 : il précède et surpasse toute logique de licence commerciale.

## 3. Accès à toutes les éditions

- Le Product Owner peut consulter et utiliser la plateforme telle qu'elle se comporte sous chacune des quatre éditions officielles (Community, Starter, Business, Enterprise, cf. [features/FEATURE_MATRIX.md](../features/FEATURE_MATRIX.md)), à des fins de vérification, de démonstration ou de support, sans avoir à souscrire à aucune d'entre elles.

## 4. Accès à tous les modules

- Aucun module, y compris ceux réservés en édition Enterprise (Plugins, Connecteurs, cf. [features/FEATURE_MATRIX.md](../features/FEATURE_MATRIX.md) §16-17), n'est hors de portée du Product Owner.
- Cet accès inclut les capacités expérimentales non encore publiées (cf. §7).

## 5. Licence développeur à vie

- Le Product Owner dispose d'une licence propre, distincte de toute licence d'Organisation cliente, qualifiée de **licence développeur à vie** :
  - elle n'est **jamais limitée dans le temps** (aucune date d'expiration, contrairement aux licences commerciales définies dans [features/LICENSE_MANAGEMENT.md](../features/LICENSE_MANAGEMENT.md) §7),
  - elle est **indépendante de tout abonnement commercial** : elle ne dépend d'aucune souscription, d'aucun paiement récurrent, et n'est jamais affectée par la logique d'expiration ou de renouvellement définie pour les Organisations clientes,
  - elle **ne peut pas être désactivée par une licence client** : la création, la modification, l'expiration ou la révocation d'une licence d'Organisation cliente n'a strictement aucun effet sur la licence du Product Owner.

## 6. Autorité sur le système de licences

- Le Product Owner peut **créer, gérer et révoquer toutes les licences** attribuées aux Organisations clientes (cf. [features/LICENSE_MANAGEMENT.md](../features/LICENSE_MANAGEMENT.md)), au-delà des capacités données à un simple Administrateur d'Organisation qui ne gère que sa propre Organisation (cf. [features/USER_MANAGEMENT.md](../features/USER_MANAGEMENT.md) §3).
- Cette autorité s'exerce au niveau du système dans son ensemble, à travers toutes les Organisations, contrairement au cloisonnement strict par Organisation qui s'applique à tous les autres rôles (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §4).

## 7. Activation des fonctionnalités expérimentales

- Le Product Owner peut activer, pour lui-même ou pour une Organisation cliente déterminée (dans un cadre de test explicite, ex. programme Beta), des fonctionnalités expérimentales non encore incluses dans le canal Stable (cf. [tech/RELEASE_PROCESS.md](../tech/RELEASE_PROCESS.md) §2).
- L'activation d'une fonctionnalité expérimentale pour une Organisation cliente reste une action explicite et journalisée (cf. §9), jamais une activation silencieuse ou par défaut.

## 8. Accès aux outils de diagnostic, d'administration et de maintenance

- Le Product Owner dispose d'un accès complet aux outils de diagnostic (Journal, cf. [features/AUDIT_AND_HISTORY.md](../features/AUDIT_AND_HISTORY.md) §3), aux outils d'administration transversale (au-delà du périmètre d'une seule Organisation, cf. [features/ADMINISTRATION.md](../features/ADMINISTRATION.md)) et aux outils de maintenance (mises à jour, sauvegardes, cf. [tech/UPDATE_SYSTEM.md](../tech/UPDATE_SYSTEM.md), [tech/BACKUP_AND_RECOVERY.md](../tech/BACKUP_AND_RECOVERY.md)).
- Cet accès est nécessaire à la responsabilité globale du Product Owner sur la plateforme et son évolution, distinct de l'accès d'un Administrateur d'Organisation qui reste strictement borné à sa propre Organisation.

## 9. Journalisation intégrale

- **Toutes les actions du Product Owner sont journalisées**, sans aucune exception, selon les mêmes principes d'immutabilité que ceux définis dans [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §3 : le rôle Product Owner ne bénéficie d'aucune dispense de traçabilité.
- En particulier, toute consultation de donnée d'une Organisation cliente, toute création ou révocation de licence, et toute activation de fonctionnalité expérimentale sont enregistrées avec la même rigueur qu'une action d'Administrateur (qui, quoi, quand).
- Cette journalisation intégrale est ce qui distingue le rôle Product Owner d'un accès non contrôlé : le pouvoir étendu de ce rôle est strictement contrebalancé par une traçabilité absolue de son usage.

## 10. Ce rôle n'est pas un contournement du système de licences

- Le rôle Product Owner est conçu, documenté et implémenté comme une partie intégrante et légitime de l'architecture d'ORBITA AI, définie au même titre que tout autre rôle (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md), [features/USER_MANAGEMENT.md](../features/USER_MANAGEMENT.md)), jamais comme une exception cachée, un compte de service non documenté ou une porte dérobée technique.
- Il coexiste avec le système de licences commerciales sans jamais s'y substituer pour les Organisations clientes : celles-ci restent strictement soumises à [features/LICENSE_MANAGEMENT.md](../features/LICENSE_MANAGEMENT.md), le rôle Product Owner n'affectant jamais leur propre périmètre de licence.
- L'existence et le périmètre exact de ce rôle sont documentés dans ce dossier de planification et doivent être repris tels quels dans toute documentation technique ultérieure (cf. [tech/SECURITY_REQUIREMENTS.md](../tech/SECURITY_REQUIREMENTS.md)) : sa légitimité repose sur sa transparence documentée, jamais sur sa discrétion.

## 11. Unicité du rôle

- Un seul compte Product Owner existe dans le système à un instant donné, réservé au créateur d'ORBITA AI. Ce rôle n'est ni délégable à un tiers, ni dupliqué pour un usage d'équipe : tout besoin d'accès élargi pour d'autres contributeurs internes est couvert par des rôles distincts (ex. rôle Administrateur interne à des fins de support), eux-mêmes soumis au cloisonnement standard par Organisation.

## 12. Implémentation

- La mise en œuvre technique du rôle Product Owner est planifiée en Phase 10 — Licences (cf. [DEVELOPMENT_PHASES.md](DEVELOPMENT_PHASES.md)), une fois le système de licences des Organisations clientes lui-même opérationnel, garantissant que le rôle Product Owner est vérifié comme strictement non affecté par ce système (cf. [QUALITY_GATES.md](QUALITY_GATES.md) — critères Phase 10).
