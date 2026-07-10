# ORBITA AI — Module Dependencies (Planning)

> Ce document définit, du point de vue de la planification du développement, quels modules peuvent être construits en parallèle et lesquels doivent attendre l'achèvement d'un autre. Il s'appuie sur les dépendances architecturales déjà fixées dans [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md), qu'il décline en calendrier d'exécution. Il ne modifie aucune règle de dépendance déjà établie.

## 1. Principe

Un module ne peut être développé qu'une fois les modules dont il dépend architecturalement (cf. [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md)) livrés et validés selon la [DEFINITION_OF_DONE.md](DEFINITION_OF_DONE.md). Deux modules sans relation de dépendance entre eux peuvent être développés en parallèle, par des équipes ou contributeurs distincts, sans risque d'interférence.

## 2. Modules devant être développés séquentiellement (aucun parallélisme possible)

| Module | Doit attendre | Raison |
|---|---|---|
| Core | — | Fondation de tout le système ; aucun autre module ne peut commencer avant lui (cf. [architecture/SYSTEM_ARCHITECTURE.md](../architecture/SYSTEM_ARCHITECTURE.md) §7). |
| Import Engine | Core | Dépend de Security et du Domaine portés par Core. |
| Analytics Engine | Import Engine, History | Analyse des données consolidées par l'Import Engine (cf. [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md) §3). |
| Forecast Engine | Analytics Engine | S'appuie directement sur les résultats de l'Analytics Engine. |
| Reporting Engine | Analytics Engine, Forecast Engine | Assemble les résultats des deux moteurs d'analyse. |
| Dashboard | Analytics Engine, Forecast Engine | Restitue les résultats déjà produits par ces moteurs. |
| ORION | Analytics Engine, Forecast Engine, History | Interprète les résultats déjà produits par ces modules. |
| Licensing | Core | Peut être développé tôt (dépend uniquement de Core) mais son **application complète** à tous les modules (Phase 10) requiert que ces modules existent déjà. |
| Plugin System | Core, Licensing | Nécessite la vérification de licence avant toute activation d'extension. |
| Connecteurs | Plugin System, Import Engine | Construits comme extensions rattachées au point d'extension de l'Import Engine. |

## 3. Modules pouvant être développés en parallèle

- **Security, History, Audit, Settings** (au sein de la Phase 2 — Core) : ces quatre modules dépendent uniquement de Core et n'ont pas de dépendance entre eux (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §Synthèse) ; ils peuvent être développés simultanément par des contributeurs distincts.
- **Client Management, Product Analytics, Sales Analytics** (au sein de la Phase 4) : ces trois modules dépendent tous de l'Analytics Engine mais n'ont pas de dépendance entre eux ; ils peuvent être développés en parallèle une fois l'Analytics Engine achevé.
- **Reporting Engine et Dashboard** (Phases 5 et 7) : bien que planifiées à des moments distincts pour des raisons de priorité produit (cf. [IMPLEMENTATION_ORDER.md](IMPLEMENTATION_ORDER.md)), ces deux modules n'ont pas de dépendance directe l'un envers l'autre au-delà de leur dépendance commune à l'Analytics Engine et au Forecast Engine ; un réordonnancement de leur développement respectif est possible sans effet de bord.
- **Alert System et Notification Center** (Phase 8) : peuvent être développés en parallèle d'ORION, sous réserve que l'intégration finale entre les trois (cf. [features/ORION.md](../features/ORION.md) §8) soit vérifiée avant la clôture de la phase.
- **Administration et Search Engine** (Phase 9) : ne dépendent l'un de l'autre en aucune façon ; développables en parallèle.

## 4. Modules ne pouvant jamais être développés en parallèle du Core

Aucun module fonctionnel (Import Engine à Connecteurs) ne peut débuter son développement avant la validation complète de la Phase 2 — Core, celle-ci portant le Domaine et les services transversaux dont dépend l'intégralité du système (cf. [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md) §2).

## 5. Cas particulier — Update Manager

Le module Update Manager (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §12) ne dépend que de Core et peut être développé à tout moment après la Phase 2, en parallèle de toute autre phase, sa mise en œuvre complète n'étant toutefois requise qu'à l'approche de la Phase 15 — Version Alpha, première version nécessitant un mécanisme de distribution.

## 6. Principe transversal

Le parallélisme entre modules ne dispense jamais du respect des [QUALITY_GATES.md](QUALITY_GATES.md) : un module développé en parallèle d'un autre au sein d'une même phase doit individuellement satisfaire la [DEFINITION_OF_DONE.md](DEFINITION_OF_DONE.md) avant que la phase ne soit considérée comme close.
