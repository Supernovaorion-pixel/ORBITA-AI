# ORBITA AI — Feature Specification: Client Management

> Ce document spécifie le comportement fonctionnel complet de la gestion des Clients, couvrant les écrans Clients et Grands Comptes (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §9 et §12).

## 1. Objectif

Consulter et analyser l'activité commerciale par Client, depuis la vue de portefeuille jusqu'à la fiche individuelle détaillée.

## 2. Fiche client

Chaque Client dispose d'une fiche consolidée comportant :
- son identification et ses attributs de référence (territoire, segment, Commercial en charge),
- sa performance courante (CA, Marge, évolution) sur la période sélectionnée,
- son historique d'activité (§3),
- les Produits et Familles qu'il consomme,
- les Alertes actives le concernant (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md)).

## 3. Historique

- La fiche client donne accès à l'historique complet de son activité (transactions, évolution de la relation) sur toute la profondeur de données disponible pour l'Organisation (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §Historique).
- L'historique est consultable sous forme de trajectoire visuelle (tendance) et de détail tabulaire.

## 4. Performance

- La performance d'un Client est mesurée par les mêmes indicateurs que ceux du Dashboard et de l'Analytics Engine (CA, Marge, Panier moyen, Fréquence — cf. [DASHBOARD.md](DASHBOARD.md) §2), appliqués à son seul périmètre.
- La performance est toujours présentée avec sa comparaison à la période précédente et, lorsque pertinent, à la moyenne du segment auquel le Client appartient.

## 5. Potentiel

- Le potentiel d'un Client représente une estimation de la valeur commerciale non encore réalisée (produits non achetés mais pertinents au regard de son profil, fréquence d'achat inférieure à des clients comparables).
- Le potentiel est calculé par l'Analytics Engine et peut être affiné par le Forecast Engine lorsqu'une tendance suffisante existe.
- Un potentiel significatif et non exploité peut générer une alerte de type "Opportunité identifiée" (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md) §2).

## 6. Activité

- L'activité récente d'un Client (dernières transactions, dernière interaction significative constatée dans les données importées) est mise en avant en tête de fiche, avant les indicateurs agrégés.
- Une absence d'activité prolongée par rapport à la fréquence habituelle du Client déclenche un signal spécifique (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md) §2 — Perte significative).

## 7. Grands Comptes

- Un Client peut être désigné comme **Grand Compte**, un statut configurable par un utilisateur habilité (cf. [SETTINGS.md](SETTINGS.md)), déclenchant :
  - un suivi renforcé (seuils d'alerte plus sensibles, cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md) §3),
  - une fiche enrichie (historique long terme, interlocuteurs clés, risques identifiés),
  - une visibilité dédiée pour la Direction Commerciale et la Direction Générale (cf. [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md)).
- Le statut de Grand Compte n'altère jamais la donnée sous-jacente du Client : il n'ajoute qu'un niveau d'attention et de suivi supplémentaire.

## 8. Portefeuille et droits d'accès

- Un Commercial ne consulte que les Clients de son propre portefeuille ; un Responsable Régional consulte l'ensemble des Clients de son territoire ; un Directeur Commercial ou la Direction Générale consultent l'ensemble de l'Organisation (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §5).

## 9. Cohérence avec les autres modules

La fiche Client s'appuie exclusivement sur les données consolidées par l'Import Engine et les analyses de l'Analytics Engine et du Forecast Engine, sans recalcul propre (cf. [architecture/MODULE_DEPENDENCIES.md](../architecture/MODULE_DEPENDENCIES.md)).
