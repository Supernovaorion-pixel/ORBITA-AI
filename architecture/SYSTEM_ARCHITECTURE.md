# ORBITA AI — System Architecture

> Ce document définit l'architecture globale du système. Il ne mentionne aucun langage, framework, base de données ou technologie : il décrit exclusivement la structure logique que toute implémentation future devra respecter. Cette architecture est conçue pour rester valide au moins dix ans.

## 1. Vision globale

ORBITA AI est conçu comme un **système modulaire orchestré autour d'un noyau (Core)**, entouré de moteurs fonctionnels spécialisés (Import, Analytics, Forecast, Reporting, etc.) qui communiquent entre eux sans jamais dépendre les uns des autres directement (cf. [EVENT_SYSTEM.md](EVENT_SYSTEM.md)).

Cette architecture doit permettre, sans reconstruction :

- de décliner le logiciel en plusieurs éditions commerciales (Community, Starter, Business, Enterprise — cf. [docs/BUSINESS_MODEL.md](../docs/BUSINESS_MODEL.md)) par simple activation/désactivation de modules,
- de fonctionner en Cloud comme en On-Premise,
- d'accueillir de nouveaux connecteurs, de nouveaux moteurs d'intelligence artificielle, de nouvelles langues, de nouvelles organisations et de nouveaux plugins,
- d'exposer une API future sans remise en cause du noyau.

## 2. Responsabilités du système

Le système, dans son ensemble, a la responsabilité de :

1. **Représenter fidèlement** l'activité commerciale d'une ou plusieurs organisations clientes.
2. **Garantir l'intégrité** et la cohérence de cette représentation dans le temps.
3. **Mettre à disposition** cette représentation sous forme d'analyses, de visualisations et de recommandations.
4. **Encadrer l'accès** à cette représentation selon les droits, la licence et le rôle de chaque utilisateur.
5. **Rester stable** face à la croissance du volume de données, du nombre d'organisations et du nombre de fonctionnalités.

## 3. Séparation des responsabilités

Le système repose sur une séparation stricte entre :

- **ce qui représente le métier** (le Domaine — cf. [DOMAIN_MODEL.md](DOMAIN_MODEL.md) et [APPLICATION_LAYERS.md](APPLICATION_LAYERS.md)),
- **ce qui orchestre les cas d'usage** (l'Application),
- **ce qui restitue l'information à l'utilisateur** (la Présentation),
- **ce qui relie le système au monde extérieur** (l'Infrastructure : sources de données, connecteurs, environnement d'exécution).

Aucune couche ne doit porter une responsabilité qui appartient à une autre. Cette séparation est la condition de la pérennité de l'architecture : elle permet de remplacer une couche (par exemple l'infrastructure, lors d'un changement d'environnement d'exécution) sans toucher au Domaine, qui porte la valeur durable du système.

## 4. Architecture modulaire

Le système est découpé en **modules fonctionnels autonomes**, chacun couvrant une responsabilité unique et clairement délimitée (cf. [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md)). Un module :

- expose ce qu'il offre aux autres modules à travers une interface stable et explicite,
- ne connaît des autres modules que ce que cette interface expose,
- peut évoluer, être remplacé ou être désactivé sans affecter le fonctionnement des autres modules, tant que son interface reste respectée.

## 5. Découpage logique

Le découpage logique du système suit trois axes complémentaires :

- **Axe fonctionnel** : chaque module correspond à un domaine fonctionnel du produit (cf. [docs/FUNCTIONAL_SPECIFICATION.md](../docs/FUNCTIONAL_SPECIFICATION.md)).
- **Axe transversal (couches)** : chaque module est lui-même structuré selon les couches applicatives communes du système (cf. [APPLICATION_LAYERS.md](APPLICATION_LAYERS.md)).
- **Axe organisationnel** : le système traite nativement plusieurs organisations clientes, chacune cloisonnée logiquement des autres (multi-organisation, cf. [DOMAIN_MODEL.md](DOMAIN_MODEL.md) §Organisation).

## 6. Indépendance des modules

L'indépendance des modules est un principe non négociable de l'architecture :

- un module ne doit jamais accéder directement à l'état interne d'un autre module,
- toute communication inter-module transite par des interfaces explicites ou par le système d'événements (cf. [EVENT_SYSTEM.md](EVENT_SYSTEM.md)),
- les dépendances autorisées entre modules sont strictement encadrées (cf. [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md)), et toute dépendance circulaire est proscrite.

Cette indépendance garantit qu'une édition Community du logiciel peut n'embarquer qu'un sous-ensemble de modules, sans que cela ne compromette la cohérence du système, et qu'une édition Enterprise peut ajouter des modules (plugins, connecteurs) sans modifier le noyau.

## 7. Position du Core

Le module **Core** (cf. [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md) §1) constitue le socle commun : il porte le Domaine, les règles transversales (sécurité, licences, événements) et n'a de dépendance vers aucun autre module. Tous les autres modules peuvent dépendre du Core ; le Core ne dépend jamais d'eux.

## 8. Principe de stabilité à long terme

Toute décision d'architecture doit être évaluée à l'aune d'une question : *"Cette décision tiendra-t-elle si le système double de volume, double de modules, ou change d'environnement d'exécution ?"* Une décision qui ne résiste pas à cette question doit être reconsidérée avant d'être actée (cf. [ARCHITECTURE_DECISIONS.md](ARCHITECTURE_DECISIONS.md)).
