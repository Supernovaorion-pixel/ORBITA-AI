# ORBITA AI — Versioning Strategy

> Ce document définit la stratégie officielle de gestion des versions du logiciel. Aucun outil ou mécanisme technique de distribution n'est prescrit.

## 1. Principe général

ORBITA AI adopte le **Semantic Versioning (SemVer)** comme convention officielle de numérotation, garantissant que le numéro de version communique sans ambiguïté la nature d'un changement, aussi bien aux équipes internes qu'aux organisations clientes (particulièrement importantes pour les déploiements On-Premise, cf. [docs/BUSINESS_MODEL.md](../docs/BUSINESS_MODEL.md) §5).

## 2. Semantic Versioning

Une version est exprimée sous la forme `MAJEUR.MINEUR.CORRECTIF` :

- **MAJEUR** — incrémenté lors d'un changement qui rompt la compatibilité ascendante (cf. [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §8), par exemple une évolution du Domaine qui modifie le sens d'une entité existante.
- **MINEUR** — incrémenté lors de l'ajout d'une fonctionnalité ou d'un module, sans rupture de compatibilité (ex. ajout d'un nouveau connecteur, d'un nouveau type de rapport).
- **CORRECTIF** — incrémenté lors d'une correction de comportement, sans changement de périmètre fonctionnel.

Une rupture de compatibilité ascendante (incrément MAJEUR) doit toujours être une décision explicite et documentée (cf. [ARCHITECTURE_DECISIONS.md](ARCHITECTURE_DECISIONS.md)), jamais une conséquence non anticipée d'un changement mineur.

## 3. Canaux de version (maturité)

Chaque version transite, avant sa disponibilité générale, par les canaux de maturité suivants :

| Canal | Signification | Public concerné |
|---|---|---|
| **Alpha** | Version en développement actif, périmètre fonctionnel non stabilisé. | Usage interne exclusivement. |
| **Beta** | Version fonctionnellement complète pour le périmètre visé, en cours de validation. | Organisations clientes volontaires, dans un cadre encadré. |
| **RC (Release Candidate)** | Version considérée comme prête, en dernière vérification avant publication générale. | Validation finale, périmètre gelé (aucune nouvelle fonctionnalité ajoutée à ce stade). |
| **Stable** | Version publiée en disponibilité générale, recommandée pour un usage en production. | Toutes les Organisations clientes. |

Aucune version n'atteint le canal **Stable** sans être passée par les étapes de vérification propres aux canaux précédents (cf. [docs/PROJECT_RULES.md](../docs/PROJECT_RULES.md) §9).

## 4. LTS (Long Term Support)

- Certaines versions **Stable** sont désignées comme versions **LTS**, bénéficiant d'un support et de correctifs prolongés au-delà du cycle standard.
- Les versions LTS sont particulièrement destinées aux Organisations en déploiement **On-Premise** ou de licence **Enterprise** (cf. [docs/BUSINESS_MODEL.md](../docs/BUSINESS_MODEL.md)), pour lesquelles la stabilité prolongée prime sur l'accès immédiat aux dernières fonctionnalités.
- Une version LTS ne reçoit que des correctifs (incréments CORRECTIF) et des évolutions mineures rétrocompatibles (incréments MINEUR) ; elle ne reçoit jamais d'incrément MAJEUR sans devenir, de fait, une nouvelle ligne LTS distincte.

## 5. Application aux éditions commerciales

- La numérotation de version est unique et commune à l'ensemble du système, indépendamment de l'édition commerciale (Community à Enterprise) : une même version MAJEUR.MINEUR.CORRECTIF du Core et des modules natifs sous-tend toutes les éditions, seul le périmètre activé par la licence diffère (cf. [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md) §8, [docs/LICENSE_SYSTEM.md](../docs/LICENSE_SYSTEM.md)).
- Cette unicité garantit qu'aucune édition ne dérive du Core en une base de code distincte à maintenir séparément.

## 6. Application aux modes de déploiement

- Les organisations en mode **Cloud** bénéficient d'une mise à jour continue vers les versions **Stable** les plus récentes, orchestrée par le module `Update Manager` (cf. [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md) §12).
- Les organisations en mode **On-Premise** choisissent leur rythme de mise à jour, typiquement aligné sur les versions **LTS**, avec un accompagnement explicite lors d'un changement de version MAJEUR.

## 7. Versionnement des interfaces entre modules

- Les interfaces exposées par un module vers les autres (cf. [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md)) suivent le même principe de compatibilité : un changement qui romprait une interface existante constitue un changement MAJEUR pour le module concerné, quelle que soit l'ampleur du changement interne à ce module.
- Cette discipline garantit qu'un module peut évoluer en interne (refactorisation, amélioration de performance) sans imposer de changement aux modules qui en dépendent, tant que son interface reste stable.

## 8. Principe transversal

Le choix du canal et du numéro de version n'est jamais une formalité administrative : il communique une garantie de stabilité à l'ensemble des Organisations clientes, particulièrement critique pour les déploiements Enterprise et On-Premise. Toute incertitude sur la classification d'un changement (MAJEUR, MINEUR, CORRECTIF) doit être résolue en faveur de la classification la plus prudente.
