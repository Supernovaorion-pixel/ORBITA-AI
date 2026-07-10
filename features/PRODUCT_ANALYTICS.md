# ORBITA AI — Feature Specification: Product Analytics

> Ce document spécifie le comportement fonctionnel complet de l'analyse par Produit et par Famille, couvrant les écrans Produits et Familles (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §10-11).

## 1. Objectif

Analyser la performance commerciale au niveau du Produit et de son regroupement en Famille, afin d'éclairer les décisions relatives à l'offre commerciale de l'Organisation.

## 2. Familles

- Une Famille regroupe plusieurs Produits partageant une caractéristique commune (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §Produit-Famille).
- La vue Familles présente la performance agrégée de chaque Famille (CA, Marge) avec accès à la décomposition par Produit la composant.
- La structure des Familles est configurable par un Administrateur (cf. [SETTINGS.md](SETTINGS.md)), reflétant l'organisation commerciale propre à chaque Organisation cliente.

## 3. Produits

- Chaque Produit dispose d'une fiche consolidée comportant sa performance (CA, Marge), son évolution, les Clients qui l'achètent et sa Famille de rattachement.
- La liste des Produits est filtrable et triable selon tout indicateur de performance (cf. [ux/COMPONENT_LIBRARY.md](../ux/COMPONENT_LIBRARY.md) §5).

## 4. Contribution

- La contribution d'un Produit ou d'une Famille représente sa part dans le CA ou la Marge totale de l'Organisation ou d'un périmètre donné, sur une période sélectionnée.
- La contribution peut être analysée en Pareto (cf. [ANALYTICS_ENGINE.md](ANALYTICS_ENGINE.md) §7) pour identifier les produits générant l'essentiel de la performance.

## 5. Rotation

- La rotation d'un Produit mesure la fréquence à laquelle il est acheté sur une période donnée, rapportée au volume ou au nombre de Clients concernés.
- Une rotation en forte baisse par rapport à la tendance habituelle du Produit peut déclencher une alerte (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md) §2 — Perte significative), au même titre qu'un Client.

## 6. Marge

- La Marge est calculée et comparée au niveau du Produit et de la Famille, permettant d'identifier les décalages entre volume de vente (CA) et rentabilité réelle (Marge, Marge %).
- Un Produit à fort CA mais à faible Marge est mis en évidence distinctement d'un Produit équilibré, pour éclairer les priorités commerciales.

## 7. Analyse croisée

- Toute analyse de Produit ou de Famille peut être croisée avec les autres axes de l'Analytics Engine (territoire, commercial, client) pour répondre à des questions telles que "quels territoires sous-performent sur cette Famille" (cf. [ANALYTICS_ENGINE.md](ANALYTICS_ENGINE.md) §5).

## 8. Cohérence avec les autres modules

Les fiches Produit et Famille s'appuient exclusivement sur les données consolidées par l'Import Engine et les calculs de l'Analytics Engine, sans logique de calcul propre à ce module (cf. [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md)).
