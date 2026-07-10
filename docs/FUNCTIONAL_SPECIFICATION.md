# ORBITA AI — Functional Specification

> Ce document décrit les grands modules fonctionnels du logiciel et leur périmètre. Il ne décrit ni leur implémentation technique, ni leur interface visuelle.

## 1. Import

**Objectif** : constituer et maintenir à jour la source de données de référence de l'organisation.

- Réception de données commerciales issues de sources externes (historique de ventes, pipeline, objectifs, référentiels de territoires et d'équipes, données client).
- Contrôle de cohérence et de qualité des données reçues avant intégration.
- Traçabilité de chaque import (origine, date, périmètre couvert).
- Gestion des mises à jour incrémentales, sans duplication ni perte d'historique.

## 2. Dashboard

**Objectif** : offrir une vision instantanée et hiérarchisée de la performance commerciale.

- Vue de synthèse adaptée au rôle de l'utilisateur consultant (cf. [USER_PERSONAS.md](USER_PERSONAS.md)).
- Indicateurs clés de performance (KPI) contextualisés par période, territoire, équipe ou produit.
- Comparaison immédiate entre performance réalisée et objectifs fixés.
- Signalement visuel des écarts significatifs nécessitant l'attention du décideur.

## 3. Analytics

**Objectif** : permettre l'exploration approfondie de la donnée commerciale.

- Analyse comparative multi-axes (période, territoire, équipe, produit, client).
- Décomposition d'un indicateur global en ses composantes.
- Identification de tendances, de saisonnalités et d'anomalies.
- Exploration libre guidée, sans nécessiter de compétence en analyse de données.

## 4. Forecast

**Objectif** : anticiper la performance commerciale future.

- Projection de tendances à partir des données historiques et de l'activité en cours.
- Estimation de l'atteinte des objectifs à échéance donnée.
- Mise en évidence des facteurs de risque ou d'opportunité identifiés dans les projections.
- Actualisation continue des projections à mesure que de nouvelles données sont disponibles.

## 5. Reporting

**Objectif** : formaliser et partager les constats issus de l'analyse.

- Génération de rapports structurés reflétant les données et analyses consultées.
- Modèles de rapport adaptés aux différents publics (direction, régional, opérationnel).
- Périodicité configurable (ponctuelle, régulière) pour les rapports récurrents.
- Conservation des rapports générés pour référence ultérieure.

## 6. Administration

**Objectif** : permettre à chaque organisation cliente de gérer son propre environnement.

- Gestion des utilisateurs et de leurs rôles au sein de l'organisation.
- Définition des droits d'accès par profil, territoire ou périmètre de données.
- Configuration de la structure organisationnelle représentée dans la plateforme (hiérarchie, territoires, équipes).
- Supervision de l'activité et de l'utilisation de la plateforme au sein de l'organisation.

## 7. Licences

**Objectif** : déterminer et faire respecter le périmètre d'usage contractuel de chaque organisation cliente.

- Activation des fonctionnalités correspondant à l'offre souscrite (cf. [BUSINESS_MODEL.md](BUSINESS_MODEL.md)).
- Suivi des limites d'usage associées à la licence (nombre d'utilisateurs, périmètre fonctionnel).
- Fonctionnement détaillé dans [LICENSE_SYSTEM.md](LICENSE_SYSTEM.md).

## 8. ORION

**Objectif** : assister l'utilisateur dans l'interprétation de la donnée et la prise de décision.

- Réponse aux questions posées en langage naturel sur la performance commerciale.
- Mise en évidence proactive de signaux pertinents (écarts, tendances, opportunités).
- Formulation de synthèses et de recommandations fondées sur les données disponibles.
- Rôle, capacités et limites décrits en détail dans [AI_OVERVIEW.md](AI_OVERVIEW.md).

## 9. Exports

**Objectif** : permettre l'utilisation des données et analyses en dehors de la plateforme.

- Extraction de données brutes ou agrégées vers des formats bureautiques standards.
- Extraction de rapports formalisés dans des formats destinés au partage ou à l'archivage.
- Conservation de la cohérence entre la donnée exportée et la donnée source au moment de l'export.

## 10. Paramètres

**Objectif** : adapter la plateforme à la réalité de chaque organisation.

- Configuration des objectifs commerciaux par période, territoire ou équipe.
- Personnalisation des indicateurs mis en avant selon les priorités de l'organisation.
- Préférences d'affichage et de notification par utilisateur.

## 11. Historique

**Objectif** : conserver et restituer la trajectoire de la performance dans le temps.

- Conservation de l'ensemble des données importées et de leurs évolutions successives.
- Consultation de l'état de la performance à une date ou période antérieure.
- Comparaison de trajectoires entre différentes périodes.

## 12. Alertes

**Objectif** : signaler de façon proactive les événements méritant l'attention du décideur.

- Détection d'écarts significatifs par rapport aux objectifs ou aux tendances attendues.
- Notification ciblée selon le rôle et le périmètre de responsabilité du destinataire.
- Configuration des seuils et types d'événements déclenchant une alerte.

## 13. Interactions entre modules

Les modules décrits ci-dessus ne sont pas indépendants : **Import** alimente l'ensemble de la plateforme ; **Dashboard**, **Analytics** et **Forecast** s'appuient sur cette donnée consolidée ; **Reporting** et **Exports** restituent les constats produits par ces modules ; **ORION** et **Alertes** opèrent de façon transversale, en s'appuyant sur l'ensemble des données et analyses disponibles ; **Administration**, **Licences** et **Paramètres** encadrent l'usage de la plateforme par chaque organisation cliente ; **Historique** garantit la continuité et la traçabilité de l'ensemble dans le temps.
