# ORBITA AI — Feature Specification: Search Engine

> Ce document spécifie le comportement fonctionnel complet de la recherche globale, définie dans ses principes de navigation par [ux/NAVIGATION.md](../ux/NAVIGATION.md) §6.

## 1. Objectif

Permettre à tout utilisateur de retrouver directement une entité ou un contenu de la plateforme, sans naviguer manuellement à travers la Sidebar.

## 2. Recherche globale

- Accessible depuis tout écran via le raccourci `/` ou l'icône dédiée en Topbar (cf. [ux/NAVIGATION.md](../ux/NAVIGATION.md) §5-6).
- Porte sur les entités du Domaine accessibles à l'utilisateur : Clients, Produits, Familles, Commerciaux, Rapports, et accessoirement les écrans de configuration (Utilisateurs, Plugins) pour les profils habilités.
- La recherche ne retourne jamais une entité hors du périmètre de droits d'accès de l'utilisateur qui la formule (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §5).

## 3. Filtres

- Les résultats de recherche sont groupés par type d'entité (Clients, Produits, Commerciaux, Rapports, Autres), dans un ordre de priorité constant (cf. [ux/NAVIGATION.md](../ux/NAVIGATION.md) §6).
- L'utilisateur peut restreindre la recherche à un type d'entité particulier pour affiner ses résultats.

## 4. Suggestions

- Pendant la saisie, des suggestions apparaissent progressivement, priorisant les entités les plus pertinentes pour l'utilisateur (ex. Clients de son propre portefeuille avant les Clients d'un autre territoire, lorsque son rôle lui donne visibilité sur les deux).
- En l'absence de saisie, la recherche propose les éléments récemment consultés par l'utilisateur (§5), afin de faciliter un accès rapide à un contenu déjà visité.

## 5. Historique

- Les recherches et consultations récentes de l'utilisateur sont conservées pour lui proposer un accès rapide, propre à son propre usage, jamais partagé avec d'autres utilisateurs de l'Organisation.
- Cet historique peut être effacé à tout moment par l'utilisateur depuis son Profil (cf. [SETTINGS.md](SETTINGS.md) §3).

## 6. Comportement de sélection

- La sélection d'un résultat de recherche ouvre directement la fiche ou l'écran correspondant à l'entité choisie, sans étape intermédiaire.
- Si le résultat sélectionné dépend d'un contexte (période, périmètre), la recherche applique le contexte courant de l'utilisateur (période active du Dashboard, par exemple) par défaut, ajustable ensuite sur l'écran de destination.

## 7. Cohérence avec ORION

- La recherche globale et ORION (cf. [ORION.md](ORION.md)) répondent à des besoins distincts et complémentaires : la recherche retrouve une entité connue, ORION répond à une question ou produit une analyse. Une requête de recherche qui s'apparente à une question en langage naturel peut proposer, en complément des résultats, une invitation à la poser directement à ORION.
