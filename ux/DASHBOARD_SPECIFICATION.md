# ORBITA AI — Dashboard Specification

> Ce document définit précisément la structure officielle de l'écran Dashboard, l'écran le plus consulté du produit. Aucune maquette, aucun composant graphique n'est produit ici.

## 1. Disposition générale

Le Dashboard est structuré en quatre bandes horizontales, dans cet ordre strict, de haut en bas :

1. **Bande de filtres** — sélection de période et de périmètre.
2. **Bande de cartes KPI** — indicateurs clés en un coup d'œil.
3. **Bande de graphiques** — visualisations principales de tendance et de comparaison.
4. **Bande de détail** — tableau de décomposition et accès aux écrans d'analyse approfondie.

Cette disposition est identique pour tous les rôles utilisateurs ; seul le contenu de chaque bande varie selon le rôle et son périmètre de responsabilité (cf. [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md)).

## 2. Zones

### 2.1 Zone de filtres
- Sélecteur de **période** (mois, trimestre, année, personnalisé), toujours positionné en premier à gauche.
- Sélecteur de **périmètre** (territoire, équipe, produit), visible selon le rôle : un Commercial ne voit pas de sélecteur de territoire, son périmètre étant implicite.
- Comparaison activable (période précédente, année précédente) affectant l'ensemble du Dashboard de façon cohérente.

### 2.2 Zone de cartes KPI
Voir §3.

### 2.3 Zone de graphiques
- Un graphique principal de tendance (évolution du CA ou de l'indicateur prioritaire du rôle) occupant la largeur dominante.
- Un ou deux graphiques secondaires de comparaison (par territoire, produit ou commercial), positionnés à sa droite ou en dessous selon l'espace disponible (cf. [RESPONSIVE_RULES.md](RESPONSIVE_RULES.md)).
- Règles de choix et de construction des graphiques définies dans [DATA_VISUALIZATION.md](DATA_VISUALIZATION.md).

### 2.4 Zone de détail
- Tableau de décomposition (par commercial, client, produit ou territoire selon le rôle), avec accès direct vers l'écran d'Analytics correspondant pour approfondir.
- Toujours positionnée en dernier : elle n'est consultée qu'après lecture de la synthèse visuelle.

## 3. Cartes KPI officielles

Les cartes suivantes constituent le catalogue officiel des indicateurs affichables sur le Dashboard. Toutes les cartes partagent rigoureusement le même comportement (cf. §4) ; seule la sélection et l'ordre des cartes varient selon le rôle et la configuration de l'Organisation (cf. écran Paramètres).

| Carte | Ce qu'elle représente |
|---|---|
| **CA** | Chiffre d'affaires réalisé sur la période et le périmètre sélectionnés. |
| **Marge** | Marge brute réalisée en valeur. |
| **Marge %** | Marge exprimée en pourcentage du CA. |
| **Évolution** | Variation de l'indicateur principal par rapport à la période de comparaison activée. |
| **Projection** | Estimation de fin de période, issue du Forecast Engine. |
| **Clients actifs** | Nombre de clients ayant généré une activité sur la période. |
| **Objectifs** | Taux d'atteinte de l'objectif fixé pour le périmètre et la période. |
| **Alertes** | Nombre d'alertes actives sur le périmètre consulté. |
| **Potentiel** | Estimation de la valeur commerciale non encore réalisée sur le périmètre (opportunités identifiées). |
| **Panier moyen** | Valeur moyenne d'une transaction sur la période. |
| **Fréquence** | Fréquence moyenne d'achat ou d'interaction commerciale sur la période. |
| **Croissance** | Taux de croissance de l'indicateur principal sur une période longue (glissante). |

## 4. Comportement commun de toute carte KPI

Chaque carte, quel que soit l'indicateur qu'elle représente, respecte strictement la même structure et le même comportement :

1. **Libellé** de l'indicateur, en style `caption`, toujours en position supérieure.
2. **Valeur principale**, en style `type.data` avec chiffres tabulaires (cf. [branding/TYPOGRAPHY.md](../branding/TYPOGRAPHY.md) §6), immédiatement visible sans interaction.
3. **Indicateur de variation** (hausse, baisse, stable) par rapport à la période de comparaison, avec une couleur sémantique cohérente (cf. [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md) §4) : jamais de couleur positive/négative arbitraire, toujours alignée sur le sens réel de la variation pour l'objectif métier concerné.
4. **Micro-graphique de tendance** (sparkline) optionnel, discret, jamais dominant par rapport à la valeur principale.
5. **Accès au détail** : un clic sur la carte ouvre systématiquement la vue Analytics correspondante, pré-filtrée sur l'indicateur et le périmètre de la carte — jamais un comportement différent d'une carte à l'autre.
6. **État de chargement et état vide** identiques pour toutes les cartes (cf. [LOADING_STATES.md](LOADING_STATES.md), [EMPTY_STATES.md](EMPTY_STATES.md)).

Aucune carte ne doit introduire une exception à ce comportement commun ; l'ajout d'une future carte KPI doit se conformer strictement à cette structure.

## 5. Filtres

- Les filtres de la bande supérieure (§2.1) s'appliquent à l'intégralité du Dashboard de façon synchrone : aucune carte ni aucun graphique ne peut afficher un périmètre ou une période différente de celle sélectionnée globalement.
- Un filtre local supplémentaire peut être appliqué à un graphique ou tableau spécifique de la bande de détail, sans jamais contredire les filtres globaux, uniquement pour les affiner.

## 6. Navigation depuis le Dashboard

- Toute carte KPI, tout graphique et toute ligne du tableau de détail constitue un point d'entrée vers un écran d'analyse approfondie (Analytics, Forecast, Alertes), conformément au parcours défini dans [USER_FLOWS.md](USER_FLOWS.md) §1-2.
- Le Dashboard ne doit jamais être un "cul-de-sac" : chaque élément affiché doit permettre d'aller plus loin si l'utilisateur le souhaite.

## 7. Priorité des informations

L'ordre de priorité de lecture, du plus critique au plus secondaire, est fixe :

1. Alertes actives sur le périmètre (si présentes).
2. Écarts significatifs à l'objectif (carte Objectifs, indicateur de variation négatif).
3. Indicateurs de synthèse (CA, Marge, Marge %).
4. Indicateurs de tendance et de projection (Évolution, Projection, Croissance).
5. Indicateurs de contexte (Clients actifs, Potentiel, Panier moyen, Fréquence).
6. Détail tabulaire, consulté à la demande.

## 8. Hiérarchie visuelle

- La bande de cartes KPI utilise une taille et un poids typographique uniformes pour toutes les cartes (cf. §4) : aucune carte n'est visuellement plus grande qu'une autre, la priorité de lecture (§7) étant portée par l'ordre et non par la taille.
- Seule une alerte critique associée à un indicateur peut justifier une mise en évidence visuelle additionnelle (bordure ou icône sémantique), jamais un changement de taille de la carte.
- Le graphique principal de tendance occupe toujours plus d'espace visuel que les graphiques secondaires, conformément à son rôle de repère central de lecture.
