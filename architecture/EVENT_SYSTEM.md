# ORBITA AI — Event System

> Ce document définit les principes du système de communication interne entre modules. Aucun mécanisme technique (file de messages, bus logiciel particulier) n'est prescrit : uniquement les règles de conception que ce système devra respecter.

## 1. Objectif

Le système d'événements permet à un module de signaler qu'un fait s'est produit, sans avoir à connaître ni à appeler directement les modules susceptibles de s'y intéresser. Il est le principal mécanisme garantissant l'indépendance des modules définie dans [SYSTEM_ARCHITECTURE.md](SYSTEM_ARCHITECTURE.md) §6 et les règles de [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md).

## 2. Principe fondamental

- Un module **émet** un événement pour signaler un fait accompli ("un import a été finalisé", "un écart significatif a été détecté"), sans connaître qui, le cas échéant, réagira à ce fait.
- Un module **s'abonne** à un événement qui l'intéresse, sans que le module émetteur n'ait besoin de connaître son existence.
- Cette relation émetteur/abonné remplace tout appel direct entre modules qui ne relève pas d'une dépendance fonctionnelle stable et assumée (cf. [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md)).

## 3. Ce qu'un événement représente

- Un événement décrit un **fait passé**, jamais une instruction ("ImportTermine", pas "LancerAnalyse").
- Un événement porte l'information strictement nécessaire pour que ses abonnés puissent réagir (identité de l'Organisation concernée, nature du fait, référence à l'entité de Domaine concernée), sans exposer l'état interne du module émetteur.
- Le vocabulaire des événements suit strictement les termes du [Glossaire](../docs/GLOSSARY.md).

## 4. Exemples de circulation (à titre d'illustration fonctionnelle)

- `Import Engine` émet un événement lorsqu'un import est finalisé → `Analytics Engine` et `History` s'y abonnent pour déclencher respectivement une analyse et une historisation, sans qu'`Import Engine` ne les connaisse.
- `Analytics Engine` ou `Forecast Engine` émettent un événement lorsqu'un écart significatif est détecté → `Notification Center` s'y abonne pour générer une alerte, et `ORION` s'y abonne pour pouvoir en rendre compte à l'utilisateur.
- `Licensing` émet un événement lors d'un changement de périmètre de licence → tous les modules concernés s'y abonnent pour ajuster leur comportement, sans que `Licensing` n'ait à connaître chacun d'eux.

## 5. Pourquoi éviter les dépendances directes

- Un appel direct entre deux modules crée un couplage qui rend chacun d'eux plus difficile à faire évoluer ou remplacer isolément.
- Le système d'événements permet d'ajouter un nouveau module abonné à un fait existant (par exemple, un futur module d'analyse prédictive) sans modifier le module émetteur — condition essentielle de l'extensibilité à long terme (cf. [EXTENSIBILITY.md](EXTENSIBILITY.md)).
- Il permet également de désactiver un module (par exemple dans une édition Community qui n'embarque pas `Notification Center`) sans que les modules émetteurs n'aient à en tenir compte.

## 6. Garanties attendues du système d'événements

- **Traçabilité** : toute émission d'événement significatif doit pouvoir être retracée (cf. [LOGGING_STRATEGY.md](LOGGING_STRATEGY.md), module `Audit`).
- **Non-blocage** : l'émission d'un événement ne doit jamais bloquer le fonctionnement du module émetteur en attendant la réaction de ses abonnés.
- **Résilience** : l'absence ou la défaillance d'un abonné ne doit jamais empêcher le module émetteur de poursuivre son fonctionnement normal.
- **Cloisonnement par Organisation** : un événement émis dans le périmètre d'une Organisation ne doit jamais être reçu par un abonné agissant pour une autre Organisation.

## 7. Limites du système d'événements

- Le système d'événements ne remplace pas les dépendances fonctionnelles stables et assumées définies dans [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md) (par exemple, `Analytics Engine` interroge directement les données consolidées via une interface stable, il n'attend pas un événement pour cela).
- Il est réservé aux communications où le module émetteur n'a pas, et ne doit pas avoir, à connaître ses destinataires.
