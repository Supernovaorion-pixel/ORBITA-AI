# ORBITA AI — Module Dependencies

> Ce document définit les règles de dépendance autorisées entre les modules définis dans [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md). Aucune implémentation n'est décrite.

## 1. Principe général

Une dépendance entre deux modules n'est légitime que si elle reflète une relation fonctionnelle réelle et durable (cf. table de [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md) §Synthèse). Toute dépendance doit passer par l'interface explicite d'un module ou par le système d'événements (cf. [EVENT_SYSTEM.md](EVENT_SYSTEM.md)), jamais par un accès direct à l'état interne d'un autre module.

## 2. Hiérarchie de dépendance autorisée

Le système est organisé en trois niveaux de dépendance, du plus stable au plus périphérique :

**Niveau 1 — Fondation**
`Core`, `Security`, `Licensing`

**Niveau 2 — Moteurs et services fonctionnels**
`Import Engine`, `Analytics Engine`, `Forecast Engine`, `Reporting Engine`, `Export Engine`, `History`, `Audit`, `Settings`, `Administration`, `Update Manager`

**Niveau 3 — Restitution et extension**
`Dashboard`, `ORION`, `Notification Center`, `Plugin System`

### Règle de circulation
- Un module d'un niveau donné peut dépendre de tout module d'un niveau strictement inférieur (plus proche du Niveau 1).
- Un module ne peut jamais dépendre d'un module de niveau supérieur ou de même niveau, sauf via le système d'événements (communication indirecte, non une dépendance directe).
- `Core` (Niveau 1) ne dépend d'aucun autre module du système.

## 3. Dépendances explicitement autorisées

| Module | Peut dépendre de |
|---|---|
| Core | — |
| Security | Core |
| Licensing | Core |
| Import Engine | Core, Security |
| Analytics Engine | Core, Import Engine, History |
| Forecast Engine | Core, Analytics Engine, History |
| Reporting Engine | Core, Analytics Engine, Forecast Engine, Export Engine |
| Export Engine | Core, Security |
| History | Core |
| Audit | Core, Security |
| Settings | Core |
| Administration | Core, Security, Licensing, Settings |
| Update Manager | Core |
| Dashboard | Core, Analytics Engine, Forecast Engine, Settings |
| ORION | Core, Analytics Engine, Forecast Engine, History (via événements pour Notification Center) |
| Notification Center | Core, Settings (déclenché par événements des moteurs) |
| Plugin System | Core, Licensing |

## 4. Dépendances explicitement interdites

- **Aucun module de Niveau 1** (`Core`, `Security`, `Licensing`) ne peut dépendre d'un module de Niveau 2 ou 3.
- **Aucun moteur fonctionnel** (`Import Engine`, `Analytics Engine`, `Forecast Engine`, `Reporting Engine`) ne peut dépendre directement de `Dashboard`, `ORION` ou `Notification Center` (la restitution ne doit jamais influencer le calcul).
- **`Import Engine` ne dépend jamais de `Analytics Engine` ou `Forecast Engine`** : l'entrée de la donnée est indépendante de son analyse.
- **`Plugin System` ne peut jamais être une dépendance d'un module natif** : les modules du noyau ignorent l'existence des plugins qu'ils rendent possibles (cf. [EXTENSIBILITY.md](EXTENSIBILITY.md)) ; c'est le Plugin System qui s'interface avec eux, jamais l'inverse.
- **Aucun module ne peut dépendre de `Update Manager`** : la mise à jour orchestre le système de l'extérieur, elle n'est jamais un service invoqué par la logique métier.

## 5. Interdiction des dépendances circulaires

- Une dépendance circulaire (module A dépend de module B qui dépend directement ou indirectement de module A) est strictement interdite, sans exception.
- Toute évolution qui introduirait une dépendance circulaire doit être résolue par l'introduction d'une interface partagée au niveau du `Core`, ou par le passage à une communication événementielle asynchrone (cf. [EVENT_SYSTEM.md](EVENT_SYSTEM.md)), jamais par une exception ponctuelle à la présente règle.
- La vérification de l'absence de dépendance circulaire fait partie des conditions de validation avant toute fusion de code (cf. [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §9).

## 6. Cas particulier : ORION

`ORION` a un statut transversal (cf. [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md) §7) : il peut consulter les résultats de plusieurs moteurs (Niveau 2), mais aucun moteur ne doit jamais dépendre d'ORION en retour. ORION communique vers `Dashboard` et `Notification Center` exclusivement par le système d'événements, jamais par appel direct.

## 7. Cas particulier : Plugin System et connecteurs

Les connecteurs ERP/CRM et extensions futures (cf. [EXTENSIBILITY.md](EXTENSIBILITY.md)) s'intègrent exclusivement via le `Plugin System`, qui dépend du `Core` et du `Licensing`. Un plugin ne peut jamais introduire de dépendance vers un module natif autrement que par les interfaces que ce module expose publiquement.
