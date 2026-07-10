# ORBITA AI — Feature Specification: Export Engine

> Ce document spécifie le comportement fonctionnel complet du module Export Engine (cf. [architecture/MODULE_ARCHITECTURE.md](../architecture/MODULE_ARCHITECTURE.md) §14).

## 1. Objectif

Permettre l'extraction de données ou de rapports de la plateforme vers un usage externe, conformément à [architecture/DATA_FLOW.md](../architecture/DATA_FLOW.md) §6.

## 2. Types d'export

- **Export de données brutes ou agrégées** — extraction d'un tableau consulté (Clients, Produits, Historique, résultats d'Analytics) dans son état filtré courant.
- **Export de rapport formalisé** — extraction d'un Rapport généré par le Reporting Engine (cf. [REPORTING_ENGINE.md](REPORTING_ENGINE.md)).
- **Export du Dashboard** — extraction de l'état courant du Dashboard consulté (cf. [DASHBOARD.md](DASHBOARD.md) §7).

## 3. Tous les formats

| Format | Usage |
|---|---|
| **Excel** | Données structurées destinées à un retraitement ou un recoupement externe. |
| **PDF** | Contenu formaté destiné au partage ou à l'archivage, mise en page fixe. |
| **PowerPoint** | Contenu structuré en diapositives destiné à la présentation orale. |
| **Texte délimité (CSV et équivalents)** | Données brutes destinées à une réintégration dans un autre système. |

La disponibilité de chaque format dépend de la nature du contenu exporté (cf. [REPORTING_ENGINE.md](REPORTING_ENGINE.md) §7-9) : un export de données tabulaires propose Excel et texte délimité, un export de rapport de synthèse propose PDF et PowerPoint.

## 4. Paramètres d'export

- **Périmètre** — territoire, équipe, produit, période, hérité par défaut du contexte depuis lequel l'export est déclenché, ajustable avant génération.
- **Colonnes** — pour un export de données tabulaires, sélection des colonnes à inclure, cohérente avec la configuration d'affichage du tableau d'origine (cf. [ux/COMPONENT_LIBRARY.md](../ux/COMPONENT_LIBRARY.md) §5).
- **Format** — choix parmi les formats disponibles pour le contenu concerné (§3).
- **Destination** — téléchargement direct ou envoi programmé à des destinataires désignés, dans le cadre d'une planification de rapport (cf. [REPORTING_ENGINE.md](REPORTING_ENGINE.md) §5).

## 5. Options

- **Inclusion des filtres actifs** — un export documente toujours, en en-tête ou en propriété du fichier, le périmètre et la période exacts qu'il représente.
- **Anonymisation** — option permettant, pour certains exports partagés en dehors du périmètre de droits standard, de masquer des champs sensibles (ex. nom nominatif d'un client) tout en conservant les agrégats.
- **Volume maximal** — un export dont le volume dépasse un seuil raisonnable pour un traitement immédiat est basculé en traitement asynchrone (cf. [ux/LOADING_STATES.md](../ux/LOADING_STATES.md) §2.4), avec notification à son achèvement.

## 6. Traçabilité

- Chaque export réalisé est journalisé (qui, quoi, quand, périmètre) conformément à [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §3, en cohérence avec l'exigence de confidentialité des données exportées (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §1).
- L'historique des exports réalisés est consultable depuis l'écran Exports (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §14).

## 7. Restriction par droits et licence

- Un export ne peut jamais porter sur un périmètre de données hors des droits d'accès de l'utilisateur qui le déclenche.
- La disponibilité de certaines options d'export (ex. export PowerPoint, export planifié) peut être restreinte selon l'offre souscrite par l'Organisation (cf. [FEATURE_MATRIX.md](FEATURE_MATRIX.md)).
