# ORBITA AI — Feature Specification: Alert System

> Ce document spécifie le comportement fonctionnel complet du module Alertes, porté techniquement par le Notification Center et alimenté par l'Analytics Engine et le Forecast Engine (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md)).

## 1. Objectif

Signaler de façon proactive tout événement significatif nécessitant l'attention d'un utilisateur, sans que celui-ci ait à le rechercher activement (cf. [docs/FUNCTIONAL_SPECIFICATION.md](../docs/FUNCTIONAL_SPECIFICATION.md) §12).

## 2. Catalogue des alertes

Le catalogue officiel d'alertes couvre a minima :
- **Écart à un objectif** — un périmètre s'éloigne significativement de sa trajectoire vers un Objectif défini (cf. [FORECAST_ENGINE.md](FORECAST_ENGINE.md) §5).
- **Anomalie de tendance** — une rupture significative par rapport à la tendance ou à la saisonnalité habituelle est détectée (cf. [ANALYTICS_ENGINE.md](ANALYTICS_ENGINE.md) §8).
- **Perte significative** — une baisse marquée d'activité sur un client, produit ou territoire (cf. [ANALYTICS_ENGINE.md](ANALYTICS_ENGINE.md) §15).
- **Opportunité identifiée** — une hausse ou un potentiel significatif détecté (cf. [ANALYTICS_ENGINE.md](ANALYTICS_ENGINE.md) §16).
- **Risque sur un Grand Compte** — signal de désengagement ou de baisse d'activité sur un compte stratégique (cf. [CLIENT_MANAGEMENT.md](CLIENT_MANAGEMENT.md) §5).
- **Anomalie d'import** — anomalie significative détectée lors d'un Import (cf. [IMPORT_ENGINE.md](IMPORT_ENGINE.md) §8).
- **Alerte de licence** — approche ou dépassement d'une limite contractuelle (cf. [LICENSE_MANAGEMENT.md](LICENSE_MANAGEMENT.md) §6).

## 3. Déclencheurs

- Chaque type d'alerte repose sur un seuil ou une règle de détection configurable par un utilisateur habilité (cf. [SETTINGS.md](SETTINGS.md)), avec une valeur par défaut raisonnable fournie nativement.
- Un déclencheur peut être défini au niveau de l'Organisation ou affiné pour un périmètre particulier (ex. seuil différent pour un Grand Compte par rapport à un client standard).
- Une alerte n'est générée qu'une seule fois pour un même événement : une situation qui persiste ne génère pas de nouvelles alertes répétées tant qu'elle n'évolue pas significativement.

## 4. Priorités

Chaque alerte est classée selon un niveau de priorité :

| Priorité | Signification | Traitement |
|---|---|---|
| **Critique** | Impact majeur avéré ou imminent sur un objectif ou un compte stratégique. | Notification immédiate, reste visible jusqu'à traitement explicite. |
| **Élevée** | Écart significatif nécessitant une attention rapprochée. | Notification standard, mise en avant dans l'écran Alertes. |
| **Informative** | Signal utile mais ne nécessitant pas d'action immédiate (ex. opportunité identifiée). | Consultable dans l'écran Alertes, sans notification intrusive. |

Le classement de priorité suit une logique constante : gravité de l'impact métier avant ancienneté de l'alerte (cf. [ux/DASHBOARD_SPECIFICATION.md](../ux/DASHBOARD_SPECIFICATION.md) §7).

## 5. Historique

- Toute alerte générée, qu'elle soit active, traitée ou archivée, est conservée et consultable, avec sa date de création, son évolution et les actions prises à son égard.
- L'historique des alertes permet d'évaluer, dans le temps, la pertinence des seuils configurés (§3) et d'ajuster ceux-ci si nécessaire.

## 6. Validation

- Un utilisateur destinataire d'une alerte peut la marquer comme **prise en compte**, indiquant qu'il en a connaissance sans que la situation sous-jacente ne soit nécessairement résolue.
- Une alerte prise en compte reste visible tant que la situation qui l'a déclenchée persiste, mais n'est plus signalée par une notification active.

## 7. Archivage

- Une alerte dont la situation sous-jacente est résolue (retour à une trajectoire normale, objectif de nouveau atteignable) est automatiquement archivée.
- Une alerte peut également être archivée manuellement par un utilisateur habilité, avec un motif consigné (ex. "écart expliqué et accepté").
- L'archivage n'efface jamais l'alerte : elle reste consultable dans l'historique (§5), conformément aux principes de traçabilité définis dans [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §2.

## 8. Destinataires

- Une alerte est acheminée aux utilisateurs dont le périmètre de responsabilité est concerné (ex. une alerte sur un territoire est acheminée à son Responsable Régional et remonte au Directeur Commercial), conformément aux droits d'accès définis dans [USER_MANAGEMENT.md](USER_MANAGEMENT.md).
- Un utilisateur peut configurer ses préférences de réception (cf. [SETTINGS.md](SETTINGS.md)), sans pouvoir désactiver la réception des alertes critiques relevant strictement de son périmètre de responsabilité.

## 9. Cohérence avec ORION

Toute alerte active peut être approfondie directement depuis le panneau ORION, qui en fournit l'explication et, lorsque pertinent, une recommandation d'action (cf. [ORION.md](ORION.md) §8, §17).
