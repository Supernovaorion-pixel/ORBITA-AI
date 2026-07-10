# ORBITA AI — Feature Specification: Reporting Engine

> Ce document spécifie le comportement fonctionnel complet du module Reporting Engine (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §5).

## 1. Objectif

Assembler les données et analyses de la plateforme en rapports formalisés, destinés au partage ou à l'archivage, conformément à [architecture/DATA_FLOW.md](../architecture/DATA_FLOW.md) §5.

## 2. Rapports

- Un rapport est généré à partir d'un modèle prédéfini (ex. synthèse mensuelle de direction, performance régionale, revue de compte client) ou à partir de l'état courant d'un écran consulté (Dashboard, Analytics).
- Un rapport généré est figé à son instant de création : il reflète fidèlement les données disponibles à ce moment, sans se recalculer rétroactivement si les données sources évoluent ensuite (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §Rapport).
- Chaque rapport indique explicitement son périmètre (territoire, équipe, produit), sa période et sa date de génération.

## 3. Modèles de rapport

Le catalogue officiel de modèles inclut a minima :
- Synthèse de performance (CA, Marge, Objectifs) pour un périmètre et une période donnés,
- Revue de compte (dédiée à un Client ou Grand Compte),
- Performance commerciale individuelle (dédiée à un Commercial),
- Performance produit/famille,
- Synthèse d'alertes actives sur une période.

De nouveaux modèles peuvent être ajoutés sans modification du moteur d'assemblage (cf. [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md) §5).

## 4. Exports

- Tout rapport généré peut être extrait dans un format d'usage externe (cf. §6-8), via le module Export Engine (cf. [EXPORT_ENGINE.md](EXPORT_ENGINE.md)).
- L'export d'un rapport conserve fidèlement sa mise en forme et son contenu tels que générés.

## 5. Planification

- Un rapport peut être planifié pour une génération récurrente (quotidienne, hebdomadaire, mensuelle, trimestrielle), à destination d'un ou plusieurs utilisateurs désignés.
- La planification précise le modèle utilisé, le périmètre, la période glissante à couvrir et les destinataires.
- Un rapport planifié qui échoue à se générer (ex. donnée source indisponible) déclenche une notification à son propriétaire plutôt qu'un échec silencieux (cf. [NOTIFICATION_CENTER.md](NOTIFICATION_CENTER.md)).

## 6. Historique

- L'ensemble des rapports générés, qu'ils soient ponctuels ou issus d'une planification, est conservé et consultable depuis l'écran Rapports (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §5).
- L'historique permet de retrouver un rapport précédemment partagé sans avoir à le régénérer.

## 7. Format PDF

- Format destiné au partage et à l'archivage formel (comité de direction, revue de compte) ; mise en page fixe garantissant une lecture identique quel que soit l'outil utilisé pour l'ouvrir.
- Toujours disponible pour l'ensemble des modèles de rapport du catalogue officiel.

## 8. Format Excel

- Format destiné à l'exploitation ultérieure de la donnée (recoupement, retraitement) ; conserve la donnée sous forme structurée et exploitable, au-delà de sa seule mise en forme visuelle.
- Disponible pour les rapports comportant des tableaux de détail significatifs.

## 9. Format PowerPoint

- Format destiné à la présentation orale (comité, réunion d'équipe) ; structuré en diapositives reprenant la hiérarchie de synthèse du rapport (indicateurs clés en ouverture, détail par la suite).
- Disponible pour les modèles de rapport de synthèse (Direction, Performance régionale).

## 10. Principe de fidélité

Quel que soit le format d'extraction choisi (§7-9), le contenu chiffré et les constats présentés restent rigoureusement identiques : seule la mise en forme diffère selon l'usage visé du format.
