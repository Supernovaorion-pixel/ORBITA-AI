# ORBITA AI — Component Library (UX Specification)

> Ce document catalogue les composants d'interface officiels du produit et leur comportement fonctionnel. Il ne contient ni code, ni CSS, ni spécification graphique détaillée (couleurs, tailles exactes — cf. [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md)) : uniquement les règles de comportement que tout composant doit respecter.

## 1. Principe

Un composant listé ici est **unique** dans le produit : il n'existe qu'une seule définition de "bouton primaire" ou de "tableau de données", utilisée à l'identique dans tous les écrans (cf. [DESIGN_SYSTEM.md](DESIGN_SYSTEM.md) §7). Toute variante non prévue par ce document doit être ajoutée ici avant d'être utilisée dans un écran.

## 2. Boutons

- **Primaire** — une seule action primaire par écran ou par carte ; jamais deux boutons primaires visibles simultanément dans une même zone.
- **Secondaire** — actions alternatives ou complémentaires à l'action primaire.
- **Ghost/texte** — actions tertiaires, peu fréquentes, jamais utilisées pour une action destructrice.
- **Destructif** — toujours accompagné d'une confirmation (cf. [MICRO_INTERACTIONS.md](MICRO_INTERACTIONS.md) §9).
- États visuels : cf. [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md) §11.

## 3. Champs de formulaire

- Libellé toujours visible au-dessus du champ (jamais uniquement en placeholder, cf. [ACCESSIBILITY_UI.md](ACCESSIBILITY_UI.md)).
- Message d'aide ou d'erreur positionné immédiatement sous le champ.
- Comportement de validation : à la perte de focus (`blur`) pour la validation de format, jamais uniquement à la soumission du formulaire entier.

## 4. Cartes (Cards)

- Structure commune définie dans [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §6.
- Deux variantes fonctionnelles : **carte KPI** (cf. [DASHBOARD_SPECIFICATION.md](DASHBOARD_SPECIFICATION.md) §4) et **carte de contenu** (regroupement d'informations, ex. fiche synthétique sur un écran de détail).
- Une carte n'est cliquable dans son intégralité que si elle mène à un seul et unique écran de destination ; si elle contient plusieurs actions possibles, celles-ci sont explicites (boutons), jamais ambiguës.

## 5. Tableaux de données

Composant central du produit (Clients, Produits, Commerciaux, Historique, Journal, Audit...). Comportement commun obligatoire :

- **Tri** — activable sur toute colonne porteuse d'une valeur ordonnable (numérique, date, alphabétique) par clic sur l'en-tête ; un seul critère de tri actif à la fois, indiqué visuellement dans l'en-tête concerné.
- **Filtres** — accessibles au-dessus du tableau, combinables entre eux ; un filtre actif est toujours visible sous forme de badge retirable, jamais caché dans un panneau qu'il faudrait rouvrir pour vérifier son état.
- **Recherche** — champ de recherche textuelle libre, appliqué aux colonnes pertinentes du tableau (jamais à l'intégralité de la base au-delà du périmètre affiché).
- **Pagination** — par blocs de taille constante à travers tout le produit (cf. [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §7) ; le défilement infini n'est jamais utilisé pour des tableaux de gestion (Utilisateurs, Clients), réservé le cas échéant aux flux chronologiques (Journal, Audit, fil ORION).
- **Sélection** — case à cocher en première colonne lorsque des actions groupées existent ; une sélection active affiche une barre d'actions contextuelle au-dessus du tableau.
- **Export** — action toujours disponible depuis l'en-tête du tableau, opérant sur les lignes filtrées actuellement affichées (jamais sur l'intégralité de la base sans confirmation explicite du périmètre).
- **Colonnes** — configurables en affichage/masquage par l'utilisateur pour les tableaux à fort nombre de colonnes (Clients, Produits) ; l'ordre des colonnes par défaut place toujours l'identifiant métier principal en première position.
- **Actions** — regroupées en fin de ligne (icône ou menu contextuel), jamais dispersées entre plusieurs colonnes.

## 6. Graphiques

Les règles de construction et de choix de chaque type de graphique sont définies dans [DATA_VISUALIZATION.md](DATA_VISUALIZATION.md). Tout graphique, quel que soit son type, partage :
- une légende explicite si plus d'une série est représentée,
- une infobulle (tooltip) au survol ou au focus d'un point de donnée (cf. [MICRO_INTERACTIONS.md](MICRO_INTERACTIONS.md)),
- un accès à la donnée sous forme tabulaire équivalente, pour l'accessibilité (cf. [ACCESSIBILITY_UI.md](ACCESSIBILITY_UI.md) §6).

## 7. Menus et sous-menus

- Structure définie dans [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §11 et [NAVIGATION.md](NAVIGATION.md).
- Un menu contextuel (clic droit ou icône "actions") ne contient jamais plus de sept actions ; au-delà, un regroupement ou un renvoi vers un écran dédié est nécessaire.

## 8. Modales

- Réservées aux actions nécessitant une confirmation ou une saisie courte et bloquante (cf. [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §10).
- Ne sont jamais utilisées pour afficher un contenu de consultation longue (préférer un panneau latéral ou un écran dédié).

## 9. Panneaux latéraux (drawers)

- Utilisés pour une consultation ou édition complémentaire à l'écran courant sans le quitter (ex. fiche rapide d'un Client depuis un tableau, panneau ORION).
- Toujours refermables par `Échap` ou par une action explicite, jamais uniquement par clic en dehors sans indication visuelle.

## 10. Badges et étiquettes de statut

- Toujours associés à une couleur sémantique et à un libellé textuel, jamais à la couleur seule (cf. [ACCESSIBILITY_UI.md](ACCESSIBILITY_UI.md) §3).
- Un même statut métier (ex. "En retard", "Actif", "Critique") utilise toujours le même badge à travers tout le produit.

## 11. Infobulles (tooltips)

- Utilisées pour un complément d'information non essentiel à la compréhension immédiate ; une information indispensable ne doit jamais être reléguée à une infobulle uniquement accessible au survol.

## 12. Panneau ORION

- Composant transversal accessible depuis tout écran (cf. [NAVIGATION.md](NAVIGATION.md) §3), structuré en zone de conversation, suggestions contextuelles et zone de saisie (cf. écran ORION, [SCREEN_STRUCTURE.md](SCREEN_STRUCTURE.md) §20).
- Comportement identique qu'il soit ouvert en panneau latéral rapide ou en écran dédié complet.

## 13. Filtres de période et de périmètre

- Composant unique réutilisé sur Dashboard, Analytics, Forecast, Historique, Rapports : mêmes libellés, même ordre de présentation (période avant périmètre), même mécanisme de comparaison (cf. [DASHBOARD_SPECIFICATION.md](DASHBOARD_SPECIFICATION.md) §2.1).

## 14. Principe de non-prolifération

Avant de concevoir un nouveau composant, tout écran futur doit d'abord vérifier si un composant existant de ce catalogue répond au besoin. Un nouveau composant n'est ajouté à ce document que si aucune combinaison des composants existants ne permet de répondre au besoin fonctionnel identifié.
