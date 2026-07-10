# ORBITA AI — Interaction Rules

> Ce document définit les règles générales d'interaction entre l'utilisateur et le produit. Le détail des animations et retours visuels fins est défini dans [MICRO_INTERACTIONS.md](MICRO_INTERACTIONS.md) ; ce document couvre les règles de comportement, pas le rendu.

## 1. Principe général

Toute action de l'utilisateur produit un retour perceptible, immédiat ou différé selon sa nature. Aucune action ne doit laisser l'utilisateur dans l'incertitude quant à sa prise en compte par le système (cf. [architecture/ERROR_HANDLING.md](../architecture/ERROR_HANDLING.md) §4, appliqué à l'expérience utilisateur).

## 2. Cible d'interaction

- Toute zone cliquable respecte la taille minimale définie dans [branding/ACCESSIBILITY.md](../branding/ACCESSIBILITY.md) §3 (40×40px).
- Une zone qui n'est pas interactive ne doit jamais présenter d'indice visuel la faisant paraître cliquable (curseur, changement d'état au survol).

## 3. Clic

- Un clic simple déclenche toujours l'action principale attendue de l'élément (navigation, sélection, activation).
- Un double-clic n'est jamais requis pour accéder à une fonctionnalité standard du produit.
- Un clic sur un élément déjà actif ou déjà sélectionné ne produit jamais d'effet secondaire inattendu (bascule non désirée).

## 4. Sélection

- La sélection d'une ou plusieurs lignes dans un tableau (cf. [COMPONENT_LIBRARY.md](COMPONENT_LIBRARY.md) §5) persiste tant que l'utilisateur reste sur l'écran, y compris après un tri ou un changement de page de pagination.
- La sélection est explicitement effaçable par une action dédiée, jamais uniquement en quittant l'écran.

## 5. Glisser-déposer (drag)

- Le glisser-déposer est réservé à des usages ponctuels et non essentiels (ex. réorganisation de cartes personnalisables sur un Dashboard configurable, dépôt d'un fichier sur l'écran Import).
- Toute action réalisable par glisser-déposer doit disposer d'une alternative accessible sans manipulation directe (ex. bouton "sélectionner un fichier" en complément du dépôt par glisser, réordonnancement par menu contextuel en alternative au glisser de carte).
- Un élément en cours de déplacement indique clairement sa zone de dépôt valide.

## 6. Navigation clavier

- L'ensemble des interactions définies dans ce document doit être réalisable au clavier, conformément à [ACCESSIBILITY_UI.md](ACCESSIBILITY_UI.md).
- L'ordre de tabulation suit l'ordre de lecture visuel de haut en bas, de gauche à droite.

## 7. Persistance du contexte

- Les filtres actifs (période, périmètre, recherche) sont conservés lorsque l'utilisateur navigue vers un écran connexe dans le cadre d'un même parcours (cf. [USER_FLOWS.md](USER_FLOWS.md)), par exemple d'une carte KPI du Dashboard vers Analytics.
- Un retour en arrière (fil d'Ariane, navigation) restitue l'état exact de l'écran quitté (filtres, tri, pagination), jamais un état par défaut réinitialisé.

## 8. Confirmation

- Toute action destructrice ou irréversible (suppression, résiliation, modification d'un droit d'accès sensible) requiert une confirmation explicite de l'utilisateur avant exécution (cf. [MICRO_INTERACTIONS.md](MICRO_INTERACTIONS.md) §9).
- Une action réversible ou sans conséquence significative (changement de filtre, tri) ne requiert jamais de confirmation : la confirmation est réservée aux actions qui le justifient réellement, pour ne pas ralentir l'usage courant.

## 9. Annulation

- Lorsque techniquement pertinent, une action récente non critique (ex. suppression d'un filtre enregistré) propose une option d'annulation immédiate plutôt qu'une confirmation préalable systématique, réduisant la friction sans compromettre la sécurité.

## 10. Cohérence entre écrans

- Un même geste (clic sur une carte, clic sur une ligne de tableau) produit toujours la même catégorie de résultat (ouverture d'un détail ou d'une analyse) quel que soit l'écran, conformément au principe de cohérence absolue défini dans [UX_PRINCIPLES.md](UX_PRINCIPLES.md) §4.3.

## 11. Interaction avec ORION

- L'ouverture du panneau ORION depuis un écran ne modifie jamais l'état de cet écran en arrière-plan.
- Une recommandation ou un lien proposé par ORION suit les mêmes règles de navigation que le reste du produit (cf. §7) : le contexte de la question posée est reporté sur l'écran de destination.
