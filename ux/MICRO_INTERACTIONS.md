# ORBITA AI — Micro-Interactions

> Ce document définit les retours fins d'interaction (feedback immédiat) associés à chaque type d'événement utilisateur. Les durées, courbes d'accélération et principes généraux de mouvement sont définis dans [branding/MOTION_GUIDELINES.md](../branding/MOTION_GUIDELINES.md) ; ce document précise leur application à des événements d'interaction précis. Aucune implémentation n'est décrite.

## 1. Hover (survol)

- Tout élément interactif change d'état visuel au survol (couleur de fond ou de texte, cf. [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md) §11), en 100 à 150ms (cf. [branding/MOTION_GUIDELINES.md](../branding/MOTION_GUIDELINES.md) §2).
- Le survol ne déclenche jamais à lui seul une action (ouverture de contenu, navigation) : il ne fait qu'indiquer l'interactivité de l'élément.
- Une ligne de tableau survolée change de fond (cf. [branding/DESIGN_PRINCIPLES.md](../branding/DESIGN_PRINCIPLES.md) §7), sans déplacement ni redimensionnement du contenu.

## 2. Focus

- Identique en comportement au survol pour les éléments qui partagent les deux états, avec en complément l'anneau de focus obligatoire défini dans [branding/ACCESSIBILITY.md](../branding/ACCESSIBILITY.md) §5.
- Le focus apparaît instantanément (sans délai de transition perceptible) afin de ne jamais ralentir la navigation clavier.

## 3. Clic

- Un clic produit un changement d'état immédiat (couleur "active/pressed", cf. [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md) §11), sans effet de rebond ni de compression exagérée (cf. [branding/MOTION_GUIDELINES.md](../branding/MOTION_GUIDELINES.md) §3).
- Un clic déclenchant une action différée (chargement, traitement) fait immédiatement basculer l'élément vers un état "en cours" perceptible (cf. [LOADING_STATES.md](LOADING_STATES.md)), pour confirmer la prise en compte du clic sans attendre la fin du traitement.

## 4. Drag (glisser-déposer)

- L'élément saisi suit le curseur sans latence perceptible ; sa zone d'origine reste visuellement indiquée (espace réservé ou estompage léger) pendant le déplacement.
- Une zone de dépôt valide se distingue visuellement (bordure ou fond `brand.primary.tint`) dès que l'élément déplacé la survole.
- Le relâchement en dehors d'une zone de dépôt valide restitue l'élément à sa position d'origine par une transition courte (`ease.out`, cf. [branding/MOTION_GUIDELINES.md](../branding/MOTION_GUIDELINES.md) §3), jamais une disparition brutale.

## 5. Loading (chargement)

- Cf. règles complètes dans [LOADING_STATES.md](LOADING_STATES.md). Au niveau micro-interaction : tout passage vers un état de chargement s'accompagne d'une transition de fondu courte (150ms) entre le contenu précédent et l'indicateur de chargement, jamais d'un remplacement instantané et abrupt.

## 6. Notifications

- Une notification apparaît par glissement discret depuis le bord supérieur droit de l'écran (zone Topbar), avec fondu combiné, en 200 à 250ms.
- Une notification informative se retire automatiquement après quelques secondes ; une notification liée à une alerte critique reste affichée jusqu'à action explicite de l'utilisateur (cf. [ERROR_STATES.md](ERROR_STATES.md) §7 sur la distinction Alerte/Erreur).
- Plusieurs notifications simultanées s'empilent verticalement sans se chevaucher, la plus récente en position la plus proche de la Topbar.

## 7. Succès

- Confirmation par une couleur sémantique de succès (cf. [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md) §4), toujours accompagnée d'un message textuel bref, jamais de la couleur seule.
- La confirmation d'une action (ex. rapport généré, import validé) apparaît immédiatement après l'achèvement réel de l'action, jamais anticipée avant sa confirmation effective par le système.

## 8. Erreur

- Cf. [ERROR_STATES.md](ERROR_STATES.md) pour le contenu et l'emplacement. Au niveau micro-interaction : l'apparition d'un message d'erreur ne doit jamais s'accompagner d'un mouvement brusque (tremblement, secousse) qui dramatiserait excessivement la situation, contraire à la sobriété de la marque.

## 9. Confirmation (actions destructrices ou sensibles)

- Toute action définie comme requérant une confirmation (cf. [INTERACTION_RULES.md](INTERACTION_RULES.md) §8) ouvre une modale dédiée (cf. [COMPONENT_LIBRARY.md](COMPONENT_LIBRARY.md) §8), jamais une simple bascule de couleur du bouton initial.
- Le bouton de confirmation d'une action destructrice utilise systématiquement la couleur sémantique critique (cf. [branding/COLOR_SYSTEM.md](../branding/COLOR_SYSTEM.md) §11 — bouton destructif), jamais la couleur primaire de la marque.
- La modale de confirmation reformule explicitement l'action et son caractère irréversible, jamais une simple question générique ("Êtes-vous sûr ?") sans rappel du contexte précis.

## 10. Principe transversal

Chaque micro-interaction doit rester perceptible sans être remarquée consciemment par l'utilisateur : une micro-interaction qui attire l'attention sur elle-même, plutôt que sur l'information ou l'action qu'elle accompagne, est contraire au principe de sobriété qui gouverne l'ensemble de l'expérience ORBITA AI (cf. [UX_PRINCIPLES.md](UX_PRINCIPLES.md) §3).
