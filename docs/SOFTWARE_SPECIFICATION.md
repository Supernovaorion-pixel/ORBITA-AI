# ORBITA AI — Software Specification

> Ce document décrit le logiciel dans son ensemble, du point de vue de ce qu'il fait pour l'utilisateur. Il ne fait référence à aucune technologie, langage ou architecture d'implémentation — ces sujets relèvent de [TECHNICAL_SPECIFICATION.md](TECHNICAL_SPECIFICATION.md).

## 1. Nature du logiciel

ORBITA AI est une plateforme logicielle premium d'intelligence commerciale. Elle centralise, structure et analyse les données de performance commerciale d'une organisation, et met cette analyse à disposition de ses utilisateurs à travers une interface unifiée, assistée par ORION.

## 2. Fonctionnalités attendues (vue d'ensemble)

### 2.1 Centralisation de la donnée
Le logiciel doit être capable de recevoir et consolider des données commerciales issues de sources multiples (historique de ventes, pipeline, objectifs, données de territoire, données client), afin de constituer une source de référence unique et cohérente.

### 2.2 Visualisation et pilotage
Le logiciel doit présenter cette donnée sous forme de tableaux de bord clairs, hiérarchisés selon le rôle de l'utilisateur qui consulte, avec des indicateurs de performance pertinents à chaque niveau de l'organisation (national, régional, individuel).

### 2.3 Analyse
Le logiciel doit permettre d'explorer la donnée en profondeur : comparaison de périodes, de territoires, d'équipes ou de produits, identification de tendances et d'écarts par rapport aux objectifs.

### 2.4 Anticipation
Le logiciel doit être capable de projeter des tendances futures à partir des données historiques et courantes, afin d'anticiper la performance à venir plutôt que de seulement constater la performance passée.

### 2.5 Restitution
Le logiciel doit permettre de produire des rapports formalisés, destinés à être partagés ou archivés, reflétant fidèlement les données et analyses consultées à l'écran.

### 2.6 Export
Le logiciel doit permettre l'extraction des données et analyses vers des formats exploitables en dehors de la plateforme, pour les besoins ponctuels des utilisateurs.

### 2.7 Assistance intelligente
Le logiciel intègre un assistant, ORION, capable d'interpréter les données à la demande de l'utilisateur, de répondre à des questions sur la performance commerciale, et de mettre en évidence des signaux qui mériteraient l'attention du décideur (cf. [AI_OVERVIEW.md](AI_OVERVIEW.md)).

### 2.8 Alertes
Le logiciel doit pouvoir signaler de façon proactive tout événement significatif (écart important à un objectif, anomalie, opportunité identifiée), sans que l'utilisateur ait à le rechercher activement.

### 2.9 Historique et traçabilité
Le logiciel doit conserver un historique consultable des données et de leur évolution, permettant de reconstituer la trajectoire de la performance dans le temps.

### 2.10 Administration
Le logiciel doit permettre à une organisation cliente de gérer ses utilisateurs, leurs droits d'accès, et la configuration de son propre environnement, de façon autonome.

### 2.11 Gestion des licences
Le logiciel doit intégrer un système de licences déterminant les fonctionnalités et le niveau d'usage accessibles à chaque organisation cliente, selon son offre (cf. [BUSINESS_MODEL.md](BUSINESS_MODEL.md) et [LICENSE_SYSTEM.md](LICENSE_SYSTEM.md)).

### 2.12 Paramétrage
Le logiciel doit permettre à chaque organisation d'adapter certains éléments de configuration (structure organisationnelle, objectifs, préférences d'affichage) à sa propre réalité, sans intervention extérieure.

## 3. Portée fonctionnelle détaillée

Le détail de chacun de ces domaines fonctionnels — leur périmètre exact, leurs cas d'usage et leurs interactions — est défini dans [FUNCTIONAL_SPECIFICATION.md](FUNCTIONAL_SPECIFICATION.md).

## 4. Ce que le logiciel n'est pas

Pour clarifier le périmètre, ORBITA AI n'est explicitement pas :

- un outil de gestion de la relation client (CRM) destiné à la saisie quotidienne d'interactions commerciales,
- un outil de gestion de projet ou de tâches,
- un outil de messagerie ou de collaboration d'équipe,
- un outil de comptabilité ou de facturation.

ORBITA AI est conçu pour s'appuyer sur les données produites par ces systèmes, sans chercher à les remplacer.

## 5. Exigence générale de qualité

Chaque fonctionnalité livrée doit répondre à l'exigence suivante : elle doit permettre à un décideur de répondre plus vite et avec plus de confiance à une question réelle sur la performance commerciale de son organisation. Toute fonctionnalité qui ne satisfait pas ce critère ne relève pas de la vision du produit (cf. [PROJECT_VISION.md](PROJECT_VISION.md)).
