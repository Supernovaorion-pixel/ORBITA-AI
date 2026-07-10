# ORBITA AI — Feature Specification: ORION

> Ce document spécifie entièrement le comportement fonctionnel d'ORION, du point de vue de l'expérience et des capacités offertes à l'utilisateur. Conformément à [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md), aucun modèle ou mécanisme d'intelligence artificielle n'est décrit ici : uniquement le comportement attendu.

## 1. Objectif

ORION est le point d'assistance transversal d'ORBITA AI, accessible depuis tout écran (cf. [ux/NAVIGATION.md](../ux/NAVIGATION.md) §3), chargé d'interpréter les données de la plateforme et d'assister la décision de l'utilisateur.

## 2. Conversation

- ORION se présente sous forme d'un fil de conversation en langage naturel, consultable en panneau rapide depuis tout écran ou en vue complète depuis l'écran ORION (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §20).
- Une conversation conserve son fil pour l'utilisateur qui l'a menée, consultable ultérieurement sans avoir à reformuler ses questions précédentes.
- ORION ne mobilise jamais, pour répondre, de données hors du périmètre d'accès de l'utilisateur qui l'interroge (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §1).

## 3. Questions

- L'utilisateur pose une question libre en langage naturel portant sur la performance commerciale, un indicateur, une entité (client, produit, commercial) ou une période.
- ORION peut demander une précision lorsque la question est ambiguë (période ou périmètre non spécifié), plutôt que de répondre sur la base d'une supposition non explicitée.
- Toute réponse chiffrée est traçable jusqu'aux données et analyses qui la fondent (cf. [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md) §4).

## 4. Suggestions

- En l'absence de question posée, ORION propose des suggestions contextuelles fondées sur l'activité récente de l'Organisation et le rôle de l'utilisateur (ex. "Consulter l'évolution du territoire Nord", si celui-ci présente un écart notable).
- Les suggestions sont renouvelées à mesure que la situation de l'Organisation évolue, jamais figées après leur première présentation.

## 5. Analyse

- ORION peut mobiliser les capacités de l'Analytics Engine (cf. [ANALYTICS_ENGINE.md](ANALYTICS_ENGINE.md)) pour répondre à une question d'exploration (comparaison, décomposition, tendance), sans dupliquer la logique de calcul de ce moteur.
- Le résultat d'une analyse demandée à ORION peut toujours être approfondi en un clic vers l'écran Analytics correspondant, pré-filtré selon la question posée.

## 6. Explication

- ORION peut expliquer, en langage naturel, la raison probable d'un constat (ex. "pourquoi le CA du mois est en baisse"), en identifiant les contributeurs principaux à la variation observée.
- Une explication reste toujours accompagnée des données qui la justifient, jamais présentée comme une affirmation sans support vérifiable.

## 7. Synthèse

- ORION peut produire, à la demande, une synthèse condensée d'un périmètre ou d'une période (ex. "résume la performance du trimestre pour mon équipe"), reprenant les constats les plus significatifs sans exhaustivité inutile.

## 8. Détection d'anomalies

- ORION signale, sans sollicitation explicite, les anomalies significatives détectées dans le périmètre de responsabilité de l'utilisateur (écart important à une tendance ou à un objectif), en cohérence avec le module Alertes (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md)).
- Une anomalie signalée par ORION est toujours accompagnée d'une explication (§6) et, lorsque pertinent, d'une recommandation (§16).

## 9. Prévisions

- ORION peut restituer en langage naturel le contenu d'une prévision produite par le Forecast Engine (cf. [FORECAST_ENGINE.md](FORECAST_ENGINE.md)), y compris son niveau d'incertitude, sans jamais présenter une projection comme une certitude.

## 10. Rapports

- ORION peut déclencher la génération d'un rapport à partir d'une demande formulée en langage naturel (ex. "génère la synthèse mensuelle de la région Est"), en s'appuyant sur le Reporting Engine (cf. [REPORTING_ENGINE.md](REPORTING_ENGINE.md)) sans dupliquer sa logique d'assemblage.

## 11. Résumé quotidien

- Synthèse courte, générée automatiquement chaque jour, mettant en avant les faits marquants de la veille pour le périmètre de responsabilité de l'utilisateur (nouvelles alertes, écarts significatifs, jalons atteints).
- Consultable depuis l'écran Accueil et le fil ORION, sans sollicitation active de l'utilisateur.

## 12. Résumé hebdomadaire

- Synthèse consolidant la semaine écoulée : tendance générale, comparaison à la semaine précédente, alertes traitées et en cours.
- Adaptée au rythme de pilotage des rôles régionaux et commerciaux (cf. [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md)).

## 13. Résumé mensuel

- Synthèse de fin de mois, articulée autour de l'atteinte des objectifs mensuels et de la projection du mois suivant, adaptée au rythme de pilotage de la Direction Commerciale et de la Direction Générale.

## 14. Aide contextuelle

- Depuis n'importe quel écran, l'utilisateur peut solliciter ORION pour obtenir une explication sur ce qu'il consulte (un indicateur, un graphique, une fonctionnalité), sans quitter son contexte de travail.

## 15. Explication des KPI

- Pour toute carte KPI du Dashboard (cf. [DASHBOARD.md](DASHBOARD.md) §2), ORION peut expliquer la méthode de calcul de l'indicateur, la période couverte et les principaux facteurs expliquant sa valeur actuelle.

## 16. Explication des graphiques

- Pour tout graphique consulté (cf. [ux/DATA_VISUALIZATION.md](../ux/DATA_VISUALIZATION.md)), ORION peut décrire en langage naturel ce que le graphique représente et les constats principaux qui s'en dégagent, utile en particulier pour les utilisateurs peu familiers de la lecture graphique.

## 17. Recommandations

- Une recommandation est une proposition d'action ou d'attention, distincte d'une réponse factuelle, toujours accompagnée de la donnée qui la motive (cf. [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md) §2).
- ORION ne prend jamais d'action à la place de l'utilisateur : une recommandation reste soumise à la décision et à l'exécution de ce dernier (cf. [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md) §4).

## 18. Actions prioritaires

- ORION peut établir, à la demande ou dans le cadre d'un résumé (§11-13), une liste courte des actions jugées les plus prioritaires pour l'utilisateur au regard de son périmètre (ex. "3 comptes à recontacter cette semaine"), fondée sur les analyses, prévisions et alertes disponibles.
- Cette liste reste indicative : elle n'engage aucune action automatique du système.

## 19. Limites fonctionnelles

Conformément à [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md) §4, ORION n'agit jamais en dehors du périmètre de données de la plateforme, ne modifie jamais une donnée de sa propre initiative, et signale explicitement toute question à laquelle il ne peut répondre de façon fiable plutôt que de formuler une réponse approximative.
