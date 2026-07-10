# ORBITA AI — Extensibility

> Ce document définit les principes garantissant que le système peut accueillir de nouvelles capacités sans être reconstruit. Aucune technologie de plugin n'est prescrite.

## 1. Principe directeur

L'extensibilité d'ORBITA AI repose sur un principe simple : **le noyau ignore l'existence de ce qui l'étend**. Toute extension future (connecteur, moteur, rapport, dashboard) s'ajoute au système sans modifier le Core ni les modules natifs, conformément à la règle du Plugin System définie dans [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md) §7.

## 2. Mécanisme général

- Chaque module natif expose des **points d'extension** explicites (interfaces stables) à travers lesquels une extension peut s'intégrer, sans jamais accéder à autre chose que ce que ces points exposent.
- Le module **Plugin System** (cf. [MODULE_ARCHITECTURE.md](MODULE_ARCHITECTURE.md) §13) est seul responsable de la découverte, de l'activation et de l'isolation des extensions.
- Une extension ne peut jamais modifier le comportement d'un module natif au-delà de ce que son point d'extension autorise explicitement.

## 3. Plugins

- Un plugin est une capacité additionnelle, développée indépendamment du cœur du logiciel, activée ou désactivée sans redéploiement du noyau.
- Un plugin est toujours rattaché à un point d'extension d'un module précis (par exemple, un nouveau type de rapport s'intègre au point d'extension du `Reporting Engine`).
- Un plugin est soumis au même cloisonnement par Organisation que le reste du système (cf. [DOMAIN_MODEL.md](DOMAIN_MODEL.md) §4) et aux mêmes règles de sécurité (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md)).

## 4. Connecteurs ERP et CRM

- Les connecteurs ERP et CRM sont une catégorie particulière d'extension, rattachée au point d'extension de l'`Import Engine` : ils traduisent une source de données externe vers le format attendu par le système, sans que l'`Import Engine` n'ait à connaître la nature de la source d'origine.
- Un nouveau connecteur peut être ajouté sans modification du module `Import Engine` lui-même, tant qu'il respecte l'interface d'entrée attendue.
- Plusieurs connecteurs peuvent coexister pour une même Organisation, chacun alimentant la même source de référence consolidée.

## 5. Nouveaux dashboards et nouveaux rapports

- `Dashboard` et `Reporting Engine` exposent chacun un point d'extension permettant l'ajout de nouvelles vues ou de nouveaux modèles de restitution, sans modification de leur logique d'assemblage existante.
- Une nouvelle vue ou un nouveau modèle de rapport consomme les résultats déjà produits par les moteurs d'analyse (`Analytics Engine`, `Forecast Engine`) via leurs interfaces stables, sans dupliquer leur logique de calcul (cf. principe DRY, [CODING_PRINCIPLES.md](CODING_PRINCIPLES.md)).

## 6. Nouveaux moteurs d'intelligence artificielle

- `ORION` est conçu pour pouvoir s'appuyer, à terme, sur plusieurs moteurs d'interprétation ou de recommandation, chacun rattaché à un point d'extension dédié.
- L'ajout d'un nouveau moteur ne modifie ni le Domaine, ni les autres moteurs fonctionnels (`Analytics Engine`, `Forecast Engine`) : il consomme les mêmes résultats consolidés qu'eux, via les mêmes interfaces stables.
- Le choix du ou des moteurs actifs pour une Organisation relève de la configuration (`Settings`) et de sa licence (`Licensing`), jamais d'une modification du Domaine.

## 7. Plusieurs langues

- La prise en charge de nouvelles langues est un point d'extension transversal, indépendant du Domaine et de la logique métier : le Domaine manipule des concepts, jamais des libellés dans une langue donnée.
- L'ajout d'une langue se traduit par l'ajout de ressources de traduction, sans modification d'aucun module fonctionnel.

## 8. Plusieurs organisations

- Le cloisonnement par Organisation (cf. [DOMAIN_MODEL.md](DOMAIN_MODEL.md) §4) est un invariant du Domaine dès l'origine : l'ajout d'une nouvelle Organisation cliente ne nécessite aucune évolution du système, il s'agit d'une nouvelle instance des mêmes entités.

## 9. Mises à jour automatiques

- Le module `Update Manager` est conçu pour permettre la distribution progressive de nouvelles versions du système sans interruption de service, en environnement Cloud comme On-Premise (cf. [VERSIONING_STRATEGY.md](VERSIONING_STRATEGY.md)).
- L'extensibilité du système (ajout de plugins, connecteurs, moteurs) est conçue pour rester compatible avec ce mécanisme de mise à jour continue, sans nécessiter d'intervention manuelle sur les extensions déjà installées.

## 10. API future

- Bien qu'aucune API ne soit développée à ce stade, l'architecture en couches (cf. [APPLICATION_LAYERS.md](APPLICATION_LAYERS.md)) garantit qu'une future API ne serait qu'une nouvelle **Présentation** supplémentaire, s'appuyant sur les cas d'usage déjà exposés par la couche Application de chaque module.
- Aucune règle métier ne doit jamais être développée au niveau d'une présentation (actuelle ou future) : cette discipline est ce qui rendra une future API immédiatement réalisable sans réécriture du Domaine.

## 11. Principe de validation de toute extension

Toute nouvelle extension, quelle que soit sa nature, doit être évaluable au regard d'une question simple : *"Cette extension nécessite-t-elle une modification du noyau existant ?"* Si la réponse est oui, ce n'est pas l'extension qui doit être adaptée, mais le point d'extension du module concerné qui doit être revu — jamais une exception ponctuelle aux règles de [MODULE_DEPENDENCIES.md](MODULE_DEPENDENCIES.md).
