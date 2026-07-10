# ORBITA AI — Feature Specification: Forecast Engine

> Ce document spécifie le comportement fonctionnel complet du module Forecast Engine (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §4).

## 1. Objectif

Anticiper la performance commerciale future à partir des données historiques et de l'activité courante, pour tout périmètre de l'Organisation.

## 2. Prévisions

- Une prévision est produite pour un indicateur (CA, Marge), un périmètre (territoire, équipe, produit, famille) et un horizon donnés.
- Toute prévision est accompagnée d'un intervalle de confiance visuel (cf. [ux/DATA_VISUALIZATION.md](../ux/DATA_VISUALIZATION.md) — graphique Line), jamais présentée comme une valeur unique et certaine.
- Une prévision est recalculée automatiquement à chaque nouvel Import validé impactant son périmètre (cf. [IMPORT_ENGINE.md](IMPORT_ENGINE.md)).

## 3. Hypothèses

- Toute prévision repose sur des hypothèses explicites (tendance historique récente, saisonnalité identifiée, objectifs déclarés), consultables par l'utilisateur qui souhaite comprendre la logique d'une projection.
- Une hypothèse peut être ajustée manuellement par un utilisateur habilité (ex. exclure un événement exceptionnel passé du calcul de tendance), l'ajustement étant tracé et réversible.

## 4. Simulations

- L'utilisateur peut simuler l'effet d'une hypothèse modifiée (ex. "si le CA du dernier trimestre progresse de 5% supplémentaires") sans que cette simulation n'affecte la prévision officielle de l'Organisation.
- Une simulation est temporaire et propre à la session de l'utilisateur qui la réalise, jamais partagée automatiquement avec les autres utilisateurs sans action explicite de sauvegarde ou de partage.

## 5. Objectifs

- Un Objectif est défini pour un périmètre et une période donnés (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md)), configurable depuis [SETTINGS.md](SETTINGS.md).
- Le Forecast Engine calcule en continu la probabilité d'atteinte de chaque Objectif actif, à partir de la tendance réelle observée et de la prévision produite.
- Un Objectif significativement menacé (probabilité d'atteinte faible à mesure que la période avance) déclenche une Alerte (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md)).

## 6. Projection annuelle

- Estimation de la performance de fin d'année civile ou d'exercice, actualisée en continu à mesure que de nouvelles données réelles sont disponibles.
- Décomposable par mois restant à courir, afin de visualiser la trajectoire nécessaire pour atteindre l'objectif annuel.

## 7. Projection mensuelle

- Estimation de la performance de fin de mois en cours, avec un niveau de précision plus élevé qu'une projection annuelle du fait de l'horizon plus court.
- Utilisée en priorité par les rôles opérationnels (Commercial, Responsable Régional) pour le pilotage à court terme (cf. [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md)).

## 8. Scénarios

- Un scénario regroupe un ensemble d'hypothèses ajustées (§3) et de simulations (§4) sauvegardé pour être consulté ou comparé ultérieurement (ex. scénario "optimiste", scénario "prudent").
- Plusieurs scénarios peuvent être comparés côte à côte pour un même périmètre et horizon, sans qu'aucun ne remplace la prévision officielle sauf validation explicite d'un utilisateur habilité.

## 9. Principe de fiabilité

- Toute prévision, simulation ou scénario indique explicitement la période de données historiques sur laquelle elle s'appuie et son niveau de fiabilité relatif (ex. fiabilité réduite pour un périmètre disposant de peu d'historique).
- Le Forecast Engine ne produit jamais de projection présentée comme certaine : l'incertitude est toujours communiquée (cf. [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md) et cohérence avec les recommandations d'ORION, [ORION.md](ORION.md)).
