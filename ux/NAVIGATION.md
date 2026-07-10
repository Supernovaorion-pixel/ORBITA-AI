# ORBITA AI — Navigation

> Ce document définit la navigation complète du produit : structure des menus, sous-menus, raccourcis et grands parcours utilisateurs. Aucune implémentation n'est décrite.

## 1. Structure générale

La navigation repose sur trois zones fixes, conformes à [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §12-13 :

- **Sidebar** (navigation principale, gauche) — accès à l'ensemble des écrans, organisés par section.
- **Topbar** (haut, fixe) — identité de l'écran courant, accès à ORION, notifications, profil utilisateur.
- **Zone de contenu** — l'écran actif, éventuellement structuré en sous-onglets internes (cf. [SCREEN_STRUCTURE.md](SCREEN_STRUCTURE.md)).

## 2. Arborescence de la Sidebar

```
Accueil

Pilotage
├── Dashboard
├── Analytics
├── Forecast
└── Alertes

Référentiel
├── Commerciaux
├── Clients
├── Grands Comptes
├── Produits
└── Familles

Données
├── Import
└── Historique

Restitution
├── Rapports
└── Exports

ORION

Administration
├── Utilisateurs
├── Licences
├── Plugins
└── Connecteurs

Système
├── Journal
├── Audit
└── Support
```

- Chaque section est un regroupement visuel (libellé en style `overline`, cf. [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §12), jamais un niveau de navigation cliquable en tant que tel.
- La section **Administration** n'est visible que pour les utilisateurs disposant des droits correspondants (cf. persona Administrateur, [docs/USER_PERSONAS.md](../docs/USER_PERSONAS.md)) ; son absence pour un utilisateur non habilité ne laisse aucun espace vide dans la sidebar.
- Le contenu exact de chaque section peut être restreint selon la licence de l'Organisation (cf. [docs/LICENSE_SYSTEM.md](../docs/LICENSE_SYSTEM.md)) : un module non inclus dans l'offre souscrite n'apparaît pas dans la sidebar plutôt que d'apparaître désactivé.

## 3. Éléments de la Topbar

De gauche à droite :

1. **Titre de l'écran courant** (`H3`) et, si applicable, fil d'Ariane (breadcrumb) pour les écrans imbriqués (ex. Clients → Fiche client).
2. **Accès rapide à ORION** — icône persistante, ouvrant un panneau de conversation sans quitter l'écran courant (cf. section ORION, [SCREEN_STRUCTURE.md](SCREEN_STRUCTURE.md)).
3. **Centre de notifications** — accès aux alertes récentes (cf. écran Alertes).
4. **Menu Profil** — accès à Profil, Paramètres, et déconnexion.

## 4. Sous-menus et écrans imbriqués

- Les écrans **Clients**, **Produits**, **Commerciaux**, **Grands Comptes** disposent d'une vue liste et d'une vue fiche (détail d'une entité) accessible en un clic depuis la liste ; la fiche est toujours accessible par une URL/état propre, jamais uniquement par une modale plein écran.
- L'écran **Administration** regroupe **Utilisateurs**, **Licences**, **Plugins** et **Connecteurs** sous une navigation secondaire (onglets horizontaux) propre à cette section, distincte de la Sidebar.
- L'écran **Paramètres** regroupe ses propres sous-sections (Objectifs, Notifications, Préférences d'affichage, Langue) sous une navigation secondaire identique en principe à celle d'Administration.

## 5. Raccourcis clavier officiels

| Raccourci | Action |
|---|---|
| `G` puis `D` | Aller au Dashboard |
| `G` puis `A` | Aller à Analytics |
| `G` puis `F` | Aller à Forecast |
| `G` puis `C` | Aller à Clients |
| `G` puis `R` | Aller à Rapports |
| `/` | Ouvrir la recherche globale |
| `O` | Ouvrir/fermer le panneau ORION |
| `N` | Ouvrir le centre de notifications |
| `Échap` | Fermer tout panneau, modale ou menu actif |
| `?` | Afficher la liste des raccourcis disponibles |

Ces raccourcis suivent le modèle "aller à" (`G` + lettre), cohérent avec les standards professionnels et jamais en conflit avec la saisie de texte dans un champ actif.

## 6. Recherche globale

- Accessible depuis tout écran via `/` ou une icône dédiée en Topbar.
- Permet de retrouver directement une entité (Client, Produit, Commercial, Rapport) sans naviguer par la Sidebar.
- Les résultats sont groupés par type d'entité, dans l'ordre : Clients, Produits, Commerciaux, Rapports, Autres.

## 7. Grands parcours utilisateurs (vue d'ensemble)

Le détail pas à pas de chaque parcours est défini dans [USER_FLOWS.md](USER_FLOWS.md). Les grands parcours structurants sont :

1. **Connexion → Accueil → Dashboard** (parcours quotidien standard).
2. **Alerte reçue → Analytics → ORION → Action** (parcours d'investigation d'un écart).
3. **Import de données → Historique → Dashboard actualisé** (parcours de mise à jour de la donnée).
4. **Dashboard → Rapports → Export** (parcours de restitution).
5. **Administration → Utilisateurs → Licences** (parcours de gestion de l'organisation).
6. **Question libre → ORION → Recommandation** (parcours d'assistance).

## 8. Principe de retour

Depuis n'importe quel écran imbriqué (fiche, sous-section), un retour à la vue parente est toujours possible par le fil d'Ariane en Topbar ou par la touche `Échap`, sans jamais dépendre exclusivement du bouton "retour" du navigateur ou système.

## 9. Cohérence de la navigation entre éditions

La structure de la Sidebar reste identique dans son principe pour toutes les éditions (Community à Enterprise, cf. [docs/BUSINESS_MODEL.md](../docs/BUSINESS_MODEL.md)) : seules certaines sections ou certains écrans sont absents selon le périmètre de licence, jamais réorganisés différemment. Cette stabilité garantit qu'un utilisateur migrant d'une édition à une autre retrouve immédiatement ses repères.
