# ORBITA AI — Feature Specification: Import Engine

> Ce document spécifie le comportement fonctionnel complet du module **Import Engine** (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §2). Aucune technologie, aucun format de fichier technique précis n'est imposé : ce document décrit le comportement attendu, pas son implémentation.

## 1. Objectif du module

Le module Import Engine reçoit, contrôle et intègre les données commerciales externes d'une Organisation dans sa source de référence, conformément à [architecture/DATA_FLOW.md](../architecture/DATA_FLOW.md) §2.

## 2. Formats acceptés

- Fichiers tabulaires structurés (feuilles de calcul, fichiers texte délimités).
- Flux structurés provenant de connecteurs ERP/CRM (cf. [CONNECTORS.md](CONNECTORS.md)).
- Un import ne peut porter que sur un seul type d'entité à la fois (ex. Clients, Ventes, Produits) : un fichier mélangeant plusieurs types d'entités est rejeté avec un message explicite.

## 3. Détection automatique

- Le module détecte automatiquement la structure du fichier importé : présence d'une ligne d'en-tête, séparateur utilisé, encodage des caractères.
- Le module identifie automatiquement le type d'entité probable (Client, Produit, Vente, Objectif) à partir des colonnes présentes, et propose cette identification à l'utilisateur pour confirmation avant intégration.
- Si la structure ne peut être déterminée avec un niveau de confiance suffisant, l'import est suspendu et l'utilisateur est invité à préciser manuellement la nature du fichier.

## 4. Correspondance des colonnes (mapping)

- Le module propose une correspondance automatique entre les colonnes du fichier importé et les champs attendus du Domaine (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md)), fondée sur la ressemblance des libellés de colonnes.
- L'utilisateur peut corriger manuellement toute correspondance avant validation ; aucune donnée n'est intégrée tant que chaque colonne obligatoire n'est pas associée à un champ du Domaine.
- Une correspondance validée pour une source donnée est mémorisée et réutilisée automatiquement lors des imports suivants issus de la même source, jusqu'à modification explicite.

## 5. Validation

- Chaque ligne du fichier est contrôlée avant intégration selon des règles de validation propres à chaque type d'entité (format de date, valeur numérique attendue, référence à une entité existante).
- Une ligne invalide n'empêche pas l'intégration des lignes valides du même fichier : l'import distingue systématiquement ce qui est accepté de ce qui est rejeté.
- Les doublons (ligne déjà présente selon une clé d'identification propre à l'entité) sont détectés et signalés avant intégration, jamais intégrés silencieusement en double.

## 6. Prévisualisation

- Avant validation définitive, l'utilisateur consulte un aperçu représentatif des données telles qu'elles seront intégrées (échantillon significatif de lignes), avec mise en évidence des lignes en anomalie.
- La prévisualisation affiche le résumé quantitatif de l'import à venir : nombre de lignes acceptées, nombre de lignes en erreur, nombre de doublons détectés.
- Aucune donnée n'est intégrée à la source de référence tant que l'utilisateur n'a pas validé explicitement l'import depuis cet aperçu.

## 7. Historique

- Chaque import réalisé (réussi, partiel ou en échec) est conservé et consultable depuis l'écran Import (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §3), avec sa date, son origine, son statut et son détail.
- L'historique des imports alimente le module History (cf. [architecture/DATA_FLOW.md](../architecture/DATA_FLOW.md) §7).

## 8. Erreurs

- Toute ligne rejetée est accompagnée d'un motif de rejet explicite et localisé (numéro de ligne, colonne concernée, nature de l'anomalie).
- Un import peut être relancé après correction de la source, sans conserver les anomalies de la tentative précédente.
- Les règles générales de représentation des erreurs suivent [ux/ERROR_STATES.md](../ux/ERROR_STATES.md) §2.2.

## 9. Fusion

- Lorsqu'une donnée importée correspond à une entité déjà existante dans la source de référence (ex. un Client déjà connu), le module propose une **fusion** : les champs non renseignés dans la donnée existante sont complétés, les champs renseignés dans les deux sources font l'objet d'une règle de priorité explicite (la donnée la plus récente prévaut, sauf configuration contraire définie par l'Organisation).
- Une fusion ne supprime jamais silencieusement une information déjà présente : toute divergence est tracée dans l'historique de l'entité concernée.

## 10. Remplacement

- Un import peut être configuré en mode **remplacement** : les données d'un périmètre donné (ex. objectifs d'une période) sont intégralement remplacées par le contenu du nouvel import, plutôt que fusionnées.
- Le remplacement est une action explicitement choisie par l'utilisateur avant validation, jamais un comportement par défaut silencieux.
- L'état remplacé reste consultable via l'Historique (cf. [architecture/DATA_FLOW.md](../architecture/DATA_FLOW.md) §7), aucune donnée n'étant jamais définitivement effacée par un remplacement.

## 11. Import incrémental

- Mode par défaut : seules les données nouvelles ou modifiées depuis le dernier import sont intégrées, les entités déjà connues et inchangées n'étant pas retraitées.
- Adapté aux imports récurrents et automatisés (connecteurs, cf. [CONNECTORS.md](CONNECTORS.md)).

## 12. Import complet

- Mode alternatif dans lequel l'intégralité du périmètre couvert par le fichier est traitée, y compris les entités déjà connues, avec application des règles de fusion (§9) à chacune.
- Recommandé lors de la première intégration d'une source ou d'une reprise de données.

## 13. Qualité des données

- Le module calcule, pour chaque import, un indicateur de qualité synthétique (proportion de lignes acceptées sans anomalie, complétude des champs non obligatoires mais recommandés).
- Cet indicateur est consultable dans le rapport d'import (§14) et contribue à la fiabilité perçue de l'ensemble de la source de référence de l'Organisation.

## 14. Rapport d'import

À l'issue de chaque import, un rapport est généré et conservé, comportant :
- la date, l'origine et le mode (incrémental ou complet) de l'import,
- le nombre de lignes traitées, acceptées, fusionnées, rejetées et en doublon,
- l'indicateur de qualité des données (§13),
- le détail des anomalies rencontrées (§8).

Ce rapport est accessible depuis l'écran Import et peut être exporté (cf. [EXPORT_ENGINE.md](EXPORT_ENGINE.md)).
