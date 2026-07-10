# ORBITA AI — Screen Structure

> Ce document décrit la structure officielle de chaque écran du produit : objectif, zones principales et hiérarchie de priorité de l'information. Aucune maquette, aucun composant graphique n'est produit ici — uniquement la structure. Le détail des composants utilisés est défini dans [COMPONENT_LIBRARY.md](COMPONENT_LIBRARY.md).

## Principe commun à tous les écrans

Chaque écran suit la structure Topbar + Sidebar + zone de contenu définie dans [NAVIGATION.md](NAVIGATION.md) et [DESIGN_SYSTEM.md](DESIGN_SYSTEM.md) §4. La description ci-dessous porte exclusivement sur la zone de contenu propre à chaque écran.

---

## 1. Accueil

**Objectif** : point d'entrée après connexion, orientant l'utilisateur vers ce qui mérite son attention immédiate, sans dupliquer le Dashboard.

**Zones** : bandeau de bienvenue contextuel (date, résumé en une phrase généré par ORION) ; liste des alertes prioritaires du jour ; accès rapide aux écrans les plus utilisés par le rôle de l'utilisateur ; rappel des rapports récemment générés.

**Priorité** : signal du jour avant toute donnée chiffrée détaillée — l'Accueil informe, il n'analyse pas.

## 2. Dashboard

**Objectif** : vision consolidée et hiérarchisée de la performance. Structure complète définie dans [DASHBOARD_SPECIFICATION.md](DASHBOARD_SPECIFICATION.md).

## 3. Import

**Objectif** : superviser l'intégration des données commerciales externes.

**Zones** : zone de dépôt/déclenchement d'import ; liste des imports récents avec statut (réussi, en erreur, en cours) ; détail des anomalies détectées pour un import donné ; historique des sources déjà connectées.

**Priorité** : le statut du dernier import prime sur l'historique complet, toujours visible en premier.

## 4. Historique

**Objectif** : consulter l'évolution des données et de la performance dans le temps.

**Zones** : sélecteur de période et de périmètre (territoire, équipe, produit) ; vue chronologique (graphique en ligne temporelle) ; tableau détaillé des états successifs ; comparaison entre deux périodes sélectionnées.

**Priorité** : la trajectoire visuelle précède le détail tabulaire, consultable à la demande.

## 5. Rapports

**Objectif** : générer, consulter et gérer les rapports formalisés.

**Zones** : liste des rapports existants (avec statut, date, périmètre) ; modèles de rapport disponibles ; aperçu du rapport sélectionné ; actions de génération, planification et partage.

**Priorité** : les rapports récents et planifiés priment sur le catalogue de modèles.

## 6. Analytics

**Objectif** : exploration approfondie et comparative de la donnée commerciale.

**Zones** : sélecteurs d'axes d'analyse (période, territoire, équipe, produit, client) ; zone de visualisation principale (graphique adapté à l'analyse en cours, cf. [DATA_VISUALIZATION.md](DATA_VISUALIZATION.md)) ; décomposition détaillée sous forme de tableau ; panneau de comparaison multi-axes.

**Priorité** : la question posée par l'utilisateur (choix des axes) détermine la mise en avant ; aucune hiérarchie fixe de graphique par défaut autre que la vue de synthèse initiale.

## 7. Forecast

**Objectif** : anticiper la performance future.

**Zones** : sélecteur d'horizon de projection ; graphique de projection avec intervalle de confiance visuel ; indicateurs de probabilité d'atteinte des objectifs ; facteurs de risque/opportunité mis en évidence.

**Priorité** : l'estimation d'atteinte de l'objectif prime sur le détail méthodologique de la projection.

## 8. Commerciaux

**Objectif** : suivre la performance individuelle des commerciaux.

**Zones** : liste des commerciaux avec indicateurs de synthèse (cf. [COMPONENT_LIBRARY.md](COMPONENT_LIBRARY.md) — Tableaux) ; fiche individuelle détaillée (performance, historique, portefeuille) accessible depuis la liste.

**Priorité** : la comparaison à l'objectif individuel prime sur le classement entre commerciaux.

## 9. Clients

**Objectif** : consulter l'activité commerciale par client.

**Zones** : liste filtrable des clients ; fiche client détaillée (historique d'achats, produits, tendance) accessible depuis la liste.

**Priorité** : l'activité récente d'un client prime sur ses données d'identification.

## 10. Produits

**Objectif** : suivre la performance par produit.

**Zones** : liste des produits avec indicateurs de vente ; fiche produit détaillée (évolution, clients associés, famille de rattachement).

**Priorité** : la tendance de vente prime sur la fiche descriptive du produit.

## 11. Familles

**Objectif** : analyser la performance par regroupement de produits.

**Zones** : liste des familles avec performance agrégée ; vue de décomposition par produit au sein d'une famille sélectionnée.

**Priorité** : la vue agrégée précède toujours la décomposition, consultée à la demande.

## 12. Grands Comptes

**Objectif** : suivi dédié des clients stratégiques nécessitant une attention particulière.

**Zones** : liste restreinte aux comptes désignés comme stratégiques ; fiche enrichie (historique long terme, interlocuteurs clés, risques identifiés) par rapport à la fiche Client standard.

**Priorité** : les signaux de risque ou d'opportunité sur un grand compte priment sur la donnée de volume brute.

## 13. Alertes

**Objectif** : centraliser les signalements proactifs nécessitant l'attention de l'utilisateur.

**Zones** : liste des alertes actives, triées par gravité puis par date ; détail de l'alerte sélectionnée avec accès direct à l'analyse concernée ; configuration des seuils d'alerte (selon droits).

**Priorité** : les alertes critiques priment systématiquement sur les alertes informatives, quelle que soit leur ancienneté.

## 14. Exports

**Objectif** : gérer les extractions de données réalisées.

**Zones** : formulaire de création d'un nouvel export (périmètre, format, destination) ; historique des exports réalisés avec statut.

**Priorité** : la création d'un nouvel export prime visuellement sur l'historique, toujours accessible en second plan.

## 15. Administration

**Objectif** : point d'entrée de la gestion de l'organisation cliente.

**Zones** : synthèse de l'état de l'organisation (utilisateurs actifs, licence, usage) ; navigation secondaire vers Utilisateurs, Licences, Plugins, Connecteurs (cf. [NAVIGATION.md](NAVIGATION.md) §4).

**Priorité** : les situations nécessitant une action (licence proche d'une limite, utilisateur en attente) priment sur les statistiques générales.

## 16. Utilisateurs

**Objectif** : gérer les comptes et droits d'accès de l'organisation.

**Zones** : liste des utilisateurs avec rôle et statut ; formulaire d'ajout/modification ; détail des permissions par utilisateur.

**Priorité** : les demandes en attente (invitation non acceptée, droits à valider) priment sur la liste complète.

## 17. Licences

**Objectif** : consulter et gérer le contrat de licence de l'organisation.

**Zones** : synthèse de l'offre souscrite et de son périmètre ; suivi des volumes utilisés par rapport aux limites ; historique de facturation et de renouvellement (cf. [docs/LICENSE_SYSTEM.md](../docs/LICENSE_SYSTEM.md)).

**Priorité** : tout dépassement ou approche de limite prime visuellement sur le reste de l'écran.

## 18. Plugins

**Objectif** : gérer les extensions activées pour l'organisation.

**Zones** : liste des plugins disponibles selon la licence ; état d'activation de chacun ; détail de configuration d'un plugin sélectionné (cf. [architecture/EXTENSIBILITY.md](../architecture/EXTENSIBILITY.md)).

**Priorité** : les plugins actifs priment visuellement sur le catalogue des plugins disponibles non activés.

## 19. Connecteurs

**Objectif** : gérer les connexions aux systèmes externes (ERP, CRM) alimentant l'Import.

**Zones** : liste des connecteurs configurés avec statut de synchronisation ; formulaire de configuration d'un nouveau connecteur ; historique de synchronisation par connecteur.

**Priorité** : un connecteur en échec de synchronisation prime sur les connecteurs fonctionnant normalement.

## 20. ORION

**Objectif** : interface dédiée à l'historique complet des échanges avec l'assistant, au-delà du panneau rapide accessible depuis la Topbar.

**Zones** :
- **Fil de conversation** — historique des questions et réponses, organisé chronologiquement.
- **Suggestions contextuelles** — questions ou analyses proposées par ORION en fonction de l'activité récente de l'organisation, présentées avant toute saisie de l'utilisateur.
- **Résumés automatiques** — synthèses périodiques (quotidienne, hebdomadaire) générées sans sollicitation explicite, consultables comme des entrées du fil.
- **Recommandations** — mises en avant visuellement distinctes d'une réponse factuelle, toujours accompagnées de la donnée qui les justifie.
- **Zone de saisie** — question libre en langage naturel, avec accès rapide aux suggestions.

**Priorité** : une alerte ou recommandation active prime sur l'historique de conversation ; le fil de conversation reste secondaire par rapport à toute information appelant une action immédiate.

## 21. Paramètres

**Objectif** : configuration de l'organisation et des préférences.

**Zones** : sous-sections Objectifs, Notifications, Préférences d'affichage, Langue (cf. [NAVIGATION.md](NAVIGATION.md) §4).

**Priorité** : les objectifs commerciaux (impact direct sur le Dashboard et les Alertes) priment sur les préférences d'affichage.

## 22. Profil

**Objectif** : gestion du compte personnel de l'utilisateur.

**Zones** : informations personnelles ; sécurité du compte (authentification) ; préférences individuelles de notification.

**Priorité** : la sécurité du compte prime sur les préférences de confort.

## 23. Journal

**Objectif** : consultation du journal technique général d'activité de la plateforme pour l'organisation (cf. [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §2).

**Zones** : liste chronologique des événements système, filtrable par module et par gravité.

**Priorité** : les événements de gravité élevée priment visuellement, sans masquer l'accès à l'historique complet.

## 24. Audit

**Objectif** : traçabilité des actions utilisateurs et administratives (cf. [architecture/LOGGING_STRATEGY.md](../architecture/LOGGING_STRATEGY.md) §3).

**Zones** : liste chronologique des actions (qui, quoi, quand), filtrable par utilisateur, type d'action et période.

**Priorité** : réservé aux profils habilités (Administrateur, Contrôle de Gestion) ; aucune hiérarchie de mise en avant au-delà de l'ordre chronologique, la neutralité étant essentielle à la fonction d'audit.

## 25. Support

**Objectif** : accès à l'assistance et à la documentation d'usage.

**Zones** : accès à la documentation utilisateur ; formulaire de contact/ticket ; état des demandes en cours.

**Priorité** : une demande en cours de traitement prime sur l'accès à la documentation générale.

## 26. À propos

**Objectif** : informations institutionnelles (version, licence, mentions).

**Zones** : version du logiciel et canal (cf. [architecture/VERSIONING_STRATEGY.md](../architecture/VERSIONING_STRATEGY.md)) ; mentions légales ; informations de contact de l'éditeur.

**Priorité** : écran à faible fréquence de consultation ; aucune priorisation dynamique, contenu strictement statique.
