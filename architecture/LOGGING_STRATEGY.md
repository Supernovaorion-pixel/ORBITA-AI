# ORBITA AI — Logging Strategy

> Ce document définit les principes de journalisation du système. Aucun outil ou format technique n'est prescrit.

## 1. Objectif

La journalisation d'ORBITA AI répond à quatre finalités distinctes, chacune avec ses propres exigences : l'**audit** (traçabilité et conformité), le **diagnostic** (compréhension d'un dysfonctionnement), le **support** (assistance à un utilisateur ou une organisation) et les **logs techniques généraux** (fonctionnement courant du système). Ces quatre finalités ne doivent jamais être confondues dans un flux unique indifférencié.

## 2. Logs (journal technique général)

- Enregistrent le déroulement courant du système : démarrage, achèvement d'un traitement, événements internes significatifs (cf. [EVENT_SYSTEM.md](EVENT_SYSTEM.md)).
- Destinés en priorité à la supervision du bon fonctionnement du système, pas à l'analyse fine d'un incident.
- Doivent permettre de distinguer un fonctionnement normal d'une anomalie sans ambiguïté.

## 3. Audit

- Enregistre toute action significative affectant les données ou la configuration d'une Organisation : accès, création, modification, suppression, changement de droits ou de licence (cf. module `Audit`, [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md) §17).
- Répond systématiquement aux trois questions : **qui**, **quoi**, **quand** (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §4).
- Les journaux d'audit sont conservés dans des conditions garantissant leur intégrité : une fois enregistrée, une entrée d'audit ne peut jamais être modifiée ou supprimée, y compris par un Administrateur.
- Les journaux d'audit sont strictement cloisonnés par Organisation, au même titre que toute autre donnée du Domaine (cf. [DOMAIN_MODEL.md](DOMAIN_MODEL.md) §4).

## 4. Diagnostic

- Destiné à comprendre la cause d'un dysfonctionnement ou d'une erreur (cf. [ERROR_HANDLING.md](ERROR_HANDLING.md)).
- Peut contenir un niveau de détail technique plus fin que les logs généraux, mais reste soumis aux mêmes exigences de confidentialité : aucune donnée métier sensible d'une Organisation ne doit apparaître en clair dans un journal de diagnostic accessible à un périmètre technique large.
- Doit permettre de relier un incident constaté à son contexte exact (module concerné, séquence d'opérations), sans nécessiter de reproduire l'incident pour le comprendre.

## 5. Support

- Ensemble d'informations mobilisables pour assister une Organisation cliente rencontrant une difficulté d'usage.
- S'appuie sur les journaux d'audit et de diagnostic existants plutôt que de constituer un flux de journalisation séparé et redondant (cf. principe DRY, [CODING_PRINCIPLES.md](CODING_PRINCIPLES.md)).
- L'accès aux informations de support relatives à une Organisation est strictement limité aux personnes habilitées à intervenir pour cette Organisation.

## 6. Niveaux de gravité

Toute entrée de journal (hors audit, qui n'a pas de notion de gravité graduée) est associée à un niveau de gravité explicite, permettant de distinguer :
- l'information courante (fonctionnement normal),
- l'avertissement (situation à surveiller, sans impact immédiat),
- l'erreur (échec d'une opération, cf. [ERROR_HANDLING.md](ERROR_HANDLING.md)),
- l'incident critique (impact potentiel sur la disponibilité ou l'intégrité du système).

## 7. Rétention

- La durée de conservation de chaque type de journal est définie selon sa finalité : les journaux d'audit sont conservés sur une durée longue, cohérente avec les obligations de conformité de l'Organisation cliente ; les journaux de diagnostic peuvent être conservés sur une durée plus courte, suffisante pour couvrir le délai raisonnable de détection d'un incident.
- La rétention des journaux respecte les mêmes principes de gestion des données que ceux définis dans [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §7.

## 8. Principe transversal

La journalisation ne doit jamais être ajoutée après coup à une fonctionnalité : elle est conçue en même temps que le comportement qu'elle documente, au même titre que la gestion des erreurs (cf. [ERROR_HANDLING.md](ERROR_HANDLING.md) §7).
