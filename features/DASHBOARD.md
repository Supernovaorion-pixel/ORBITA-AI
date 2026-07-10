# ORBITA AI — Feature Specification: Dashboard

> Ce document spécifie le comportement fonctionnel complet du module Dashboard. La structure visuelle de référence est définie dans [ux/DASHBOARD_SPECIFICATION.md](../ux/DASHBOARD_SPECIFICATION.md) ; ce document en précise le comportement fonctionnel détaillé.

## 1. Objectif

Offrir une vision consolidée et hiérarchisée de la performance commerciale, adaptée au rôle de l'utilisateur consultant (cf. [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md)).

## 2. Cartes KPI

Le Dashboard affiche les cartes officielles suivantes, chacune avec un comportement strictement identique (cf. [ux/DASHBOARD_SPECIFICATION.md](../ux/DASHBOARD_SPECIFICATION.md) §4) :

| Carte | Comportement fonctionnel |
|---|---|
| **CA** | Somme des ventes réalisées sur la période et le périmètre actifs. |
| **Marge** | Somme de la marge brute réalisée sur le même périmètre. |
| **Marge %** | Marge rapportée au CA, exprimée en pourcentage. |
| **Évolution** | Variation de l'indicateur principal du rôle par rapport à la période de comparaison sélectionnée. |
| **Projection** | Estimation de fin de période, calculée par le Forecast Engine (cf. [FORECAST_ENGINE.md](FORECAST_ENGINE.md)). |
| **Clients actifs** | Nombre de clients distincts ayant généré au moins une transaction sur la période. |
| **Objectifs** | Taux d'atteinte de l'objectif fixé pour le périmètre et la période (cf. [SALES_ANALYTICS.md](SALES_ANALYTICS.md) §3). |
| **Alertes** | Nombre d'alertes actives sur le périmètre consulté (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md)). |
| **Potentiel** | Valeur commerciale estimée non encore réalisée sur le périmètre. |
| **Panier moyen** | CA moyen par transaction sur la période. |
| **Fréquence** | Nombre moyen de transactions par client sur la période. |
| **Croissance** | Taux de variation de l'indicateur principal sur une période longue glissante (ex. 12 mois glissants). |

Chaque carte, au clic, ouvre la vue Analytics correspondant à son indicateur, pré-filtrée sur le périmètre et la période affichés.

## 3. Graphiques

- **Graphique de tendance principal** — évolution de l'indicateur prioritaire du rôle sur la période sélectionnée et la période de comparaison, en superposition.
- **Graphique de comparaison** — répartition de l'indicateur principal par territoire, équipe ou produit selon le rôle (cf. [ux/DATA_VISUALIZATION.md](../ux/DATA_VISUALIZATION.md) pour le choix du type de graphique adapté à chaque comparaison).
- Chaque graphique dispose d'une bascule permettant de changer l'indicateur représenté sans quitter le Dashboard (ex. passer de CA à Marge sur le même graphique de tendance).

## 4. Filtres

- **Période** — sélection d'un mois, trimestre, année ou plage personnalisée ; valeur par défaut : mois en cours.
- **Périmètre** — territoire, équipe ou produit, selon le rôle de l'utilisateur et son périmètre de responsabilité (cf. [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md)) : un utilisateur ne peut jamais sélectionner un périmètre hors de ses droits d'accès.
- **Comparaison** — activation d'une comparaison à la période précédente ou à la même période de l'année précédente ; désactivée par défaut.
- Tout changement de filtre s'applique instantanément à l'ensemble des cartes KPI, graphiques et tableaux de détail affichés (cf. [ux/DASHBOARD_SPECIFICATION.md](../ux/DASHBOARD_SPECIFICATION.md) §5).

## 5. Interactions

- Clic sur une carte KPI → ouverture d'Analytics pré-filtré (cf. §2).
- Clic sur un point ou une barre de graphique → affichage d'une infobulle détaillée, puis, sur second clic, accès à la décomposition complète dans Analytics.
- Clic sur une ligne du tableau de détail → accès à la fiche de l'entité concernée (Client, Produit, Commercial).
- Survol d'une carte ou d'un point de graphique → affichage de la valeur exacte et de la variation, sans navigation.

## 6. Raccourcis

- `G` puis `D` — accès direct au Dashboard depuis tout écran (cf. [ux/NAVIGATION.md](../ux/NAVIGATION.md) §5).
- `R` — réinitialisation des filtres du Dashboard à leur valeur par défaut (période en cours, aucun périmètre restrictif au-delà des droits de l'utilisateur).

## 7. Exports

- Le Dashboard peut être exporté dans son état courant (période, périmètre et comparaison actifs) sous forme de rapport de synthèse (cf. [REPORTING_ENGINE.md](REPORTING_ENGINE.md)).
- Un export du Dashboard reflète exactement les données affichées au moment de sa génération, jamais une donnée recalculée ultérieurement.

## 8. Vues

- **Vue par défaut** — vue synthétique correspondant au rôle de l'utilisateur, définie par sa fonction (Direction Générale, Directeur Commercial, Responsable Régional, Commercial, Contrôle de Gestion).
- **Vue personnalisée** — un utilisateur peut réorganiser l'ordre des cartes KPI affichées et choisir, parmi le catalogue officiel (§2), celles pertinentes pour son usage quotidien ; cette personnalisation est propre à l'utilisateur et n'affecte jamais la vue des autres utilisateurs de l'Organisation.
- **Vue Organisation** — disponible pour la Direction Générale et le Directeur Commercial, consolidant l'ensemble des périmètres de l'Organisation sans restriction de territoire.

## 9. Cohérence avec les autres modules

- Les données affichées proviennent exclusivement des résultats consolidés par l'Analytics Engine et le Forecast Engine (cf. [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md)) ; le Dashboard n'effectue aucun calcul propre.
- Le Dashboard reflète l'état de la donnée après le dernier Import validé (cf. [IMPORT_ENGINE.md](IMPORT_ENGINE.md)) ; un import en cours de traitement est signalé explicitement (cf. [ux/LOADING_STATES.md](../ux/LOADING_STATES.md) §3).
