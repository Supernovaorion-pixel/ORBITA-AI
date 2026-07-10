# ORBITA AI — Domain Model

> Ce document décrit les entités métier fondamentales du système et leurs relations. Aucun schéma de base de données, aucun type technique n'est défini ici : uniquement la structure conceptuelle du Domaine.

## 1. Entités et définitions

Les définitions complètes de chaque terme figurent dans [docs/GLOSSARY.md](../docs/GLOSSARY.md). Ce document se concentre sur les **relations** entre entités.

- **Organisation** — entité cliente racine ; tout le reste du modèle existe dans le périmètre d'une Organisation.
- **Utilisateur** — personne accédant au système, rattachée à une Organisation.
- **Permissions** — droits attribués à un Utilisateur, définissant ce qu'il peut voir et faire.
- **Licence** — cadre contractuel attribué à une Organisation, déterminant son périmètre d'usage.
- **Import** — opération d'intégration de données externes dans une Organisation.
- **Commercial** — Utilisateur (ou représentation d'une personne) portant un périmètre de vente au sein de l'Organisation.
- **Client** — tiers avec lequel l'Organisation réalise ou vise une activité commerciale.
- **Produit** — bien ou service commercialisé par l'Organisation.
- **Famille** — regroupement de Produits partageant une caractéristique commune.
- **Facture** — document constatant une transaction commerciale entre l'Organisation et un Client.
- **Objectif** — cible de performance définie pour un périmètre donné (Organisation, Commercial, Produit...).
- **Prévision** — estimation de performance future dérivée de l'Historique.
- **Alerte** — signalement généré lorsqu'un écart ou événement significatif est détecté.
- **Rapport** — restitution formalisée d'analyses portant sur ces entités.
- **Historique** — ensemble des états successifs des entités dans le temps.

## 2. Relations entre entités

### Organisation — au centre du modèle
- Une **Organisation** possède une **Licence** (une Organisation ↔ une Licence active à un instant donné).
- Une **Organisation** regroupe plusieurs **Utilisateurs**.
- Une **Organisation** regroupe plusieurs **Commerciaux**, **Clients**, **Produits**, **Familles**, **Factures**, **Objectifs**, **Imports**, **Rapports**, **Alertes** et **Prévisions**.
- Aucune entité autre qu'Organisation et Licence n'existe indépendamment d'une Organisation : toute donnée est rattachée, directement ou indirectement, à une Organisation unique (cloisonnement, cf. [SYSTEM_ARCHITECTURE.md](SYSTEM_ARCHITECTURE.md) §5).

### Utilisateur et Permissions
- Un **Utilisateur** appartient à une seule **Organisation**.
- Un **Utilisateur** dispose d'un ensemble de **Permissions**, dérivées de son rôle (cf. [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md)) et, le cas échéant, restreintes à un périmètre (territoire, équipe).
- Un **Utilisateur** peut être associé à un **Commercial** (lorsqu'il représente une personne portant un périmètre de vente), sans que cette association soit systématique (un Administrateur, par exemple, n'est pas nécessairement un Commercial).

### Licence
- Une **Licence** est attribuée à une **Organisation** et détermine le périmètre fonctionnel accessible (cf. [docs/LICENSE_SYSTEM.md](../docs/LICENSE_SYSTEM.md)).
- Une **Licence** encadre indirectement le nombre d'**Utilisateurs** et le volume de données (**Imports**, **Historique**) autorisés pour l'Organisation.

### Import et Historique
- Un **Import** appartient à une **Organisation** et alimente les entités opérationnelles (**Client**, **Produit**, **Facture**, etc.).
- Chaque modification apportée par un **Import** génère un état conservé dans l'**Historique**.
- L'**Historique** est transversal : il conserve les états successifs de toute entité qui évolue dans le temps (Facture, Objectif, Prévision, Client, etc.).

### Commercial, Client, Produit, Famille, Facture
- Un **Commercial** est rattaché à un périmètre de **Clients** et/ou de **Produits** au sein de son Organisation.
- Un **Client** peut être associé à plusieurs **Factures**.
- Un **Produit** appartient à une **Famille** (une Famille regroupe plusieurs Produits).
- Une **Facture** relie un **Client**, un ou plusieurs **Produits**, et implicitement un **Commercial** (celui à qui la performance associée est rattachée).

### Objectif, Prévision, Alerte
- Un **Objectif** est défini pour un périmètre (Organisation, Commercial, Produit ou Famille) et une période donnée.
- Une **Prévision** est calculée en référence à un **Objectif** et à l'**Historique** correspondant au même périmètre.
- Une **Alerte** est générée lorsqu'un écart significatif est constaté entre une donnée réalisée (issue des **Factures** ou d'autres indicateurs) et un **Objectif** ou une **Prévision**.

### Rapport
- Un **Rapport** agrège des données et analyses portant sur un ou plusieurs périmètres (Organisation, Commercial, Produit, Famille, période) à un instant donné.
- Un **Rapport** ne modifie jamais les entités qu'il restitue : il en est une représentation figée à sa date de génération.

## 3. Schéma relationnel synthétique

```
Organisation 1—1 Licence
Organisation 1—N Utilisateur ——(Permissions)
Organisation 1—N Commercial
Organisation 1—N Client
Organisation 1—N Produit ——N—1 Famille
Organisation 1—N Facture ——N—1 Client
                         ——N—N Produit
Organisation 1—N Objectif ——1—1 Prévision (par périmètre/période)
Organisation 1—N Import ——N Historique (états)
Organisation 1—N Alerte ——(référence) Objectif / Prévision / Facture
Organisation 1—N Rapport ——(agrège) Analyses / Prévisions / Historique
Utilisateur   N—1 Commercial (association optionnelle)
```

## 4. Principe de cloisonnement

Aucune relation ne doit jamais permettre à une entité d'une Organisation de référencer une entité d'une autre Organisation. Ce cloisonnement est un invariant du Domaine, non une simple règle applicative (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §1).
