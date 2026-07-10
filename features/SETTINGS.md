# ORBITA AI — Feature Specification: Settings

> Ce document spécifie l'ensemble des options configurables du produit, couvrant l'écran Paramètres (cf. [ux/SCREEN_STRUCTURE.md](../ux/SCREEN_STRUCTURE.md) §21) et l'écran Profil (§22).

## 1. Objectif

Permettre à chaque Organisation et à chaque utilisateur d'adapter la plateforme à sa propre réalité, sans intervention extérieure (cf. [docs/FUNCTIONAL_SPECIFICATION.md](../docs/FUNCTIONAL_SPECIFICATION.md) §10).

## 2. Paramètres au niveau Organisation (Administrateur)

### 2.1 Objectifs
- Définition des Objectifs par périmètre (Organisation, territoire, équipe, Commercial) et par période, avec la logique de cascade décrite dans [SALES_ANALYTICS.md](SALES_ANALYTICS.md) §3.

### 2.2 Structure organisationnelle
- Définition et évolution des territoires, équipes et rattachements de Commerciaux (cf. [architecture/DOMAIN_MODEL.md](../architecture/DOMAIN_MODEL.md)).
- Définition de la structure des Familles de Produits (cf. [PRODUCT_ANALYTICS.md](PRODUCT_ANALYTICS.md) §2).

### 2.3 Seuils d'alerte
- Configuration des déclencheurs et seuils du catalogue d'alertes (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md) §3), au niveau global ou affiné par périmètre.

### 2.4 Grands Comptes
- Désignation des Clients bénéficiant du statut de Grand Compte (cf. [CLIENT_MANAGEMENT.md](CLIENT_MANAGEMENT.md) §7).

### 2.5 Cartes KPI par défaut
- Sélection et ordre par défaut des cartes KPI affichées pour chaque rôle sur le Dashboard (cf. [DASHBOARD.md](DASHBOARD.md) §2), personnalisable ensuite individuellement par chaque utilisateur.

### 2.6 Langue par défaut de l'Organisation
- Langue appliquée par défaut aux nouveaux utilisateurs de l'Organisation, chacun pouvant ensuite ajuster sa propre préférence (§3.3).

## 3. Paramètres au niveau utilisateur (Profil)

### 3.1 Informations personnelles
- Identité, coordonnées de contact de l'utilisateur.

### 3.2 Sécurité du compte
- Gestion de l'authentification personnelle (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §6).

### 3.3 Langue
- Langue d'affichage de l'interface, indépendante de la langue par défaut de l'Organisation.

### 3.4 Notifications
- Choix des types d'alertes et de notifications reçus et de leur canal (cf. [NOTIFICATION_CENTER.md](NOTIFICATION_CENTER.md)), sans pouvoir désactiver les alertes critiques relevant de son périmètre de responsabilité (cf. [ALERT_SYSTEM.md](ALERT_SYSTEM.md) §8).

### 3.5 Préférences d'affichage
- Mode de densité par défaut (cf. [ux/DESIGN_SYSTEM.md](../ux/DESIGN_SYSTEM.md) §2) et thème clair/sombre, l'interface étant dark-first par défaut.
- Personnalisation des cartes KPI affichées sur son propre Dashboard, dans les limites de son rôle (cf. [DASHBOARD.md](DASHBOARD.md) §8).

## 4. Contraintes de configuration

- Toute modification d'un paramètre au niveau Organisation est journalisée et attribuée à l'utilisateur qui l'a réalisée (cf. [AUDIT_AND_HISTORY.md](AUDIT_AND_HISTORY.md)).
- Un paramètre modifié ne s'applique jamais rétroactivement à une donnée déjà consolidée (ex. modifier un seuil d'alerte ne réévalue pas les alertes déjà archivées).
- La disponibilité de certains paramètres avancés (seuils d'alerte affinés par périmètre, personnalisation étendue) peut dépendre de l'offre de licence de l'Organisation (cf. [FEATURE_MATRIX.md](FEATURE_MATRIX.md)).

## 5. Principe de simplicité

Un paramètre n'est exposé à la configuration que s'il correspond à un besoin réel et récurrent des Organisations clientes (cf. principe YAGNI, [architecture/CODING_PRINCIPLES.md](../architecture/CODING_PRINCIPLES.md) §4) : la plateforme privilégie des valeurs par défaut pertinentes plutôt qu'une configuration exhaustive rarement utilisée.
