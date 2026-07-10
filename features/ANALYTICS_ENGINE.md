# ORBITA AI — Feature Specification: Analytics Engine

> Ce document spécifie le comportement fonctionnel complet du module Analytics Engine (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §3).

## 1. Objectif

Produire des analyses comparatives et explicatives à partir des données consolidées de l'Organisation, permettant d'explorer la performance commerciale au-delà de la synthèse offerte par le Dashboard.

## 2. CA (Chiffre d'affaires)

- Calculé sur toute période et tout périmètre sélectionné (territoire, équipe, produit, famille, client).
- Décomposable par n'importe lequel de ces axes simultanément (ex. CA par territoire et par famille de produit).

## 3. Marge

- Calculée en valeur et en pourcentage du CA (Marge %), pour tout périmètre et toute période.
- Analysable en décomposition, au même titre que le CA (§2), afin d'identifier les périmètres à faible rentabilité malgré un CA élevé.

## 4. Évolution

- Variation d'un indicateur entre deux périodes comparables (période précédente, même période de l'année précédente, ou toute plage personnalisée).
- Présentée en valeur absolue et en pourcentage, jamais l'une sans l'autre.

## 5. Comparatifs

- Comparaison simultanée de plusieurs périmètres de même nature (ex. plusieurs territoires entre eux, plusieurs commerciaux entre eux) sur un même indicateur et une même période.
- Comparaison d'un périmètre à la moyenne ou à la médiane de son ensemble de référence (ex. un commercial comparé à la moyenne de son équipe).

## 6. Top / Flop

- Classement des entités (clients, produits, commerciaux, territoires) selon un indicateur choisi (CA, Marge, Évolution), sur une période donnée.
- Le nombre d'entités affichées dans un Top/Flop est configurable par l'utilisateur (ex. Top 10, Top 20).
- Un Flop met en évidence les entités en plus forte baisse ou les plus faibles contributions, jamais uniquement les plus faibles valeurs absolues sans tenir compte de leur évolution lorsque cette lecture est pertinente.

## 7. Pareto

- Analyse de concentration identifiant la part des entités (clients, produits) responsable de l'essentiel d'un indicateur (règle des 80/20), cf. [ux/DATA_VISUALIZATION.md](../ux/DATA_VISUALIZATION.md) — graphique Pareto.
- Permet de répondre à des questions telles que "quels clients représentent 80% du CA".

## 8. Saisonnalité

- Mise en évidence de motifs récurrents dans le temps (variations mensuelles ou trimestrielles typiques), calculée à partir de plusieurs années d'historique lorsque disponibles.
- Utilisée pour distinguer une variation normale saisonnière d'un écart réellement anormal (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md)).

## 9. Analyse régionale

- Décomposition de la performance par territoire (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md)), avec comparaison entre territoires et suivi de leur évolution respective.

## 10. Analyse commerciale

- Décomposition de la performance par Commercial, incluant l'atteinte de ses objectifs individuels, son évolution et son positionnement au sein de son équipe (cf. [SALES_ANALYTICS.md](SALES_ANALYTICS.md)).

## 11. Analyse client

- Décomposition de la performance par Client, incluant son historique, sa fréquence d'achat, son panier moyen et sa tendance (cf. [CLIENT_MANAGEMENT.md](CLIENT_MANAGEMENT.md)).

## 12. Analyse produit

- Décomposition de la performance par Produit, incluant sa contribution au CA et à la marge globale, et sa rotation (cf. [PRODUCT_ANALYTICS.md](PRODUCT_ANALYTICS.md)).

## 13. Analyse famille

- Décomposition de la performance au niveau d'une Famille de produits, avec possibilité de descendre au niveau des produits qui la composent (cf. [PRODUCT_ANALYTICS.md](PRODUCT_ANALYTICS.md)).

## 14. Analyse grands comptes

- Analyse dédiée aux clients désignés comme stratégiques (Grands Comptes, cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §12), intégrant un suivi de tendance long terme et une mise en évidence des risques de perte d'activité, au-delà de l'analyse client standard (§11).

## 15. Analyse des pertes

- Identification des baisses significatives de performance sur un périmètre donné (client, produit, territoire) entre deux périodes, avec mise en évidence des contributeurs principaux à cette baisse.
- Alimente le système d'Alertes lorsque le seuil de significativité configuré est franchi (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md)).

## 16. Analyse des gains

- Identification symétrique des hausses significatives de performance, permettant de valoriser les réussites (nouveaux clients, croissance produit) au même titre que les pertes sont surveillées.

## 17. Principe transversal de l'Analytics Engine

Toute analyse produite par ce module reste strictement dérivée des données consolidées existantes (cf. [architecture/DATA_FLOW.md](../architecture/DATA_FLOW.md) §4) : aucune analyse ne modifie la donnée source, et toute analyse doit pouvoir être retracée jusqu'aux données qui la fondent (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §2).
