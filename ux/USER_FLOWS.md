# ORBITA AI — User Flows

> Ce document décrit, étape par étape, les parcours utilisateurs structurants du produit. Aucune maquette ni implémentation n'est décrite : uniquement la séquence logique des écrans et actions.

## 1. Connexion → Accueil → Dashboard (parcours quotidien standard)

1. L'utilisateur s'authentifie (cf. [docs/SECURITY_PRINCIPLES.md](../docs/SECURITY_PRINCIPLES.md) §6).
2. Il est dirigé vers **Accueil**, qui présente un résumé contextuel et les alertes prioritaires du jour.
3. Depuis l'Accueil, il accède au **Dashboard** correspondant à son rôle (cf. [DASHBOARD_SPECIFICATION.md](DASHBOARD_SPECIFICATION.md)).
4. Il consulte les cartes KPI et identifie, le cas échéant, un écart nécessitant investigation.

**Point de sortie naturel** : soit l'utilisateur ferme la session sans anomalie détectée, soit il engage le parcours d'investigation (§2).

## 2. Alerte reçue → Analytics → ORION → Action (parcours d'investigation)

1. L'utilisateur reçoit une notification (Topbar) ou consulte l'écran **Alertes**.
2. Il sélectionne l'alerte concernée, qui l'oriente directement vers la vue **Analytics** pré-filtrée sur le périmètre et la période de l'alerte.
3. Il explore la décomposition de l'écart (par territoire, produit ou commercial selon la nature de l'alerte).
4. Il ouvre le panneau **ORION** (accessible sans quitter l'écran) pour obtenir une interprétation en langage naturel de la cause probable.
5. ORION propose, si pertinent, une recommandation d'action (cf. [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md) §2).
6. L'utilisateur peut générer un **Rapport** ponctuel de la situation ou clore l'alerte comme traitée.

**Règle de parcours** : l'utilisateur ne doit jamais avoir à ressaisir un filtre déjà appliqué en passant d'un écran à l'autre au sein de ce parcours (cf. [INTERACTION_RULES.md](INTERACTION_RULES.md) §7).

## 3. Import de données → Historique → Dashboard actualisé

1. Un Administrateur ou un connecteur automatisé déclenche un **Import**.
2. L'écran **Import** affiche la progression et, à l'issue, le statut (réussi, partiel, en erreur) avec le détail des anomalies éventuelles (cf. [ERROR_STATES.md](ERROR_STATES.md)).
3. Une fois l'import validé, l'**Historique** enregistre le nouvel état des données.
4. Le **Dashboard** et les écrans d'**Analytics**/**Forecast** reflètent automatiquement les données mises à jour, sans action supplémentaire de l'utilisateur.

**Règle de parcours** : l'utilisateur qui consulte le Dashboard pendant qu'un import est en cours voit une indication explicite que les données affichées ne reflètent pas encore le dernier import (cf. [LOADING_STATES.md](LOADING_STATES.md)).

## 4. Dashboard → Rapports → Export (parcours de restitution)

1. Depuis le **Dashboard** ou **Analytics**, l'utilisateur déclenche la génération d'un rapport à partir de la vue actuellement affichée.
2. L'écran **Rapports** s'ouvre avec un aperçu pré-rempli reflétant le périmètre consulté.
3. L'utilisateur choisit un modèle de restitution, ajuste si besoin le périmètre, puis génère le rapport.
4. Depuis le rapport généré, il accède directement à l'écran **Exports** pour extraire le document dans le format souhaité.

**Règle de parcours** : le passage du Dashboard au Rapport ne doit jamais nécessiter de reconfigurer manuellement les filtres déjà actifs.

## 5. Administration → Utilisateurs → Licences (parcours de gestion de l'organisation)

1. Un Administrateur accède à l'écran **Administration**, qui présente une synthèse d'état (utilisateurs, licence, usage).
2. Il se rend sur **Utilisateurs** pour ajouter un collaborateur, lui attribuer un rôle et un périmètre d'accès (cf. [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md)).
3. Si l'ajout dépasse le volume autorisé par la licence, l'écran **Licences** est proposé immédiatement, avec le constat du dépassement et les options de régularisation (cf. [docs/LICENSE_SYSTEM.md](../docs/LICENSE_SYSTEM.md) §4).

**Règle de parcours** : un dépassement de licence ne doit jamais bloquer silencieusement l'action de l'Administrateur ; il doit toujours être explicité avec un chemin de résolution clair.

## 6. Question libre → ORION → Recommandation (parcours d'assistance)

1. Depuis n'importe quel écran, l'utilisateur ouvre le panneau **ORION** (raccourci `O` ou icône Topbar).
2. Il pose une question en langage naturel portant sur la performance commerciale.
3. ORION répond en s'appuyant sur les données et analyses déjà disponibles dans son périmètre d'accès (cf. [docs/AI_OVERVIEW.md](../docs/AI_OVERVIEW.md) §4).
4. Si la réponse implique un écran d'analyse plus détaillé (Analytics, Forecast), ORION propose un accès direct vers cet écran, pré-filtré selon le contexte de la question.

**Règle de parcours** : ORION ne doit jamais faire quitter à l'utilisateur l'écran sur lequel il se trouvait sans action explicite de sa part.

## 7. Connecteur ERP/CRM → Import → Vérification (parcours d'intégration)

1. Un Administrateur configure un nouveau connecteur depuis l'écran **Connecteurs**.
2. Le connecteur alimente automatiquement le module **Import** selon la fréquence configurée.
3. L'Administrateur vérifie le statut de synchronisation depuis l'écran **Connecteurs**, avec accès direct au détail d'un import généré par ce connecteur en cas d'anomalie.

## 8. Principe transversal des parcours

Tout parcours doit pouvoir être interrompu à n'importe quelle étape et repris plus tard sans perte de contexte (cf. [architecture/ERROR_HANDLING.md](../architecture/ERROR_HANDLING.md) §5, appliqué à l'expérience utilisateur). Aucun parcours ne doit imposer une séquence rigide sans point de sortie clair vers la navigation principale (cf. [NAVIGATION.md](NAVIGATION.md) §8).
