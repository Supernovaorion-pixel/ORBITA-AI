# ORBITA AI — Feature Specification: Sales Analytics

> Ce document spécifie le comportement fonctionnel complet de l'analyse par Commercial, par Région (territoire) et le suivi des Objectifs et classements, couvrant l'écran Commerciaux (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §8).

## 1. Objectif

Suivre et comparer la performance commerciale individuelle et par territoire, en lien direct avec les Objectifs fixés par l'Organisation.

## 2. Commerciaux

- Chaque Commercial dispose d'une fiche consolidée comportant sa performance (CA, Marge, évolution), son portefeuille de Clients, les Produits qu'il vend et son taux d'atteinte d'Objectif.
- La fiche présente systématiquement la comparaison entre la performance individuelle et la moyenne de son équipe ou territoire, sans jamais présenter un chiffre individuel hors de tout contexte comparatif.

## 3. Objectifs

- Un Objectif est défini pour un périmètre (Organisation, territoire, équipe, Commercial) et une période donnée (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §Objectif), configurable depuis [SETTINGS.md](SETTINGS.md).
- Le taux d'atteinte d'un Objectif est recalculé en continu à mesure que de nouvelles données sont importées.
- Un Objectif peut être défini en cascade (Objectif d'Organisation décliné par territoire puis par Commercial) ; la cohérence entre les niveaux (la somme des Objectifs individuels correspondant à l'Objectif du territoire) est vérifiée et signalée en cas d'écart lors de la configuration.

## 4. Régions (Territoires)

- Un territoire regroupe plusieurs Commerciaux et Clients selon le découpage commercial propre à l'Organisation (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md) §Territoire).
- La performance d'un territoire est la consolidation de la performance de ses Commerciaux et Clients, comparable à celle des autres territoires de l'Organisation.
- La structure des territoires est configurable par un Administrateur et peut évoluer dans le temps sans perte de l'historique déjà consolidé aux découpages précédents (cf. [architecture/DATA_FLOW.md](../architecture/DATA_FLOW.md) §7).

## 5. Classements

- Un classement ordonne les Commerciaux ou les territoires selon un indicateur choisi (CA, Marge, taux d'atteinte d'Objectif, Évolution) sur une période donnée.
- Un classement est toujours consulté à l'intérieur d'un périmètre de comparaison cohérent (ex. un Commercial n'est classé qu'au sein de son propre territoire ou de son équipe, sauf consultation explicite d'un classement élargi par un rôle habilité à cette vue globale).
- Le classement met en avant la progression (évolution de position) autant que la position elle-même, pour valoriser la dynamique et pas seulement le rang absolu.

## 6. Droits d'accès

- Un Commercial consulte sa propre fiche et son propre classement au sein de son périmètre, sans visibilité sur le détail individuel des autres Commerciaux hors de son équipe, sauf configuration contraire de l'Organisation.
- Un Responsable Régional consulte l'ensemble des Commerciaux de son territoire ; le Directeur Commercial et la Direction Générale consultent l'ensemble de l'Organisation (cf. [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md), [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §5).

## 7. Cohérence avec les autres modules

Les indicateurs de performance des Commerciaux et territoires proviennent exclusivement de l'Analytics Engine et du Forecast Engine pour les projections d'atteinte d'Objectif (cf. [FORECAST_ENGINE.md](FORECAST_ENGINE.md) §5), sans calcul propre à ce module.
